# RockyRoadRage
Rocky Road Rage is a fun, split-screen Multiplayer game about racing rocks.

## IMPORTANT IMPORT INFO FOR DEVS
1) Blender models need to be exported as FBX before using them in Unity. 
The .blend import is broken and produces weird errors when importing directly. 
Ask Paul for more info.

2) For collision make sure to add the "Ground" layer as tag since it uses this tag to identify collisions.
Also check the provide "Provides Contacts" option for terrain.
