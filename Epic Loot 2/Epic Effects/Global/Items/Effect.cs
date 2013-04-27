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
    public class ItemEffect : Effect<Item>
    {
        public ItemEffect(Item item) : base(item)
        {

        }

        public override void Initialize()
        { //Apply changes (when item is spawned)
            foreach(MouseTip m in toolTips)
            {
                AddStaticTip(m);
            }
        }

        public void AddStaticTip(MouseTip m)
        { //Add to the tooltip variables. Not ideal but it should work well enough.
            if(String.IsNullOrEmpty(item.toolTip3)) item.toolTip3 = m.text;
            else if(String.IsNullOrEmpty(item.toolTip4)) item.toolTip4 = m.text;
            else if(String.IsNullOrEmpty(item.toolTip5)) item.toolTip5 = m.text;
            else if(String.IsNullOrEmpty(item.toolTip6)) item.toolTip6 = m.text;
            else if(String.IsNullOrEmpty(item.toolTip7)) item.toolTip7 = m.text;
        }
    }
}