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

**Coming from AutomaticFuel?** Your settings carry over. On first run OttoFuel looks
for `TastyChickenLegs.AutomaticFuel.cfg` and copies its values into its own file. It
never touches an OttoFuel config that already exists.

On a clean install OttoFuel writes a starter config with `LeaveLastItem = true`, so
it never empties a chest to zero. Every other setting keeps its stock value.

Every setting lives in `BepInEx/config/potto007.OttoFuel.cfg`, and you can change it in a
text editor or in game through Configuration Manager. `Enabled` and `IsOn` sit in the
unnamed section at the top of the file, which BepInEx writes as `[]`.

| Section | Key | Default | What it does |
| --- | --- | --- | --- |
| (unnamed) | `Enabled` | `true` | Turns the mod on or off. |
| (unnamed) | `IsOn` | `true` | Whether fueling is currently on. |
| General | `ToggleKey` | `F10` | The key that toggles fueling. Leave it blank to disable the key. |
| 1 - General | `Lock Configuration` | `On` | When on, only a server admin can change the settings. |
| General | `DropRange` | `15` | How far, 1 to 50 meters, to pull dropped fuel. |
| General | `Use Dropped Items for Fuel` | `true` | Use fuel lying on the ground, not only fuel in chests. |
| Fireplace | `FireplaceRange` | `5` | How far, 1 to 50 meters, to pull fuel from chests for fires. |
| Fireplace | `FuelDisallowTypes` | `RoundLog,FineWood` | Items never used as fuel, comma separated. |
| Fireplace | `RefuelStandingTorches` | `true` | Refuel standing torches. |
| Fireplace | `RefuelWallTorches` | `true` | Refuel wall torches. |
| Fireplace | `RefuelBraziers` | `true` | Refuel braziers. |
| Fireplace | `RefuelFirePits` | `true` | Refuel fire pits. |
| Fireplace | `RefuelHearth` | `true` | Refuel hearths. |
| Fireplace | `RefuelHotTub` | `true` | Refuel hot tubs. |
| Oven | `RefuelOvens` | `true` | Refuel the stone oven and any other cooking station that burns fuel. |
| Oven | `OvenRange` | `5` | How far, 1 to 50 meters, to pull fuel from chests for ovens. |
| Smelters | `SmelterOreRange` | `15` | How far, 1 to 50 meters, to pull ore from chests for smelters. |
| Smelters | `SmelterFuelRange` | `15` | How far, 1 to 50 meters, to pull fuel from chests for smelters. |
| Smelters | `OreDisallowTypes` | `RoundLog,FineWood` | Items never used as ore, comma separated. |
| Smelters | `LeaveLastItem` | `false` | Leave the last item in a chest instead of emptying it. A clean install writes `true`. |
| Smelters | `DistributedFueling` | `true` | Add one piece of fuel or ore at a time, so it spreads across every smelter. |
| Smelters | `AllowStackSmelters` | `false` | Let smelters and kilns stack, which drops the smoke and the smoke blocked check. |
| Smelters | `BlastFurnaceTakesAll` | `true` | Let the blast furnace take every ore. |
| Smelters | `RestrictKilnOutput` | `false` | Stop fueling kilns past a coal limit. |
| Smelters | `RestrictKilnOutputAmount` | `50` | The amount of coal, 1 to 1000, that shuts off kiln fueling. |
| Smelters | `Turn Off Kiln` | `false` | Leave kilns alone. |
| Smelters | `Turn off Smelter` | `false` | Leave smelters alone. |
| Smelters | `Turn off Blast Furnace` | `false` | Leave blast furnaces alone. |
| Smelters | `Turn Off Windmills` | `false` | Leave windmills alone. |
| Smelters | `Turn Off SpinningWheel` | `false` | Leave spinning wheels alone. |

___________________________
#### Installation: (manual)  

Extract the DLL from the zip file into `<GameDirectory>\BepInEx\plugins`, then start the
game.
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
