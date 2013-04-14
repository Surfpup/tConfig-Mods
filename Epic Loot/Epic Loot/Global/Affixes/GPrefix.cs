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
using System.IO;
//using Terraria;

namespace Epic_Loot
{
    public class GPrefix : Epic_Loot.Prefix
    { //Generated, needs to save additional data
        public List<float> randValues;
        public int curRand = 0;
        public float rarity;
        public Dictionary<string, Delegate> delegates;
        public GPrefix(string name, bool suffix = false, IPrefix code = null)
            : base(name, suffix, code)
        {
            randValues = new List<float>();
            delegates = new Dictionary<string, Delegate>();
        }
        public GPrefix AddDel(string name, Delegate d)
        {
            if (delegates.ContainsKey(name))
            {
                delegates[name] = Delegate.Combine(delegates[name], d);
            }
            else delegates[name] = d;
            return this;
        }
        public GPrefix SetID(string ID)
        {
            this.identifier = ID;
            return this;
        }
        public GPrefix AddRand(float r)
        {
            this.randValues.Add(r);
            return this;
        }
        public override void Save(BinaryWriter writer)
        {
            base.Save(writer);
            writer.Write(randValues.Count);
            for (int i = 0; i < randValues.Count; i++) writer.Write(randValues[i]);
            writer.Write(rarity);
        }
        public override void Load(BinaryReader reader, int v)
        {
            Console.WriteLine("Loading stuff");
            base.Load(reader, v);
            int num = reader.ReadInt32();
            randValues = new List<float>();
            for (int i = 0; i < num; i++) randValues.Add(reader.ReadSingle());
            this.rarity = reader.ReadSingle();

            Console.WriteLine("identifier:"+identifier);
            //Add dprefix stuff

            this.Roll(true);
        }
        public void Roll(bool preGen=false, float skewMod=0f)
        {
            if (!ModGeneric.prefixByName.ContainsKey(identifier))
            {
                this.affix = "";
                this.identifier = "";
                return;
            }
            Console.WriteLine("Loading item affix: " + identifier);
            DPrefix d = ModGeneric.prefixByName[identifier];
            foreach (Requirement r in d.customRequirements)
            {
                this.customRequirements.Add(r);
            }

            foreach (ItemMod m in d.itemMods)
            {
                this.Mod(m);
            }
            foreach (DPrefix.PModifier<float> m in d.playerModGensF)
            {
                float val = SkewedRand(m.min, m.max, skewMod, preGen);

                this.Mod(m.gen(val));
                if (m.tip != null) this.AddTip(m.tip(val));
            }
            foreach (DPrefix.PModifier<int> m in d.playerModGens)
            {
                int val = SkewedRand(m.min, m.max, skewMod, preGen);

                this.Mod(m.gen(val));
                if (m.tip != null) this.AddTip(m.tip(val));
            }
            foreach (DPrefix.IModifier<float> m in d.itemModGensF)
            {
                float val = SkewedRand(m.min, m.max, skewMod, preGen);

                this.Mod(m.gen(val));
                if (m.tip != null) this.AddTip(m.tip(val));
            }
            foreach (DPrefix.IModifier<int> m in d.itemModGens)
            {
                int val = SkewedRand(m.min, m.max, skewMod, preGen);

                this.Mod(m.gen(val));
                if (m.tip != null) this.AddTip(m.tip(val));
            }
            foreach (DPrefix.DelMod<int> m in d.delModGens)
            {
                int val = SkewedRand(m.min, m.max, skewMod, preGen);

                this.AddDel(m.name, m.gen(val));
                if (m.tip != null) this.AddTip(m.tip(val));
            }
            foreach (DPrefix.DelMod<float> m in d.delModGensF)
            {
                float val = SkewedRand(m.min, m.max, skewMod, preGen);

                this.AddDel(m.name, m.gen(val));
                if (m.tip != null) this.AddTip(m.tip(val));
            }
            foreach (PlayerMod m in d.playerMods)
            {
                this.Mod(m);
            }
            foreach (Terraria.MouseTip s in d.toolTips)
            {
                this.AddTip(s);
            }

            /*
            this.AddTip("Avg Rand: "+avgRand);
            int rare = ModGeneric.SkewedRand(0, 6, avgRand);
            this.Mod( (Item i) => { i.rare += rare; }); //Add to rarity!*/
            if (d.affixes.Count > 0)
            { //Modify the affix depending on how good the roll is!
                float avgRand = rarity;
                double val = (avgRand * (double)(d.affixes.Count - 1));
                int index = (int)Math.Round(val, System.MidpointRounding.AwayFromZero);
                this.affix = d.affixes[index];
            }
        }
        public int SkewedRand(int min, int max, float skewMod, bool preGen)
        { //Higher values are more rare
            /*Use exponents to make higher values harder to achieve
            Cubing it makes last 10% pretty hard to get to.
            */
            int range = max - min;
            if (range == 0) return min;
            if(preGen) 
            {
                int v = (int)Math.Round((((double)randValues[curRand] * range) + min), System.MidpointRounding.AwayFromZero); //Normal value
                curRand++;
                return v;
            }

            double r = Rand.Skew(ModGeneric.rand.NextDouble(), skewMod);
            this.AddRand((float)r);
            //randAmt += (float)r;
            //randTotal += 1f;
            double val = ((r * (float)range) + (float)min); //Normal value
            return (int)Math.Round(val, System.MidpointRounding.AwayFromZero);
        }
        public float SkewedRand(float min, float max, float skewMod, bool preGen)
        { //Higher values are more rare
            /*Use exponents to make higher values harder to achieve
            Cubing it makes last 10% pretty hard to get to.
            */
            float range = max - min;
            if (range == 0f) return min;
            if(preGen) 
            {
                float v = (float)(((double)randValues[curRand] * range) + min);
                curRand++;
                return v;
            }

            double r = Rand.Skew(ModGeneric.rand.NextDouble(), skewMod);
            this.AddRand((float)r);

            double val = ((r * range) + (float)min); //Normal value
            return (float)val;
        }
    }
}