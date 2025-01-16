using System.Collections.Generic;
using UnityEngine;
using TMPro;
using InputManager;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [SerializeField] private TMP_Text DeviceNumber;
    [SerializeField] private Button nextPlayerButton;
    [SerializeField] private Button backPlayerButton;
    [SerializeField] private Button confirmPlayer;
    [SerializeField] private CC characterController; // Referenze to CC-Klasse
    private List<InputDevice> devices;
    private int currentDeviceIndex = 0;
    private List<InputDevice> confirmedDevices = new List<InputDevice>(); 

    
    void Start()
    {
        // initialise Devices from CC 
        devices = characterController.GetDetectedDevices();
        if (devices == null || devices.Count == 0)
        {
            Debug.LogError("No Device recognized!");
            DeviceNumber.text = "No Device recognized!";
            return;
        }

        UpdateDeviceDisplay();

        // Update Device
        InputSystem.onDeviceChange += OnDeviceChange;
    }

    private void OnDeviceChange(InputDevice device, InputDeviceChange change)
    {
        devices = characterController.GetDetectedDevices();
        currentDeviceIndex = Mathf.Clamp(currentDeviceIndex, 0, devices.Count - 1);
        UpdateDeviceDisplay();
    }

    void OnDestroy()
    {
        // -= deactive Device
        InputSystem.onDeviceChange -= OnDeviceChange;
    }
    
    private void UpdateDeviceDisplay()
    {
        if (devices.Count == 0)
        {
            DeviceNumber.text = "No device available!";
            return;
        }

        InputDevice currentDevice = devices[currentDeviceIndex];
        DeviceNumber.text = $"Device: {currentDevice.displayName}";
    }

    
    // iterate through Device-List --------------------------------
    public void NextDevice()
    {
        currentDeviceIndex = (currentDeviceIndex + 1) % devices.Count;
        UpdateDeviceDisplay();
    }

    public void PreviousDevice()
    {
        currentDeviceIndex = (currentDeviceIndex - 1 + devices.Count) % devices.Count;
        UpdateDeviceDisplay();
    }

    public void ConfirmDevice()
    {
        InputDevice selectedDevice = devices[currentDeviceIndex];
        if (confirmedDevices.Contains(selectedDevice))
        {
            Debug.LogWarning($"Device {selectedDevice.displayName} is already confirmed!");
            return; 
        }

        confirmedDevices.Add(selectedDevice);
        Debug.Log($"Device {selectedDevice.displayName} confirmed for a new player.");
    }
    // -------------------------------------------------------------

    public void ConfirmPlayers()
    {
        foreach (var device in confirmedDevices)
        {
            // create Player based on confirmed Devices
            characterController.CreatePlayer(device);
        }

        Debug.Log($"{confirmedDevices.Count} Player created.");
    }
    
    /*
    public void ConfirmSelection()
    {
        List<PlayerData> players = characterController.GetPlayerDataList();
        if (players != null && players.Count > 0)
        {
            PlayerData
                currentPlayer =
                    players[currentDeviceIndex]; // use Index = Device connected to Player - 
            Debug.Log(
                $"Player {currentPlayer.PlayerIndex + 1} with Device {currentPlayer.ControlScheme} confirmed.");
        }
        else
        {
            Debug.LogError("No Player Data available!");
        }
    }
    */
}
