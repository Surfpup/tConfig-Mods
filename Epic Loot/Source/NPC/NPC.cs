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
using System.IO;
using System.Collections;

using Terraria;

namespace Epic_Loot
{
    public class Global_NPC : CustomNPC
    {
        public Global_NPC(NPC npc)
            : base(npc)
        {

        }

        public const int MAX_DROP_AMT = 3;
        public static float itemChanceModifier = 10f;

        public bool netTransferred = false;
        List<GNPCAffix> affixes;

        public float rarity;

        public bool AIran = false;

        //public float[] elemental;
        public void Initialize()
        {
            AIran = false;
            affixes = new List<GNPCAffix>();
            //elemental = new float[(Enum.GetNames(typeof(ModGeneric.Elements)).Length)];
            rarity = 0f;
            if (Main.netMode != 1)
            {
                AssignAffixes(Rand.SkewedRand(1, ModGeneric.MAX_AFFIX));

                InitAffixes();
                //AssignElements();
            }

            //NetMessage.SendModData(ModWorld.modIndex, ModWorld.SYNC_AFFIXES, -1, -1, (byte)playerID, (byte)enemyID); //need to send player ID ...
        }
        public void InitAffixes()
        {
            if (this.npc.townNPC) return;
            if (String.IsNullOrEmpty(this.npc.displayName)) this.npc.displayName = this.npc.name;
            foreach (GNPCAffix p in affixes)
            {
                p.AffixName(ref this.npc.displayName);
                rarity += (p.rarity / 10f);
            }
            RunAffixMethod("Initialize");
        }
        public void AssignElements()
        {
            if (npc.townNPC) return;
            //int element = ModGeneric.rand.Next(elemental.Length);
            //npc.color = ModGeneric.elementColors[element];
            //elemental[element] += Rand.SkewedRand(0.5f, 1f);
        }
        /*public float GetElement(int element)
        {
            return elemental[element];
        }*/
        public void Save(BinaryWriter writer)
        {
            //if (!this.netTransferred)
            //{
                writer.Write((byte) affixes.Count);
                foreach (GNPCAffix p in affixes)
                {
                    p.Save(writer);
                }
                this.netTransferred = true;
                //Console.WriteLine("Saved affixes for NPC "+this.npc.name+","+this.npc.whoAmI);
            //}
        }
        public void Load(BinaryReader reader, int v)
        {
            if (!netTransferred)
            {
                int num = (int) reader.ReadByte();

                affixes = new List<GNPCAffix>();
                for (int i = 0; i < num; i++)
                {
                    GNPCAffix p = new GNPCAffix("bla");
                    p.Load(reader, v);
                    affixes.Add(p);
                }
                if (num > 0) InitAffixes();
                netTransferred = true;
                Main.NewText("Loaded " + num + " affixes for NPC "+this.npc.name);
            }
        }
        /*public bool PreAI()
        {
            int actualType = this.type;
            int pretendType = this.type;
            if (this.name != null && Config.npcDefs.aiPretendType.TryGetValue(this.name, out pretendType))
                this.type = pretendType;

            AIReal(ignoreCustomCode);

            //newcode
            //check to make sure the type wasn't modified...
            if (this.type == pretendType) this.type = actualType;
	
        }*/
        public void AssignAffixes(int amt)
        {
            if (this.npc.townNPC) return;
            ArrayList valid = new ArrayList();
            for (int i = 0; i < ModGeneric.npcAffixes.Count; i++)
            {
                //if (ModGeneric.prefixes[i].Check(item)) 
                valid.Add(i);
            }
            for (int i = 0; i < amt; i++)
            {
                if (valid.Count == 0) return;
                int index = ModGeneric.rand.Next(0, valid.Count);
                int pre = (int)valid[index];
                affixes.Add(ModGeneric.npcAffixes[pre].Roll());
                valid.RemoveAt(index);
            }
        }

