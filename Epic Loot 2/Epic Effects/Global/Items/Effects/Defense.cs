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
using Effects;

namespace Effects.Items
{

    public class Defense : Effect<Item>
    {
        int amt;

        public Defense(Item item) : base(item)
        {

        }

        public static bool Check(Item item)
        {
            return (item.accessory || item.bodySlot != -1 || item.legSlot != -1 || item.headSlot != -1);;
        }
        
        public void Load(int amt)
        {
            //Percentage of mana increase
            this.amt = amt;

            if(amt<0) {
                base.AddTooltip("-"+amt+" Defense", Colors.Red);
                this.name = "Decreased Defense";
            } else {
                base.AddTooltip("+"+amt+" Defense", Colors.Green);
                this.name = "Increased Defense";
            }

            this.Initialize();
        }

        public override void Initialize()
        {
            this.AddDelegate("Effects", (Effects_Del) Effects);
        }

        public void Effects(Player p)
        {
            p.statDefense += amt;
        }

        public override void Load(float[] vals)
        {
            this.Load((int) vals[0]);
        }

        public override int numVals { set{} get { return 1; }}
    }
}