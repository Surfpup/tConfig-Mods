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
using System.Collections;
using System.IO;
using Terraria;

namespace Epic_Loot
{
    public class Global_Item : CustomItem
    {
        public Global_Item(Item item)
            : base(item)
        {

        }

        List<GPrefix> prefixes;
        Prefix combined = null;
        public void Initialize()
        {
            prefixes = new List<GPrefix>();
            combined = new Prefix("Combined");
        }
        public void Save(BinaryWriter writer)
        {
            writer.Write(prefixes.Count);
            foreach (Prefix p in prefixes)
            {
                p.Save(writer);
            }
        }
        public void Load(BinaryReader reader, int v)
        {
            int num = reader.ReadInt32();

            prefixes = new List<GPrefix>();
            for (int i = 0; i < num; i++)
            {
                GPrefix p = new GPrefix("bla");
                p.Load(reader, v);
                if (p.identifier != "")
                    prefixes.Add(p);
            }
            if (num > 0) InitAffixes();
        }
        public bool OverridePrefix(int pre)
        {
            if (pre == -3)
            { //Return whether there are valid prefixes
                ArrayList valid = new ArrayList();
                for (int i = 0; i < ModGeneric.prefixes.Count; i++)
                {
                    if (ModGeneric.prefixes[i].Check(item)) valid.Add(i);
                }
                return valid.Count > 0; //ability to reforge
                //return true;
            }
            if (pre == 0) return true;
            if (pre > 0) return item.RealPrefix(pre);

            if (pre == -2)
            {
                //Reset item stats to clear prefixes
                Item i;
                if (this.item.name != null && Config.itemDefs.byName.TryGetValue(this.item.name, out i))
                {
                    Config.CopyAttributes(this.item, i);
                }
            }
            prefixes = new List<GPrefix>();
            int num = Rand.SkewedRand(0, ModGeneric.MAX_AFFIX); //Number of prefixes to add

            AssignPrefixes(num);
            //Combine all prefixes into one?

            InitAffixes();
            return true;
        }
        public void InitAffixes()
        {
            combined = new Prefix("Combined");

            float avgRand = 0f;
            float totalRand = 0f;
            foreach (GPrefix prefix in prefixes)
            {
                combined.pAdd.defense += prefix.pAdd.defense;
                combined.pAdd.crit += prefix.pAdd.crit;
                combined.pAdd.mana += prefix.pAdd.mana;
                combined.pAdd.damage += prefix.pAdd.damage;
                combined.pAdd.moveSpeed += prefix.pAdd.moveSpeed;
                combined.pAdd.meleeSpeed += prefix.pAdd.meleeSpeed;
                combined.pAdd.meleeDamage += prefix.pAdd.meleeDamage;
                combined.pAdd.rangedDamage += prefix.pAdd.rangedDamage;
                combined.pAdd.magicDamage += prefix.pAdd.magicDamage;
                combined.pAdd.meleeCrit += prefix.pAdd.meleeCrit;
                combined.pAdd.rangedCrit += prefix.pAdd.rangedCrit;
                combined.pAdd.magicCrit += prefix.pAdd.magicCrit;
                combined.playerMods.AddRange(prefix.playerMods);
                combined.itemMods.AddRange(prefix.itemMods);
                combined.customRequirements.AddRange(prefix.customRequirements);
                combined.toolTips.AddRange(prefix.toolTips);
                combined.dynamicTips.AddRange(prefix.dynamicTips);

                //'register' delegates
                foreach (string name in prefix.delegates.Keys)
                {
                    Delegate curDel = null;
                    item.delegates.TryGetValue(name, out curDel);
                    if (curDel != null)
                    {
                        Delegate newDel = Delegate.Combine(curDel, prefix.delegates[name]);
                        item.delegates[name] = newDel;
                    }
                    else item.delegates[name] = prefix.delegates[name];
                }

                avgRand += prefix.rarity;
                totalRand += 1f;
            }
            if (totalRand > 0f)
            {
                combined.AddTip("Rarity: " + avgRand);
                avgRand = avgRand / totalRand;
                int rare = Rand.SkewedRand(0, 6, avgRand);
                item.rare += rare;
            }
            item.prefix = 0;

            combined.Apply(item);
            if (item.rare < -1)
            {
                item.rare = -1;
            }
            if (item.rare > 6)
            {
                item.rare = 6;
            }
        }
        public void InitRandomAffixes(float skewMod = 1f)
        {
            int amt = Rand.SkewedRand(0, ModGeneric.MAX_AFFIX, skewMod);
            ArrayList valid = new ArrayList();
            for (int i = 0; i < ModGeneric.prefixes.Count; i++)
            {
                if (ModGeneric.prefixes[i].Check(item)) valid.Add(i);
            }
            for (int i = 0; i < amt; i++)
            {
                if (valid.Count == 0) return;
                int index = ModGeneric.rand.Next(0, valid.Count);
                int pre = (int)valid[index];
                prefixes.Add(ModGeneric.prefixes[pre].Roll(skewMod));
                valid.RemoveAt(index);
            }
            InitAffixes();
        }
        public void AssignPrefixes(int amt, float skewMod = 1f)
        {
            ArrayList valid = new ArrayList();
            for (int i = 0; i < ModGeneric.prefixes.Count; i++)
            {
                if (ModGeneric.prefixes[i].Check(item)) valid.Add(i);
            }
            for (int i = 0; i < amt; i++)
            {
                if (valid.Count == 0) return;
                int index = ModGeneric.rand.Next(0, valid.Count);
                int pre = (int)valid[index];
                prefixes.Add(ModGeneric.prefixes[pre].Roll(skewMod));
                valid.RemoveAt(index);
            }
        }
        public void AffixName(ref string name)
        {
            //Assign an affix for each stat that's modified, by a range
            //I.e. Large is +12-17% size, Massive is +18% and higher
            try {
                foreach (GPrefix prefix in prefixes)
                {
                    if (!name.Contains(prefix.affix))
                    {
                        if (!prefix.suffix) name = prefix.affix + " " + name;
                        else name = name + " " + prefix.affix;
                    }
                }
            } catch(Exception e) {
                //Main.NewText("AffixName Exception: "+e.Message);
            }
        }

