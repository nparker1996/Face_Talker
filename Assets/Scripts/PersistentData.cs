using OpenAI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance { get; private set; } // Static reference to the single instance

    private int userWordCount = 0;
    private int chatWordCount = 0;
    private int sessionCount = 0;
    private int queryCount = 0;

    private long sessionStartTime = 0;
    private long sessionTime = 0;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this; // Assign this instance as the singleton
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject); // Destroy duplicate instances
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        // check if analytics exists, check in if do
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetStartTime()
    {
        sessionStartTime = DateTime.Now.Ticks;
    }

    public int GetUserWordCount() { return userWordCount; }
    public int GetChatWordCount() { return chatWordCount; }
    public int GetSessionCount() { return sessionCount; }
    public int GetQueryCount() { return queryCount; }
    public long GetSessionTime() { return sessionTime; }

    public void IncrementQueryCount()
    {
        queryCount++;
    }
    public void IncrementSessionCount()
    {
        sessionCount++;
    }

    public void RecordSession(List<ChatMessage> chatMessages)
    {
        if(sessionStartTime != 0) sessionTime += (DateTime.Now.Ticks - sessionStartTime);
        sessionStartTime = 0;

        for(int i = 1; i < chatMessages.Count; i++)
        {
            ChatMessage chatMessage = chatMessages[i];
            int wordCount = chatMessage.Content.Trim().Split(' ').Length;
            if (chatMessage.Role == "user") userWordCount += wordCount;
            else chatWordCount += wordCount;
        }

    }
}
