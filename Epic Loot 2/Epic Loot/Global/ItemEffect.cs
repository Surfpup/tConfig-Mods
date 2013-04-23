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

namespace Epic_Loot
{
    //Effects specifically designed for Items
    public class ItemEffect : Effect
    {
        public Item item;

        public ItemEffect(Item item)
        {
            this.item = item;
        }
        /*
        public virtual void SetItem(Item item)
        {
            this.item = item;
        }*/

        public virtual void Initialize()
        { //Apply changes (when item is spawned)

        }

        public virtual bool Check()
        { //Check requirements
            return true;
        }

        public virtual void Apply(Player player)
        { //This one gets called every frame
        }

        public void AddDelegate(string name, Delegate addDel)
        {
            Delegate curDel = null;
            item.delegates.TryGetValue(name, out curDel);
            if (curDel != null)
            {
                Delegate newDel = Delegate.Combine(curDel, addDel);
                item.delegates[name] = newDel;
            }
            else item.delegates[name] = addDel;
        }

        /*public MouseTip[] UpdateTooltip()
        { //Used to display player-modified stats
            List<MouseTip> tips = new List<MouseTip>();
            if (pAdd.defense != 0f)
            {
                string symbol = "+";
                if (pAdd.defense < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.defense + " Defense", true));

            }
            if (pAdd.crit != 0f)
            {
                string symbol = "+";
                if (pAdd.crit < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.crit + "% Critical Hit Chance", true));
            }
            if (pAdd.meleeCrit != 0f)
            {
                string symbol = "+";
                if (pAdd.meleeCrit < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.meleeCrit + "% Melee Crit Chance", true));
            }
            if (pAdd.rangedCrit != 0f)
            {
                string symbol = "+";
                if (pAdd.rangedCrit < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.rangedCrit + "% Ranged Crit Chance", true));
            }
            if (pAdd.magicCrit != 0f)
            {
                string symbol = "+";
                if (pAdd.magicCrit < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.magicCrit + "% Magic Crit Chance", true));
            }
            if (pAdd.mana != 0f)
            {
                string symbol = "+";
                if (pAdd.mana < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.mana + " Mana", true));
            }
            if (pAdd.damage != 0f)
            {
                string symbol = "+";
                if (pAdd.damage < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.damage*100), 2) + "% Damage", true));
            }
            if (pAdd.meleeDamage != 0f)
            {
                string symbol = "+";
                if (pAdd.meleeDamage < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.meleeDamage * 100), 2) + "% Melee Damage", true));
            }
            if (pAdd.rangedDamage != 0f)
            {
                string symbol = "+";
                if (pAdd.rangedDamage < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.rangedDamage * 100), 2) + "% Ranged Damage", true));
            }
            if (pAdd.magicDamage != 0f)
            {
                string symbol = "+";
                if (pAdd.magicDamage < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.magicDamage * 100), 2) + "% Magic Damage", true));
            }
            if (pAdd.moveSpeed != 0)
            {
                string symbol = "+";
                if (pAdd.moveSpeed < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.moveSpeed*100), 2) + "% Movement Speed", true));
            }
            if (pAdd.meleeSpeed != 0)
            {
                string symbol = "+";
                if (pAdd.meleeSpeed < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.meleeSpeed*100), 2) + "% Melee Speed", true));
            }
            tips.AddRange(toolTips);
            return tips.ToArray();
        }*/
    }
}