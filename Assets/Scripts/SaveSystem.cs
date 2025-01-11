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

    public static void Save()
    {
        try
        {
            RaceInfoSystem.GetInstance().SaveGlobalCoins(ref _saveData.saveData);
            RaceInfoSystem.GetInstance().SaveCharacterInformation(ref _saveData.saveData);

            string jsonData = JsonUtility.ToJson(_saveData, true); // Pretty Print für Lesbarkeit
            File.WriteAllText(FilePath(), jsonData);

            Debug.Log("Speicherdatei erfolgreich erstellt/aktualisiert.");
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Fehler beim Speichern der Datei: {ex.Message}");
        }
    }

    public static void Load()
    {
        string filePath = FilePath();

        if (!File.Exists(filePath))
        {
            Debug.LogWarning($"Speicherdatei nicht gefunden: {filePath}. Es werden Standardwerte verwendet.");
            return;
        }

        try
        {
            string savedData = File.ReadAllText(filePath);
            _saveData = JsonUtility.FromJson<SaveData>(savedData);

            RaceInfoSystem.GetInstance().LoadGlobalCoins(ref _saveData.saveData);
            RaceInfoSystem.GetInstance().LoadCharacterInformation(ref _saveData.saveData);
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"Fehler beim Laden der Speicherdatei: {ex.Message}");
        }
    }
}
