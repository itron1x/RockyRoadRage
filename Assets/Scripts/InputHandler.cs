using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class InputHandler : MonoBehaviour{
    [SerializeField] private Button button;
    [SerializeField] private TextMeshProUGUI infoText;
    
    [SerializeField] private GameObject selectCharacter;
    [SerializeField] private ChooseCharacter chooseCharacter;

    private bool _listen;
    private bool _executed;
    private RaceInfoSystem _raceInfoSystem;
    private int _playerIndex;
    private ChooseCharacterController _chooseCharacterController;

    void Awake(){
        _listen = false;
        button.enabled = true;
        _raceInfoSystem = RaceInfoSystem.GetInstance();
        _chooseCharacterController = gameObject.GetComponentInParent<ChooseCharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_listen){
            if (Keyboard.current != null && _chooseCharacterController.DeviceUsed(Keyboard.current.deviceId) && Keyboard.current.enterKey.wasPressedThisFrame){
                if (_executed == false){
                    print("Keyboard");
                    _raceInfoSystem.AddInput(Keyboard.current);
                    _chooseCharacterController.RemoveDevice(Keyboard.current.deviceId);
                    _executed = true;
                    PlayerAdded();
                }
            }
            else{
                foreach (var controller in Gamepad.all){
                    if (controller != null && _chooseCharacterController.DeviceUsed(controller.deviceId) && controller.startButton.wasPressedThisFrame){
                        if (_executed == false){
                            print("Controller");
                            _raceInfoSystem.AddInput(controller);
                            _chooseCharacterController.RemoveDevice(controller.deviceId);
                            _executed = true;
                            PlayerAdded();
                        }
                    }
                }
            }
        }
        
    }

    public void AddInput(){
        infoText.enabled = true;
        _listen = true;

        chooseCharacter.UpdateBoughtCharacters();
        chooseCharacter.UpdateArrows();
    }
    
    private void PlayerAdded(){
        _listen = false;
        gameObject.SetActive(false);
        selectCharacter.SetActive(true);
    }

    public void PrintPlayers(){
        List<InputDevice> playerInputs = _raceInfoSystem.GetPlayerInputs();
        List<int> playerCharacter = _raceInfoSystem.GetPlayerCharacter();

        if (playerInputs != null){
            for (int i = 0; i < playerInputs.Count; i++){
                print("Input: " + playerInputs[i].name + ", Character " + playerCharacter?[i]);
            }
        }
    }

    public ChooseCharacter GetChooseCharacter(){
        return chooseCharacter;
    }

    public void ResetInputs(){
        gameObject.SetActive(true);
        selectCharacter.SetActive(false);
        selectCharacter.GetComponent<ChooseCharacter>().ResetCharacter();
        infoText.enabled = false;
        button.enabled = true;
        button.interactable = true;
        _executed = false;
    }

    public Button GetButton(){
        return button;
    }
}
