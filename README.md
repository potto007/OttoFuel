# OttoFuel

### Updated for Valheim 1.0

OttoFuel keeps your production burning. It pulls fuel and ore out of nearby chests
and off the ground, then feeds them to the things that need them.

**Maintainer:** Paul Otto

**This mod is a fork of [AutomaticFuel](https://thunderstore.io/c/valheim/p/TastyChickenLegs/AutomaticFuel/) by TastyChickenLegs.**
TastyChickenLegs built AutomaticFuel on the idea and original code of Aedenthorn's
AutoFuel. Credit for the mod goes to both of them. OttoFuel carries that work
forward under the same public-domain licence.

--------------------

### What it fuels

- Torches, standing torches, wall torches, braziers and campfires
- Hearths and hot tubs
- Smelters, blast furnaces, charcoal kilns, windmills and spinning wheels
- **Stone ovens and any other cooking station that burns fuel (new in 1.5.0)**
- Shield generators

### Other features

- Stacked smelters. Removes most smoke and the "smoke blocked" check.
- The blast furnace accepts every ore.
- Kiln output limit, so a kiln stops once you have enough coal.
- Per-object switches, so you can turn any of it off.
- ServerSync. A server pushes its config down to the clients.

--------------------

### Changes in this fork

- **1.5.0** - Renamed to OttoFuel. Added stone oven and cooking station refuelling.
- **1.4.9** - Ported to the Valheim 1.0 API. See the notes below.

The 1.0 port covers these API breaks:

| Break in Valheim 1.0 | Fix |
| --- | --- |
| `Character.Message` gained a `log` parameter | The argument is passed |
| `Smelter.RPC_AddOre` gained a `cheated` parameter | Both call sites send it |
| `Inventory.Changed` gained two parameters | Resolved through `AccessTools` |
| `Inventory.RemoveItem` changed a default | The old behaviour is pinned |
| `ZRoutedRpc.Everybody` became a constant | ServerSync rebuilt against 1.0 |

--------------------

`A config file BepInEx/config/PaulOtto.OttoFuel.cfg is created after you run the game once with this mod.`

**Note for people who used AutomaticFuel:** the config file name changed, so your old
settings do not carry over. Copy them across by hand if you want to keep them.

You can adjust the config values by editing this file using a text editor or in-game using the Config Manager﻿.

|Config Option|Definition
|---|---|
|verifyClient| This turns on and off the need to verify other clients.  Users can now play without the mod on a server with the mod.|
|fireplaceRange| The maximum range to pull fuel from containers for fireplaces|
|smelterOreRange| The maximum range to pull fuel from containers for smelters|
|smelterFuelRange| The maximum range to pull ore from containers for smelters|
|fuelDisallowTypes| Types of item to disallow as fuel (i.e. anything that is consumed), comma-separated.|
|oreDisallowTypes| Types of item to disallow as ore (i.e. anything that is transformed), comma-separated).|
|refuelStandingTorches| Refuel standing torches|
|refuelWallTorches| Refuel wall torches|
|refuelFirePits| Refuel fire pits|
|restrictKilnOutput| Restrict kiln output|
|restrictKilnOutputAmount| Amount of coal to shut off kiln fueling|
|distributedFilling| If true, refilling will occur one piece of fuel or ore at a time, making filling take longer but be better distributed between objects|
|leaveLastItem| Don't use last of item in chest|
|StackSmelters| Allow the ability to stack smelters and kilns.  Turns off the smoke and prohibits the blocked smoke check.|
|RestrictKiln| Turn off the Kiln|
|RefuelHotTub| Turn on and off refueling of hottub.. bathtub whatever|
|RefuelHearth| Turn on and off the refueling of the hearth|
|RefuelOvens| Turn on and off the refueling of stone ovens and other fuelled cooking stations|
|OvenRange| The maximum range to pull fuel from containers for ovens|

Custom Toggle key to turn on and off mod in-game. 

You can adjust the ranges for containers and ground pulling (default 10 meters each).

Reset the config by opening the in-game console `(F5)` and typing autofuel reset and pressing Enter.

___________________________
#### Installation: (manual)  

