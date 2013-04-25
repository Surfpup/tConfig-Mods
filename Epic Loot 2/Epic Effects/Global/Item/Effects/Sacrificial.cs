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
    /*
    Possible INI definition:
    [Effects]
    Sacrificial=(0.1,1),(4,1)

    Need to come up with streamlined way of utilizing custom INI settings.

    Furthermore, coming up with a streamlined way of doing it via code would be great.

    */
    public class Sacrificial : Effect<Item>
    {
        public HealthCost costAffix;
        public ManaPercent manaAffix;

        public Sacrificial(Item item) : base(item)
        {
            manaAffix = new ManaPercent(item);

            costAffix = new HealthCost(item);
        }

        /*public override void SetItem(Item item)
        {
            base.SetItem(item);

            manaAffix.SetItem(item);
            costAffix.SetItem(item);
        }*/

        public static bool Check(Item item)
        {
            return ManaPercent.Check(item) && HealthCost.Check(item);
        }

        public void Load(float percent, float healthPercent)
        {
            manaAffix.Load(percent);

            costAffix.Load( (int)Math.Round((double)(manaAffix.amt * healthPercent)) );
        }

        public override void Load(float[] vals)
        {
            this.Load(vals[0], vals[1]);
        }

        public override int numVals { set{} get { return 2; }}
    }
}