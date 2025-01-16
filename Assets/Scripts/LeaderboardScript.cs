using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LeaderboardScript : MonoBehaviour
{
    [Header("Buttons")]
    [SerializeField] private SceneAsset mainMenuScene;
    
    [Header("Leaderboard")]
    [SerializeField] private List<TextMeshProUGUI> leaderboardTexts;
    public int mapIndex = 0;
    
    public void OnBackToMainMenu()
    {
        Debug.Log("Loading Main Menu");
        SceneManager.LoadScene(mainMenuScene.name);
    }
    
    public void RefreshLeaderboard()
    {
        RaceInfoSystem raceInfoSystem = RaceInfoSystem.GetInstance();
        List<LeaderBoardEntry> leaderboardEntries = raceInfoSystem.GetLeaderboardData()[mapIndex].Leaderboard;
        for (int i = 0; i < Math.Min(leaderboardTexts.Count, leaderboardEntries.Count); i++)
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(leaderboardEntries[i].Time).DateTime;
            leaderboardTexts[i].text = (i+1)+". - "+leaderboardEntries[i].Name+"\t\t"+dateTime.ToString("mm:ss.fff");
        }
        SaveSystem.SaveLeaderboard();
    }
    
}
