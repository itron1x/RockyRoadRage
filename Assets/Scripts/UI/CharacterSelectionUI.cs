using System.Collections.Generic;
using InputManager;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UI;

public class CharacterSelectionIU : MonoBehaviour
{
    [SerializeField] private TMP_Text playerNameText;
    [SerializeField] private Image characterPreviewImage;
    [SerializeField] private Button nextButton;
    [SerializeField] private Button backButton;
    [SerializeField] private Button confirmButton;
    [SerializeField] private ShopManager shopManager; // Referenz auf den ShopManager
    private List<GameObject> availableCharacters;

    private PlayerData playerData;
    private List<GameObject> availablePrefabs;
    private int currentPrefabIndex = 0;
    private System.Action<PlayerData, GameObject> onCharacterSelectedCallback;

    public void InitializeCharacters(PlayerData data, List<GameObject> prefabs, System.Action<PlayerData, GameObject> onCharacterSelected)
    {
        RaceInfoSystem raceInfoSystem = RaceInfoSystem.GetInstance();
        availableCharacters = new List<GameObject>();

        foreach (GameObject character in shopManager.GetAvailableCharacters())
        {
            CharacterDetails characterDetails = character.GetComponent<CharacterDetails>();
            if (raceInfoSystem.IsBought(characterDetails.GetCharacterName()))
            {
                availableCharacters.Add(character);
            }
        }

        Debug.Log($"Gekaufte Charaktere geladen: {availableCharacters.Count}");
        
        playerData = data;
        availablePrefabs = prefabs;
        onCharacterSelectedCallback = onCharacterSelected;

        playerNameText.text = $"Spieler {data.PlayerIndex + 1}";
        UpdateCharacterPreview();

        nextButton.onClick.AddListener(NextCharacter);
        backButton.onClick.AddListener(PreviousCharacter);
        confirmButton.onClick.AddListener(ConfirmSelection);
    }
    
    public void StartCharacterSelection(PlayerData playerData, List<GameObject> availablePrefabs, System.Action<PlayerData, GameObject> onCharacterSelectedCallback)
    {
        // Starte die Auswahl mit den übergebenen Daten
        InitializeCharacters(playerData, availablePrefabs, onCharacterSelectedCallback);

        // Aktiviert die UI, falls sie deaktiviert war
        gameObject.SetActive(true);

        Debug.Log($"Charakterauswahl für Spieler {playerData.PlayerIndex + 1} gestartet.");
    }

    private void NextCharacter()
    {
        currentPrefabIndex = (currentPrefabIndex + 1) % availablePrefabs.Count;
        UpdateCharacterPreview();
    }

    private void PreviousCharacter()
    {
        currentPrefabIndex = (currentPrefabIndex - 1 + availablePrefabs.Count) % availablePrefabs.Count;
        UpdateCharacterPreview();
    }

    private void UpdateCharacterPreview()
    {
        characterPreviewImage.sprite = availablePrefabs[currentPrefabIndex].GetComponent<SpriteRenderer>().sprite;
    }

    private void ConfirmSelection()
    {
        GameObject selectedPrefab = availablePrefabs[currentPrefabIndex];
        playerData.SelectedPrefab = selectedPrefab; // Speichert das ausgewählte Prefab in PlayerData

        // Callback aufrufen, um CC zu informieren
        onCharacterSelectedCallback?.Invoke(playerData, selectedPrefab);

        Debug.Log($"Charakter {selectedPrefab.name} für Spieler {playerData.PlayerIndex + 1} bestätigt.");
    }
    
}