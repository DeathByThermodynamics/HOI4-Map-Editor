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

The code will create a `MapEditor` folder in the mod installation folder, but otherwise will not touch the input folder!

Important: `common` (and especially country ids) are needed for the application to function properly - if there are no tags, you won't be able to set the owner of a state.

If the starter window does not move past `Initializing Map Load` after ~10 seconds, you may have the wrong folder chosen. As reference, the loader completes loading the vanilla map after ~2-3 minutes - this will be optimized in future builds.

![alt text](https://github.com/DeathByThermodynamics/HOI4-Map-Editor/blob/master/caption1.jpg)

After it finishes, you `start` and then `generate map` in order to render the map with the editor. I plan on creating a separate window for generating the map, so be aware that for now, clicking generate map again after the map has loaded will crash the application.

## Current Functions
### GUI and Functions
![alt text](https://github.com/DeathByThermodynamics/HOI4-Map-Editor/blob/master/caption2.jpg)

I've redone the GUI and gotten rid of the messy windows for a sleeker built-in GUI, limiting the application itself to a single window. I've also made the three tabs keep their information even when moving to another, so feel free to add a few more military factories to Upper Austria before turning Vienna into a 50 VP province. Keep in note you must click 'Save State' before clicking on another province, or your changes will be lost.

#### State Editing
Within the state view (as seen in the bottom left corner), the editor can:
- change buildings in a state and province. It is recommended to change railways and supply nodes in `Nudge` instead, as you can visualize those.
- change the basis stats of a state, such as manpower, state category, and more.
- change ownership of a state
- change the resources of a state


#### Provincial Ownership
By shift clicking a province outside of a state (which is not lake/ocean) while selecting a state, you will transfer that province to your selected state. This will also transfer the province's properties, such as any buildings or victory points. The colour of the province will also update if it is switching owners.

By pressing `Create New State` in the State view in the bottom left corner, you can create a new state with the same owner and the singular province that you were selecting. Keep in mind, you cannot delete states currently - and I am not sure if I want to add this because it has the potential to massively affect state IDs and focus trees, by extension.

Note that strategic regions cannot yet be edited, but will update whenever a province transfers states.

### Reload Map
Whenever the ownership of the state or province is changed, the colour of the province(s) will update to reflect that. Before exiting the editor, please click on `Reload Map` in order to make the map changes permanent - otherwise, the next time you enter the application, the visual changes will have been reverted.

### Saving

Every time 'save' is clicked in the province view window, a plain file is generated which saves the state of the map in the editor, in order to reduce the amount of times that one might need to export or load. The map state is also saved when shift+click is used to transfer provinces. This save state is stored within a folder in your input folder (likely `MapEditor`) and thus you can load different states just by specifying the input folder.

### Exporting

Clicking the Export button will export the strategic regions and states of your mod into the output folder. As a fair warning, currently the files will just be named their IDs. These can be directly moved into a mod folder and should not cause map errors (other than the change in buildings error which can be fixed in nudge by validating). 
