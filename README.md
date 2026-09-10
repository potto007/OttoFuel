![OttoFuel - Keep the Fires Burning](https://raw.githubusercontent.com/potto007/OttoFuel/master/docs/images/ottofuel-title.png)

# OttoFuel

### Updated for Valheim 1.0

OttoFuel keeps your production burning. It pulls fuel and ore out of nearby chests
and off the ground, then feeds them to the things that need them.

**Maintainer:** Paul Otto

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


`A config file BepInEx/config/potto007.OttoFuel.cfg is created after you run the game once with this mod.`

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

The full release history lives in [CHANGELOG.md](CHANGELOG.md), and it renders on
the Changelog tab of the Thunderstore package page.


## Credits

OttoFuel is maintained by **Paul Otto**.

It continues [AutomaticFuel](https://thunderstore.io/c/valheim/p/TastyChickenLegs/AutomaticFuel/)
by **TastyChickenLegs**, who built that mod on the idea and original code of
**Aedenthorn's** AutoFuel. Credit for the mod belongs to both of them. OttoFuel
carries their work forward under the same public-domain licence.

### More mods by TastyChickenLegs

> * [No Smoke Stay Lit](https://valheim.thunderstore.io/package/TastyChickenLeg/NoSmokeStayLit/)
> * [No Smoke Simplified](https://valheim.thunderstore.io/package/TastyChickenLegs/NoSmokeSimplified/)
> * [Honey Please](https://valheim.thunderstore.io/package/TastyChickenLegs/HoneyPlease/)
> * [Forsaken Powers Plus](https://valheim.thunderstore.io/package/TastyChickenLeg/ForsakenPowersPlus/)
> * [Recycle Plus](https://valheim.thunderstore.io/package/TastyChickenLeg/RecyclePlus/)
> * [Blast Furnace Takes All](https://valheim.thunderstore.io/package/TastyChickenLeg/BlastFurnaceTakesAll/)
> * [Timed Torches Stay Lit](https://valheim.thunderstore.io/package/TastyChickenLeg/TimedTorchesStayLit/)
> * [Drop More Loot](https://valheim.thunderstore.io/package/TastyChickenLegs/DropMoreLoot/)
