/*
Created by Surfpup
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