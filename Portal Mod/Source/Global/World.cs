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

//Netmessages
const int SENDLOCATIONS = 1;
const int PLACEPORTAL = 2;
const int DESTROYPORTAL = 3;

//Max number of portal pairs
public const int maxPortals = 15;

static int modIndex;

public static string[] portalTypes = new string[]{
//Craftable:
	"Blue Portal", "Green Portal", "Yellow Portal", "Purple Portal", "Red Portal",
//Spawned
	"Black Blue Portal", "Black Empty Portal", "Black Green Portal", "Black Magenta Portal", "Black Red Portal", "Black Yellow Portal", "Dark Orange Portal", "Magenta Portal", "Rainbow Portal", "Turquoise Portal"
	};

//

//We need to store the location of every portal in the world
static Vector2[] locA; //Two arrays for each set of colors. locA[0] teleports to locB[0]
static Vector2[] locB;

public static void Initialize(int modID) {
	locA = new Vector2[maxPortals];
	locB = new Vector2[maxPortals];
	modIndex = modID;
	
	//Console.WriteLine("ModWorld initialized");
	//Main.NewText("ModWorld Initialized");
}

public static void Teleport(int x, int y) {
	//Player is teleporting from x,y
	
	//So the player doesn't kill himself
	if((int)(Main.player[Main.myPlayer].statLifeMax * .1) >= Main.player[Main.myPlayer].statLife) return;
	string tileName=Main.tileName[Main.tile[x,y].type];
	int index = Array.IndexOf(portalTypes,tileName);
	if(index==-1) {
		Main.NewText("Error: Invalid tile name? "+tileName);
		return;
	}	
	Vector2 from = new Vector2(x, y);
	Vector2 dest = new Vector2();
	bool found=false;
	
	if(from.X == locA[index].X && from.Y==locA[index].Y && locB[index].X!=0f && locB[index].Y != 0f) {
		//if(!CheckPortal(locB[index], tileName)) locB[index] = new Vector2();
		//else {
			found=true;
			dest = locB[index];
		//}
	}
	else if(from.X == locB[index].X && from.Y==locB[index].Y && locA[index].X!=0f && locA[index].Y != 0f) {
		//if(!CheckPortal(locA[index], tileName)) locA[index] = new Vector2();
		//else {
			found=true;
			dest = locA[index];
		//}
	}
	if(!found) {
		Main.NewText("No matching portal found!");
		Main.NewText("Locations:");
		//For debugging purposes, print out entire arrays
		/*for(int i=0;i<maxPortals;i++) {
			Main.NewText("A: "+locA[i].X+","+locA[i].Y+", B:"+locB[i].X+","+locB[i].Y);
		}*/
		return;
	}

	//Teleport!
	int featherLength=60;
	if(Main.netMode==1) featherLength+=100;
	Main.player[Main.myPlayer].AddBuff(8, 150, false); //Prevent fall damage
	Main.player[Main.myPlayer].position.X = (dest.X * 0x10) + 8; //- (player.width / 2);
	Main.player[Main.myPlayer].position.Y = ((dest.Y+3) * 0x10) - Main.player[Main.myPlayer].height;
	
	//Main.player[Main.myPlayer].statLife -= (int)(Main.player[Main.myPlayer].statLifeMax * .1); //Subtract 10% of max life
	Main.player[Main.myPlayer].Hurt((int)(Main.player[Main.myPlayer].statLifeMax * .01 * (Main.player[Main.myPlayer].statDefense/2)), 0);
	
	Main.player[Main.myPlayer].AddBuff(23, 360, false); //Curse - Can't use items for a little while
	//Paralyze the player into place for a bit while loading tiles
	if(Main.netMode==1) Main.player[Main.myPlayer].AddBuff("Portal Sickness", 120, false);
	//TODO: Add 'portal sickness' ?
}
public static bool CheckPortal(Vector2 loc, string tileName) {
	bool tileExists = Main.tile[(int)loc.X,(int)loc.Y]!=null && Main.tile[(int)loc.X,(int)loc.Y].active;
	if(tileExists && Main.tile[(int)loc.X,(int)loc.Y].type == Config.tileDefs.ID[tileName]) return true;
	return false;
}
public static bool CanPlacePortal(string name) {
	for(int i=0;i<maxPortals;i++) {
		if(portalTypes[i]==name) {
			if(locA[i].X != 0f && locA[i].Y != 0f && locB[i].X!=0f && locB[i].Y != 0f) {
				Main.NewText("There are already two portals of this type");
				return false;
			}
			else return true;
		}
	}
	Main.NewText("Error: Invalid tile name? "+name);
	return false;
}
public static void PlacePortal(int x, int y) {
	if(Main.netMode==1) return;
	string name=Main.tileName[Main.tile[x,y].type];
	int i = Array.IndexOf(portalTypes,name);
	if(locA[i].X == 0 && locA[i].Y == 0) {
		//if(Main.netMode==1) NetMessage.SendModData(modIndex, PLACEPORTAL, -1, -1, (byte)0, (byte) i, x, y);
		locA[i] = new Vector2(x, y);
		NetMessage.SendModData(modIndex, PLACEPORTAL, -1, -1, (byte)0, (byte) i, x, y);
		Main.NewText("Portal "+i+"A Initialized");
		if(Main.netMode==2) NetMessage.SendData(25, -1, -1, "Portal "+i+"A Initialized", 255, 225f, 25f, 25f, 0);
	}
	else if(locB[i].X==0 && locB[i].Y == 0) {
		//if(Main.netMode==1) NetMessage.SendModData(modIndex, PLACEPORTAL, -1, -1, (byte)1, (byte) i, x, y);
		locB[i] = new Vector2(x, y);
		NetMessage.SendModData(modIndex, PLACEPORTAL, -1, -1, (byte)1, (byte) i, x, y);
		Main.NewText("Portal "+i+"B Initialized");
		if(Main.netMode==2) NetMessage.SendData(25, -1, -1, "Portal "+i+"B Initialized", 255, 225f, 25f, 25f, 0);
	}
	else {
		Main.NewText("There are already two portals of this type");
		if(Main.netMode==2) NetMessage.SendData(25, -1, -1, "There are already two portals of this type", 255, 225f, 25f, 25f, 0);
	}
	//return;
	//Main.NewText("Error: Invalid tile name? "+name);
	//if(Main.netMode==2) NetMessage.SendData(25, -1, -1, "Error: Invalid tile name? "+name, 255, 225f, 25f, 25f, 0);
}

