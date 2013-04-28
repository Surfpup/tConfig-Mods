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
using System.Reflection;
using Terraria;

namespace Epic_Loot
{
    //TODO:
    /*
        -Use reflection: http://stackoverflow.com/questions/4598633/how-to-identify-each-parameter-type-in-a-c-sharp-method
          to get the parameter types in constructors for the effects
        -Determine how to randomize those parameters.
            -May need handlers for floats, ints, strings, booleans
    */

    /*public class ItemEffectDef
    { //Basic definition is pointer to the type and a spawn method
        public Type type;
        public ItemEffectDef(Type type)
        {
            this.type=type;

            //Add to global arrays
            ModGeneric.itemEffects.Add(this);
        }
        public virtual ItemEffect Gen(Item item) {
            return (Effects.ItemEffect)Activator.CreateInstance(type, item);
        }
    }*/

    public class ItemAffixDef
    { //Generates an ItemAffix
        public Type type;
        public string name; //Name of affix
        public List<Stat> range; //List of ranges for the values to input to the affix

        public ItemAffixDef(string name, Type type, params Stat[] range)
        {
            this.Construct(name, type, range);
        }

        public ItemAffixDef(Type type, params Stat[] range)
        {
            this.Construct(type.Name, type, range);   
        }

        private void Construct(string name, Type type, Stat[] range)
        {
            this.name = type.Namespace+"-"+name;
            this.type = type;

            this.range.AddRange(range);

            //Add to global arrays
            ModGeneric.itemAffixes.Add(this);
        }

        public ItemAffix Gen(Item item)
        {
            return new ItemAffix(item, this);
        }

        public bool Check(Item item)
        {
            MethodInfo inf = type.GetMethod("Check");
            return (bool) inf.Invoke("Check", new object[] { item });
        }
    }

    public class ItemAffix
    {
        Item item;
        ItemAffixDef affix; //Definition of affix
        public Effects.ItemEffect effect; //The actual affix itself
        int seed; //Seed used for RNG
        float skewMod; //Modifer that skews the RNG. Decided upon creation of affix, based on various factors.

        //public override string type {set{} get{return "ItemAffix";}}

        public ItemAffix(Item item, ItemAffixDef affix)
        {
            this.item = item;
            this.affix = affix;
        }

        public void InitRandom(float skewMod = 1f)
        { //Initialize with random values
            //skewMod is a modifier that may skew the results (maybe based on player or world stats)

            this.seed = ModGeneric.rand.Next(); //Get a random number to use for seed
            this.skewMod = skewMod;
            
            this.Load();
        }

        public void Load()
        {
            Random newRand = new Random(seed); //New RNG using seed

            List<object> param = new List<object>();
            param.Add(this.item);

            //List<float> randVals = new List<float>(affix.range.Count);
            for(int i=0;i<affix.range.Count;i++)
            { //Generate values using new RNG
                float val = (float) Rand.Skew(newRand.NextDouble(), skewMod);
                //TODO: Calculate rarity using the rand values

                float normVal = Rand.Normalize(affix.range[i],val); //Normalized value with range
                //randVals.Add(normVal);
                if(affix.range[i].type == Stat.Kind.Float)
                    param.Add(normVal);
                else if(affix.range[i].type == Stat.Kind.Int)
                    param.Add((int) normVal);
            }

            effect = (Effects.ItemEffect)Activator.CreateInstance(affix.type, false, 
                        System.Reflection.BindingFlags.CreateInstance, null, 
                        param.ToArray(),
                        null, null);


            //effect = (Effects.ItemEffect)Activator.CreateInstance(affix.type, item);
            //if(affix.range.Count != effect.numVals) throw new Exception("Incorrect number of range parameters for affix");
        }

        public void Initialize()
        {
            effect.Initialize();
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