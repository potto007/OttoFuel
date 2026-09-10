# Changelog

All notable changes to OttoFuel. Versions from v1.4.7 down are the history of
AutomaticFuel by TastyChickenLegs, which OttoFuel continues.

## v1.5.2
- Added `CHANGELOG.md`, rendered on the Thunderstore package page.
- Raised the BepInEx pack dependency to the current release.
- Reworked the package categories.
- Tidied the README.

## v1.5.1
- New title banner and package icon.

## v1.5.0
- Renamed to OttoFuel. Forked from AutomaticFuel 1.4.7 by TastyChickenLegs.
- Added refuelling for the stone oven and any other cooking station that burns fuel.
- Added the config keys `Oven / RefuelOvens` and `Oven / OvenRange`.
- The config file is now `potto007.OttoFuel.cfg`. Settings do not carry over.

## v1.4.9
- Ported to the Valheim 1.0 API:
  - `Character.Message` gained a `log` parameter.
  - `Smelter.RPC_AddOre` gained a `cheated` parameter.
  - `Inventory.Changed` gained two parameters.
  - `Inventory.RemoveItem` changed a default; the old behaviour is pinned.
  - `ZRoutedRpc.Everybody` became a constant, so ServerSync was rebuilt.
- Rebuilt against the current Unity and game assemblies.

## v1.4.7

- option to disable blast furnace and smelter default is on
- Added ability to turn off using floor drops for fuel

## v1.4.6

- Updated for newest patch and added Shield Generators

## v1.4.5

- Updated server sync

## v1.4.4

- Updated for The Bog Witch version

## v1.4.3

- Updated for newest version
- Added Iron Ore to the list of items to be made in Blast Furnace
- Thanks EiraValkyrie for reporting the problem

## v1.4.2

- removed for errors in build

## v1.4.1

- Updated for final release of Ashlands.

## v1.4.0

- updated for Valheim 0.218.12  Ashlands PTB

- THIS VERSION WILL NOT WORK IN ANYTHING BELOW 0.218
- Use the older version for non Ashalands Versions.

## v1.3.9

- updated for Valheim 0.271.22

## v1.3.8

- Updated to the newest Haldor's Quest Version

## v1.3.7

- Updated to newest Test Branch of Valheim
- Increased amount of selectable coal before turning off kilns
- fixed a few spelling errors

## v1.3.6

- Updated to newest BepInEx and Valheim Patch 0.214.300

## v1.3.5

- updated to the newest version of Valheiml 0.214.2
- added back the toggle to turn off and on Automatic Fueling.

- 1.3.4 - fixed several RPC bugs and bumped version for incorrect README.MD

## v1.3.0

- add ability to turn off Spinning Wheel and Windmills

## v1.2.0

- Added ability to turn off Hearth and BathTub.  This allows the mod to coexists with other mods like BetterWards

## v1.1.9

- Fixed bug in turning off Kilns.  When kilns were turned off all "Smelters" stopped working.

## v1.1.8

- Option to turn off Kilns
- Ability to pull from carts
- Added ownership checks to containers for multiplayer duping

## v1.1.7

- Added option for Blast Furnace to take all fuel.  Default is on.
- Lots of code cleanup.

## v1.16

- Fix for smelter oject not set to an instance error.  Widmills and Spinning Wheels are smelters and they were causing errors when checking their smoke.

## v1.15

- Enabled the ability to turn off the verification of clients.  This keeps the server from kicking players that do not have the mod.
- Added the ability to stack smelters and kilns.

## v1.1.4

- Added ServerSync - Install on the Clients and Server and the Server will control the config.
- fine tuned the container code to include custom chests and mods
- confirmed working with Drawers Mod.

## v1.1.3

- Fixed the black iron chests and custom chest.  Can now pull from all containers
- Confirmed working with Eitr Refinery

## v1.1.2

- Fixed the toggle key

## v1.1.0

- added the options for torches, campfires and hearths

## v1.0.1

- initial release
