# HOI4 Map Editor
## Introduction

A map editor for modding Hearts of Iron IV. 
Currently, the application is (to-be) built in C# while the setup and file retrieval is handled by Python.

This project is made to make modding Hearts of Iron IV more accessible, as well as increasing the efficiency of modding for experienced users. The current nudger tool built-in to Hearts of Iron IV requires individual file editing and game reloading to see the effects of map editing, while this application strives to achieve the same thing in real time.

Currently working on bridging the two. The app can be run, but the current process is complicated and requires installations of Python.
A build will be released once all planned features of the first stage are completed.

## Current GUI
![alt text](https://github.com/DeathByThermodynamics/HOI4-Map-Editor/blob/master/hoi4editorpreview.png)

Currently, the editor can:
- import state / map data based on your HO4 mod installation folder
- change buildings in a state
- change basic stats of a state
- change ownership of a state
- add resources to a state
- add a province to a state, removing it from any other states. All VPs and province buildings are also transferred.
- export state data to be used in your mod
- strategic regions cannot yet be edited, but will update whenever a province transfers states.
