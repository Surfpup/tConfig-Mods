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

    public class ManaPercent : Effect<Item>
    {
        float percent;
        public int amt;

        public ManaPercent(Item item) : base(item)
        {

        }

        public static bool Check(Item item)
        {
            return item.magic;
        }
        
        public void Load(float percent)
        {
            //Percentage of mana increase
            this.percent = percent;

            if(percent<0)
                this.name = "Decreased Mana %";
            else this.name = "Increased Mana %";

            this.Initialize();
        }

        public override void Initialize()
        {
            amt = (int)Math.Round((double)((float)item.mana * percent));
            item.mana += amt;

            if(amt<0)
                base.AddTooltip("-"+Math.Round((double)percent*100f*-1,2)+"% ("+amt+") Mana Cost", Colors.Red);
            else
                base.AddTooltip("+"+Math.Round((double)percent*100f,2)+"% ("+amt+") Mana Cost", Colors.Green);
        }

        public override void Load(float[] vals)
        {
            this.Load(vals[0]);
        }

        public override int numVals { set{} get { return 1; }}
    }
}