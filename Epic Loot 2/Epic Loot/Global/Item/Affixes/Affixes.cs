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
    public class ItemAffixes
    {
        public static void DefineAffixes()
        {
            ModGeneric.itemAffixes = new List<ItemAffixDef>();

            new ItemAffixDef(typeof(Effects.Items.Sacrificial), new Stat(0.1f, 1f), new Stat(4f, 1f));
            new ItemAffixDef("Adept", typeof(Effects.Items.ManaPercent), new Stat(-0.01f, -0.20f));


            //ModGeneric.prefixes.Add(elementalDamage);
            Codable.RunGlobalMethod("ModGeneric", "AddEpicItemAffixes", ModGeneric.itemAffixes);
        }
    }
}