using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace InputManager
{
    public class CC : MonoBehaviour
    {
        [SerializeField] private List<GameObject> playerPrefabs; // Liste von Prefabs für unterschiedliche Steuerungsgeräte
        private List<PlayerData> playerDataList = new List<PlayerData>(); // Liste der Spieler-Daten

        private InputAction navigateAction;
        private List<InputDevice> detectedDevices = new List<InputDevice>();
        
        void Start()
        {
            // Hole die "Navigate"-Action aus dem Input-System
            navigateAction = GetNavigateAction();
            if (navigateAction != null)
            {
                navigateAction.performed += OnNavigate;
            }
            
            // Erkenne alle verfügbaren Geräte
            if (Keyboard.current != null) detectedDevices.Add(Keyboard.current);
            foreach (var gamepad in Gamepad.all) detectedDevices.Add(gamepad);
        }

        public List<InputDevice> GetDetectedDevices()
        {
            return detectedDevices;
        }
        
        void Update()
        {
            DetectInputAndCreatePlayer();
        }

        void OnDestroy()
        {
            if (navigateAction != null)
            {
                navigateAction.performed -= OnNavigate;
            }
        }

        private InputAction GetNavigateAction()
        {
            // Hier kannst du das Input-Asset laden, falls notwendig
            return null; // Placeholder für InputAction-Initialisierung
        }

        private void OnNavigate(InputAction.CallbackContext context)
        {
            Vector2 input = context.ReadValue<Vector2>();
            Debug.Log($"Navigation Input: {input}");
        }

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

        public void CreatePlayer(InputDevice device)
        {
            string controlScheme = AssignControlScheme(device);
            GameObject selectedPrefab = SelectPlayerPrefab(controlScheme);

            // Spieler erstellen
            PlayerInput newPlayer = PlayerInput.Instantiate(
                selectedPrefab,
                playerIndex: -1,
                splitScreenIndex: -1,
                controlScheme: controlScheme,
                pairWithDevice: device
            );

            // Spieler-Daten speichern
            PlayerData playerData = new PlayerData(newPlayer.playerIndex, device, controlScheme)
            {
                SelectedPrefab = null
            };
            playerDataList.Add(playerData);

            AssignDevicesToPlayer(newPlayer);

            // Charakterauswahl starten
            CharacterSelectionIU characterSelectionUI = Object.FindFirstObjectByType<CharacterSelectionIU>();
            if (characterSelectionUI != null)
            {
                characterSelectionUI.StartCharacterSelection(playerData, playerPrefabs, HandleCharacterSelection);
            }
            else
            {
                Debug.LogError("CharacterSelectionIU nicht gefunden!");
            }

            Debug.Log($"Spieler {newPlayer.playerIndex + 1} mit Gerät {device.displayName} und Steuerungsschema {controlScheme} erstellt.");
        }


        private void HandleCharacterSelection(PlayerData playerData, GameObject selectedPrefab)
        {
            // Aktualisiere den ausgewählten Charakter in der PlayerData-Liste
            PlayerData existingPlayer = playerDataList.Find(p => p.PlayerIndex == playerData.PlayerIndex);
            if (existingPlayer != null)
            {
                existingPlayer.SelectedPrefab = selectedPrefab;
                Debug.Log($"Spieler {existingPlayer.PlayerIndex + 1} hat Charakter {selectedPrefab.name} ausgewählt.");
            }
        }

        private string AssignControlScheme(InputDevice device)
        {
            return device.name switch
            {
                "XInputControllerWindows" => "Gamepad",
                "Keyboard" => "Keyboard&Mouse",
                _ => "Generic"
            };
        }

        private void AssignDevicesToPlayer(PlayerInput playerInput)
        {
            // Beispiel: Tastatur und Maus für den Spieler aktivieren
            playerInput.actions.devices = new InputDevice[] { Keyboard.current, Mouse.current };
        }
        
        private GameObject SelectPlayerPrefab(string controlScheme)
        {
            if (controlScheme == "Gamepad" && playerPrefabs.Count > 1)
            {
                return playerPrefabs[1];
            }
            return playerPrefabs[0];
        }

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
