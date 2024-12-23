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
    
    private void Awake()
    {
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
    
    //Despite what JetBrains tells you, this method is actually being used. Don't trust the bots.
    void OnPlayerJoined(PlayerInput playerInput)
    {
        idleCamera.gameObject.SetActive(false);
        Debug.Log("Player Joined at " + playerInput.gameObject.transform.position);
        GameObject newPlayer = playerInput.gameObject;
        //Debug.Log("Player " + newPlayer.name);
        
        _players.Add(newPlayer);
        int playerIndex = _players.IndexOf(newPlayer);
        
        Vector3 spawnPoint = _spawnPointLocations[playerIndex];
        
        Debug.Log("Player " + newPlayer.tag + " spawned at " + spawnPoint);
        
        newPlayer.GetComponent<Rigidbody>().MovePosition(spawnPoint); //whenever an object has a Rigidbody, it needs to be moved this way to integrate with the physics engine.
    }

}
