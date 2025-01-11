using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player_2._0{
    public class CharacterController2 : MonoBehaviour{
        [SerializeField] 
        private List<GameObject> prefabs;

        private int _player;
        private int _globalCount;
        private List<int> _playerCharacters;
        
        void Awake(){
            _playerCharacters = new List<int>();
            _globalCount = 0;
            _player = 2;
            for (int i = 0; i < _player - 1; i++){
                _playerCharacters.Add(i);
            }

            // PlayerInputManager.instance.playerPrefab = prefabs[_playerCharacters[_globalCount]];
        }
        
        // Method called if a player joins. TODO: potentially not necessary.
        public void OnPlayerJoined(PlayerInput playerInput){
            // print(playerInput.devices[0].device.name);
            //
            // switch (playerInput.devices[0].device.name){
            //     case "XInputControllerWindows":
            //         PlayerInputManager.instance.playerPrefab = prefabs[1];
            //         break;
            //     default:
            //         PlayerInputManager.instance.playerPrefab = prefabs[0];
            //         break;
            // }
            print(playerInput.devices[0].deviceId);
            print(_globalCount);
            
            PlayerInputManager.instance.playerPrefab = prefabs[_playerCharacters[_globalCount]];
            _globalCount++;
        }
    }
}
