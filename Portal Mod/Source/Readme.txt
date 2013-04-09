Portal Mod - Created by Surfpup, Sprites by Yoraiz0r

-Each portal links to exactly one other portal of the same color.
-5 pairs of portals may be crafted
	-Portals must be crafted at a Demon Altar with the following: 1 Mana Crystal, 1 Life Crystal, and 24 Colored Gem
	-The color of the portal created depends on the color of the gem you use.
		-Red Portal: Ruby
		-Green Portal: Emerald
		-Blue Portal: Sapphire
		-Yellow Portal: Topaz
		-Purple Portal: Amethyst
	-Crafted Portals can be destroyed and placed as you would expect
-10 pairs of portals spawn at random locations during world generation. These portals cannot be destroyed or moved. (If they can, it's a bug!)
-When used, portals damage you by about 10% of your total health.
-At least one portal will always spawn in the Dungeon.
-Networking code implemented to make it work as expected in multiplayer

Changelog
v1.1.0
	-Merged the two 'versions' of the mod into one. Some portals will spawn, others must be crafted.
		-Added 10 new portals, which spawn during world generation and cannot be destroyed.
		-The other 5 portals are craftable
	
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