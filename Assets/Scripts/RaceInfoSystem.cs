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
    
    private void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public static RaceInfoSystem GetInstance()
    {
        return _instance;
    }
    
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
    
}
