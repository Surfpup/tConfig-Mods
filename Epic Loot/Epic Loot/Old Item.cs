/*
Created by Surfpup.

This is the code I started with, which simply enables you to have multiple prefixes, and utilizes the vanilla prefixes. 
It was scrapped in favor of having completely new dynamic prefixes, but this code may still be useful for a different mod.
Also note, some of the code may be outdated.
*/
List<int> prefixes;
Prefix combined = null;
public void Initialize()
{
	prefixes = new List<int>();
	combined = new Prefix("Combined");
}
public void PostPrefix()
{
	prefixes = new List<int>();
	int num=4; //Number of prefixes to add
	int numObtained = 0; //Number of prefixes obtained
	if(item.prefix>0) {
		prefixes.Add(item.prefix);
		numObtained++;
	}
	int value=0; //Value sum, which will be averaged
	for(int i=0;i<num;i++)
	{
		item.RealPrefix(-1);
		if(item.prefix>0)
		{
			prefixes.Add(item.prefix);
			value+=item.value;
			numObtained++;
		}
	}
	//Combine all prefixes into one?
	
	if(numObtained>0) item.value = (int)(value/numObtained);
	
	combined = new Prefix("Combined");
	foreach(int p in prefixes)
	{
		Prefix prefix = Prefix.prefixes[p];
		combined.pAdd.defense += prefix.pAdd.defense;
		combined.pAdd.crit += prefix.pAdd.crit;
		combined.pAdd.mana += prefix.pAdd.mana;
		combined.pAdd.damage += prefix.pAdd.damage;
		combined.pAdd.moveSpeed += prefix.pAdd.moveSpeed;
		combined.pAdd.meleeSpeed += prefix.pAdd.meleeSpeed;
	}
	
	item.prefix = 0;
}
public void AffixName(ref string name)
{
	//Assign an affix for each stat that's modified, by a range
	//I.e. Large is +12-17% size, Massive is +18% and higher
	foreach(int p in prefixes)
	{
		Prefix prefix = Prefix.prefixes[p];
		if(!name.Contains(prefix.affix)) {
			if(!prefix.suffix) name = prefix.affix + " " + name;
			else name = name + " " + prefix.affix;
		}
	}
}
public void Effects(Player player)
{
	combined.Apply(player);
	/*foreach(int p in prefixes)
	{
		Prefix prefix = Prefix.prefixes[p];
		prefix.Apply(player);
	}*/
}
public string[] UpdateTooltip()
{
	//return new string[]{item.name+" "+item.type+" "+item.prefix};
	return combined.UpdateTooltip();
}
