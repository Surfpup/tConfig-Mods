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

//Rand.cs: Useful methods related to generating random numbers

using System;
using System.Collections.Generic;
using System.Text;

using Terraria;

namespace Epic_Loot
{
    public class Rand
    {
        public static double Skew(double r = -1.0, float skewMod = 1f)
        {
            if (r == -1.0) r = ModGeneric.rand.NextDouble();
            float mod = ModGeneric.skewValue;
            r = Math.Pow(r, mod);
            r = Math.Pow(r, 1f - ModPlayer.magicFind);
            r = Math.Pow(r, skewMod);
            return r;
        }
        public static int SkewedRand(int min, int max, double r = -1.0, float skewMod = 1f)
        { //Higher values are more rare
            /*Use exponents to make higher values harder to achieve
            Cubing it makes last 10% pretty hard to get to.
            */
            int range = max - min;
            if (range == 0) return min;
            r = Skew(r, skewMod);
            double val = ((r * range) + min); //Normal value
            return (int)Math.Round(val, System.MidpointRounding.AwayFromZero);
        }
        public static int SkewedRand(int min, int max, float skewMod)
        {
            return SkewedRand(min, max, ModGeneric.rand.NextDouble(), skewMod);
        }
        public static float SkewedRand(float min, float max, double r = -1.0, float skewMod = 1f)
        { //Higher values are more rare
            /*Use exponents to make higher values harder to achieve
            Cubing it makes last 10% pretty hard to get to.
            */
            float range = max - min;
            if (range == 0f) return min;
            r = Skew(r, skewMod);
            double val = ((r * range) + min); //Normal value
            return (float)val;
        }
        public static float SkewedRand(float min, float max, float skewMod)
        {
            return SkewedRand(min, max, ModGeneric.rand.NextDouble(), skewMod);
        }
    }
}