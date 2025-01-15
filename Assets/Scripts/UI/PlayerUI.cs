using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InputManager;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Button nextPlayerButton;
    [SerializeField] private Button backPlayerButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private CC characterController; // Referenz zur CC-Klasse
    private List<InputDevice> devices;
    private int currentDeviceIndex = 0;

    private List<InputManager.PlayerData> players;
    private List<InputDevice> detectedDevices = new List<InputDevice>();

    void Start()
    {
        devices = characterController.GetDetectedDevices(); // Geräte abrufen
        if (devices == null || devices.Count == 0)
        {
            Debug.LogError("Keine Geräte erkannt!");
            return;
        }

        UpdateDeviceDisplay();
    }

    private void UpdateDeviceDisplay()
    {
        if (devices.Count == 0)
        {
            playerNameText.text = "Keine Geräte verfügbar";
            return;
        }

        InputDevice currentDevice = devices[currentDeviceIndex];
        playerNameText.text = $"Gerät: {currentDevice.displayName}";
    }

    public void NextDevice()
    {
        currentDeviceIndex = (currentDeviceIndex + 1) % devices.Count;
        UpdateDeviceDisplay();
    }

    public void PreviousDevice()
    {
        currentDeviceIndex = (currentDeviceIndex - 1 + devices.Count) % devices.Count;
        UpdateDeviceDisplay();
    }

    public void ConfirmDevice()
    {
        InputDevice selectedDevice = devices[currentDeviceIndex];
        characterController.CreatePlayer(selectedDevice); // Spieler mit dem ausgewählten Gerät erstellen
        Debug.Log($"Gerät {selectedDevice.displayName} wurde als Spieler hinzugefügt.");
    }

    public void ConfirmSelection()
    {
        List<PlayerData> players = characterController.GetPlayerDataList();
        if (players != null && players.Count > 0)
        {
            PlayerData
                currentPlayer =
                    players[currentDeviceIndex]; // Verwende den Index, falls das Gerät mit einem Spieler verbunden ist
            Debug.Log(
                $"Spieler {currentPlayer.PlayerIndex + 1} mit Steuerung {currentPlayer.ControlScheme} bestätigt.");
        }
        else
        {
            Debug.LogError("Keine Spieler-Daten verfügbar!");
        }
    }
}
