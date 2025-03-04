using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class RaceInfoSystem : MonoBehaviour
{
    private static RaceInfoSystem _instance;
    
    private List<InputDevice> _playerInputs = new List<InputDevice>();
    private List<int> _playerCharacter = new List<int>();
    private List<string> _playerName = new List<string>();
    
    private float _raceSpeed = 1;
    private int _activeMapIndex = 0;

    public int ActiveMapIndex
    {
        get => _activeMapIndex;
        set => _activeMapIndex = value;
    }

    private int _globalCoins;
    private Hashtable _characterInformation;
    private RaceControlManager _activeRaceControlManager;

    public RaceControlManager ActiveRaceControlManager
    {
        set => _activeRaceControlManager = value;
    }

    public LeaderBoardEntry LeaderBoardEntry{ get; set; }
    
    List<LeaderBoardEntry> _map0Leaderboard = new List<LeaderBoardEntry>();
    List<LeaderBoardEntry> _map1Leaderboard = new List<LeaderBoardEntry>();
    List<LeaderBoardEntry> _map2Leaderboard = new List<LeaderBoardEntry>();
    
    private float globalVolume;
    public float GlobalVolume { get => globalVolume; set => globalVolume = value; }
    private float musicVolume;
    public float MusicVolume { get => musicVolume; set => musicVolume = value; }
    private float fxVolume;
    public float FxVolume { get => fxVolume; set => fxVolume = value; }
    
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
        _characterInformation.Add("Pebble Pete", true);
        _characterInformation.Add("Cubic Chris", false);
        _characterInformation.Add("Triangle Tam", false);
        _characterInformation.Add("Smooth Sally", false);
        _characterInformation.Add("Lava Larry", false);
        
        _activeMapIndex = 0;
        
        // Load saved data.
        SaveSystem.Load();
        SaveSystem.LoadVolume();
    }

    public static RaceInfoSystem GetInstance()
    {
        return _instance;
    }

     public void SetRaceSpeed(float speed)
     {
         _raceSpeed = speed;
     }

     public float GetRaceSpeed()
     {
         return _raceSpeed;
     }

    public void AddInput(InputDevice playerInput)
    {
        _playerInputs.Add(playerInput);
    }

    public void AddCharacter(int characterId){
        _playerCharacter.Add(characterId);
    }

    public void AddName(string playerName){
        _playerName.Add(playerName);
    }

    public void ResetPlayers(){
        _playerInputs.Clear();
        _playerCharacter.Clear();
        _playerName.Clear();
    }

    public List<InputDevice> GetPlayerInputs()
    {
        return _playerInputs;
    }

    public List<int> GetPlayerCharacter()
    {
        return _playerCharacter;
    }

    public List<string> GetPlayerName(){
        return _playerName;
    }

    public int GetGlobalCoins(){
        return _globalCoins;
    }

    public void AddGlobalCoins(int globalCoins){
        _globalCoins += globalCoins;
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
        data.lavaLarry = (bool)_characterInformation["Lava Larry"];
    }

    public void LoadGlobalCoins(ref PlayerSaveData data){
        _globalCoins = data.coins;
    }

    public void LoadCharacterInformation(ref PlayerSaveData data){
        _characterInformation["Pebble Pete"] = data.pebblePete;
        _characterInformation["Cubic Chris"] = data.cubicChris;
        _characterInformation["Triangle Tam"] = data.triangleTam;
        _characterInformation["Smooth Sally"] = data.smoothSally;
        _characterInformation["Lava Larry"] = data.lavaLarry;
    }

    public void BuyCharacter(string characterName, int price){
        _characterInformation[characterName] = true;
        _globalCoins -= price;
    }

    public bool IsBought(string characterName){
        return (bool)_characterInformation[characterName];
    }

    //0 = Map1, 1 = Map2, 2 = Map3
    public bool AddGlobalLeaderboardEntry(int map, long time, string playerName){
        List<LeaderBoardEntry> leaderboardEntries = new List<LeaderBoardEntry>();
        switch (map){
            case 0:
                leaderboardEntries = _map0Leaderboard;
                break;
            case 1:
                leaderboardEntries = _map1Leaderboard;
                break;
            case 2:
                leaderboardEntries = _map2Leaderboard;
                break;
        }
        
        if (leaderboardEntries.Count <= 10 || leaderboardEntries.Last().time >= time){
            //Create new LeaderBoardEntry and add to Leaderboard
            LeaderBoardEntry = new LeaderBoardEntry(time, playerName);
            leaderboardEntries.Add(LeaderBoardEntry);
            
            //Sort Leaderboard
            leaderboardEntries = leaderboardEntries.OrderBy(player => player.time).ToList();
            if (leaderboardEntries.Count > 10){
                leaderboardEntries.Remove(leaderboardEntries.Last());
            }
            
            switch (map){
                case 0:
                    _map0Leaderboard = leaderboardEntries;
                    break;
                case 1:
                    _map1Leaderboard = leaderboardEntries;
                    break;
                case 2:
                    _map2Leaderboard = leaderboardEntries;
                    break;
            }
            
            return true;
        }
        return false;
    }

    public List<LeaderBoardEntry> GetGlobalLeaderboardData(int mapIndex){
        switch (mapIndex){
            case 0:
                return _map0Leaderboard;
            case 1:
                return _map1Leaderboard;
            case 2:
                return _map2Leaderboard;
        }

        return null;
    }

    public void SaveGlobalLeaderboardData(ref LeaderboardData data){
        data.map0 = _map0Leaderboard;
        data.map1 = _map1Leaderboard;
        data.map2 = _map2Leaderboard;
    }

    public void LoadGlobalLeaderboardData(ref LeaderboardData data){
        _map0Leaderboard = data.map0;
        _map1Leaderboard = data.map1;
        _map2Leaderboard = data.map2;
    }

    public void SaveVolumeData(ref VolumeData data){
        data.globalVolume = globalVolume;
        data.musicVolume = musicVolume;
        data.fxVolume = fxVolume;
    }

    public void LoadVolumeData(ref VolumeData data){
        globalVolume = data.globalVolume;
        musicVolume = data.musicVolume;
        fxVolume = data.fxVolume;
    }

    public void ResetSystem(){
        _playerInputs.Clear();
        _playerCharacter.Clear();
        _activeMapIndex = 0;
        _raceSpeed = 1;
        Time.timeScale = 1 * GetRaceSpeed();
    }

    public static void BackToMainMenu()
    {
        GetInstance().ResetSystem();
        SceneManager.LoadScene("Menu");
    }

    public void RequestPause()
    {
        _activeRaceControlManager.togglePause();
    }
}

[System.Serializable]
public class LeaderBoardEntry{
    public string name;
    public long time;

    public LeaderBoardEntry(long time, string playerName){
        name = playerName;
        this.time = time;
    }
}

[System.Serializable]
public struct PlayerSaveData{
    public int coins;
    public bool pebblePete;
    public bool cubicChris;
    public bool triangleTam;
    public bool smoothSally;
    public bool lavaLarry;
}

[System.Serializable]
public struct LeaderboardData{
    public List<LeaderBoardEntry> map0;
    public List<LeaderBoardEntry> map1;
    public List<LeaderBoardEntry> map2;
}

[System.Serializable]
public struct VolumeData{
    public float globalVolume;
    public float musicVolume;
    public float fxVolume;
}

