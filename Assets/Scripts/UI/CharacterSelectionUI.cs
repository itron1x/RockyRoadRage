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
    [SerializeField] private ShopManager shopManager; // Reference to ShopManager
    [SerializeField] private List<GameObject> characterPrefabs; 
    
    private List<GameObject> availableCharacters;

    private PlayerData playerData;
    private List<GameObject> allCharacters;
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

        Debug.Log($"IsBought Characters loaded: {availableCharacters.Count}");
        
        playerData = data;
        allCharacters = prefabs;
        onCharacterSelectedCallback = onCharacterSelected;

        playerNameText.text = $"Player {data.PlayerIndex + 1}";
        UpdateCharacterPreview();

        nextButton.onClick.AddListener(NextCharacter);
        backButton.onClick.AddListener(PreviousCharacter);
        confirmButton.onClick.AddListener(ConfirmSelection);
    }
    
    public void StartCharacterSelection(PlayerData playerData, List<GameObject> availablePrefabs, System.Action<PlayerData, GameObject> onCharacterSelectedCallback)
    {
        InitializeCharacters(playerData, availablePrefabs, onCharacterSelectedCallback);

        // activate UI ! 
        gameObject.SetActive(true);

        Debug.Log($"Choose Character for Player {playerData.PlayerIndex + 1} started.");
    }

    private void NextCharacter()
    {
        currentPrefabIndex = (currentPrefabIndex + 1) % allCharacters.Count;
        UpdateCharacterPreview();
    }

    private void PreviousCharacter()
    {
        currentPrefabIndex = (currentPrefabIndex - 1 + allCharacters.Count) % allCharacters.Count;
        UpdateCharacterPreview();
    }

    private void UpdateCharacterPreview()
    {
        characterPreviewImage.sprite = allCharacters[currentPrefabIndex].GetComponent<SpriteRenderer>().sprite;
    }

    private void ConfirmSelection()
    {
        GameObject selectedPrefab = allCharacters[currentPrefabIndex];
        playerData.SelectedPrefab = selectedPrefab; // save selected Character in playerData

        // call Callback to inform CC
        onCharacterSelectedCallback?.Invoke(playerData, selectedPrefab);

        Debug.Log($"Character {selectedPrefab.name} for Player {playerData.PlayerIndex + 1} confirmed.");
    }
    
}