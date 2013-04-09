/*
Copyright 2012 Surfpup

This program is free software: you can redistribute it and/or modify
it under the terms of the GNU General Public License as published by
the Free Software Foundation, either version 3 of the License, or
(at your option) any later version.

This program is distributed in the hope that it will be useful,
but WITHOUT ANY WARRANTY; without even the implied warranty of
MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
GNU General Public License for more details.

You should have received a copy of the GNU General Public License
along with this program.  If not, see <http://www.gnu.org/licenses/>.
*/

/*
Altered findSurface code taken from Dogsonofawolf's Other Worlds mod
*/

static int tileWidth = Main.tile.GetUpperBound(0) - 1;
static int tileHeight = Main.tile.Length / tileWidth;
static int realTileWidth = tileWidth/2;
static int realTileHeight = tileHeight/2;
public const int BIOMEWIDTH=100 ;//ok, half-width, so sue me
public const int BIOMEHEIGHT=100;
public const int BIOMETHRESHOLD = 200;
int jungleDist = tileWidth/20;
int jungleX = WorldGen.JungleX;
int islandClearance = 150;

public static void ModifyWorld() {
	//Pick random x, y, search for nearest ground tile to place on
	int i=0;
	int dungeonPortal = WorldGen.genRand.Next(ModWorld.maxPortals);
	while(i<ModWorld.maxPortals) {
		if(i==dungeonPortal) {
			PlaceTileRandomXYOnDungeon(3, 4, Config.tileDefs.ID[ModWorld.portalTypes[i]]);
			PlaceTileRandomXY(3, 4, Config.tileDefs.ID[ModWorld.portalTypes[i]]);
		}
		else {
			//int y=WorldGen.genRand.Next((int)Main.worldSurface, Main.maxTilesY - 150);
			PlaceTileRandomXY(3, 4, Config.tileDefs.ID[ModWorld.portalTypes[i]]);
			PlaceTileRandomXY(3, 4, Config.tileDefs.ID[ModWorld.portalTypes[i]]);
		}
		i++;
	}
}

public static int PlaceTileRandomXY(int width, int height, int type)
{
	while(true) {
		int xPos=WorldGen.genRand.Next(100, Main.maxTilesX - 100);
		int randomYStart = WorldGen.genRand.Next(50, Main.maxTilesY - 150);
		for(int traceY = randomYStart;traceY < tileHeight/2; traceY++)
		{
			if(Main.tile[xPos, traceY] != null && Main.tile[xPos, traceY].active && Config.CheckPlaceTile(xPos, traceY-1, width, height, type))
			{
				WorldGen.PlaceTile(xPos, traceY-1, type, true);

				return traceY;
			}
		}
	}
	return -1;
}
public static int PlaceTileRandomXYOnDungeon(int width, int height, int type)
{
	while(true) {
		int xPos=WorldGen.genRand.Next(WorldGen.dungeonX-200, WorldGen.dungeonX+200);
		int randomYStart = WorldGen.genRand.Next(WorldGen.dungeonY+100, Main.maxTilesY - 150);
		for(int traceY = randomYStart;traceY < tileHeight/2; traceY++)
		{
			if(Main.tile[xPos, traceY] != null && Main.tile[xPos, traceY].active && Main.tileDungeon[Main.tile[xPos, traceY].type] && Config.CheckPlaceTile(xPos, traceY-1, width, height, type))
			{
				WorldGen.PlaceTile(xPos, traceY-1, type, true);
				return traceY;
			}
		}
	}
	return -1;
} 