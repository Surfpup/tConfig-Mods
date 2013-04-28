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
    /*
        Handler of Item Effects

        Effects could be affixes, or they could just be plain effects.
        Effects method Initialize() is called when initializing the item or adding the effect.
        Effects method Apply() is called within the items' Effects() method and affects the player.
        Effects can have save state, so they can save and load data.

        This handler needs to additionally store the ID of the effects,
        and upon loading, know which one to spawn.

    */
    public class Global_Item : CustomItem
    {
        public Global_Item(Item item)
            : base(item)
        {

        }

        List<ItemAffix> effects;

        public void Initialize()
        {
            effects = new List<ItemAffix>();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(effects.Count);
            foreach (ItemAffix p in effects)
            {
                p.Save(writer);
            }
        }

        public void Load(BinaryReader reader, int v)
        {
            int num = reader.ReadInt32();

            effects = new List<ItemAffix>();
            for (int i = 0; i < num; i++)
            {
                //Get the ID or the name of the effect
                int id = reader.ReadInt32();
                ItemAffix e = (ItemAffix) ModGeneric.itemAffixes[id].Gen(this.item);
                e.Load(reader, v);
                effects.Add(e);
            }
            if (num > 0) InitAffixes();
        }
        public bool OverridePrefix(int pre)
        {
            if (pre == -3)
            { //Return whether there are valid prefixes
                ArrayList valid = new ArrayList();
                for (int i = 0; i < ModGeneric.itemAffixes.Count; i++)
                {
                    if(ModGeneric.itemAffixes[i].Check(this.item))
                        valid.Add(i);
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
            effects = new List<ItemAffix>();
            int num = Rand.SkewedRand(0, ModGeneric.MAX_AFFIX); //Number of prefixes to add

            AssignAffixes(num);

            InitAffixes();
            return true;
        }

        public void InitAffixes()
        {
            item.prefix = 0;

            foreach(ItemAffix e in effects)
                e.Initialize();
        }

        public void AssignAffixes(int amt, float skewMod = 1f)
        {
            ArrayList valid = new ArrayList();
            for (int i = 0; i < ModGeneric.itemAffixes.Count; i++)
            {
                if(ModGeneric.itemAffixes[i].Check(this.item))
                    valid.Add(i);
            }
            for (int i = 0; i < amt; i++)
            {
                if (valid.Count == 0) return;
                int index = ModGeneric.rand.Next(0, valid.Count);
                int pre = (int)valid[index];
                ItemAffix a = (ItemAffix) ModGeneric.itemAffixes[pre].Gen(this.item); //Generate the affix
                a.InitRandom(skewMod); //Generate the random values
                effects.Add(a); //Add to list of effects
                valid.RemoveAt(index); //Remove from possible affixes
            }
        }
        /*public void AffixName(ref string name)
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
        }*/

        /*public void Effects(Player player)
        {
            //Main.NewText("Effects run");
            //combined.Apply(player);
            foreach(ItemEffect e in effects)
            {
                e.Apply(player);
            }
        }*/

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
       /* public bool RunPrefixMethod(string name, params object[] parameters)
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
        }*/
        public MouseTip[] UpdateTooltip()
        {
            List<MouseTip> tips = new List<MouseTip>();
            foreach(ItemAffix e in effects)
            {
                tips.AddRange(e.effect.toolTips);
            }

            return tips.ToArray();
        }
        public List<ItemAffix> GetEffects()
        {
            return effects;
        }
    }
}