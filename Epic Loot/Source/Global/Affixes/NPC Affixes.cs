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

namespace Terraria
{
    public class NPC_Affixes
    {
        //public const int MAX_SUMMONS = 10;
        //public static int summoned = 0; //Number of summoned mobs; limited!
        public delegate void Initialize_Del(NPC npc);

        static List<string> namedNPCs = new List<string>(new string[] {
		"Slimeling", "Slimer2", "Green Slime", "Pinky", "Baby Slime", "Black Slime", "Purple Slime",
		"Red Slime", "Yellow Slime", "Jungle Slime", "Little Eater", "Big Eater", "Short Bones", 
		"Big Boned", "Heavy Skeleton", "Little Stinger", "Big Stinger"
		});

        public static void DefineNPCAffixes()
        {
            ModGeneric.npcAffixByName = new Dictionary<string, NPCAffix>();
            ModGeneric.npcAffixes = new List<NPCAffix>();

            ModGeneric.npcAffixes.AddRange(
                new NPCAffix[]{ 
			new NPCAffix("Guarding").AddDel( "Initialize", (int val) => { Initialize_Del d = (NPC npc) => { npc.defense += val; }; return d; }, 1, 10),
			new NPCAffix("Stubborn").AddDel( "Initialize", (float val) => { Initialize_Del d = (NPC npc) => { npc.lifeMax = (int) ((float) npc.lifeMax * val); npc.life = npc.lifeMax; }; return d; }, 1.05f, 1.2f),
			new NPCAffix("Menacing").AddAffix("Angry", "Menacing").AddDel( "Initialize", (float val) => { Initialize_Del d = (NPC npc) => { npc.defDamage = (int)((float)npc.defDamage * val); npc.damage = npc.defDamage; }; return d; }, 1.05f, 1.2f),
			new NPCAffix("Large").AddAffix("Large", "Huge", "Hugemongous").AddDel( "Initialize", (float val) => { Initialize_Del d = (NPC npc) => { 
					npc.scale += val; 	
					if(namedNPCs.Contains(npc.name)) { //REQUIRED check if you want the hitboxes to be correct!
						npc.width = (int)((float)npc.width * (1f+val));
						npc.height = (int)((float)npc.height * (1f+val));
					}
				}; return d; }, 0.10f, 0.5f),
			new NPCAffix("Quick").AddDel( "PostAI", (int val) => { Initialize_Del d = (NPC npc) => { npc.RunMethod("MultipleAI", val); }; return d; }, 50, 100),
			new NPCAffix("Rejuvenating").AddDel("UpdateNPC", (int val) => { Initialize_Del d = (NPC npc) => { npc.lifeRegen+=val;	}; return d; }, 1, 20),

            //Disabled for lag/excessive summoning purposes
			/*new NPCAffix("Summoner").AddDel("PostAI", (int val) => { Initialize_Del d = (NPC npc) => { 
				if((Main.netMode == 2 || Main.netMode == 0) && ModGeneric.rand.Next(50000) < val) {
					NPC.NewNPC((int) npc.position.X, (int) npc.position.Y, npc.type);
				}
			}; return d; }, 1, 1000),*/
		});

            Codable.RunGlobalMethod("ModGeneric", "AddEpicNPCAffixes", ModGeneric.npcAffixes);
        }
    }
}