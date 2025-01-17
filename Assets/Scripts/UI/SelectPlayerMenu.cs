using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using TMPro;

public class SelectPlayerMenu : MonoBehaviour
{
    public TMP_Text displayPlayer; // Text, um die aktuelle Auswahl anzuzeigen
    public Button nextButton;
    public Button backButton;
    
    private RaceInfoSystem raceInfoSystem;
    private List<InputDevice> devices = new List<InputDevice>();

    private int currentIndex = 0;

    void Start()
    {
        Debug.Log("Start-Methode von SelectPlayerMenu aufgerufen.");

        // Hole die Liste der Input-Devices
        // devices = new List<InputDevice>(InputSystem.devices);
        
        // Hole die RaceInfoSystem-Instanz
        raceInfoSystem = RaceInfoSystem.GetInstance();
        if (raceInfoSystem == null)
        {
            Debug.LogError("RaceInfoSystem ist nicht verfügbar!");
            displayPlayer.text = "RaceInfoSystem nicht verfügbar.";
            return;
        }
        Debug.Log("RaceInfoSystem erfolgreich gefunden.");
        
        // Hole die Liste der Input-Devices aus RaceInfoSystem
        devices = raceInfoSystem.GetPlayerInputs();
        Debug.Log($"Geräte in RaceInfoSystem gefunden: {devices.Count}");
        
        // Prüfe, ob Geräte gefunden wurden
        if (devices.Count == 0)
        {
            Debug.LogWarning("RaceInfoSystem erfolgreich initializer aber keine Eingabegeräte gefunden. Füge Testgerät hinzu.");
            var testDevice = InputSystem.devices.Count > 0 ? InputSystem.devices[0] : null;
            if (testDevice != null)
            {
                raceInfoSystem.AddInput(testDevice); // Spieler ohne Prefab hinzufügen
                devices = raceInfoSystem.GetPlayerInputs();
                Debug.Log($"Testgerät hinzugefügt: {testDevice.displayName}");
            }
        }
        
        if (devices.Count == 0)
        {
            Debug.LogError("Auch nach Hinzufügen eines Testgeräts keine Geräte verfügbar.");
            displayPlayer.text = "Keine Geräte verfügbar.";
            return;
        }

        Debug.Log($"Gefundene Geräte: {devices.Count}");

        // Buttons mit Funktionen verknüpfen
        nextButton.onClick.AddListener(NextDevice);
        backButton.onClick.AddListener(PreviousDevice);

        // Zeige das erste Gerät an
        UpdateUI();
    }

    void UpdateUI()
    {
        Debug.Log($"UpdateUI aufgerufen. Aktueller Index: {currentIndex}, Anzahl Geräte: {devices.Count}");

        // Name des aktuellen Geräts anzeigen
        if (devices.Count > 0 && currentIndex < devices.Count)
        {
            string deviceName = devices[currentIndex].displayName;
            Debug.Log($"Zeige Gerät: {deviceName}");
            displayPlayer.text = $"Gerät: {deviceName}";
            Debug.Log($"Angezeigter Text: {displayPlayer.text}");
        }
        else
        {
            Debug.LogError("Ungültiger Index oder keine Geräte verfügbar.");
            displayPlayer.text = "Keine Geräte verfügbar.";
            Debug.Log($"Angezeigter Text: {displayPlayer.text}");
        }
    }

    public void NextDevice()
    {
        Debug.Log($"NextDevice aufgerufen. Aktueller Index: {currentIndex}");
        if (currentIndex < devices.Count - 1)
        {
            currentIndex++;
            Debug.Log($"Index erhöht: {currentIndex}");
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Nächstes Gerät nicht verfügbar. Am Ende der Liste.");
        }
    }

    public void PreviousDevice()
    {
        Debug.Log($"PreviousDevice aufgerufen. Aktueller Index: {currentIndex}");
        if (currentIndex > 0)
        {
            currentIndex--;
            Debug.Log($"Index verringert: {currentIndex}");
            UpdateUI();
        }
        else
        {
            Debug.LogWarning("Vorheriges Gerät nicht verfügbar. Am Anfang der Liste.");
        }
    }
}