Extract DLL from zip file into `"<GameDirectory>\Bepinex\plugins"`  
Start the game.
___________________________
#### Installation (Automatic)
Use the R2Modmanager on Thunderstore.  Search for the mod and install
___________________________

#### Server Configuration
``````
For Servers that want to control ther configuration, install on the server and clients.  
The server config will push down.

For Servers that cannot use mods, simply install this on the clients only and it will work just fine.  Clients will all need
the same settings in their config files for optimal results.

For people that run a server and don't want to verify clients turn the "verifyclients" setting off.
``````
### Version Information
___________________________

1.4.7

option to disable blast furnace and smelter default is on
Added ability to turn off using floor drops for fuel

1.4.6

Updated for newest patch and added Shield Generators


1.4.5

Updated server sync

1.4.4

Updated for The Bog Witch version 


1.4.3

Updated for newest version
Added Iron Ore to the list of items to be made in Blast Furnace
-- Thanks EiraValkyrie for reporting the problem

1.4.2

removed for errors in build


1.4.1 

Updated for final release of Ashlands.



1.4.0

updated for Valheim 0.218.12  Ashlands PTB  

THIS VERSION WILL NOT WORK IN ANYTHING BELOW 0.218
Use the older version for non Ashalands Versions.



1.3.9

- updated for Valheim 0.271.22


1.3.8

- Updated to the newest Haldor's Quest Version

1.3.7

- Updated to newest Test Branch of Valheim 
- Increased amount of selectable coal before turning off kilns
- fixed a few spelling errors



1.3.6

- Updated to newest BepInEx and Valheim Patch 0.214.300



1.3.5

- updated to the newest version of Valheiml 0.214.2
- added back the toggle to turn off and on Automatic Fueling.

1.3.4 - fixed several RPC bugs and bumped version for incorrect README.MD


1.3.0

- add ability to turn off Spinning Wheel and Windmills


1.2.0

- Added ability to turn off Hearth and BathTub.  This allows the mod to coexists with other mods like BetterWards 

1.1.9

- Fixed bug in turning off Kilns.  When kilns were turned off all "Smelters" stopped working.


1.1.8

- Option to turn off Kilns
- Ability to pull from carts
- Added ownership checks to containers for multiplayer duping


1.1.7

- Added option for Blast Furnace to take all fuel.  Default is on.
- Lots of code cleanup.

1.16

- Fix for smelter oject not set to an instance error.  Widmills and Spinning Wheels are smelters and they were causing errors when checking their smoke.


1.15

- Enabled the ability to turn off the verification of clients.  This keeps the server from kicking players that do not have the mod.
- Added the ability to stack smelters and kilns.  

1.1.4

- Added ServerSync - Install on the Clients and Server and the Server will control the config.
- fine tuned the container code to include custom chests and mods
- confirmed working with Drawers Mod.

1.1.3

- Fixed the black iron chests and custom chest.  Can now pull from all containers
- Confirmed working with Eitr Refinery

1.1.2

- Fixed the toggle key

1.1.0

- added the options for torches, campfires and hearths

1.0.1

- initial release

##	Now for the shameless plug

> ### My Other Mods:
>>* [No Smoke Stay Lit](https://valheim.thunderstore.io/package/TastyChickenLeg/NoSmokeStayLit/)
>>* [No Smoke Simplified](https://valheim.thunderstore.io/package/TastyChickenLegs/NoSmokeSimplified/)
>>* [Honey Please](https://valheim.thunderstore.io/package/TastyChickenLegs/HoneyPlease/)
>>* [Automatic Fuel](https://valheim.thunderstore.io/package/TastyChickenLeg/AutomaticFuel/)
>>* [Forsaken Powers Plus](https://valheim.thunderstore.io/package/TastyChickenLeg/ForsakenPowersPlus/)
>>* [Recycle Plus](https://valheim.thunderstore.io/package/TastyChickenLeg/RecyclePlus/)
>>* [Blast Furnace Takes All](https://valheim.thunderstore.io/package/TastyChickenLeg/BlastFurnaceTakesAll/)
>>* [Timed Torches Stay Lit](https://valheim.thunderstore.io/package/TastyChickenLeg/TimedTorchesStayLit/)
>>* [Drop More Loot](https://valheim.thunderstore.io/package/TastyChickenLegs/DropMoreLoot/)