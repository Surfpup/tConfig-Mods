/*
Copyright 2013 Surfpup

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
using System.IO;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace Effects
{
        /*public delegate bool Requirement(Item item);

        //Some common Item requirements
        Requirement armor = (Item item) => (item.accessory || item.bodySlot != -1 || item.legSlot != -1 || item.headSlot != -1);
        Requirement melee = (Item item) => item.melee;
        Requirement ranged = (Item item) => item.ranged;
        Requirement magic = (Item item) => item.magic;
        Requirement proj = (Item item) => item.ranged || item.magic;
        Requirement weapon = (Item item) => item.melee || item.ranged || item.magic;*/

    public class Effect<T> where T:Codable
    {
        public delegate bool CanUse_Del(Player p, int i);
        public delegate void OnSpawn_Del(Player p, int i);
        public delegate bool PreShoot_Del(Player P, Vector2 ShootPos, Vector2 ShootVelocity, int projType, int Damage, float knockback, int owner);
        public delegate void DealtNPC_Del(Player myPlayer, NPC npc, double damage);
        public delegate void UpdatePlayer_Del(Player myPlayer);
        public delegate void DealtPlayer(Player myPlayer, double damage, NPC npc);
        public delegate void DamagePlayer_Del(Player p, ref int d, NPC npc);

        public delegate bool Requirement(T item);

        public enum Colors{Normal,Green,Red};

        public virtual string name {set; get;} //Name of the effect. Might be used in description display.
        public List<MouseTip> toolTips; //Tooltips, or lines of text to describe the effect
        public virtual int numVals {set;get;} //Number of random values this effect uses

        public string type; //The name of this type of effect, specified in the effect definition

        public T item;

        public Effect(T item)
        {
            toolTips = new List<MouseTip>();
            this.item = item;
        }

        public virtual void Initialize()
        { //Apply changes (when item is spawned)

        }

        //If the effect wants to have some save state, it can
        public virtual void Load(BinaryReader reader, int version)
        {

        }

        public virtual void Save(BinaryWriter writer)
        {
            
        }

        public virtual void Load(float[] vals)
        { //Important function for initializing effect with specified values

        }

        public void AddTooltip(string text, Colors color)
        { //Simplifies things with the Colors enum
            if(color==Colors.Normal) this.toolTips.Add(new MouseTip(text, false, false));
            else if(color==Colors.Green) this.toolTips.Add(new MouseTip(text, true, false));
            else if(color==Colors.Red) this.toolTips.Add(new MouseTip(text, true, true));
        }

        public void AddDelegate(string name, Delegate addDel)
        { //Used to add hooks
            Delegate curDel = null;
            item.delegates.TryGetValue(name, out curDel);
            if (curDel != null)
            {
                Delegate newDel = Delegate.Combine(curDel, addDel);
                item.delegates[name] = newDel;
            }
            else item.delegates[name] = addDel;
        }
    }
}