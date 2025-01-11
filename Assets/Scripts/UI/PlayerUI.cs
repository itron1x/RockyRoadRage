using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InputManager;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Button nextPlayerButton;
    [SerializeField] private Button backPlayerButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private InputManager.CharacterController characterController;
    
    private int currentPlayerIndex = 0;
    private List<InputManager.PlayerData> players;
    
    void Start()
    {
        
        /*
        if (characterController == null)
        {
            Debug.LogError("Kein CharacterController gefunden!");
            return;
        }
        */
        
        // Holt die Liste der Spieler
        players = characterController.GetPlayerDataList();

        if (players == null || players.Count == 0)
        {
            Debug.LogError("Keine Spieler gefunden!");
            playerNameText.text = "Keine Spieler verfügbar";
            return;
        }

        InitializeUI();
    }

    private void InitializeUI()
    {
        UpdateUI();

        nextPlayerButton.onClick.AddListener(NextPlayer);
        backPlayerButton.onClick.AddListener(PreviousPlayer);
        confirmButton.onClick.AddListener(ConfirmSelection);
    }

    private void UpdateUI()
    {
        if (players.Count == 0)
        {
            playerNameText.text = "Keine Spieler verfügbar";
            return;
        }

        PlayerData currentPlayer = players[currentPlayerIndex];
        playerNameText.text = $"Spieler {currentPlayer.PlayerIndex + 1} - Steuerung: {currentPlayer.ControlScheme}";
    }

    public void NextPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex + 1) % players.Count;
        UpdateUI();
    }

    public void PreviousPlayer()
    {
        currentPlayerIndex = (currentPlayerIndex - 1 + players.Count) % players.Count;
        UpdateUI();
    }

    public void ConfirmSelection()
    {
        PlayerData currentPlayer = players[currentPlayerIndex];
        Debug.Log($"Spieler {currentPlayer.PlayerIndex + 1} mit Steuerung {currentPlayer.ControlScheme} bestätigt.");
    }
}
