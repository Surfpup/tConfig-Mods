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
using System.Collections;
using System.IO;

using Terraria;

namespace Epic_Loot
{
    public class ModPlayer
    {
        public static float magicFind = 0f; //Helps find better items!

        public static float magicFindBonus = 0f; //Bonus from current playthrough

        public static void PreUpdatePlayer(Player p)
        {
            magicFind = magicFindBonus;

            //Calculate base magic find

            //Environmental effects
            if (Main.hardMode) magicFind += .2f;

            //Bosses
            if (NPC.downedBoss1) magicFind += 0.05f;
            if (NPC.downedBoss2) magicFind += 0.05f;
            if (NPC.downedBoss3) magicFind += 0.05f;

            if (magicFind > 1f) magicFind = 1f;
        }

        public static void IncreaseMF(float amt)
        {
            magicFind += amt;
            if (magicFind > 1f) magicFind = 1f;
        }

        public static void PostLoad(Player p)
        {
            magicFindBonus = 0f; //Reset bonus
        }

        public static void CreatePlayer(Player p)
        {
            magicFindBonus = 0f; //Reset bonus
        }

        public static bool CrystalUse(Player p) {
            return true;
        }
    }
}