using OpenAI;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SessionData
{
    public long sessionStartTime = 0;
    public long sessionEndTime = 0;
    public int userWordCount = 0;
    public int chatWordCount = 0;
    public int queryCount = 0;
    public List<SerializableChatMessage> chatMessages = new List<SerializableChatMessage>();

    public SessionData()
    {
        sessionStartTime = DateTime.Now.Ticks;
    }
    public SessionData(long startTime)
    {
        sessionStartTime = startTime;
    }

    public long SessionTime()
    {
        return Math.Max(sessionEndTime - sessionStartTime, 0L);
    }

    public void IncrementQueryCount()
    {
        queryCount++;
    }

    public string ToJson()
    {
        return JsonUtility.ToJson(this);
    }
}

// doing this because ChatMessage is not serializable
[Serializable]
public class SerializableChatMessage
{
    public string Role;
    public string Content;

    public SerializableChatMessage(string role, string content)
    {
        Role = role; Content = content;
    }
}