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

namespace Effects.Items
{
    public class HealthCost : ItemEffect
    {
        int cost;

        public HealthCost(Item item) : base(item)
        {

        }

        public static bool Check(Item item)
        {
            return true;
        }

        public void Initialize(int cost)
        {
            this.name = "Added Health Cost";  

            this.cost = cost;
            //this.AddDelegate("CanUse", (Func<Player, int, bool>) CanUse);
            this.item.Register(ref this.item.CanUse, this, "CanUse");
            base.AddTooltip("+"+cost+" Health Cost", Colors.Red);
            Main.NewText("Loaded HealthCost "+cost);
            base.Initialize();
        }

        public bool CanUse(Player p, int ind)
        { 
            if(cost>p.statLife) return false;

            float defMod = (p.statDefense/2f);
            int dmg = (int)((cost) + defMod);
            p.Hurt(dmg, 0);
            return true;
        }
    }
}