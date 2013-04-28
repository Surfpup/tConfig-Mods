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

    public class Defense : ItemEffect
    {
        int amt;

        public Defense(Item item, int amt) : base(item)
        {
            this.amt = amt;
        }

        public static bool Check(Item item)
        {
            return (item.accessory || item.bodySlot != -1 || item.legSlot != -1 || item.headSlot != -1);
        }
        
        public override void Initialize()
        {
            if(!Check(item)) return;
            
            if(amt<0) {
                base.AddTooltip("-"+amt+" Defense", Colors.Red);
                this.name = "Decreased Defense";
            } else {
                base.AddTooltip("+"+amt+" Defense", Colors.Green);
                this.name = "Increased Defense";
            }

            this.item.Register(ref this.item.Effects, this, "Effects");
            base.Initialize();
        }

        public void Effects(Player p)
        {
            p.statDefense += amt;
        }
    }
}