using TMPro;
using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class AnalyticsMenu : MainMenu
{
    private PersistentData persistentData;
    [SerializeField] private TextMeshProUGUI UserWordCountText;
    [SerializeField] private TextMeshProUGUI ChatWordCountText;
    [SerializeField] private TextMeshProUGUI SessionCountText;
    [SerializeField] private TextMeshProUGUI QueryCountText;
    [SerializeField] private TextMeshProUGUI SessionTimeText;

    void Awake()
    {
        PersistentData pd = GameObject.FindFirstObjectByType<PersistentData>();
        persistentData = pd;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(persistentData != null)
        {
            int totalUserWordCount = 0;
            int totalChatWordCount = 0;
            int totalQueryCount = 0;
            long totalSessionTime = 0;

            foreach(SessionData session in persistentData.GetSessions())
            {
                totalUserWordCount += session.userWordCount;
                totalChatWordCount += session.chatWordCount;
                totalQueryCount += session.queryCount;
                totalSessionTime += session.SessionTime();
            }

            UserWordCountText.text += totalUserWordCount;
            ChatWordCountText.text += totalChatWordCount;
            SessionCountText.text += persistentData.GetSessionCount();
            QueryCountText.text += totalQueryCount;
            SessionTimeText.text += TimeSpan.FromTicks(totalSessionTime);
        }
    }
}
