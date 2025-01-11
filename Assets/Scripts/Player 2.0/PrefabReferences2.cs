using System.Collections.Generic;
using Player;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_2._0{
    public class PrefabReferences2 : MonoBehaviour{
        private GameObject _activeCharacter;
    
        [SerializeField] private List<GameObject> characters;
        
        [Header("References")]
        [SerializeField] private PlayerController2 playerController;
        [SerializeField] private LookFollower2 lookFollower;
        [SerializeField] private CinemachineCamera cinemachineCamera;
        
        [SerializeField] private GameObject overlays;
        [SerializeField] private GameObject eyes;

        void Start(){
            _activeCharacter = GetCharacterByName("Pebble Pete");
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
        
            //Update Rigidbody, Target and camera
            playerController.SetRigidbody(_activeCharacter.GetComponent<Rigidbody>());
            playerController.SetGroundDetection(_activeCharacter.GetComponentInChildren<GroundDetection2>());
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
    }
}
