using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private SceneAsset mainMenuScene;
    
    [Header("Leaderboard")]
    [SerializeField] private List<TextMeshProUGUI> leaderboardTexts;
    
    public void OnBackToMainMenu()
    {
        Debug.Log("Loading Main Menu");
        SceneManager.LoadScene(mainMenuScene.name);
    }
    
    public void SetLeaderboard(List<LeaderBoardEntry> leaderboardEntries)
    {
        for (int i = 0; i < Math.Min(leaderboardTexts.Count, leaderboardEntries.Count); i++)
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(leaderboardEntries[i].time).DateTime;
            leaderboardTexts[i].text = (i+1)+".\t "+leaderboardEntries[i].name+"\t\t"+dateTime.ToString("mm:ss.fff");
        }
        SaveSystem.SaveLeaderboard();
    }
    
}
