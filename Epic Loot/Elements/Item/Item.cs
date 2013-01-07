int[] elemental;
public void Initialize()
{
	elemental = new int[Enum.GetNames(typeof(ModGeneric.Elements)).Length];
}
public void IncreaseElement(int element, int amt)
{
	elemental[element] += amt;
}
public void DamageNPC(Player myPlayer, NPC npc, ref int damage, ref float knockback)
{
	//Apply elemental damage
	for (int i = 0; i < elemental.Length; i++)
	{
		if (elemental[i] > 0)
		{
			float npcElement = 0f;
			if (npc.RunMethod("GetElement", i))
			{
				npcElement = (float) Codable.customMethodReturn;
			}
			damage += (int)(elemental[i] * (1f - npcElement));
			//npc.HitEffect(myPlayer.direction, dmg);
			//npc.StrikeNPC(dmg, 0f, myPlayer.direction, false);
		}
	}
}