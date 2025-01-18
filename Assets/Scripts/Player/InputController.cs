using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Users;
using UnityEngine.UI;

namespace Player{
    public class InputController : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI playerText;
    
        private Func<bool> _eventHappened;
    
        private int _globalCount;
        // Start is called once before the first execution of Update after the MonoBehaviour is created
        void Awake()
        {
            foreach (var inputDevice in InputUser.GetUnpairedInputDevices()){
                print(inputDevice);
            }
        }

        void Update(){
            print("Printing players:");
            foreach (var player in PlayerInput.all){
                print(player);
            }
        }
    
        public void AddDevice(){
            PlayerInputManager.instance.EnableJoining();
            // StartCoroutine(AddNewDevice());
            InputSystem.onEvent +=
                (eventPtr, inputDevice) => {
                    print("Test");
                    PlayerInputManager.instance.JoinPlayer(_globalCount, _globalCount,null,inputDevice);
                    PlayerInputManager.instance.DisableJoining();
                    playerText.text = "New device:\n ID: " + inputDevice.deviceId + "\n Name: " + inputDevice.name;
                    // _eventHappened.Invoke();
                    // GetComponentInParent<Button>().interactable = true;
                };
        
            // print(PlayerInput.all.Count);
            // foreach (var inputDevice in PlayerInput.all){
            //     print(inputDevice.devices[0].deviceId);
            // }
            // foreach (var device in InputSystem.devices){
            //     PlayerInputManager.instance.playerPrefab = prefabs[(int)_playerSpawns[_globalCount]];
            //     PlayerInputManager.instance.JoinPlayer();
            //     
            // }
        }

        IEnumerator AddNewDevice(){
            yield return new WaitUntil(_eventHappened);
            print("Waiting for players...");
            InputSystem.onEvent +=
                (eventPtr, inputDevice) => {
                    print("Test");
                    PlayerInputManager.instance.JoinPlayer(_globalCount, _globalCount, "test", inputDevice);
                    PlayerInputManager.instance.DisableJoining();
                    playerText.text = "New device:\n ID: " + inputDevice.deviceId + "\n Name: " + inputDevice.name;
                    _eventHappened.Invoke();
                    GetComponent<Button>().interactable = true;
                };
        }
    }
}
