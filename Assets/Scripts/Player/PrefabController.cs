using System.Collections.Generic;
using CheckpointSystem;
using Collectables;
using Unity.Cinemachine;
using UnityEngine;

namespace Player{
    public class PrefabController : MonoBehaviour{
        [SerializeField] private GameObject activeCharacter;
    
        [SerializeField] private List<GameObject> characters;
        
        [Header("Controller")]
        [SerializeField] private PlayerController playerController;
        
        [Header("Camera")]
        [SerializeField] private LookFollower lookFollower;
        [SerializeField] private Camera mainCamera;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        [Header("Overlays")]
        [SerializeField] private GameObject overlays;
        [SerializeField] private GameObject eyes;
        
        [Header("Collectables")]
        [SerializeField] private CoinController coinController;
        
        [Header("Race Telemetry")]
        [SerializeField] private RaceTelemetry raceTelemetry;

        private int _playerIndex;
        
        void Awake(){
            // _activeCharacter = GetCharacterByName("Pebble Pete");
            
            overlays.layer = LayerMask.NameToLayer("Player" + _playerIndex);
        }

        void Update(){
            // if (Keyboard.current.digit1Key.wasPressedThisFrame){
            //     SetCharacter("Pebble Pete");
            // }    
            // else if (Keyboard.current.digit2Key.wasPressedThisFrame){
            //     SetCharacter("Triangle Tam");
            // }
            // else if (Keyboard.current.digit3Key.wasPressedThisFrame){
            //     SetCharacter("Cubic Chris");
            // }
            // else if (Keyboard.current.digit4Key.wasPressedThisFrame){
            //     SetCharacter("Smooth Sally");
            // }
            // else if (Keyboard.current.digit5Key.wasPressedThisFrame){
            //     SetCharacter("Lava Larry");
            // }

            // //TODO: add get device
            // if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame){
            //     print("Keyboard was pressed with ID: " + Keyboard.current.deviceId);
            // }
            // else{
            //     foreach (var controller in Gamepad.all){
            //         if (controller != null && controller.rightTrigger.wasPressedThisFrame){
            //             print("Controller was pressed with ID: " + controller.deviceId);
            //         }
            //     }
            // }
        }
    
        public GameObject GetCharacter(){
            return activeCharacter;
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
                case "Lava Larry":
                    return characters[4];
            }
            throw new System.Exception("Unknown character");
        }

        public void SetCharacter(string characterName, string playerName){
            // Disable current character
            activeCharacter.SetActive(false);
            
            Transform position = activeCharacter.transform;
        
            //Update character to new one.
            activeCharacter = GetCharacterByName(characterName);
            activeCharacter.SetActive(true);
            
            activeCharacter.GetComponent<Rigidbody>().MovePosition(position.position);
            
            PlayerCharacteristics characteristics = activeCharacter.GetComponent<PlayerCharacteristics>();
            
            //Update PlayerController
            playerController.SetRigidbody(characteristics.GetRigidbody());
            playerController.SetGroundDetection(characteristics.GetGroundDetection());
            playerController.SetSpeed(characteristics.GetSpeed());
            playerController.SetAcceleration(characteristics.GetAcceleration());
            playerController.SetJumpForce(characteristics.GetJumpHeight());
            playerController.SetMass(characteristics.GetMass());
            
            cinemachineCamera.Follow = activeCharacter.transform;
        
            //Update Eyes and Name Target
            lookFollower.SetTarget(activeCharacter.transform);
            lookFollower.SetEyes(characterName);
            
            //Set Playername
            raceTelemetry.SetPlayerName(playerName);
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
