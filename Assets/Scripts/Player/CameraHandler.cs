using System.Collections.Generic;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player{
    //By default, all Cinemachine cameras are linked to one brain. To separate these brains we need to assign each camera to it's separate brain by adding the cameras to different channels.
    //Link: https://docs.unity3d.com/Packages/com.unity.cinemachine@3.1/manual/CinemachineMultipleCameras.html
    public class CameraHandler : MonoBehaviour{
        [SerializeField]
        private List<OutputChannels> outputChannels;

        public void AddPlayer(PlayerInput player){
            Transform playerParent = player.transform.parent;

            //Get single channel for camera
            OutputChannels cameraChannel = outputChannels[player.playerIndex];
           
            //Add index to parent
            playerParent.GetComponent<PrefabController>().SetPlayerIndex(player.playerIndex);
            
            //Set culling mask
            playerParent.GetComponentInChildren<Camera>().cullingMask &= ~(1 << LayerMask.NameToLayer("Player"+player.playerIndex));
            
            //Add individual channels to each player
            playerParent.GetComponentInChildren<CinemachineCamera>().OutputChannel = cameraChannel;
            playerParent.GetComponentInChildren<CinemachineBrain>().ChannelMask = cameraChannel;

            //Divide inputs to seperate players
            playerParent.GetComponentInChildren<CinemachineInputAxisController>().PlayerIndex = player.playerIndex;
        }
    }
}
