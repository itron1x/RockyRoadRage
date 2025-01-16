using System.Collections.Generic;
using Collectables;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace Player_2._0{
    public class PrefabController : MonoBehaviour{
        private GameObject _activeCharacter;
    
        [SerializeField] private List<GameObject> characters;
        
        [Header("Controller")]
        [SerializeField] private PlayerController2 playerController;
        
        [Header("Camera")]
        [SerializeField] private LookFollower2 lookFollower;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        [Header("Overlays")]
        [SerializeField] private GameObject overlays;
        [SerializeField] private GameObject eyes;
        
        [Header("Collectables")]
        [SerializeField] private CoinController coinController;

        private int _playerIndex;
        
        void Awake(){
            _activeCharacter = GetCharacterByName("Pebble Pete");
            
            overlays.layer = LayerMask.NameToLayer("Player" + _playerIndex);
           
            //TODO: add get device
            foreach (var controller in Gamepad.all){
                print(controller.deviceId);
            }
        }

        void Update(){
            if (Keyboard.current.digit1Key.wasPressedThisFrame){
                SetCharacter("Pebble Pete");
            }    
            else if (Keyboard.current.digit2Key.wasPressedThisFrame){
                SetCharacter("Triangle Tam");
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame){
                SetCharacter("Cubic Chris");
            }
            else if (Keyboard.current.digit4Key.wasPressedThisFrame){
                SetCharacter("Smooth Sally");
            }

            //TODO: add get device
            if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame){
                print("Keyboard was pressed with ID: " + Keyboard.current.deviceId);
            }
            else{
                foreach (var controller in Gamepad.all){
                    if (controller != null && controller.rightTrigger.wasPressedThisFrame){
                        print("Controller was pressed with ID: " + controller.deviceId);
                    }
                }
            }
        }
    
        public GameObject GetCharacter(){
            return _activeCharacter;
        }

        public GameObject GetCharacterByName(string characterName){
            switch (characterName){
                case "Pebble Pete":
                    return characters[0];
                case "Cubic Chris":
                    return characters[1];
                case "Triangle Tam":
                    return characters[2];
                case "Smooth Sally":
                    return characters[3];
            }
            throw new System.Exception("Unknown character");
        }

        public void SetCharacter(string characterName){
            // Disable current character
            _activeCharacter.SetActive(false);
        
            //Update character to new one.
            _activeCharacter = GetCharacterByName(characterName);
            _activeCharacter.SetActive(true);
            
            PlayerCharacteristics characteristics = _activeCharacter.GetComponent<PlayerCharacteristics>();
            
            //Update PlayerController
            // playerController.SetRigidbody(_activeCharacter.GetComponent<Rigidbody>());
            // playerController.SetGroundDetection(_activeCharacter.GetComponentInChildren<GroundDetection2>());
            playerController.SetRigidbody(characteristics.GetRigidbody());
            playerController.SetGroundDetection(characteristics.GetGroundDetection());
            playerController.SetSpeed(characteristics.GetSpeed());
            playerController.SetAcceleration(characteristics.GetAcceleration());
            playerController.SetJumpForce(characteristics.GetJumpHeight());
            playerController.SetMass(characteristics.GetMass());
            
            cinemachineCamera.Follow = _activeCharacter.transform;
        
            //Update Eyes and Name Target
            lookFollower.SetTarget(_activeCharacter.transform);
            lookFollower.SetEyes(characterName);
        }

        public GameObject GetOverlays(){
            return overlays;
        }

        public GameObject GetEye(){
            return eyes;
        }

        public CoinController GetCoinController(){
            return coinController;
        }

        public int GetPlayerCoins(){
            return coinController.GetCoins();
        }

        public void SetPlayerIndex(int index){
            _playerIndex = index;
        }

        public int GetPlayerindex(){
            return _playerIndex;
        }

        public CinemachineCamera GetCinemachineCamera()
        {
            return cinemachineCamera;
        }
    }
}
