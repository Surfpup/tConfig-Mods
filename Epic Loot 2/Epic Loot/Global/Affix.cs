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
    public class Affix
    {
        Effect effect; //The actual affix itself
        List<Stat> range; //List of ranges for the values to input to the affix
        int seed; //Seed used for RNG
        float skewMod; //Modifer that skews the RNG. Decided upon creation of affix, based on various factors.

        public Affix(Effect effect, params Stat[] range)
        {
            this.effect = effect;

            if(range.Length != effect.numVals) throw new Exception("Incorrect number of range parameters for affix");
            this.range.AddRange(range);
        }

        public void Initialize(float skewMod = 1f)
        { //Initialize with random values
            //skewMod is a modifier that may skew the results (maybe based on player or world stats)

            this.seed = ModGeneric.rand.Next(); //Get a random number to use for seed
            this.skewMod = skewMod;
            
            this.Load();
        }

        public void Load()
        {
            Random newRand = new Random(seed); //New RNG using seed

            List<float> randVals = new List<float>(effect.numVals);
            for(int i=0;i<effect.numVals;i++)
            { //Generate values using new RNG
                float val = (float) Rand.Skew(newRand.NextDouble(), skewMod);
                //TODO: Calculate rarity using the rand values

                randVals.Add(Rand.Normalize(range[i],val));
            }

            effect.Load(randVals.ToArray());
        }

        public void Load(BinaryReader reader, int version)
        {
            //Load the Seed and the skewMod value
            this.seed = reader.ReadInt32();
            this.skewMod = reader.ReadSingle();

            this.Load(); //Load the affix
        }

        public void Save(BinaryWriter writer)
        {
            writer.Write(this.seed);
            writer.Write(this.skewMod);
        }
    }

    /*public class SacrificialAffix
    {
        Sacrificial effect;

        Stat manaPercent;
        Stat healthPercent;

        public SacrificialAffix(Stat manaPercent, Stat healthPercent)
        {
            this.manaPercent = manaPercent;
            this.healthPercent = healthPercent;
        }

        public override void Load(float manaPercentVal, float healthPercentVal)
        {
            effect.Load(Normalize(manaPercent, manaPercentVal), Normalize(healthPercent, healthPercentVal));
        }

        public float Normalize(Stat stat, float value)
        {
            float range = Stat.max - Stat.min;
            return (float)(((double)value * range) + min);
        }
    }*/
/*
    new DPrefix("Sacrificial")
            //This affix will replace mana cost with health cost
            //The crappier affixes will make the health to mana ratio bad..
            //As in, it might reduce mana cost by 2 but increase health cost by 10.
            .Require(magic)
            .AddVal(0.1f, 1f)
            //.AddVal(0f,1f) //Dummy value to use for storing something
            .DMod( (float[] v) => {  //Reduce mana cost
                return (Item i) => { 
                    i.mana = (int)Math.Round((double)((float)i.mana * (1f - v[0]))); 
                }; 
            })
            .AddDTip((float[] v) => {
                return (Prefix.TipMod) ((Item i) => {
                    int amt = (int)Math.Round((double)((float)i.mana * v[0]));
                    return new MouseTip("-"+Math.Round((double)v[0]*100f,2)+"% ("+amt+") Mana Cost", true);
                });
            })

            .AddVal(4f, 1f)
            .AddDel( "CanUse", (float[] v) => { 
                //CanUse_Del d =
                return (CanUse_Del) ((Player p, int ind) => { 
                    Item i = p.inventory[p.selectedItem];
                    int amt = (int)Math.Round((double)((float)i.mana / (1f-v[0])));

                    int healthCost = (int)Math.Round((double)(amt * v[1]));
                    //Main.NewText("test");
                    if(healthCost>p.statLife) return false;

                    float defMod = (p.statDefense/2f);
                    int dmg = (int)((healthCost) + defMod);
                    p.Hurt(dmg, 0);
                    return true;
                }); //return d;
            })*/
}