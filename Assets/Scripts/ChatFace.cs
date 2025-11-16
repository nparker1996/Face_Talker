using OpenAI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class UnityStringEvent : UnityEvent<string> { }


public class ChatFace : MonoBehaviour
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button button;
    [SerializeField] private ScrollRect scroll;
    private PersistentData persistentData;

    [SerializeField] private RectTransform sent;
    [SerializeField] private RectTransform received;
    
    private float height;
    private OpenAIApi openai = new OpenAIApi();

    private SessionData session;
    private List<ChatMessage> messages = new List<ChatMessage>();
    private string prompt = "Act as a care taker sitting right next to the user, have a converstation to gauge their mental & cognitive state. Don't break character. Don't ever mention that you are an AI model.";

    void Awake()
    {
        PersistentData pd = GameObject.FindFirstObjectByType<PersistentData>();
        persistentData = pd;
    }

    private void Start()
    {
        button.onClick.AddListener(SendReply);

        session = new SessionData();
    }

    private void AppendMessage(ChatMessage message)
    {
        scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, 0);

        var item = Instantiate(message.Role == "user" ? sent : received, scroll.content);
        item.GetChild(0).GetChild(0).GetComponent<Text>().text = message.Content;
        item.anchoredPosition = new Vector2(0, -height);
        LayoutRebuilder.ForceRebuildLayoutImmediate(item);
        height += item.sizeDelta.y;
        scroll.content.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        scroll.verticalNormalizedPosition = 0;
    }

    private async void SendReply()
    {
        var newMessage = new ChatMessage()
        {
            Role = "user",
            Content = inputField.text
        };

        AppendMessage(newMessage); //appending user message

        if (messages.Count == 0)
        {
            var promptMessage = new ChatMessage()
            {
                Role = "user",
                Content = prompt
            };

            messages.Add(promptMessage);
        }

        messages.Add(newMessage);

        button.enabled = false;
        inputField.text = "";
        inputField.enabled = false;

        // Complete the instruction
        var completionResponse = await openai.CreateChatCompletion(new CreateChatCompletionRequest()
        {
            Model = "gpt-4o-mini",
            Messages = messages
        });

        if (completionResponse.Choices != null && completionResponse.Choices.Count > 0)
        {
            var message = completionResponse.Choices[0].Message;
            message.Content = message.Content.Trim();

            messages.Add(message);
            AppendMessage(message); //appending openAI message

            session.IncrementQueryCount();
        }
        else
        {
            Debug.LogWarning("No text was generated from this prompt: \"" + newMessage.Content + "\"");
        }

        button.enabled = true;
        inputField.enabled = true;
    }

    public void EndSessionCall()
    {
        session.sessionEndTime = DateTime.Now.Ticks;
        if (messages.Count > 0)
        {
            messages.RemoveAt(0); // removing prompt
            session.chatMessages = messages;
        }

        foreach(var chatMessage in messages)
        {
            int wordCount = chatMessage.Content.Trim().Split(' ').Length;
            if (chatMessage.Role == "user") session.userWordCount += wordCount;
            else session.chatWordCount += wordCount;
        }

        if (persistentData != null)
        {
            persistentData.RecordSession(session);
        }
        else
        {
            Debug.LogWarning("No session found, session not recorded");
        }
    }
    
}