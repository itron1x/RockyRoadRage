using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RaceInfoSystem : MonoBehaviour
{
    private static RaceInfoSystem _instance;

    private SceneAsset _racingScene;
    private List<InputDevice> _playerInputs = new List<InputDevice>();
    private List<GameObject> _playerPrefabs = new List<GameObject>();

    private int _globalCoins;
    private Hashtable _characterInformation;
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        // Add characters to Hashtable
        _characterInformation = new Hashtable();
        _characterInformation.Add("Pebble Pete", false);
        _characterInformation.Add("Cubic Chris", false);
        _characterInformation.Add("Triangle Tam", false);
        _characterInformation.Add("Smooth Sally", false);
        
        // Load saved data.
        SaveSystem.Load();
    }

    public static RaceInfoSystem GetInstance()
    {
        return _instance;
    }
    
    // Temp manual save and load shortcut
     // public void Update(){
     //     if (Keyboard.current.enterKey.wasPressedThisFrame){
     //         print("Saving!");
     //         SaveSystem.Save();
     //     }
     //
     //     if (Keyboard.current.spaceKey.wasPressedThisFrame){
     //         print("Loading!");
     //         SaveSystem.Load();
     //     }
     // }

    public void SetRacingScene(SceneAsset racingScene)
    {
        _racingScene = racingScene;
    }

    public void AddPlayer(InputDevice playerInput, GameObject playerPrefab)
    {
        _playerInputs.Add(playerInput);
        _playerPrefabs.Add(playerPrefab);
    }

    public List<InputDevice> GetPlayerInputs()
    {
        return _playerInputs;
    }

    public List<GameObject> GetPlayerPrefabs()
    {
        return _playerPrefabs;
    }

    public void StartRace()
    {
        SceneManager.LoadScene(_racingScene.name);
    }

    public int GetGlobalCoins(){
        return _globalCoins;
    }

    public void SetGlobalCoins(int globalCoins){
        _globalCoins = globalCoins;
    }

    public void SaveGlobalCoins(ref PlayerSaveData data){
        print("Saving Coins...");
        data.coins = _globalCoins;
    }

    public void SaveCharacterInformation(ref PlayerSaveData data){
        print("Saving Character Information...");
        data.pebblePete = (bool)_characterInformation["Pebble Pete"];
        data.cubicChris = (bool)_characterInformation["Cubic Chris"];
        data.triangleTam = (bool)_characterInformation["Triangle Tam"];
        data.smoothSally = (bool)_characterInformation["Smooth Sally"];
    }

    public void LoadGlobalCoins(ref PlayerSaveData data){
        _globalCoins = data.coins;
    }

    public void LoadCharacterInformation(ref PlayerSaveData data){
        _characterInformation["Pebble Pete"] = data.pebblePete;
        _characterInformation["Cubic Chris"] = data.cubicChris;
        _characterInformation["Triangle Tam"] = data.triangleTam;
        _characterInformation["Smooth Sally"] = data.smoothSally;
    }

    public void BuyCharacter(string characterName, int price){
        _characterInformation[characterName] = true;
        _globalCoins -= price;
    }

    public bool IsBought(string characterName){
        return (bool)_characterInformation[characterName];
    }
}

[System.Serializable]
public struct PlayerSaveData{
    public int coins;
    public bool pebblePete;
    public bool cubicChris;
    public bool triangleTam;
    public bool smoothSally;
}