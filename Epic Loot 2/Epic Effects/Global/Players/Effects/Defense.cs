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

namespace Effects.Players
{

    public class Defense : Effect<Player>
    {
        int amt;

        public Defense(Player p, int amt) : base(p)
        {
            this.amt = amt;
        }

        public static bool Check(Player p)
        {
            return true;
        }
        
        public override void Initialize()
        {
            if(amt<0) {
                base.AddTooltip("-"+amt+" Defense", Colors.Red);
                this.name = "Decreased Defense";
            } else {
                base.AddTooltip("+"+amt+" Defense", Colors.Green);
                this.name = "Increased Defense";
            }

            this.AddDelegate("UpdatePlayer", (Action<Player>) this.UpdatePlayer);
            //this.item.Register(ref this.item.UpdatePlayer, this, "UpdatePlayer");
            base.Initialize();
        }

        public void UpdatePlayer(Player p)
        {
            p.statDefense += amt;
        }
    }
}