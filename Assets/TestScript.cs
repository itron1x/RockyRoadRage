using Player;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
public class TestScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private Button button;
    private bool _listen;
    private bool _executed;
    private int _playerIndex;
    void Awake(){
        text.enabled = false;
    }
    // Update is called once per frame
    void Update(){
        if (_listen){
            if (Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame){
                PlayerInput player = PlayerInputManager.instance.JoinPlayer(_playerIndex, controlScheme: null , pairWithDevice: Keyboard.current);
                if (_executed == false){
                    player.gameObject.GetComponentInParent<PrefabController>().SetCharacter("Triangle Tam", "Triangle Tam");
                    _executed = true;
                }
                PlayerAdded();
            }
            else{
                foreach (var controller in Gamepad.all){
                    if (controller != null && controller.startButton.wasPressedThisFrame){
                        PlayerInputManager.instance.JoinPlayer(_playerIndex, controlScheme: null, pairWithDevice: controller);
                        PlayerAdded();
                    }
                }
            }
        }
    }
    public void AddPlayer(){
        text.enabled = true;
        _listen = true;
    }
    private void PlayerAdded(){
        _playerIndex++;
        _listen = false;
        button.interactable = true;
        text.enabled = false;
    }
}