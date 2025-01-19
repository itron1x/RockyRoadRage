using UnityEngine;
using System.IO;

public class SaveSystem{
    private static SaveData _saveData = new SaveData();
    private static SaveLeaderboardData _saveLeaderboardData;
    private static SaveVolumeData _saveVolumeData;
    private static RaceInfoSystem _raceInfoSystem = RaceInfoSystem.GetInstance();
    
    [System.Serializable]
    public struct SaveData{
       public PlayerSaveData saveData; 
    }

    [System.Serializable]
    public struct SaveLeaderboardData{
        public LeaderboardData leaderboardData;
    }

    [System.Serializable]
    public struct SaveVolumeData{
        public VolumeData volumeData;
    }

    public static string FilePath(string filename){
        string saveFile = Application.persistentDataPath + "/" + filename + ".json";
        return saveFile;
    }

    public static void Save(){
        // Coins and character
        _raceInfoSystem.SaveGlobalCoins(ref _saveData.saveData);
        _raceInfoSystem.SaveCharacterInformation(ref _saveData.saveData);
        
        //Leaderboard
        _raceInfoSystem.SaveGlobalLeaderboardData(ref _saveLeaderboardData.leaderboardData);

        
        //Pretty Print to false make it not human-readable
        File.WriteAllText(FilePath("coins"), JsonUtility.ToJson(_saveData, true));
        File.WriteAllText(FilePath("leaderboard"), JsonUtility.ToJson(_saveLeaderboardData, true));
    }

    public static void Load(){
        try{
            string savedData = File.ReadAllText(FilePath("coins"));
            _saveData = JsonUtility.FromJson<SaveData>(savedData);

            _raceInfoSystem.LoadGlobalCoins(ref _saveData.saveData);
            _raceInfoSystem.LoadCharacterInformation(ref _saveData.saveData);
        }
        catch{
            File.WriteAllText(FilePath("coins"), "");
        }
        
        try{
            string savedLeaderboardData = File.ReadAllText(FilePath("leaderboard"));
            _saveLeaderboardData = JsonUtility.FromJson<SaveLeaderboardData>(savedLeaderboardData);
            _raceInfoSystem.LoadGlobalLeaderboardData(ref _saveLeaderboardData.leaderboardData);
        }
        catch{
            File.WriteAllText(FilePath("leaderboard"), "");
        }

    }

    public static void SaveLeaderboard(){
        _raceInfoSystem.SaveGlobalLeaderboardData(ref _saveLeaderboardData.leaderboardData);
        
        File.WriteAllText(FilePath("leaderboard"), JsonUtility.ToJson(_saveLeaderboardData, true));
    }

    public static void SaveVolume(){
        //Volume
        _raceInfoSystem.SaveVolumeData(ref _saveVolumeData.volumeData);
        
        File.WriteAllText(FilePath("volume"), JsonUtility.ToJson(_saveVolumeData, true));
    }

    public static void LoadVolume(){
        try{
            string savedVolumeData = File.ReadAllText(FilePath("volume"));
            _saveVolumeData = JsonUtility.FromJson<SaveVolumeData>(savedVolumeData);
            _raceInfoSystem.LoadVolumeData(ref _saveVolumeData.volumeData);
        }
        catch{
            File.WriteAllText(FilePath("volume"), "");
        }
    }
}
