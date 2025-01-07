using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;

public class InfoTransmitTest : MonoBehaviour
{

    public GameObject playerPrefab;
    public SceneAsset racingScene;
    
    public void TestSceneInfoTransmit()
    {
        var raceInfoSystem = RaceInfoSystem.GetInstance();
        var inputDevice = InputSystem.devices[0];
        
        raceInfoSystem.AddPlayer(inputDevice, playerPrefab);
        raceInfoSystem.SetRacingScene(racingScene);
        
        raceInfoSystem.StartRace();
    }
}
