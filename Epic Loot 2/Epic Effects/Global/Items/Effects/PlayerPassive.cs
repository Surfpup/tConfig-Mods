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
    public class PlayerPassive : ItemEffect
    { //Simply applies a player effect
        Effect<Player> effect;

        public PlayerPassive(Item item, Effect<Player> effect) : base(item)
        {
            this.effect = effect;
        }
       
        public override void Initialize()
        {
            this.name = effect.name;

            foreach(MouseTip m in effect.toolTips)
            { //Add tooltips
                base.toolTips.Add(m);
            }

            //Register updateplayer method as effects for the item
            
            this.item.Register(ref this.item.Effects, effect, "UpdatePlayer");
            base.Initialize();
        }
    }
}