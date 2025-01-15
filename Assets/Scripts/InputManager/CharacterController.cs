using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManager{
    
    public class CharacterController : MonoBehaviour
    {
        
        [SerializeField]
        private List<GameObject> playerPrefabs; // Liste von Prefabs für unterschiedliche Steuerungsgeräte

        private Dictionary<InputDevice, string> deviceControlSchemeMap = new Dictionary<InputDevice, string>();
        private List<PlayerData> playerDataList = new List<PlayerData>();


        // Methode zum Instanzieren eines neuen Spielers
        public PlayerInput InstantiatePlayer(InputDevice device)
        {
            string controlScheme = AssignControlScheme(device);

            // Wähle passendes Prefab basierend auf dem Steuerungsschema
            GameObject selectedPrefab = SelectPlayerPrefab(controlScheme);

            // Spieler mit dem gewählten Prefab und Steuerungsschema erstellen
            PlayerInput newPlayer = PlayerInput.Instantiate(
                selectedPrefab,
                playerIndex: -1, // Automatische Zuweisung eines Spieler-Indexes
                splitScreenIndex: -1, // Kein Splitscreen
                controlScheme: controlScheme,
                pairWithDevice: device // Verknüpfung mit dem Gerät
            );

            // Spieler-Daten erstellen und speichern
            PlayerData playerData = new PlayerData(newPlayer.playerIndex, device, controlScheme)
            {
                SelectedPrefab = selectedPrefab
            };
            playerDataList.Add(playerData);

            // Debug-Informationen
            Debug.Log($"Spieler {newPlayer.playerIndex + 1} mit Gerät {device.displayName} und Steuerungsschema {controlScheme} hinzugefügt.");

            return newPlayer;
        }
        
        // Wählt das passende Steuerungsschema basierend auf dem Gerät
        private string AssignControlScheme(InputDevice device)
        {
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

        // Wählt das passende Prefab basierend auf dem Steuerungsschema
        private GameObject SelectPlayerPrefab(string controlScheme)
        {
            if (controlScheme == "Gamepad" && playerPrefabs.Count > 1)
            {
                return playerPrefabs[1]; // Prefab für Gamepad
            }
            return playerPrefabs[0]; // Standard-Prefab
        }
        
        public List<PlayerData> GetPlayerDataList()
        {
            return playerDataList;
        }
        
        // Methode zum Wechseln des Steuerungsschemas eines existierenden Spielers
        public void SwitchPlayerControlScheme(PlayerInput playerInput, string newControlScheme, InputDevice newDevice)
        {
            playerInput.SwitchCurrentControlScheme(newControlScheme, newDevice);

            // Aktualisiere gespeicherte Daten
            PlayerData playerData = playerDataList.Find(p => p.PlayerIndex == playerInput.playerIndex);
            if (playerData != null)
            {
                playerData.Device = newDevice;
                playerData.ControlScheme = newControlScheme;
            }

            Debug.Log($"Spieler {playerInput.playerIndex + 1} hat das Steuerungsschema auf {newControlScheme} geändert.");
        }
    }
/*
    public class PlayerData
    {
        public int PlayerIndex { get; private set; }
        public InputDevice Device { get; set; }
        public string ControlScheme { get; set; }
        public GameObject SelectedPrefab { get; set; }

        public PlayerData(int playerIndex, InputDevice device, string controlScheme)
        {
            PlayerIndex = playerIndex;
            Device = device;
            ControlScheme = controlScheme;
            SelectedPrefab = null;
        }
    }
        
        // Zugriff auf die Liste der Spieler-Daten
        public List<PlayerData> GetPlayerDataList()
        {
            List<PlayerData> playerDataList;
            return playerDataList;
        }
        */
    }

    