        /*public void Effects(Player player)
        {
            combined.Apply(player);
            /*foreach(int p in prefixes)
            {
                Prefix prefix = Prefix.prefixes[p];
                prefix.Apply(player);
            }
        }*/
        public void Effects(Player player)
        {
            //Main.NewText("Effects run");
            combined.Apply(player);
        }

        /*public void DealtNPC(Player myPlayer, NPC npc, double damage)
        {
            RunPrefixMethod("DealtNPC", myPlayer, npc, damage);
        }
        public void UpdatePlayer(Player myPlayer)
        {
            RunPrefixMethod("UpdatePlayer", myPlayer);
        }
        public void OnSpawn(Player player, int pID)
        {
            RunPrefixMethod("OnSpawn", player, pID);
        }
        public void PreItemCheck(Player p, int i)
        {
            RunPrefixMethod("PreItemCheck", p, i);
        }
        public bool PreShoot(Player P,Vector2 ShootPos,Vector2 ShootVelocity,int projType,int Damage,float knockback,int owner)
        {
            if(RunPrefixMethod("PreShoot", P, ShootPos, ShootVelocity, projType, Damage, knockback, owner)) return false;
            return true;
        }*/
        /*public void OnEquip(Player p, int slot)
        {
            //RunPrefixMethod("OnEquip", p, slot);
        }
        public void OnUnequip(Player p, int slot)
        {
            //RunPrefixMethod("OnUnequip", p, slot);
        }*/
        public bool RunPrefixMethod(string name, params object[] parameters)
        {
            bool ran = false;
            foreach (GPrefix p in prefixes)
            {
                if (p.delegates.ContainsKey(name))
                {
                    p.delegates[name].DynamicInvoke(parameters);
                    ran = true;
                }
            }
            return ran;
        }
        public MouseTip[] UpdateTooltip()
        {
            //return new string[]{item.name+" "+item.type+" "+item.prefix};
            return combined.UpdateTooltip();
        }
        public List<GPrefix> GetPrefixes()
        {
            return prefixes;
        }
    }
}