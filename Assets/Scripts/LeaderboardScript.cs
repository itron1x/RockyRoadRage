using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using TMPro;
using UnityEditor;
using UnityEditor.U2D.Sprites;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;

public class LeaderboardScript : MonoBehaviour
{
    
    [Header("Leaderboard")]
    [SerializeField] private TextMeshProUGUI leaderboardHeader;
    [SerializeField] private List<TextMeshProUGUI> leaderboardTexts;
    
    private List<LeaderBoardEntry> _raceleaderboard = new List<LeaderBoardEntry>();
    
    public void OnBackToMainMenu()
    {
        Debug.Log("Loading Main Menu");
        RaceInfoSystem.BackToMainMenu();
    }
    
    public void SetRaceLeaderboard(List<LeaderBoardEntry> leaderboardEntries)
    {
        _raceleaderboard = leaderboardEntries;
    }

    public void ShowRaceLeaderboard()
    {
        ClearLeaderboard();
        leaderboardHeader.text = "Race Leaderboard";
        for (int i = 0; i < Math.Min(leaderboardTexts.Count, _raceleaderboard.Count); i++)
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(_raceleaderboard[i].time).DateTime;
            leaderboardTexts[i].text = (i + 1) + ".\t " + _raceleaderboard[i].name + "\t\t" + dateTime.ToString("mm:ss.fff");
        }
        Invoke("ShowGlobalLeaderboard", 10f);
    }

    public void ShowGlobalLeaderboard()
    {
        ClearLeaderboard();
        RaceInfoSystem infoSystem = RaceInfoSystem.GetInstance();
        List<LeaderBoardEntry> leaderboardEntries = infoSystem.GetGlobalLeaderboardData(infoSystem.ActiveMapIndex);
        leaderboardHeader.text = "Map Leaderboard";
        for (int i = 0; i < Math.Min(leaderboardTexts.Count, leaderboardEntries.Count); i++)
        {
            DateTime dateTime = DateTimeOffset.FromUnixTimeMilliseconds(leaderboardEntries[i].time).DateTime;
            leaderboardTexts[i].text = (i + 1) + ".\t " + leaderboardEntries[i].name + "\t\t" + dateTime.ToString("mm:ss.fff");
        }
        Invoke("ShowRaceLeaderboard", 10f);
    }

    private void ClearLeaderboard()
    {
        for (int i = 0; i < leaderboardTexts.Count; i++)leaderboardTexts[i].text = "";
    }
}
