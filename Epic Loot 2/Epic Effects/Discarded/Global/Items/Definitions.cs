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
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;
using Effects;

namespace Effects.Items
{
    public class Definitions
    {
        public static List<Def<Item>> effects; //Stores all effects
        public static Dictionary<string, Def<Item>> effectsDict;

        public static void Define()
        {
        	effects = new List<Def<Item>>();
        	effects.Add(new Def<Item>(typeof(ManaPercent), ManaPercent.Check));
        	effects.Add(new Def<Item>(typeof(HealthCost), HealthCost.Check));
        	effects.Add(new Def<Item>(typeof(Sacrificial), Sacrificial.Check));
        }

        public static void Add(Def<Item> def)
        {
        	effects.Add(def);
        	effectsDict[def.name] = def;
        }
    }
}