//This isn't running on the server >.<
public static void DestroyPortal(int x, int y) {
	//if(Main.netMode==1) return;
	if(Main.netMode==2) return; //Just to be sure
	string tileName=Main.tileName[Main.tile[x,y].type];
	int i = Array.IndexOf(portalTypes,tileName);
	Vector2 target=new Vector2(x,y);
	if(locA[i].X == target.X && locA[i].Y == target.Y) {
		if(Main.netMode==1) NetMessage.SendModData(modIndex, DESTROYPORTAL, -1, -1, (byte)0, (byte) i);
		else DestroySpecifiedPortal((byte)0, i);
		//locA[i] = new Vector2();
		//NetMessage.SendModData(modIndex, DESTROYPORTAL, -1, -1, (byte)0, (byte) i);
		//Main.NewText("Portal "+i+"A Destroyed");
		//if(Main.netMode==2) NetMessage.SendData(25, -1, -1, "Portal "+i+"A Destroyed", 255, 225f, 25f, 25f, 0);
	}
	else if(locB[i].X==target.X && locB[i].Y == target.Y) {
		if(Main.netMode==1) NetMessage.SendModData(modIndex, DESTROYPORTAL, -1, -1, (byte)1, (byte) i);
		else DestroySpecifiedPortal((byte)1, i);
		//locB[i] = new Vector2();
		//NetMessage.SendModData(modIndex, DESTROYPORTAL, -1, -1, (byte)1, (byte) i);
		//Main.NewText("Portal "+i+"B Destroyed");
		//if(Main.netMode==2) NetMessage.SendData(25, -1, -1, "Portal "+i+"B Destroyed", 255, 225f, 25f, 25f, 0);
	}
	else {
		Main.NewText("Error: There was no portal data to remove.");
		//if(Main.netMode==2) NetMessage.SendData(25, -1, -1, "Error: There was no portal data to remove.", 255, 225f, 25f, 25f, 0);
	}
	//return;
	//Main.NewText("Error: Invalid tile name? "+name);
	//if(Main.netMode==2) NetMessage.SendData(25, -1, -1, "Error: Invalid tile name? "+name, 255, 225f, 25f, 25f, 0);
}
/*public void Save(BinaryWriter writer) {
	for(int i=0;i<maxPortals;i++) {
		writer.Write((int) locA[i].X);
		writer.Write((int) locA[i].Y);
		writer.Write((int) locB[i].X);
		writer.Write((int) locB[i].Y);
	}
}
public void Load(BinaryReader reader) {
	for(int i=0;i<maxPortals;i++) {
		locA[i].X = (float)reader.ReadInt32();
		locA[i].Y = (float)reader.ReadInt32();
		if(Main.netMode!=1 && !CheckPortal(locA[i], portalTypes[i])) locA[i] = new Vector2();
		locB[i].X = (float)reader.ReadInt32();
		locB[i].Y = (float)reader.ReadInt32();
		if(Main.netMode!=1 && !CheckPortal(locB[i], portalTypes[i])) locB[i] = new Vector2();
	}
}*/
public void PlayerConnected(int playerID) {
	NetMessage.SendData(100, playerID, -1, "", modIndex, SENDLOCATIONS);
}
public void NetSend(int msg, BinaryWriter writer) {
	if(msg==SENDLOCATIONS) {
		for(int i=0;i<maxPortals;i++) {
			writer.Write((int) locA[i].X);
			writer.Write((int) locA[i].Y);
			writer.Write((int) locB[i].X);
			writer.Write((int) locB[i].Y);
		}
	}
}
public void NetReceive(int msg, BinaryReader reader) {
	if(msg==SENDLOCATIONS) {
		locA = new Vector2[maxPortals];
		locB = new Vector2[maxPortals];
		for(int i=0;i<maxPortals;i++) {
			locA[i].X = (float)reader.ReadInt32();
			locA[i].Y = (float)reader.ReadInt32();
			locB[i].X = (float)reader.ReadInt32();
			locB[i].Y = (float)reader.ReadInt32();
			//Codable.InitTile(locA[i], Main.tile[(int)locA[i].X, (int)locA[i].Y].type);
			//Codable.InitTile(locB[i], Main.tile[(int)locA[i].X, (int)locA[i].Y].type);
		}
	}
	else if(msg==PLACEPORTAL) {
		byte array = reader.ReadByte();
		int index = (int)reader.ReadByte();
		int x = reader.ReadInt32();
		int y = reader.ReadInt32();
		if(array==0) {
			locA[index] = new Vector2(x, y);
			Main.NewText("Portal "+index+"A Initialized");
		}
		else {
			locB[index] = new Vector2(x, y);
			Main.NewText("Portal "+index+"B Initialized");
		}
		//if(Main.netMode==2) //Broadcast to everyone
		//	NetMessage.SendModData(modIndex, PLACEPORTAL, -1, -1, (byte)array, (byte) index, x, y);
	}
	else if(msg==DESTROYPORTAL) {
		byte array = reader.ReadByte();
		int index = (int) reader.ReadByte();
		DestroySpecifiedPortal(array, index);
	}
}
public static void DestroySpecifiedPortal(byte array, int index) {
	if(array==0) {
		locA[index] = new Vector2();
		Main.NewText("Portal "+index+"A Destroyed");
	}
	else {
		locB[index] = new Vector2();
		Main.NewText("Portal "+index+"B Destroyed");
	}
	
	if(Main.netMode==2)
		NetMessage.SendModData(modIndex, DESTROYPORTAL, -1, -1, (byte)array, (byte) index);
}