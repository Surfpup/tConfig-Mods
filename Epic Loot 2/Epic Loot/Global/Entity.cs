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
    public abstract class Entity
    {
        public abstract int maxLife { get; set;}
        public abstract int life { get; set;}
        public abstract int defense { get; set;}
        //public abstract int damage { get; set; }
        //int GetMaxHP();
        //int GetHP();
        //int GetDefense();
    }

    public class PlayerEnt : Entity
    {
        public Player player;
        public PlayerEnt(Player p)
        {
            this.player = p;
        }

        public override int maxLife
        {
            get { return player.statLifeMax; }
            set { player.statLifeMax = value; }
        }

        public override int life
        {
            get { return player.statLife; }
            set { player.statLife = value; }
        }

        public override int defense
        {
            get { return player.statDefense; }
            set { player.statDefense = value; }
        }
    }

    public class NPCEnt : Entity
    {
        public NPC npc;
        public NPCEnt(NPC n)
        {
            this.npc = n;
        }

        public override int maxLife
        {
            get { return npc.lifeMax; }
            set { npc.lifeMax = value; }
        }

        public override int life
        {
            get { return npc.life; }
            set { npc.life = value; }
        }

        public override int defense
        {
            get { return npc.defense; }
            set { npc.defense = value; }
        }
    }
}