using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class UnityStringEvent : UnityEvent<string> { }


public class ChatFace : MonoBehaviour
{
    public UnityStringEvent onChatEvent;
    public string inputString = "what is 6 times 7?";
    public string prePrompt = "Respond briefly and help assess my mental & cognitive state, while assuming that I am elderly person.";

    private void Start()
    {
        onChatEvent.AddListener(HandleStringEvent);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T)) {
            onChatEvent.Invoke(prePrompt + " " + inputString);
        }
    }

    public void HandleStringEvent(string message)
    {
        Debug.Log("Chat Received Message: " + message);
    }
}
