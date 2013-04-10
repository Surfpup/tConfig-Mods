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
        public class NPCAffix
        {
            public List<string> affixes;
            public string affix;
            public bool suffix;

            public class DelMod<T>
            {
                public delegate Delegate DelModifier(T val);
                public T min;
                public T max;
                public DelModifier gen;

                public string name;
                public DelMod(string name, DelModifier gen, T min, T max)
                {
                    this.name = name;
                    this.gen = gen;
                    this.min = min;
                    this.max = max;
                }
            }

            public List<DelMod<int>> delModGens;
            public List<DelMod<float>> delModGensF;

            public float randAmt;
            public float randTotal;

            public NPCAffix(string name, bool suffix = false)
            {
                this.affix = name;
                this.suffix = suffix;
                affixes = new List<string>();
                delModGens = new List<DelMod<int>>();
                delModGensF = new List<DelMod<float>>();

                ModGeneric.npcAffixByName[this.affix] = this;
            }
            public virtual GNPCAffix Roll()
            { //Roll the dice!
                GNPCAffix p = new GNPCAffix(this.affix, this.suffix);

                /*foreach(Requirement r in this.customRequirements)
                {
                    p.customRequirements.Add(r);
                }*/

                foreach (DelMod<int> m in delModGens)
                {
                    int val = SkewedRand(m.min, m.max, p);

                    p.AddDel(m.name, m.gen(val));
                    //if(m.tip!=null) p.AddTip(m.tip(val));
                }
                foreach (DelMod<float> m in delModGensF)
                {
                    float val = SkewedRand(m.min, m.max, p);

                    p.AddDel(m.name, m.gen(val));
                    //if(m.tip!=null) p.AddTip(m.tip(val));
                }

                float avgRand = 0f;
                if (randTotal > 0f)
                {
                    avgRand = randAmt / randTotal;
                }
                p.rarity = avgRand;
                if (affixes.Count > 0)
                { //Modify the affix depending on how good the roll is!
                    double val = (avgRand * (double)(affixes.Count - 1));
                    int index = (int)Math.Round(val, System.MidpointRounding.AwayFromZero);
                    p.affix = this.affixes[index];
                }
                else p.affix = this.affix;

                randAmt = 0f;
                randTotal = 0f;
                return p;
            }
            public virtual int SkewedRand(int min, int max, GNPCAffix p = null)
            { //Higher values are more rare
                int range = max - min;
                if (range == 0) return min;
                double r = Rand.Skew();
                if (p != null) p.AddRand((float)r);
                randAmt += (float)r;
                randTotal += 1f;
                double val = ((r * (float)range) + (float)min); //Normal value
                return (int)Math.Round(val, System.MidpointRounding.AwayFromZero);
            }
            public virtual float SkewedRand(float min, float max, GNPCAffix p = null)
            { //Higher values are more rare
                float range = max - min;
                if (range == 0f) return min;
                double r = Rand.Skew();
                if (p != null) p.AddRand((float)r);
                randAmt += (float)r;
                randTotal += 1f;

                double val = ((r * range) + (float)min); //Normal value
                return (float)val;
            }

            public NPCAffix AddDel(string name, DelMod<int>.DelModifier del, int min, int max)
            {
                this.delModGens.Add(new DelMod<int>(name, del, min, max));
                return this;
            }
            public NPCAffix AddDel(string name, DelMod<float>.DelModifier del, float min, float max)
            {
                this.delModGensF.Add(new DelMod<float>(name, del, min, max));
                return this;
            }
            public NPCAffix AddAffix(params string[] affixes)
            {
                this.affixes.AddRange(affixes);
                return this;
            }
        }

        public class GNPCAffix
        { //Generated, needs to save additional data
            public string affixType;
            public string affix;
            public bool suffix;
            public List<float> randValues;
            public int curRand = 0;
            public float rarity;
            public Dictionary<string, Delegate> delegates;
            public GNPCAffix(string name, bool suffix = false)
            {
                this.affix = name;
                this.affixType = name;
                this.suffix = suffix;
                randValues = new List<float>();
                delegates = new Dictionary<string, Delegate>();
            }
            public GNPCAffix AddDel(string name, Delegate d)
            {
                if (delegates.ContainsKey(name))
                {
                    delegates[name] = Delegate.Combine(delegates[name], d);
                }
                else delegates[name] = d;
                return this;
            }
            public GNPCAffix AddRand(float r)
            {
                this.randValues.Add(r);
                return this;
            }
            public void Save(BinaryWriter writer)
            {
                writer.Write(this.affixType);
                writer.Write(randValues.Count);
                for (int i = 0; i < randValues.Count; i++) writer.Write(randValues[i]);
                writer.Write(rarity);
            }
            public void Load(BinaryReader reader, int v)
            {
                this.affixType = reader.ReadString();
                Console.WriteLine("Loading Affix " + this.affixType);
                int num = reader.ReadInt32();
                randValues = new List<float>();
                for (int i = 0; i < num; i++) randValues.Add(reader.ReadSingle());
                this.rarity = reader.ReadSingle();

                //Add dprefix stuff
                NPCAffix d = ModGeneric.npcAffixByName[affixType];
                /*foreach(Requirement r in d.customRequirements)
                {
                    this.customRequirements.Add(r);
                }
		
                foreach(ItemMod m in d.itemMods)
                {
                    this.Mod(m);
                }*/

                foreach (NPCAffix.DelMod<int> m in d.delModGens)
                {
                    int val = SkewedRand(m.min, m.max);

                    this.AddDel(m.name, m.gen(val));
                    //if(m.tip!=null) this.AddTip(m.tip(val));
                }
                foreach (NPCAffix.DelMod<float> m in d.delModGensF)
                {
                    float val = SkewedRand(m.min, m.max);

                    this.AddDel(m.name, m.gen(val));
                    //if(m.tip!=null) this.AddTip(m.tip(val));
                }

                if(d.affixes.Count > 0)
                { //Modify the affix depending on how good the roll is!
                    float avgRand = rarity;
                    double val = (avgRand * (double) (d.affixes.Count - 1));
                    int index = (int) Math.Round(val, System.MidpointRounding.AwayFromZero);
                    this.affix = d.affixes[index];
                }
                else this.affix=this.affixType;
            }
            public void AffixName(ref string name)
            {
                if (this.suffix) name = name + " " + this.affix;
                else name = this.affix + " " + name;
            }
            public int SkewedRand(int min, int max)
            {
                float range = max - min;
                if (range == 0f) return min;
                int v = (int)Math.Round((((double)randValues[curRand] * range) + min), System.MidpointRounding.AwayFromZero); //Normal value
                //int v = ModGeneric.Range(min, max, (double) randValues[curRand]);
                curRand++;
                return v;
            }
            public float SkewedRand(float min, float max)
            {
                float range = max - min;
                if (range == 0f) return min;
                float v = (float)(((double)randValues[curRand] * range) + min); //Normal value
                //float v = ModGeneric.Range(min, max, (double) randValues[curRand]);
                curRand++;
                return v;
            }
        }
}