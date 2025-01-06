using System;
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
    [SerializeField] private TextMeshProUGUI firstPlayerName;
    [SerializeField] private TextMeshProUGUI firstPlayerTime;
    [SerializeField] private TextMeshProUGUI firstPlayerCoins;
    [SerializeField] private TextMeshProUGUI secondPlayerName;
    [SerializeField] private TextMeshProUGUI secondPlayerTime;
    [SerializeField] private TextMeshProUGUI secondPlayerCoins;
    [SerializeField] private TextMeshProUGUI thirdPlayerName;
    [SerializeField] private TextMeshProUGUI thirdPlayerTime;
    [SerializeField] private TextMeshProUGUI thirdPlayerCoins;
    [SerializeField] private TextMeshProUGUI fourthPlayerName;
    [SerializeField] private TextMeshProUGUI fourthPlayerTime;
    [SerializeField] private TextMeshProUGUI fourthPlayerCoins;
    [SerializeField] private TextMeshProUGUI fifthPlayerName;
    [SerializeField] private TextMeshProUGUI fifthPlayerTime;
    [SerializeField] private TextMeshProUGUI fifthPlayerCoins;
    [SerializeField] private TextMeshProUGUI sixthPlayerName;
    [SerializeField] private TextMeshProUGUI sixthPlayerTime;
    [SerializeField] private TextMeshProUGUI sixthPlayerCoins;
    [SerializeField] private TextMeshProUGUI seventhPlayerName;
    [SerializeField] private TextMeshProUGUI seventhPlayerTime;
    [SerializeField] private TextMeshProUGUI seventhPlayerCoins;
    [SerializeField] private TextMeshProUGUI eighthPlayerName;
    [SerializeField] private TextMeshProUGUI eighthPlayerTime;
    [SerializeField] private TextMeshProUGUI eighthPlayerCoins;

    public void OnBackToMainMenu()
    {
        Debug.Log("Loading Main Menu");
        SceneManager.LoadScene(mainMenuScene.name);
    }    
    public void SetPlayer(int placement, string playerName, string playerTime, string playerCoins)
    {
        switch (placement)
        {
            case 1:
                SetFirstPlayer(playerName, playerTime, playerCoins);
                break;
            case 2:
                SetSecondPlayer(playerName, playerTime, playerCoins);
                break;
            case 3:
                SetThirdPlayer(playerName, playerTime, playerCoins);
                break;
            case 4:
                SetFourthPlayer(playerName, playerTime, playerCoins);
                break;
            case 5:
                SetFifthPlayer(playerName, playerTime, playerCoins);
                break;
            case 6:
                SetSixthPlayer(playerName, playerTime, playerCoins);
                break;
            case 7:
                SetSeventhPlayer(playerName, playerTime, playerCoins);
                break;
            case 8:
                SetEighthPlayer(playerName, playerTime, playerCoins);
                break;
        }
    }
    
    private void SetFirstPlayer(string playerName, string playerTime, string playerCoins)
    {
        firstPlayerName.text = playerName;
        firstPlayerTime.text = playerTime;
        firstPlayerCoins.text = playerCoins;
    }

    private void SetSecondPlayer(string playerName, string playerTime, string playerCoins)
    {
        secondPlayerName.text = playerName;
        secondPlayerTime.text = playerTime;
        secondPlayerCoins.text = playerCoins;
    }

    private void SetThirdPlayer(string playerName, string playerTime, string playerCoins)
    {
        thirdPlayerName.text = playerName;
        thirdPlayerTime.text = playerTime;
        thirdPlayerCoins.text = playerCoins;
    }

    private void SetFourthPlayer(string playerName, string playerTime, string playerCoins)
    {
        fourthPlayerName.text = playerName;
        fourthPlayerTime.text = playerTime;
        fourthPlayerCoins.text = playerCoins;
    }

    private void SetFifthPlayer(string playerName, string playerTime, string playerCoins)
    {
        fifthPlayerName.text = playerName;
        fifthPlayerTime.text = playerTime;
        fifthPlayerCoins.text = playerCoins;
    }

    private void SetSixthPlayer(string playerName, string playerTime, string playerCoins)
    {
        sixthPlayerName.text = playerName;
        sixthPlayerTime.text = playerTime;
        sixthPlayerCoins.text = playerCoins;
    }

    private void SetSeventhPlayer(string playerName, string playerTime, string playerCoins)
    {
        seventhPlayerName.text = playerName;
        seventhPlayerTime.text = playerTime;
        seventhPlayerCoins.text = playerCoins;
    }

    private void SetEighthPlayer(string playerName, string playerTime, string playerCoins)
    {
        eighthPlayerName.text = playerName;
        eighthPlayerTime.text = playerTime;
        eighthPlayerCoins.text = playerCoins;
    }

}
