Created by Surfpup

version 0.2.3

Description
-Overrides the vanilla prefix system entirely
-Items that currently have vanilla item prefixes should keep them, but back up your save files if you're worried about losing them.
-Dynamically generates affixes with random values
-Higher values are more rare than lower values; it will be very difficult to get the best affix combination
-Includes ~37 affixes (initial release)
-Adds up to 4 affixes to an item
-Affixes are applied whenever the game would normally apply a prefix. Reforging is possible currently (mostly for testing purposes), but may be disabled in the future.
-Adds a magic find stat that can be increased by affixes; the stat increases the likelihood of getting higher stat rolls for affixes, and increases the chances of getting multiple affixes.

To-do:
-Hook to allow other modders to create dynamic prefixes
-Determine how to decide what items monsters will drop, and what item drop rates will be. Could be handled in a separate mod.
-Synchronize magic find stat over multiplayer (add up or average players' values)
-Define environmental magic find modifiers; for instance, hardmode should increase magic find. Maybe different biomes will have different base magic find values as well.
-Add more affixes
-Add elemental damage to weapons with affixes, and to NPCs accordingly, based on what biome they spawned in and some randomness.
-Possibly add a method of adding desired affixes to items, using new ores, mostly as a way of rewarding exploration.

Ideas:
-Add Nephalem Valor-like system, and Elite monsters?

Changelog

v0.2.3
	-Updated to use new life variable in tConfig Beta 0.28.3h, should fix all HP affix issues
	-Fixed bug related to saving/loading NPC affixes
	
v0.2.2
	-Updated to make use of new networking hooks in beta 0.28.3g, fixes NPC syncing
	-In the default health system mode, HP affixes should be disabled (until I can figure out how to make them work)

v0.2.1
	-Added affixes:
		-'of Willpower' - Adds mana cost, increases damage
		-Vengeful - Increases damage based on how low your health is.
	-Added requirement for Painful affix to ensure that weapons that get the affix actually get bonus damage
	-Increased minimum bonus damage of Painful affix to 10%, altered some other affix minimums
	-Removed possibility of pickaxes, drills, hammers, or axes being dropped
	
v0.2.0
	-Added networking code for syncing NPC affixes
	-Added Leeching and Vampiric affixes for ranged/magic weapons
	-Fixed a out-of-range bug in loot drop function
	
v0.1.9
	-Nerfed NPC Rejuvenating affix
	-Began work on elemental damage/resistance, but code is disabled for now
	-Rearranged code utilizing new modpack builder feature (available in 0.28.2 beta) - the code is now managed as a visual studio solution.
	-Added 'Heavy' and 'Light' melee weapon affixes. Heavy reduces movement speed, and increases damage/knockback, while Light does the opposite. These may need to be rebalanced a bit.
	
v0.1.8
	-Fixed tooltip for Thirsty affix
	-Disabled Summoner affix for now
	-Balancing:
		-Nerfed loot a bit
		-Fixed loot for worms
		-Nerfed magic find affix
		-Magic find is now increased by 20% in a hardmode world
	-Added 'Stabby' melee weapon affix
	
v0.1.7
	-Added NPC affixes:
		-'Quick' - Runs the AI() method multiple times
		-'Rejuvenating' - Adds life regen. Might be overpowered or underpowered, haven't been able to test much yet.
		-'Summoner' - NPC Summons another npc of the same type semi-frequently
v0.1.6
	-Added rare chance of obtaining random weapon or armor loot from NPCs. Affixes for the loot and chance of dropping is affected by the NPC's affix power. The items dropped will have damage <= the NPC's damage and defense <= the NPC's defense.
	-Removed Color affix - I'm thinking about making all affixes affect color in some way instead...
	-Added Vampiric and Leeching affixes for weapons (get health or mana on hit)
	-Furthermore, by default you cannot get any extra health or mana from crystals. All health and mana increase must come from affixes.
		-It saves your player's original life stat so if you disable the mod, you will still have the health from before you used the mod.
		-You now start with 100 mana.
		-With the current +health and +mana affixes, you can get up to +320 health/mana (with 5 accessory slots) making the max be 420.
		-There is an option to play with default health/mana system, just toggle it in the tConfig Settings menu. This version should be compatible with other mods that add to health.
v0.1.5
	-Reorganized code
	-Added some basic NPC affixes
v0.1.4
	- Fixed Projectiles
v0.1.3
	-Nerfed some affixes, slightly upgraded some others
v0.1.1
	-Updated how magic find stat works. It acts as an exponent in the RNG formula, with the max being 1.
	-Reduced default exponent from 10 to 5 (essentially increases chances of getting better RNG values)
v0.1 - (11/11/12) - Initial Release
