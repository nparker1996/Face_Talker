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
            UserWordCountText.text += persistentData.GetUserWordCount();
            ChatWordCountText.text += persistentData.GetChatWordCount();
            SessionCountText.text += persistentData.GetSessionCount();
            QueryCountText.text += persistentData.GetQueryCount();
            SessionTimeText.text += TimeSpan.FromTicks(persistentData.GetSessionTime());
        }
    }
}
