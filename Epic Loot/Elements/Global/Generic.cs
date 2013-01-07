public enum Elements { Fire, Cold, Holy, Corrupt, Water, Nature, Earth };
public static Color[] elementColors = new Color[] { Color.Red, Color.Aqua, Color.Gold, Color.DarkMagenta, Color.Blue, Color.Green, Color.Brown };

public void AddEpicItemAffixes(List<Epic_Loot.DPrefix> prefixes) {
	Console.WriteLine("Defining Element Affixes...");
	Prefix.Requirement armor = (Item item) => { return (item.accessory || item.bodySlot != -1 || item.legSlot != -1 || item.headSlot != -1); };
	Prefix.Requirement melee = (Item item) => { return item.melee; };
	Prefix.Requirement ranged = (Item item) => { return item.ranged; };
	Prefix.Requirement magic = (Item item) => { return item.magic; };
	Prefix.Requirement proj = (Item item) => { return item.ranged || item.magic; };
	Prefix.Requirement weapon = (Item item) => { return item.melee || item.ranged || item.magic; };
	
	Epic_Loot.DPrefixGroup elementalDamage = new Epic_Loot.DPrefixGroup("Elemental");
	elementalDamage.AddPrefixes(
		new Epic_Loot.DPrefix[]{
			new Epic_Loot.DPrefix("Flaming").DMod( (int val) => { return (Item item) => { item.RunMethod("IncreaseElement", (int) ModGeneric.Elements.Fire, val); }; } , (int val) => { return new MouseTip("+"+val+" Fire Damage", true); }, 1, 20),
			new Epic_Loot.DPrefix("Freezing").DMod( (int val) => { return (Item item) => { item.RunMethod("IncreaseElement", (int) ModGeneric.Elements.Cold, val); }; } , (int val) => { return new MouseTip("+"+val+" Cold Damage", true); }, 1, 20),
			new Epic_Loot.DPrefix("Corrupt").DMod( (int val) => { return (Item item) => { item.RunMethod("IncreaseElement",(int)  ModGeneric.Elements.Corrupt, val); }; } , (int val) => { return new MouseTip("+"+val+" Corrupt Damage", true); }, 1, 20),
			new Epic_Loot.DPrefix("Earthly").DMod( (int val) => { return (Item item) => { item.RunMethod("IncreaseElement", (int) ModGeneric.Elements.Earth, val); }; } , (int val) => { return new MouseTip("+"+val+" Earth Damage", true); }, 1, 20),
			new Epic_Loot.DPrefix("Holy").DMod( (int val) => { return (Item item) => { item.RunMethod("IncreaseElement", (int) ModGeneric.Elements.Holy, val); }; } , (int val) => { return new MouseTip("+"+val+" Holy Damage", true); }, 1, 20),
			new Epic_Loot.DPrefix("Nature's").DMod( (int val) => { return (Item item) => { item.RunMethod("IncreaseElement", (int) ModGeneric.Elements.Nature, val); }; } , (int val) => { return new MouseTip("+"+val+" Nature Damage", true); }, 1, 20),
			new Epic_Loot.DPrefix("Water").DMod( (int val) => { return (Item item) => { item.RunMethod("IncreaseElement", (int) ModGeneric.Elements.Water, val); }; } , (int val) => { return new MouseTip("+"+val+" Water Damage", true); }, 1, 20),
		}, melee
		);
	prefixes.Add(elementalDamage);
}