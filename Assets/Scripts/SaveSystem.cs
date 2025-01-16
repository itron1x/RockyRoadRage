using UnityEngine;
using System.IO;

public class SaveSystem{
    private static SaveData _saveData = new SaveData();
    private static SaveLeaderboardData _saveLeaderboardData;
    private static RaceInfoSystem _raceInfoSystem = RaceInfoSystem.GetInstance();
    
    [System.Serializable]
    public struct SaveData{
       public PlayerSaveData saveData; 
    }

    [System.Serializable]
    public struct SaveLeaderboardData{
        public LeaderboardData leaderboardData;
    }

    public static string FilePath(string filename){
        string saveFile = Application.persistentDataPath + "/" + filename + ".json";
        return saveFile;
    }

    public static void Save(){
        _raceInfoSystem.SaveGlobalCoins(ref _saveData.saveData);
        _raceInfoSystem.SaveCharacterInformation(ref _saveData.saveData);
        _raceInfoSystem.SaveLeaderboardData(ref _saveLeaderboardData.leaderboardData);
        
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
            _raceInfoSystem.LoadLeaderboardData(ref _saveLeaderboardData.leaderboardData);
        }
        catch{
            File.WriteAllText(FilePath("leaderboard"), "");
        }
    }

    public static void SaveLeaderboard(){
        _raceInfoSystem.SaveLeaderboardData(ref _saveLeaderboardData.leaderboardData);
        
        File.WriteAllText(FilePath("leaderboard"), JsonUtility.ToJson(_saveLeaderboardData, true));
    }
}
