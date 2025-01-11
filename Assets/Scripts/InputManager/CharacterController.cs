using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManager{
    
    /*
    public class CharacterController : MonoBehaviour{
        [SerializeField] 
        private List<GameObject> prefabs;
        
        // Method called if a player joins. TODO: potentially not necessary.
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
    */
    
    public class CharacterController : MonoBehaviour
    {
        
        private Dictionary<InputDevice, string> deviceControlSchemeMap = new Dictionary<InputDevice, string>();
        private List<PlayerData> playerDataList = new List<PlayerData>();

        public void OnPlayerJoined(PlayerInput playerInput)
        {
            InputDevice device = playerInput.devices[0];
            string controlScheme = AssignControlScheme(device);

            // Spieler-Daten erstellen und speichern
            PlayerData playerData = new PlayerData(playerInput.playerIndex, device, controlScheme);
            playerDataList.Add(playerData);

            // Steuerungsschema zuweisen
            playerInput.SwitchCurrentControlScheme(controlScheme, device);

            // Debug-Informationen
            Debug.Log($"Spieler {playerInput.playerIndex + 1} mit Gerät {device.displayName} hinzugefügt.");
            Debug.Log($"Steuerungsschema: {controlScheme}");
        }

        private string AssignControlScheme(InputDevice device)
        {
            // Steuerungsschema basierend auf Gerät festlegen
            string controlScheme;

            switch (device.name)
            {
                case "XInputControllerWindows":
                    controlScheme = "Gamepad";
                    break;
                case "Keyboard":
                    controlScheme = "Keyboard&Mouse";
                    break;
                default:
                    controlScheme = "Generic";
                    break;
            }

            // Steuerungsschema speichern
            deviceControlSchemeMap[device] = controlScheme;
            return controlScheme;
        }

        public List<PlayerData> GetPlayerDataList()
        {
            return playerDataList;
        }
    }

    public class PlayerData
    {
        public int PlayerIndex { get; private set; }
        public InputDevice Device { get; private set; }
        public string ControlScheme { get; private set; }
        public GameObject SelectedPrefab { get; set; }

        public PlayerData(int playerIndex, InputDevice device, string controlScheme)
        {
            PlayerIndex = playerIndex;
            Device = device;
            ControlScheme = controlScheme;
            SelectedPrefab = null; // Wird später durch die Charakterauswahl zugewiesen
        }
        
        
    }

    
}
