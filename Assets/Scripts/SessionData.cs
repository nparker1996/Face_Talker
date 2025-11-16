using OpenAI;
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SessionData
{
    private static long sessionStartTime;
    public long sessionEndTime = 0;
    public int userWordCount = 0;
    public int chatWordCount = 0;
    public int queryCount = 0;
    public List<ChatMessage> chatMessages { get; set; } = new List<ChatMessage>();

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
}
