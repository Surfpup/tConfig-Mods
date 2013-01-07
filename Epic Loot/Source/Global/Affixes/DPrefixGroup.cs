/*
Copyright 2012 Surfpup

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

namespace Epic_Loot
{
    public class DPrefixGroup : DPrefix
    {
        public List<DPrefix> prefixes;
        public DPrefixGroup(string name, bool suffix = false, IPrefix code = null)
            : base(name, suffix, code)
        {
            prefixes = new List<DPrefix>();
        }
        public override GPrefix Roll(float skewMod = 0f)
        {
            int index = ModGeneric.rand.Next(0, prefixes.Count);
            return prefixes[index].Roll(skewMod);
        }
        public void AddPrefixes(DPrefix[] add, Requirement req)
        {
            prefixes.AddRange(add);
            customRequirements.Add(req);
        }
    }
}