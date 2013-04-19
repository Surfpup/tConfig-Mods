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
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Terraria;

namespace Epic_Loot
{
    public class Affix
    {
        /*public delegate bool Requirement(Item item);

        //Some common Item requirements
        Requirement armor = (Item item) => (item.accessory || item.bodySlot != -1 || item.legSlot != -1 || item.headSlot != -1);
        Requirement melee = (Item item) => item.melee;
        Requirement ranged = (Item item) => item.ranged;
        Requirement magic = (Item item) => item.magic;
        Requirement proj = (Item item) => item.ranged || item.magic;
        Requirement weapon = (Item item) => item.melee || item.ranged || item.magic;*/

        public string name {set; get;}
        public void Initialize(Item item)
        { //Apply changes (when item is spawned)
        }

        public bool Check(Item item)
        { //Check requirements
            return false;
        }

        public void Apply(Player player)
        { //This one gets called every frame
        }

        public void Apply(NPC npc)
        { //Called every frame

        }
    }

    public class SacrificialAffix : Affix
    {

    }

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
            })
}