using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManager
{
    public class CC : MonoBehaviour
    {
        [SerializeField] private List<GameObject> devicePrefabs; // one devicePrefab for each Device Type ?
        [SerializeField] private InputActionAsset inputActions;
        private List<PlayerData> playerDataList = new List<PlayerData>(); 
        private List<InputDevice> detectedDevices = new List<InputDevice>(); 
        private InputAction navigateAction;
      
        
        
        void Start()
        {
            // Initialize detected Device
            if (Keyboard.current != null) detectedDevices.Add(Keyboard.current);
            foreach (var gamepad in Gamepad.all) detectedDevices.Add(gamepad);

            // += Update (add) device
            InputSystem.onDeviceChange += OnDeviceChange;
        }

        private void OnDeviceChange(InputDevice device, InputDeviceChange change)
        {
            if (change == InputDeviceChange.Added) // Device added.
            {
                if (!detectedDevices.Contains(device))
                {
                    detectedDevices.Add(device);
                    Debug.Log($"Device added: {device.displayName}");
                }
            }
            else if (change == InputDeviceChange.Removed) // Device removed.
            {
                detectedDevices.Remove(device);
                Debug.Log($"Device removed: {device.displayName}");
            }
        }
        
        /*
        private void OnNavigate(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Debug.Log($"Navigation Input: {input}");
        }
        
        void OnDestroy()
        {
            if (navigateAction != null)
            {
                // -= Update (remove) device
                navigateAction.performed -= OnNavigate;
            }
        }
        */
        
        public List<InputDevice> GetDetectedDevices()
        {
            return detectedDevices;
        }
        
        void Update()
        {
            // DetectInputAndCreatePlayer includes CreatePlayer
            // CreatePLayer includes StartCharacterSelection - Callback from CharacterSelectionUI
            // CreatePLayer includes HandleCharacterSelection (add Character to Player)
           // DetectInputAndCreatePlayer();
        }
        

        /*
        private void DetectInputAndCreatePlayer()
        {
            // Erkennung einer Keyboard-Eingabe
            if (Keyboard.current != null && Keyboard.current.pKey.wasPressedThisFrame)
            {
                CreatePlayer(Keyboard.current);
            }

            // Erkennung einer Gamepad-Eingabe
            foreach (var controller in Gamepad.all)
            {
                if (controller != null && controller.rightTrigger.wasPressedThisFrame)
                {
                    CreatePlayer(controller);
                    break;
                }
            }
        }
        */
        
        public void CreatePlayer(InputDevice device)
        {
            string controlScheme = AssignControlScheme(device);
            GameObject selectedPrefab = SelectPlayerPrefab(controlScheme);

            // create Player with Parameter
            PlayerInput newPlayer = PlayerInput.Instantiate(
                selectedPrefab,
                playerIndex: playerDataList.Count,
                splitScreenIndex: -1,
                controlScheme: controlScheme,
                pairWithDevice: device
            );

            // save Player Data
            PlayerData playerData = new PlayerData(newPlayer.playerIndex, device, controlScheme)
            {
                SelectedPrefab = null
            };
            playerDataList.Add(playerData);

            // Start Character Selection at CharacterSelectionUI 
            CharacterSelectionIU characterSelectionUI = Object.FindFirstObjectByType<CharacterSelectionIU>();
            if (characterSelectionUI != null)
            {
                // StartCharacterSelection - Callback from CharacterSelectionUI
                // HandleCharacterSelection (add Character to Player)
                characterSelectionUI.StartCharacterSelection(playerData, devicePrefabs, HandleCharacterSelection);
            }
            else
            {
                Debug.LogError("CharacterSelectionIU not found!");
            }

            Debug.Log($"Player {newPlayer.playerIndex + 1} with device {device.displayName} and ControlScheme {controlScheme} created.");
        }


        private void HandleCharacterSelection(PlayerData playerData, GameObject selectedPrefab)
        {
            // Aktualisiere den ausgewählten Charakter in der PlayerData-Liste
            PlayerData existingPlayer = playerDataList.Find(p => p.PlayerIndex == playerData.PlayerIndex);
            if (existingPlayer != null)
            {
                existingPlayer.SelectedPrefab = selectedPrefab;
                Debug.Log($"player {existingPlayer.PlayerIndex + 1} has chosen Character {selectedPrefab.name}.");
            }
        }

        
        // assigning Device Type with devicePrefab 
        private string AssignControlScheme(InputDevice device)
        {
            return device.name switch
            {
                "XInputControllerWindows" => "Controller",
                "Keyboard" => "Keyboard",
                _ => "Generic"
            };
        }

       /*
        private void AssignDevicesToPlayer(PlayerInput playerInput)
        {
            playerInput.actions.devices = new InputDevice[] { Keyboard.current, Mouse.current };
        }
        */
       
        private GameObject SelectPlayerPrefab(string controlScheme)
        {
            if (controlScheme == "Controller" && devicePrefabs.Count > 1)
            {
                return devicePrefabs[1];
            }
            return devicePrefabs[0];
        }

        /*
        public void SwitchPlayerControlScheme(PlayerInput playerInput, InputDevice newDevice)
        {
            string newControlScheme = AssignControlScheme(newDevice);
            playerInput.SwitchCurrentControlScheme(newControlScheme, newDevice);

            PlayerData playerData = playerDataList.Find(p => p.PlayerIndex == playerInput.playerIndex);
            if (playerData != null)
            {
                playerData.Device = newDevice;
                playerData.ControlScheme = newControlScheme;
            }

            Debug.Log($"Spieler {playerInput.playerIndex + 1} hat das Steuerungsschema auf {newControlScheme} geändert.");
        }
        */

        public List<PlayerData> GetPlayerDataList()
        {
            return playerDataList;
        }
    }
    
   
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
}
