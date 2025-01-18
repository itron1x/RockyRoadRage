using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class ChooseCharacterController : MonoBehaviour
{
    [SerializeField] private Button nextButton;
    [SerializeField] private List<InputHandler> player;
    
    private List<int> _deviceIds;

    void Awake(){
        _deviceIds = new List<int>();
        _deviceIds.Add(Keyboard.current.deviceId);
        foreach (Gamepad controller in Gamepad.all){
            _deviceIds.Add(controller.deviceId);
        }
    }

    void Start(){
        nextButton.interactable = false;
    }
    void FixedUpdate()
    {
        if (EnableNext()){
            print("All players have chosen");
            nextButton.interactable = true;
        }
    }

    private bool EnableNext(){
        if (player == null) return false;
        foreach (InputHandler chooseCharacter in player){
            if (chooseCharacter.GetChooseCharacter().GetIsSelected() == false){
                return false;
            }
        }
        return true;
    }

    public void ResetPlayers(){
        foreach (InputHandler chooseCharacter in player){
            chooseCharacter.ResetInputs();
        } 
        RaceInfoSystem.GetInstance().ResetPlayers();
        ResetDeviceList();
    }

    public void RemoveDevice(int deviceId){
        _deviceIds.Remove(deviceId);
    }

    public bool DeviceUsed(int deviceId){
        return _deviceIds.Contains(deviceId);
    }

    public void ResetDeviceList(){
        _deviceIds?.Clear();
        
        _deviceIds?.Add(Keyboard.current.deviceId);
        foreach (Gamepad controller in Gamepad.all){
            _deviceIds?.Add(controller.deviceId);
        }
    }

    public Button GetNextButton(){
        return nextButton;
    }

    public Button GetAddInputButton(){
        return player[0].GetButton();
    }
}
