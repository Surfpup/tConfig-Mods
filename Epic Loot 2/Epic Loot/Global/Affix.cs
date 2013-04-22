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
        /*public delegate bool Requirement(Item item);

        //Some common Item requirements
        Requirement armor = (Item item) => (item.accessory || item.bodySlot != -1 || item.legSlot != -1 || item.headSlot != -1);
        Requirement melee = (Item item) => item.melee;
        Requirement ranged = (Item item) => item.ranged;
        Requirement magic = (Item item) => item.magic;
        Requirement proj = (Item item) => item.ranged || item.magic;
        Requirement weapon = (Item item) => item.melee || item.ranged || item.magic;*/

    public class Affix
    {
        public delegate bool CanUse_Del(Player p, int i);
        public delegate void OnSpawn_Del(Player p, int i);
        public delegate bool PreShoot_Del(Player P, Vector2 ShootPos, Vector2 ShootVelocity, int projType, int Damage, float knockback, int owner);
        public delegate void DealtNPC_Del(Player myPlayer, NPC npc, double damage);
        public delegate void UpdatePlayer_Del(Player myPlayer);
        public delegate void DealtPlayer(Player myPlayer, double damage, NPC npc);
        public delegate void DamagePlayer_Del(Player p, ref int d, NPC npc);
        public enum Colors{Normal,Green,Red};

        public virtual string name {set; get;}
        public List<MouseTip> toolTips;
        public virtual int numVals {set;get;}

        public void AddTooltip(string text, Colors color)
        {
            if(color==Colors.Normal) this.toolTips.Add(new MouseTip(text, false, false));
            else if(color==Colors.Green) this.toolTips.Add(new MouseTip(text, true, false));
            else if(color==Colors.Red) this.toolTips.Add(new MouseTip(text, true, true));
        }

        public virtual void Load(BinaryReader reader, int version)
        {

        }

        public virtual void Save(BinaryWriter writer)
        {
            
        }

        public virtual void Load(float[] vals)
        {

        }
    }

    public class ItemAffix : Affix
    {
        public Item item;

        public ItemAffix(Item item)
        {
            toolTips = new List<MouseTip>();
            this.item = item;
        }

        public virtual void Initialize()
        { //Apply changes (when item is spawned)
        }

        public virtual bool Check()
        { //Check requirements
            return false;
        }

        public virtual void Apply(Player player)
        { //This one gets called every frame
        }

        public void AddDelegate(string name, Delegate addDel)
        {
            Delegate curDel = null;
            item.delegates.TryGetValue(name, out curDel);
            if (curDel != null)
            {
                Delegate newDel = Delegate.Combine(curDel, addDel);
                item.delegates[name] = newDel;
            }
            else item.delegates[name] = addDel;
        }

        /*public MouseTip[] UpdateTooltip()
        { //Used to display player-modified stats
            List<MouseTip> tips = new List<MouseTip>();
            if (pAdd.defense != 0f)
            {
                string symbol = "+";
                if (pAdd.defense < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.defense + " Defense", true));

            }
            if (pAdd.crit != 0f)
            {
                string symbol = "+";
                if (pAdd.crit < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.crit + "% Critical Hit Chance", true));
            }
            if (pAdd.meleeCrit != 0f)
            {
                string symbol = "+";
                if (pAdd.meleeCrit < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.meleeCrit + "% Melee Crit Chance", true));
            }
            if (pAdd.rangedCrit != 0f)
            {
                string symbol = "+";
                if (pAdd.rangedCrit < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.rangedCrit + "% Ranged Crit Chance", true));
            }
            if (pAdd.magicCrit != 0f)
            {
                string symbol = "+";
                if (pAdd.magicCrit < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.magicCrit + "% Magic Crit Chance", true));
            }
            if (pAdd.mana != 0f)
            {
                string symbol = "+";
                if (pAdd.mana < 0) symbol = "-";
                tips.Add(new MouseTip(symbol + pAdd.mana + " Mana", true));
            }
            if (pAdd.damage != 0f)
            {
                string symbol = "+";
                if (pAdd.damage < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.damage*100), 2) + "% Damage", true));
            }
            if (pAdd.meleeDamage != 0f)
            {
                string symbol = "+";
                if (pAdd.meleeDamage < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.meleeDamage * 100), 2) + "% Melee Damage", true));
            }
            if (pAdd.rangedDamage != 0f)
            {
                string symbol = "+";
                if (pAdd.rangedDamage < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.rangedDamage * 100), 2) + "% Ranged Damage", true));
            }
            if (pAdd.magicDamage != 0f)
            {
                string symbol = "+";
                if (pAdd.magicDamage < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.magicDamage * 100), 2) + "% Magic Damage", true));
            }
            if (pAdd.moveSpeed != 0)
            {
                string symbol = "+";
                if (pAdd.moveSpeed < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.moveSpeed*100), 2) + "% Movement Speed", true));
            }
            if (pAdd.meleeSpeed != 0)
            {
                string symbol = "+";
                if (pAdd.meleeSpeed < 0) symbol = "";
                tips.Add(new MouseTip(symbol + Math.Round((float)(pAdd.meleeSpeed*100), 2) + "% Melee Speed", true));
            }
            tips.AddRange(toolTips);
            return tips.ToArray();
        }*/
    }

    public class ManaPercent : ItemAffix
    {
        float percent;
        public int amt;

        public ManaPercent(Item item) : base(item)
        {

        }

        public void Load(float percent)
        {
            //Percentage of mana increase
            this.percent = percent;

            if(percent<0)
                this.name = "Decreased Mana %";
            else this.name = "Increased Mana %";
        }

        public override bool Check()
        {
            return item.magic;
        }

        public override void Initialize()
        {
            amt = (int)Math.Round((double)((float)item.mana * percent));
            item.mana += amt;

            if(amt<0)
                base.AddTooltip("-"+Math.Round((double)percent*100f*-1,2)+"% ("+amt+") Mana Cost", Colors.Red);
            else
                base.AddTooltip("+"+Math.Round((double)percent*100f,2)+"% ("+amt+") Mana Cost", Colors.Green);
        }

        public override int numVals { set{} get { return 1; }}
    }

    public class HealthCost : ItemAffix
    {
        int cost;

        public HealthCost(Item item) : base(item)
        {
            
        }

        public void Load(int cost)
        {
            this.name = "Added Health Cost";  

            this.cost = cost;
            this.AddDelegate("CanUse", (CanUse_Del) CanUse);
        }

        public bool CanUse(Player p, int ind)
        { 
            if(cost>p.statLife) return false;

            float defMod = (p.statDefense/2f);
            int dmg = (int)((cost) + defMod);
            p.Hurt(dmg, 0);
            return true;
        }

        public override int numVals { set{} get { return 1; }}
    }

    public class Sacrificial : ItemAffix
    {
        public HealthCost costAffix;
        public ManaPercent manaAffix;
        public Sacrificial(Item item) : base(item)
        {
            manaAffix = new ManaPercent(item);

            costAffix = new HealthCost(item);
        }

        public override bool Check()
        {
            return manaAffix.Check() && costAffix.Check();
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

    public class AffixHandler
    {
        Affix effect;
        List<Stat> range;

        public AffixHandler(params Stat[] range)
        {
            this.range.AddRange(range);
        }

        public void Load(float[] vals)
        {
            List<float> normalized = new List<float>();
            for(int i=0;i<vals.Length;i++)
            {
                normalized.Add(Normalize(range[i], vals[i]));
            }
            effect.Load(normalized.ToArray());
        }

        public void Load(BinaryReader reader, int version)
        {
            List<float> vals = new List<float>(effect.numVals);
            for(int i=0;i<effect.numVals;i++)
            {
                vals.Add(reader.ReadSingle());
            }

            this.Load(vals.ToArray());
        }

        public void Save(BinaryWriter writer)
        {
            
        }

        public float Normalize(Stat stat, float value)
        {
            float range = stat.max - stat.min;
            return (float)(((double)value * range) + stat.min);
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