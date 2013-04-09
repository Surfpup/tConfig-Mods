Portal Mod - Created by Surfpup, Sprites by Yoraiz0r

-Each portal links to exactly one other portal of the same color.
-Portals cannot be crafted. They must be found in the world. When you generate a new world, portals will randomly spawn!
-Portals cannot be destroyed. We don't want them to be too overpowered, do we?
-When used, portals take a way a percentage of your health.
-At least one portal will always spawn in the Dungeon.
-Networking code implemented to make it work as expected in multiplayer
-I worked hard to get this to work in multiplayer, but there may still be bugs in that regard.

-Portal Mod - Craftable: Alternate version for those too lazy to go searching for a portal
	-An alternate version that does not spawn portals in the world, but lets you craft them instead.
	-Portals must be crafted at a Demon Altar with the following: 1 Mana Crystal, 1 Life Crystal, and 24 Colored Gem
		-The color of the portal created depends on the color of the gem you use.
			-Red Portal: Ruby
			-Blue Portal: Sapphire
			-Green Portal: Emerald
			-Purple Portal: Amethyst
			-Yellow Portal: Topaz
	-Portals can be destroyed and placed as you would expect


Changelog
v1.0.7
	-Added light to the portals. Works only in tConfig 0.27.1+
	
v1.0.6
	-Code updated to use new features implemented in tConfig 0.24
	
v1.0.5
	-Fixed bug causing it to crash in multiplayer

v1.0.4
	-Removed save/load functions and instead used an Initialize() method. This ensures that your save doesn't get screwed up in the chance that anything weird happens!
	-Added code to prevent player from placing more than two of the same portal
	-Rebuilt with fixes made to the modpack builder to fix some errors related to destroying tiles.
	
v1.0.3
	-Updated Tile functions to use correct coordinates
	-Added code to prevent portals from being destroyed
	-Created Craftable version
	
v1.0.2
	-Fixed bug related to portal location checking.
	
v1.0.1
	-Fixed bug where it wouldn't unset the portal location when destroying a portal in single player
	-Added a check that runs when loading the world; it will reset portal locations if they are invalid
	-tConfig version 0.23 required

v1.0 - Initial Release