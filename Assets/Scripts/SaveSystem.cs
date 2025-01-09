using UnityEngine;
using System.IO;
using Unity.VisualScripting.FullSerializer;

public class SaveSystem{
    private static SaveData _saveData = new SaveData();
    
    [System.Serializable]
    public struct SaveData{
       public PlayerSaveData saveData; 
    }

    public static string FilePath(){
        string saveFile = Application.persistentDataPath + "/save" + ".json";
        return saveFile;
    }

    public static void Save(){
        RaceInfoSystem.GetInstance().SaveGlobalCoins(ref _saveData.saveData);
        RaceInfoSystem.GetInstance().SaveCharacterInformation(ref _saveData.saveData);
        
        //Pretty Print to false make it not human-readable
        File.WriteAllText(FilePath(), JsonUtility.ToJson(_saveData, true));
    }

    public static void Load(){
        string savedData = File.ReadAllText(FilePath());
        _saveData = JsonUtility.FromJson<SaveData>(savedData);
        
        RaceInfoSystem.GetInstance().LoadGlobalCoins(ref _saveData.saveData);
        RaceInfoSystem.GetInstance().LoadCharacterInformation(ref _saveData.saveData);
    }
}
