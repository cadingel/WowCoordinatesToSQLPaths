# WowCoordinatesToSQLPaths
WowCoordinatesToSQLPaths is inspired by https://github.com/Chuck5ta/WoWCreaturePathMapper. His WoW Addon has been modified and the WowCoordinatesToSQLPaths tool has been written from scratch.

This tool can be used for creating paths for creatures and NPC for a World of Warcraft Private Server.  
Currently only tested with cmangos-classic.

# WowCoordinatesToSQLPaths Tool consists of
- World of Warcraft Addon: CoordinatesRecorder
- Waypoints to SQL tool: WowCoordinatesToSQLPaths

# How To
1. Download the latest release here : [Releases](https://github.com/oppahansi/WowCoordinatesToSQLPaths/releases)
2. Grab the Addon **CoordinatesRecorder** and install it. (Put the addon folder **CoordinatesRecorder** into your Interface folder.)
3. Make sure your character has GM privileges with the .gps command
4. Start the Game and join your server
5. Make sure the addon has been installed and you see this window:  
![Addon Frame](https://image.prntscr.com/image/531dcfb5014c478484d0eb909e62ec9f.png)  
/coordsmap show or /coordsmap hide 
6. We recommen pressing the **Reset** butotn before every recording session to make sure no old values are being saved
7. Record waypoints by moving around and pressing the **Record Waypoint** button (You only need to record the waypoints in one direction)
8. When waypoints have been recorded, log out to generate the addon log file
9. Start WowCoordinatesToSQLPaths.exe
10. Fill in the creature **guid** for creature_movement table or creature_template **entry** for creature_movement_template table / script_waypoint table and load the addon log file from your **../WoW/WTF/Account/Username/SavedVariables/CoordinatesRecorder.lua**.
11. Select the wished type of path (one-directional, bi-directional or script). By selecting **script** we assume you provide the creature_template **entry**.
12. Optionally edit the values.
13. Save the waypoints which will be saved as a sql script.
14. Execute the SQL script on your database.
15. Test and hf.

# Made using:
- VS2017 -> C# / WPF
- Notepad++ / VS Code -> Lua, XML -> WoW Addon
- Coffee -> Code

# Why remake an existing tool?
A good question. We just wanted to have some special functionalities and our own code base as practice and learning project.

# Some Screenshots
## Default Layout on startup
![Default](https://image.prntscr.com/image/229cb60932814bf18e54cf9e2981aaa0.png)

## Script Waypoints Preview
![Script Selected](https://image.prntscr.com/image/3e9ecfbf3bc14389bb854c780e7ba085.png)

## Required fields have to be filled in
![GUID Or Entry Required](https://image.prntscr.com/image/13537d2abba44f09afc6b15d14edf84c.png)

## Example data loaded
You can still switch between the different types of path / table types
![Script Waypoints](https://image.prntscr.com/image/5a1dde9e434745bab9822bac40f214aa.png)

## Autoadjusting in width to fit in all columns
![Creature Movement](https://image.prntscr.com/image/3e52f1a2a9cb4e919aa5f2ebc3c61f7c.png)

## Any fields can be edited in the data grid
![Editeable Data Grid](https://image.prntscr.com/image/0c706206eccc4acbaf1884e960554de7.png)

# TODO
- A better ui?
- CSV support, not even sure if needed
- Support for other server types - currently not interested myself, but may take a look when someone requests it.
- anything else?

# Contributing
Feel free to contribute by making changes / additions and making pull requests.

# Contributors
- [Oppahansi](https://github.com/oppahansi/)
- [Onekii](https://github.com/Onekii)
