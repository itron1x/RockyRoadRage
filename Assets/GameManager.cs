using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

public class GameManager : MonoBehaviour{
    [SerializeField]
    private PlayerInputManager playerInputManager;
    
    [SerializeField]
    private TextMeshProUGUI coinText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    public PlayerInputManager GetPlayerInputManager(){
        return playerInputManager;
    }

    public TextMeshProUGUI GetCoinText(){
        return coinText;
    }

}
