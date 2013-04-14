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

    public class DPrefix : Epic_Loot.Prefix
    {
        //public delegate PlayerMod PlayerModGen(Player player, int val);
        //public delegate PlayerMod PlayerModGenF(Player player, float val);

        public class PModifier<T>
        {
            public delegate PlayerMod PlayerModGen(T val);
            public delegate MouseTip ToolTip(T val);
            public T min;
            public T max;
            public PlayerModGen gen;
            public ToolTip tip;
            public PModifier(PlayerModGen gen, T min, T max, ToolTip tip = null)
            {
                this.gen = gen;
                this.min = min;
                this.max = max;
                this.tip = tip;
            }
        }
        public List<PModifier<int>> playerModGens;
        public List<PModifier<float>> playerModGensF;

        public class IModifier<T>
        {
            public delegate ItemMod ItemModGen(T val);
            public delegate MouseTip ToolTip(T val);
            public T min;
            public T max;
            public ItemModGen gen;
            public ToolTip tip;
            public IModifier(ItemModGen gen, T min, T max, ToolTip tip = null)
            {
                this.gen = gen;
                this.min = min;
                this.max = max;
                this.tip = tip;
            }
        }
        public List<IModifier<int>> itemModGens;
        public List<IModifier<float>> itemModGensF;

        //public List<PlayerModGen> playerModGens;
        //public List<PlayerModGenF> playerModGensF;

        //Use these for storing range of possible values
        public ItemVals<int> addMin;
        public ItemVals<float> multiplyMin;
        public PlayerVals<float> pAddMin;

        public ItemVals<int> addMax;
        public ItemVals<float> multiplyMax;
        public PlayerVals<float> pAddMax;

        public List<string> affixes;

        public class DelMod<T>
        {
            public delegate Delegate DelModifier(T val);
            public delegate MouseTip ToolTip(T val);
            public T min;
            public T max;
            public DelModifier gen;

            public ToolTip tip;
            public string name;
            public DelMod(string name, DelModifier gen, T min, T max, ToolTip tip = null)
            {
                this.name = name;
                this.gen = gen;
                this.min = min;
                this.max = max;
                this.tip = tip;
            }
        }

        public List<DelMod<int>> delModGens;
        public List<DelMod<float>> delModGensF;

        public float randAmt;
        public float randTotal;
        public float skewMod; //Temporarily stores a modifier that may increase or decrease loot goodness

        public List<Stat> sharedVals;

        public class SharedDelModifier<T>
        {
           // public delegate Delegate DelModifier(T val);
            public Func<T, Delegate> gen;

            public string name;
            public SharedDelModifier(string name, Func<T, Delegate> gen)
            {
                this.name = name;
                this.gen = gen;
            }
        }

        public List<SharedDelModifier<float[]>> sharedDelModGens;
        public List<Func<float[], PlayerMod>> sharedPlayerGens;
        public List<Func<float[], ItemMod>> sharedItemGens;
        public List<Func<float[], TipMod>> sharedTips;

        public DPrefix(string name, bool suffix = false, IPrefix code = null)
            : base(name, suffix, code)
        {
            this.affix = name;
            this.addMin = new ItemVals<int>();
            this.addMax = new ItemVals<int>();
            this.multiplyMin = new ItemVals<float>();
            this.multiplyMax = new ItemVals<float>();
            //Initialize multiply floats to 1f
            multiplyMin.defense = 1f;
            multiplyMin.crit = 1f;
            multiplyMin.mana = 1f;
            multiplyMin.damage = 1f;
            multiplyMin.scale = 1f;
            multiplyMin.knockback = 1f;
            multiplyMin.shootSpeed = 1f;
            multiplyMin.speed = 1f;

            multiplyMax.defense = 1f;
            multiplyMax.crit = 1f;
            multiplyMax.mana = 1f;
            multiplyMax.damage = 1f;
            multiplyMax.scale = 1f;
            multiplyMax.knockback = 1f;
            multiplyMax.shootSpeed = 1f;
            multiplyMax.speed = 1f;

            this.pAddMin = new PlayerVals<float>();
            this.pAddMax = new PlayerVals<float>();
            playerModGensF = new List<PModifier<float>>();
            playerModGens = new List<PModifier<int>>();
            itemModGensF = new List<IModifier<float>>();
            itemModGens = new List<IModifier<int>>();
            affixes = new List<string>();
            delModGens = new List<DelMod<int>>();
            delModGensF = new List<DelMod<float>>();

            sharedVals = new List<Stat>();
            sharedDelModGens = new List<SharedDelModifier<float[]>>();
            sharedPlayerGens = new List<Func<float[], PlayerMod>>();
            sharedItemGens = new List<Func<float[], ItemMod>>();
            sharedTips = new List<Func<float[], TipMod>>();

            ModGeneric.prefixByName[this.affix] = this;

            this.skewMod = 0f;
        }
        public virtual GPrefix Roll(float skewMod = 0f)
        { //Roll the dice!
            this.skewMod = skewMod;

            GPrefix p = new GPrefix(this.affix, this.suffix);

            p.pAdd.defense += SkewedRand((int)pAddMin.defense, (int)pAddMax.defense);
            p.pAdd.crit += SkewedRand((int)pAddMin.crit, (int)pAddMax.crit);
            p.pAdd.mana += SkewedRand((int)pAddMin.mana, (int)pAddMax.mana);
            p.pAdd.damage += SkewedRand(pAddMin.damage, pAddMax.damage);
            p.pAdd.moveSpeed += SkewedRand(pAddMin.moveSpeed, pAddMax.moveSpeed);
            p.pAdd.meleeSpeed += SkewedRand(pAddMin.meleeSpeed, pAddMax.meleeSpeed);

            p.add.damage += SkewedRand((int)addMin.damage, (int)addMax.damage);
            p.add.mana += SkewedRand((int)addMin.mana, (int)addMax.mana);
            p.add.crit += SkewedRand((int)addMin.crit, (int)addMax.crit);

            p.multiply.mana = SkewedRand(multiplyMin.mana, multiplyMax.mana);
            p.multiply.damage = SkewedRand(multiplyMin.damage, multiplyMax.damage);
            p.multiply.scale = SkewedRand(multiplyMin.scale, multiplyMax.scale);
            p.MultiplySpeedInverted(SkewedRand(multiplyMin.speed, multiplyMax.speed));
            p.multiply.knockback = SkewedRand(multiplyMin.knockback, multiplyMax.knockback);
            p.multiply.shootSpeed = SkewedRand(multiplyMin.shootSpeed, multiplyMax.shootSpeed);

            p.requirements.accessory = requirements.accessory;
            p.requirements.melee = requirements.melee;
            p.requirements.ranged = requirements.ranged;
            p.requirements.magic = requirements.magic;
            p.requirements.armor = requirements.armor;
            p.requirements.legArmor = requirements.legArmor;
            p.requirements.bodyArmor = requirements.bodyArmor;
            p.requirements.headArmor = requirements.headArmor;
            p.requirements.notVanity = requirements.notVanity;

            foreach(Stat s in sharedVals)
            {
                SkewedRand(s.min, s.max, p);
                //Main.NewText("Added range "+s.min+","+s.max+": "+val);
                //p.AddRand(val);
            }

            p.Roll(false, this.skewMod);

            float avgRand = 0f;
            if (randTotal > 0f)
            {
                avgRand = randAmt / randTotal;
                /*p.AddTip("Avg Rand: "+avgRand);
                int rare = ModGeneric.SkewedRand(0, 6, avgRand);
                p.Mod( (Item i) => { i.rare += rare; }); //Add to rarity!*/
            }
            p.rarity = avgRand;
            if (affixes.Count > 0)
            { //Modify the affix depending on how good the roll is!

                double val = (avgRand * (double)(affixes.Count - 1));
                int index = (int)Math.Round(val, System.MidpointRounding.AwayFromZero);
                p.affix = this.affixes[index];
            }
            else p.affix = this.affix;

            p.SetID(this.identifier);

            randAmt = 0f;
            randTotal = 0f;
            skewMod = 0f;
            return p;
        }
        public virtual int SkewedRand(int min, int max, GPrefix p = null)
        { //Higher values are more rare
            /*Use exponents to make higher values harder to achieve
            Cubing it makes last 10% pretty hard to get to.
            */
            int range = max - min;
            if (range == 0) return min;
            double r = Rand.Skew(ModGeneric.rand.NextDouble(), this.skewMod);
            if (p != null) p.AddRand((float)r);
            randAmt += (float)r;
            randTotal += 1f;
            double val = ((r * (float)range) + (float)min); //Normal value
            return (int)Math.Round(val, System.MidpointRounding.AwayFromZero);
        }
        public virtual float SkewedRand(float min, float max, GPrefix p = null)
        { //Higher values are more rare
            /*Use exponents to make higher values harder to achieve
            Cubing it makes last 10% pretty hard to get to.
            */
            float range = max - min;
            if (range == 0f) return min;
            double r = Rand.Skew(ModGeneric.rand.NextDouble(), this.skewMod);
            if (p != null) p.AddRand((float)r);
            randAmt += (float)r;
            randTotal += 1f;

            double val = ((r * range) + (float)min); //Normal value
            return (float)val;
        }
        /*public virtual int SkewedRand(int min, int max)
        { //Higher values are more rare
            int range = max-min;
            if(range==0) return min;
            double r = ModGeneric.rand.NextDouble();
            r = Math.Pow(r, 10.0 - ModPlayer.magicFind);
            randAmt += (float) r;
            randTotal += 1f;
            double val = (r * range) + min; //Normal value
            return (int) Math.Round(val, System.MidpointRounding.AwayFromZero);
        }*/

        //New methods which handle storing of randomly generated floats and accessing multiple ones within delegates
        public DPrefix AddVal(float min, float max)
        {
            this.sharedVals.Add(new Stat(min, max));
            return this;
        }
        public DPrefix AddDel(string name, Func<float[], Delegate> del)
        {
            this.sharedDelModGens.Add(new SharedDelModifier<float[]>(name, del));
            return this;
        }
        public DPrefix DMod(Func<float[], PlayerMod> m)
        {
            this.sharedPlayerGens.Add(m);
            return this;
        }
        public DPrefix DMod(Func<float[], ItemMod> m)
        {
            this.sharedItemGens.Add(m);
            return this;
        }
        public DPrefix AddDTip(Func<float[], TipMod> g)
        {
            sharedTips.Add(g);
            return this;
        }


        public DPrefix AddDel(string name, DelMod<int>.DelModifier del, DelMod<int>.ToolTip t, int min, int max)
        {
            this.delModGens.Add(new DelMod<int>(name, del, min, max, t));
            return this;
        }
        public DPrefix AddDel(string name, DelMod<float>.DelModifier del, DelMod<float>.ToolTip t, float min, float max)
        {
            this.delModGensF.Add(new DelMod<float>(name, del, min, max, t));
            return this;
        }
        public DPrefix AddAffix(params string[] affixes)
        {
            this.affixes.AddRange(affixes);
            return this;
        }
        public DPrefix Mod(ItemMod m)
        {
            itemMods.Add(m);
            return this;
        }
        public DPrefix Mod(PlayerMod m)
        {
            playerMods.Add(m);
            return this;
        }
        public DPrefix DMod(PModifier<int>.PlayerModGen m, PModifier<int>.ToolTip t, int min, int max)
        {
            this.playerModGens.Add(new PModifier<int>(m, min, max, t));
            return this;
        }
        public DPrefix DMod(PModifier<float>.PlayerModGen m, PModifier<float>.ToolTip t, float min, float max)
        {
            this.playerModGensF.Add(new PModifier<float>(m, min, max, t));
            return this;
        }
        public DPrefix DMod(IModifier<int>.ItemModGen m, IModifier<int>.ToolTip t, int min, int max)
        {
            this.itemModGens.Add(new IModifier<int>(m, min, max, t));
            return this;
        }
        public DPrefix DMod(IModifier<float>.ItemModGen m, IModifier<float>.ToolTip t, float min, float max)
        {
            this.itemModGensF.Add(new IModifier<float>(m, min, max, t));
            return this;
        }
        public DPrefix AddTip(MouseTip tip)
        {
            toolTips.Add(tip);
            return this;
        }
        public DPrefix AddTip(string text)
        {
            toolTips.Add(new MouseTip(text));
            return this;
        }
        public DPrefix Require(Requirement req)
        {
            customRequirements.Add(req);
            return this;
        }
        public DPrefix RequireAccessory()
        {
            requirements.accessory = true;
            return this;
        }
        public DPrefix AddPlayerDefense(int min, int max)
        {
            pAddMin.defense = min;
            pAddMax.defense = max;
            return this;
        }
        public DPrefix AddDamage(int min, int max)
        {
            addMin.damage = min;
            addMax.damage = max;
            return this;
        }
        public DPrefix AddPlayerCrit(int min, int max)
        {
            pAddMin.crit = min;
            pAddMax.crit = max;
            return this;
        }
        public DPrefix AddPlayerMana(int min, int max)
        {
            pAddMin.mana = min;
            pAddMax.mana = max;
            return this;
        }
        public DPrefix AddPlayerDmg(float min, float max)
        {
            pAddMin.damage = min;
            pAddMax.damage = max;
            return this;
        }
        public DPrefix AddPlayerMovespeed(float min, float max)
        {
            pAddMin.moveSpeed = min;
            pAddMax.moveSpeed = max;
            return this;
        }
        public DPrefix AddPlayerMeleespeed(float min, float max)
        {
            pAddMin.meleeSpeed = min;
            pAddMax.meleeSpeed = max;
            return this;
        }
        public DPrefix AddManaCost(int min, int max)
        {
            addMin.mana = min;
            addMax.mana = max;
            return this;
        }
        public DPrefix AddCrit(int min, int max)
        {
            addMin.crit = min;
            addMax.crit = max;
            return this;
        }
        public DPrefix MultiplyManaCost(float min, float max)
        {
            multiplyMin.mana = min;
            multiplyMax.mana = max;
            return this;
        }
        public DPrefix MultiplyDmg(float min, float max)
        {
            multiplyMin.damage = min;
            multiplyMax.damage = max;
            return this;
        }
        public DPrefix MultiplyScale(float min, float max)
        {
            multiplyMin.scale = min;
            multiplyMax.scale = max;
            return this;
        }
        public DPrefix MultiplySpeedInverted(float min, float max)
        {
            multiplyMin.speed = min;
            multiplyMax.speed = max;
            return this;
        }
        public DPrefix MultiplyKnockback(float min, float max)
        {
            multiplyMin.knockback = min;
            multiplyMax.knockback = max;
            return this;
        }
        public DPrefix MultiplyShootspeed(float min, float max)
        {
            multiplyMin.shootSpeed = min;
            multiplyMax.shootSpeed = max;
            return this;
        }
    }
}