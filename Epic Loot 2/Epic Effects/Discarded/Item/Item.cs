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

namespace Effects
{
    /*
        Handler of Item Effects

        Effects could be affixes, or they could just be plain effects.
        Effects method Initialize() is called when initializing the item or adding the effect.
        Effects method Apply() is called within the items' Effects() method and affects the player.
        Effects can have save state, so they can save and load data.

        This handler needs to additionally store the ID of the effects,
        and upon loading, know which one to spawn.

    */
    public class Global_Item : CustomItem
    {
        public Global_Item(Item item)
            : base(item)
        {

        }

        List<Effect<Item>> effects;

        public void Initialize()
        {
            effects = new List<Effect<Item>>();
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(effects.Count);
            foreach (Effect<Item> p in effects)
            {
                writer.Write(p.type);
                p.Save(writer);
            }
        }

        public void Load(BinaryReader reader, int v)
        {
            int num = reader.ReadInt32();

            effects = new List<Effect<Item>>();
            for (int i = 0; i < num; i++)
            {
                //Get the ID or the name of the effect
                string type = reader.ReadString();
                Effect<Item> e = Effects.Items.Definitions.effectsDict[type].Gen(this.item);
                e.Load(reader, v);
                effects.Add(e);
            }
            if (num > 0) InitEffects();
        }
        public void InitEffects()
        {
            foreach(Effect<Item> e in effects)
                e.Initialize();
        }
        public MouseTip[] UpdateTooltip()
        {
            List<MouseTip> tips = new List<MouseTip>();
            foreach(Effect<Item> e in effects)
            {
                tips.AddRange(e.toolTips);
            }

            return tips.ToArray();
        }
        public List<Effect<Item>> GetEffects()
        {
            return effects;
        }
    }
}