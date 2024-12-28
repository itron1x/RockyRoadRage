# RockyRoadRage
Rocky Road Rage is a fun, split-screen Multiplayer game about racing rocks.

## IMPORTANT IMPORT INFO FOR DEVS
1) Blender models need to be exported as FBX before using them in Unity. 
The .blend import is broken and produces weird errors when importing directly. 
Ask Paul for more info.

2) For collision make sure to add the "Ground" layer as tag since it uses this tag to identify collisions.
Also check the provide "Provides Contacts" option for terrain.

3) To change prefabs: PlayerInputManager.instance.playerPrefab (Currently called on the EventManager in the CharacterController) Link: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/PlayerInputManager.html