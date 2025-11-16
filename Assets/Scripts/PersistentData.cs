using OpenAI;
using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PersistentData : MonoBehaviour
{
    public static PersistentData Instance { get; private set; } // Static reference to the single instance

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
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sessions = new List<SessionData>();
        // check if analytics exists, check in if do
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RecordSession(SessionData session)
    {
        sessions.Add(session);
        // add to json analytics file
    }
    public List<SessionData> GetSessions() { return sessions; }
    public int GetSessionCount() { return sessions.Count; }
}
