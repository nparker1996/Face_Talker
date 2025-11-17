using OpenAI;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

[System.Serializable]
public class UnityStringEvent : UnityEvent<string> { }


public class ChatFace : MonoBehaviour, ISpeechToTextListener
{
    [SerializeField] private InputField inputField;
    [SerializeField] private Button sendButton;
    [SerializeField] private ScrollRect scroll;
    private PersistentData persistentData;

    [SerializeField] private RectTransform sent;
    [SerializeField] private RectTransform received;

    [SerializeField] private Button SpeechToTextButton;
    public bool PreferOfflineRecognition;
    private float normalizedVoiceLevel;
    private bool sttIsBusy;

    private float height;
    private OpenAIApi openai = new OpenAIApi();

    private SessionData session;
    private List<ChatMessage> messages = new List<ChatMessage>();
    private string prompt = "Act as a care taker sitting right next to the user. Assume the user is above the age of 50. You are not a profession mental health expert. " +
        "Have a converstation to gauge their mental & cognitive state. Use a friendly, warm, supportive tone throughout. Always encourage self-compassion and a hopeful outlook. " +
        "Let the user direct the conversation, but make sure user is thinking keeping the conversation related to their mental & cognitive state. " + 
        "Don't break character. Don't ever mention that you are an AI model. Avoid going over 100 words per response.";

    void Awake()
    {
        PersistentData pd = GameObject.FindFirstObjectByType<PersistentData>();
        persistentData = pd;
    }

    private void Start()
    {
        SpeechToText.Initialize("en-US");
        SpeechToTextButton.onClick.AddListener(StartSpeechToText);
        sttIsBusy = false;

        sendButton.onClick.AddListener(SendReply);
        session = new SessionData();
    }

    private void Update()
    {
        SpeechToTextButton.interactable = SpeechToText.IsServiceAvailable(PreferOfflineRecognition);
        if(sttIsBusy != SpeechToText.IsBusy())
        {
            sttIsBusy = SpeechToText.IsBusy();
            if (sttIsBusy) // Speech to text is happening
            {
                SpeechToTextButton.onClick.RemoveListener(StartSpeechToText);
                SpeechToTextButton.onClick.AddListener(StopSpeechToText);
                SpeechToTextButton.gameObject.GetComponentInChildren<Text>().text = "Stop Speaking";
            }
            else // Speech to text is not happening
            {
                SpeechToTextButton.onClick.RemoveListener(StopSpeechToText);
                SpeechToTextButton.onClick.AddListener(StartSpeechToText);
                SpeechToTextButton.gameObject.GetComponentInChildren<Text>().text = "Start Speaking";
            }
        }
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

        sendButton.enabled = false;
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

        sendButton.enabled = true;
        inputField.enabled = true;
    }

    public void EndSessionCall()
    {
        session.sessionEndTime = DateTime.Now.Ticks;
        if (messages.Count > 0)
        {
            messages.RemoveAt(0); // removing prompt

            foreach (var message in messages)
            {
                session.chatMessages.Add(new SerializableChatMessage(message.Role, message.Content));

                int wordCount = message.Content.Trim().Split(' ').Length;
                if (message.Role == "user") session.userWordCount += wordCount;
                else session.chatWordCount += wordCount;
            }
        }

        if (persistentData != null) { persistentData.RecordSession(session); }
        else { Debug.LogWarning("No session found, session not recorded"); }
    }

    #region STT
    public void StartSpeechToText()
    {
        SpeechToText.RequestPermissionAsync((permission) =>
        {
            if (permission == SpeechToText.Permission.Granted)
            {
                if (SpeechToText.Start(this, preferOfflineRecognition: PreferOfflineRecognition))
                {
                    inputField.text = "";
                }
                else { inputField.text = "Couldn't start speech recognition session!"; }
            }
            else { inputField.text = "Permission is denied!"; }
        });
    }

    public void StopSpeechToText()
    {
        SpeechToText.ForceStop();
    }

    void ISpeechToTextListener.OnReadyForSpeech()
    {
        Debug.Log("OnReadyForSpeech");
    }

    void ISpeechToTextListener.OnBeginningOfSpeech()
    {
        Debug.Log("OnBeginningOfSpeech");
    }

    void ISpeechToTextListener.OnVoiceLevelChanged(float normalizedVoiceLevel)
    {
        // Note that On Android, voice detection starts with a beep sound and it can trigger this callback. You may want to ignore this callback for ~0.5s on Android.
        this.normalizedVoiceLevel = normalizedVoiceLevel;
    }

    void ISpeechToTextListener.OnPartialResultReceived(string spokenText)
    {
        Debug.Log("OnPartialResultReceived: " + spokenText);
        inputField.text = spokenText;
    }

    void ISpeechToTextListener.OnResultReceived(string spokenText, int? errorCode)
    {
        Debug.Log("OnResultReceived: " + spokenText + (errorCode.HasValue ? (" --- Error: " + errorCode) : ""));
        inputField.text = spokenText;
        normalizedVoiceLevel = 0f;
    }

    #endregion
}