# HOI4 Map Editor
## Introduction

A map editor for modding Hearts of Iron IV.
Currently, the application is (to-be) built in C# while the setup and file retrieval is handled by Python.

This project is made to make modding Hearts of Iron IV more accessible, as well as increasing the efficiency of modding for experienced users. The current nudger tool built-in to Hearts of Iron IV requires individual file editing and game reloading to see the effects of map editing, while this application strives to achieve the same thing in real time. I've always been very annoyed with the efficiency of modding in-game (especially with map overhaul mods) and this is my solution to it.

This project was built with MapGen (a wonderful map generation tool!) in mind, and files from MapGen can be directly used in this application.

The app can be run from the build in the zip file found in /hoi4test/hoitest3.zip.
A build will be released once all planned features of the first stage are completed.

## Starting the Program
To run the program, you will need some installation of HOI4 (in order for the program to read the relevant data). This can be vanilla files or files from a mod installation (usually total overhauls). 

![alt text](https://github.com/DeathByThermodynamics/HOI4-Map-Editor/blob/master/loadingcaption1.jpg)

Here, `hoi4example` is my chosen installation folder. Currently, the program needs:
- the folders in `common`, mostly for the country ids and colour files
- `map`, for constructing the map to be displayed
- `history`, for building and state data

The code will create a `provinces` folder in the mod installation folder, but otherwise will not touch the input folder!

Important: `common` (and especially country ids) are needed for the application to function properly - if there are no tags, you won't be able to set the owner of a state.

If the starter window does not move past `Initializing Map Load` after ~10 seconds, you may have the wrong folder chosen. As reference, the loader completes loading the vanilla map after ~2-3 minutes - this will be optimized in future builds.

![alt text](https://github.com/DeathByThermodynamics/HOI4-Map-Editor/blob/master/preview2.jpg)

After it finishes, you `start` and then `generate map` in order to render the map with the editor. I plan on creating a separate window for generating the map, so be aware that for now, clicking generate map again after the map has loading will crash the application.

## Current Functions
### GUI and Functions
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

### Saving

Every time 'save' is clicked in the province view window, a plain file is generated which saves the state of the map in the editor, in order to reduce the amount of times that one might need to export or load. The map state is also saved when shift+click is used to transfer provinces. This save state is stored within a folder in your input folder (likely `MapEditor`) and thus you can load different states just by specifying the input folder.

### Exporting

Clicking the Export button will export the strategic regions and states of your mod into the output folder. As a fair warning, currently the files will just be named their IDs. These can be directly moved into a mod folder and should not cause map errors (other than the change in buildings error which can be fixed in nudge by validating). 
