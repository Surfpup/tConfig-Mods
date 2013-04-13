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

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace Epic_Loot
{
    public class ModGeneric
    {
        public const int MAX_AFFIX = 4; //Maximum of 4 affixes on an item
        public static float skewValue = 5.0f; //Once the magic find stat reachs 1 / skewValue, the RNG is normal distribution

        public static List<DPrefix> prefixes;
        public static Dictionary<string, DPrefix> prefixByName; // = new Dictionary<string, DPrefix>();

        public static List<NPCAffix> npcAffixes = new List<NPCAffix>();
        public static Dictionary<string, NPCAffix> npcAffixByName = new Dictionary<string, NPCAffix>();

        public static List<Color> colors;
        public static List<string> colorNames;

        public static Random rand = new Random();

        public static void OnLoad()
        {
	        //rand = ;
	        colors = new List<Color>();
	        colorNames = new List<string>();
	        var props = typeof(Color).GetProperties(System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.Static);
	        foreach (System.Reflection.PropertyInfo prop in props)
	        {
		        Color color = (Color) prop.GetValue(null, null);
		        colors.Add(color);
		        colorNames.Add(prop.Name);
	        }
            Console.WriteLine("Epic Loot OnLoad ran");
	        Item_Affixes.DefinePrefixes();
	        NPC_Affixes.DefineNPCAffixes();
        }
        public static void UpdateSpawn()
        {
	        for (int j = 0; j < 255; j++)
	        {
		        if (Main.player[j].active && !Main.player[j].dead)
		        {
			        Item item = Main.player[j].inventory[Main.player[j].selectedItem];
			        RunMethod(item, "UpdateSpawn");
			        for (int i = 0; i < 8; i++)
			        {
				        item = Main.player[j].armor[i];
				        RunMethod(item, "UpdateSpawn");
			        }
		        }
	        }
        }
        public static void RunMethod(Item item, string name, params object[] parameters)
        {
	        if(item.RunMethod("GetPrefixes"))
	        {
		        List<GPrefix> prefs = (List<GPrefix>) Codable.customMethodReturn;
		        foreach(GPrefix p in prefs)
		        {
			        if(p.delegates.ContainsKey(name))
			        {
				        p.delegates[name].DynamicInvoke(parameters);
			        }
		        }
	        }
        }
        public static void RunMethod(NPC npc, string name, params object[] parameters)
        { //Run an NPC affix method. All methods get the NPC passed along.
	        if(npc.RunMethod("GetPrefixes"))
	        {
		        object[] newParams = new object[parameters.Length + 1];
		        newParams[0] = npc;
		        for(int i=0;i<parameters.Length;i++) newParams[i+1] = parameters[i];
		        List<GNPCAffix> prefs = (List<GNPCAffix>) Codable.customMethodReturn;
		        foreach(GNPCAffix p in prefs)
		        {
			        if(p.delegates.ContainsKey(name))
			        {
				        p.delegates[name].DynamicInvoke(newParams);
			        }
		        }
	        }
        }
    }
}