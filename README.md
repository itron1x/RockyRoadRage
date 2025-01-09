# RockyRoadRage
Rocky Road Rage is a fun, split-screen Multiplayer game about racing rocks.

## IMPORTANT IMPORT INFO FOR DEVS
1) Blender models need to be exported as FBX before using them in Unity. 
The .blend import is broken and produces weird errors when importing directly. 
Ask Paul for more info.

2) For collision make sure to add the "Ground" layer as tag since it uses this tag to identify collisions.
Also check the provide "Provides Contacts" option for terrain.

3) To change prefabs: PlayerInputManager.instance.playerPrefab (Currently called on the EventManager in the CharacterController) Link: https://docs.unity3d.com/Packages/com.unity.inputsystem@1.5/manual/PlayerInputManager.htm; https://docs.unity3d.com/Packages/com.unity.inputsystem@1.7/manual/PlayerInputManager.html 

4) SaveSystem:
* call save and load with SaveSystem.Save() and SaveSystem.Load()
* Coins and character will be callable through RaceInfoSystem with Getter and Setter Methods
* Data is saved in %APPDATA%\LocalLow\DefaultCompany\RockyRoadRage\save.json
* IMPORTANT: During development you might need to add the RaceInfoSystem Script to your scene!

## Packages
* Cinemachine
* Post Processing (https://www.youtube.com/watch?v=rmlJUaWfmzQ)

## TODO
- [ ] Leaderboard
- [x] Save unlocked characters and coins to local disk (https://www.youtube.com/watch?v=1mf730eb5Wo)
- [ ] Player and Character Selection Menu
- [ ] Add coins to global coins after match
- [ ] Implement Difficulty (Game Speed)
