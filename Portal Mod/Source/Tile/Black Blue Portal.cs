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

public static void UseTile(Player player, int x, int y)
{
	Vector2 pos = Codable.GetPos(new Vector2(x,y));

	ModWorld.Teleport((int) pos.X, (int) pos.Y); //Attempt to teleport!
	//Main.NewText("Attempted Teleport");
}
public static void Initialize(int x, int y)
{ //This method will get called when the world is loaded, so we shouldn't have to worry about saving portal locations separately.
	Vector2 pos = Codable.GetPos(new Vector2(x,y));

	//Set the location
	ModWorld.PlacePortal((int) pos.X, (int) pos.Y);
}
public static void KillTile(int x, int y)
{
	Vector2 pos = Codable.GetPos(new Vector2(x,y));
	
	ModWorld.DestroyPortal((int) pos.X, (int) pos.Y);
}
public static bool CanDestroyTile(int x, int y) {
	//These can't be moved!
	return false;
}
public static bool CanExplode(int x, int y, int proj) {
	return false;
}