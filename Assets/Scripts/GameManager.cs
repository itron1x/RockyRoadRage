using UI;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    
    private static RaceInfoSystem _raceInfoSystem;
    private static ShopManager _shopManager;

    
    void Awake(){
        if (_instance != null){
            Destroy(gameObject);
            return;
        }
        _instance = this;
        DontDestroyOnLoad(gameObject);
        
        _raceInfoSystem = RaceInfoSystem.GetInstance();
    }

    public static GameManager GetInstance(){
        return _instance;
    }

    public RaceInfoSystem GetRaceInfoSystem(){
        return _raceInfoSystem;
    }
  
}
