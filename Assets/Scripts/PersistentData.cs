using OpenAI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Overlays;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance { get; private set; } // Static reference to the single instance
    private static string LogPath;

    private List<SessionData> sessions;

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
        LogPath = Application.persistentDataPath + "/log.json";
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sessions = new List<SessionData>();
        Debug.Log(LogPath);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecordSession(SessionData session)
    {
        sessions.Add(session);
        System.IO.File.AppendAllText(LogPath, session.ToJson() + "\n");
    }
    public List<SessionData> GetSessions() { return sessions; }
    public int GetSessionCount() { return sessions.Count; }
}