        public void PostAI()
        {
            RunAffixMethod("PostAI");
            AIran = false;
        }

        public void UpdateNPC()
        {
            if (!netTransferred)
            {
                //Synchronize
                if (Main.netMode == 2)
                {//Client
                    //this.npc.life = this.npc.lifeMax;
                    MemoryStream sendStream = new MemoryStream();
                    BinaryWriter w = new BinaryWriter(sendStream);
                    this.Save(w);
                    NetMessage.SendModData(ModWorld.modIndex, ModWorld.SYNC_AFFIXES, -1, -1, this.npc.whoAmI, sendStream);
                    w.Close();
                }
            }
            RunAffixMethod("UpdateNPC");
        }

        public void MultipleAI(int val)
        {
            if (!AIran)
            {
                AIran = true;
                if (ModGeneric.rand.Next(100) <= val) npc.AI();
            }
        }

        public bool RunAffixMethod(string name, params object[] parameters)
        {
            if (affixes == null) return false;
            bool ran = false;

            object[] newParams = new object[parameters.Length + 1];
            newParams[0] = this.npc;
            for (int i = 0; i < parameters.Length; i++) newParams[i + 1] = parameters[i];

            foreach (GNPCAffix p in affixes)
            {
                if (p.delegates.ContainsKey(name))
                {
                    p.delegates[name].DynamicInvoke(newParams);
                    ran = true;
                }
            }
            return ran;
        }

        public void NPCLoot()
        { //Loot!
            if (npc.townNPC || npc.realLife != -1) return;

            int amt = Rand.SkewedRand(0, MAX_DROP_AMT, itemChanceModifier - (this.rarity * 10f));
            if (amt == 0) return;
            if (npc.boss) amt *= 3; //Bosses drop 3 times the stuff!
            List<Item> items = new List<Item>(Config.itemDefs.byName.Values);
            List<Item> valid = new List<Item>();

            foreach (Item item in items)
            {
                if (item.damage <= 0 && item.defense <= 0) continue; //Only drop weapons or armor (for now)
                if (item.damage <= (int)(npc.damage / 2f)
                    && item.defense <= (int)(npc.defense / 2f)
                    && item.maxStack == 1
                    && item.accessory == false
                    && item.pick == 0
                    && item.hammer == 0
                    && item.axe == 0
                    )
                {
                    valid.Add(item);
                }
            }
            //Sort valid items by damage and defense
            valid.Sort(delegate(Item t1, Item t2)
                { return ((t1.damage + t1.defense).CompareTo(t2.damage + t2.defense)); }
            );
            //List<int> toDrop = new List<int>();
            for (int i = 0; i < amt; i++)
            {
                if (valid.Count == 0) return;
                int Rindex = Rand.SkewedRand(0, valid.Count - 1, 1f - this.rarity); //Main.rand.Next(0, valid.Count);
                Item item = valid[Rindex];
                //toDrop.Add(ID);
                int index = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, item.type, 1, false, 0);
                Main.item[index].netDefaults(item.netID);
                Main.item[index].RunMethod("InitRandomAffixes", 1f - this.rarity);

                valid.RemoveAt(Rindex);
            }
            //DropItems(toDrop.ToArray());

            //Add to player's current MF bonus
            if(npc.boss) ModPlayer.magicFindBonus += 0.02f;
        }
        public void DropItems(params int[] items)
        { //Spawn the specified items
            foreach (int i in items)
            {
                int index = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, i, 1, false, 0);
                Main.item[index].RunMethod("InitRandomAffixes", 1f - this.rarity);
            }
        }
        public void DropItems(params string[] items)
        { //Spawn the specified items
            foreach (string i in items)
            {
                int index = Item.NewItem((int)npc.position.X, (int)npc.position.Y, npc.width, npc.height, i, 1, false, 0);
                Main.item[index].RunMethod("InitRandomAffixes", 1f - this.rarity);
            }
        }
    }
}