using System;
using System.Collections.Generic;
using System.Numerics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using Vector3 = UnityEngine.Vector3;

public class SpawnPlayer : MonoBehaviour
{
    [SerializeField] private Transform spawnPoints;
    [SerializeField] private Camera idleCamera;
    
    private List<GameObject> _players = new List<GameObject>();
    private List<Vector3> _spawnPointLocations = new List<Vector3>();
    private RaceControlUI _raceControlUI;
    
    private void Awake()
    {
        _raceControlUI = GetComponent<RaceControlUI>();
        foreach (Transform spawnPoint in spawnPoints)
        {
            if (spawnPoint.CompareTag("Spawn"))
            {   
                Vector3 spawnLocation = spawnPoint.transform.position;
                Debug.Log("Found spawn point " + spawnPoint.name + " at " + spawnLocation);
                
                // Create a sphere at the spawn location
                //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                //sphere.transform.position = spawnLocation;

                // Optional: Customize the sphere's appearance
                //sphere.transform.localScale = Vector3.one * 0.5f; // Make the sphere smaller
                //sphere.GetComponent<Renderer>().material.color = Color.red; // Set color to red
                
                _spawnPointLocations.Add(spawnLocation);
                
            }
        }
    }
    
    public void OnPlayerJoined(PlayerInput playerInput)
    {
        idleCamera.gameObject.SetActive(false);
        GameObject newPlayer = playerInput.gameObject;
        
        _players.Add(newPlayer);
        int playerIndex = _players.IndexOf(newPlayer);

        CheckpointTracker RaceControl = GetComponent<CheckpointTracker>();
        RaceControl.AddPlayer(newPlayer.transform);
        
        Vector3 spawnPoint = _spawnPointLocations[playerIndex];
        
        _raceControlUI.DisplayUpdateText("Player " + playerIndex + " joined!");
        
        newPlayer.GetComponent<Rigidbody>().MovePosition(spawnPoint); //whenever an object has a Rigidbody, it needs to be moved this way to integrate with the physics engine.
    }

}
