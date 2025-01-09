using System;
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
    
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        SaveSystem.Load();
    }

    public static RaceInfoSystem GetInstance()
    {
        return _instance;
    }
    
    //Temp manual save and load shortcut
    // public void Update(){
    //     if (Keyboard.current.enterKey.wasPressedThisFrame){
    //         SaveSystem.Save();
    //     }
    //
    //     if (Keyboard.current.spaceKey.wasPressedThisFrame){
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
        data.coins = _globalCoins;
    }

    public void LoadGlobalCoins(ref PlayerSaveData data){
        _globalCoins = data.coins;
    }
    
}

[System.Serializable]
public struct PlayerSaveData{
    public int coins;
}
