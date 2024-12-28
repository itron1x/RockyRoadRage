using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManager{
    public class CharacterController : MonoBehaviour{
        [SerializeField] 
        private List<GameObject> prefabs;
        
        public void OnPlayerJoined(PlayerInput playerInput){
            print(playerInput.devices[0].device.name);

            switch (playerInput.devices[0].device.name){
                case "XInputControllerWindows":
                    PlayerInputManager.instance.playerPrefab = prefabs[1];
                    break;
                default:
                    PlayerInputManager.instance.playerPrefab = prefabs[0];
                    break;
            }
        }
    }
}
