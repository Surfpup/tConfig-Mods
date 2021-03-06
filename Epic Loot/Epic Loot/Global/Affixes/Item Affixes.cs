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
using Microsoft.Xna.Framework;
using Terraria;

namespace Epic_Loot
{
    public class Item_Affixes
    {
    	public delegate bool CanUse_Del(Player p, int i);
        public delegate void OnSpawn_Del(Player p, int i);
        public delegate bool PreShoot_Del(Player P, Vector2 ShootPos, Vector2 ShootVelocity, int projType, int Damage, float knockback, int owner);
        public delegate void DealtNPC_Del(Player myPlayer, NPC npc, double damage);
        public delegate void UpdatePlayer_Del(Player myPlayer);
        public delegate void DealtPlayer(Player myPlayer, double damage, NPC npc);
        public delegate void DamagePlayer_Del(Player p, ref int d, NPC npc);

        //TODO: Implement ability to specify additional affixes for higher/rarer values
        //Also, make suffixes go into one group; each item should only have one suffix. Thus, suffixes should be more unique in nature.
        public static void DefinePrefixes()
        {
            Console.WriteLine("Defining prefixes...");
            ModGeneric.prefixByName = new Dictionary<string, DPrefix>();

            Prefix.Requirement armor = (Item item) => (item.accessory || item.bodySlot != -1 || item.legSlot != -1 || item.headSlot != -1);
            Prefix.Requirement melee = (Item item) => item.melee;
            Prefix.Requirement ranged = (Item item) => item.ranged;
            Prefix.Requirement magic = (Item item) => item.magic;
            Prefix.Requirement proj = (Item item) => item.ranged || item.magic;
            Prefix.Requirement weapon = (Item item) => item.melee || item.ranged || item.magic;

            ModGeneric.prefixes = new List<DPrefix>();
            ModGeneric.prefixes.AddRange(
                new DPrefix[]{
		new DPrefix("Guarding").AddAffix("Hard", "Guarding", "Armored", "Warding").Require(armor).AddPlayerDefense(1,4),
		new DPrefix("Arcane").Require(armor).AddPlayerMana(1, 30),
		new DPrefix("Precise").Require(armor).AddPlayerCrit(1,3),
		new DPrefix("Spiked").AddAffix("Jagged", "Spiked", "Angry", "Menacing").Require(armor).AddPlayerDmg(0.01f,0.05f),
		new DPrefix("Rash").AddAffix("Brisk", "Fleeting", "Hasty", "Quick").Require(armor).AddPlayerMeleespeed(0.01f,0.05f),
		//new DPrefix("Fleeting").Require(armor).AddPlayerMovespeed(0.01f,0.05f),
		new DPrefix("Speedy").Require(armor).DMod( (float val) => { return (Player player) => { player.baseSpeed += val; player.maximumMaxSpeed += val; }; } , (float val) => { return new MouseTip("+"+Math.Round((float)(val*100f), 2)+"% Movement Speed & Max Speed", true); }, 0.01f, 0.10f),
		new DPrefix("Strong").AddAffix("Strong", "Knockbackity").Require(melee).DMod( (float v) => { return (Item i) => { i.knockBack *= v; }; }, null, 1.05f,1.20f), //, (float v) => { return "+"+((v-1f)*100f)+"% Knockback"; }
		new DPrefix("Large").AddAffix("Large", "Huge", "Hugemongous").Require(melee).DMod( (float v) => { return (Item i) => { i.scale *= v; }; }, null, 1.05f,1.20f), //, (float v) => { return "+"+((v-1f)*100f)+"% Size"; }
		new DPrefix("Swift").Require(ranged).DMod( (float v) => { return (Item i) => { i.shootSpeed *= v; }; }, null, 1.01f,1.20f), //, (float v) => { return "+"+((v-1f)*100f)+"% Projectile Velocity"; }
		new DPrefix("Adept").Require(magic).DMod( (float v) => { return (Item i) => { i.mana = (int)Math.Round((double)((float)i.mana * (1f-v))); }; }, null, 0.01f,0.20f), //, (float v) => { return "-"+((v)*100f)+"% Mana Cost"; }
		new DPrefix("Quick").Require(weapon).DMod( (float v) => { return (Item i) => { 
			v = 1f-(v-1f);
			i.useAnimation = (int)Math.Round((double)((float)i.useAnimation * v));
            i.useTime = (int)Math.Round((double)((float)i.useTime * v));
            i.reuseDelay = (int)Math.Round((double)((float)i.reuseDelay * v)); }; }, null, 1.01f,1.20f),
		new DPrefix("Dangerous").Require(weapon).DMod( (int v) => { return (Item i) => { i.crit += v; }; }, null, 1, 10),
		new DPrefix("Painful").Require(weapon).Require((Item item) => { return (int)Math.Round((double)((float)item.damage * (0.10f))) > 0; }).DMod( (float v) => { return (Item i) => { i.damage = (int)Math.Round((double)((float)i.damage * v)); }; }, null, 1.10f, 1.20f),	
		new DPrefix("Miner's").Require(armor).DMod( (float val) => { return (Player player) => { player.pickSpeed -= val; }; } , (float val) => { return new MouseTip("+"+Math.Round((float)(val*100f), 2)+"% Mining Speed", true); }, 0.01f, 0.10f),
		new DPrefix("Lucky").Require(armor).DMod( (float val) => { return (Player player) => { Codable.RunGlobalMethod("ModPlayer", "IncreaseMF", val); }; } , (float val) => { return new MouseTip("+"+Math.Round((float)(val*100f), 4)+"% Magic Find", true); }, 0.001f, 0.10f),
		new DPrefix("Angelic").Require(armor).AddDel( "OnSpawn", (int val) => { OnSpawn_Del d = (Player p, int i) => { p.statLife += val; }; return d; }, (int val) => { return new MouseTip("+"+val+" HP On Respawn"); }, 10, 50),
		new DPrefix("Celestial").Require(armor).AddDel( "OnSpawn", (int val) => { OnSpawn_Del d = (Player p, int i) => { p.statMana += val; }; return d; }, (int val) => { return new MouseTip("+"+val+" MP On Respawn"); }, 10, 40),
		new DPrefix("Battle-Ready")
			.AddDel("UpdateSpawn", 
				(float val) => { 
					Action d = () => { 
						NPC.spawnRate = (int)((double)NPC.spawnRate * (1f - val));
					};
					return d;
				}, (float val) => { return new MouseTip("+"+Math.Round((float)(val*100f), 2)+"% Increased Spawn Rate", true); }, 0.01f, 0.20f)
			.AddDel("UpdateSpawn", 
				(float val) => { 
					Action d = () => { 
						NPC.maxSpawns = (int)((float)NPC.maxSpawns * (1f + val));
					};
					return d;
				}, (float val) => { return new MouseTip("+"+Math.Round((float)(val*100f), 2)+"% Max Spawns", true); }, 0.01f, 0.20f),
		new DPrefix("Vital").Require(armor).DMod( (int v) => { return (Player p) => { p.statLifeMax2+=v; }; }, (int val) => { return new MouseTip("+"+val+" Max HP", true); }, 1, 30),
		new DPrefix("Mage's").Require(armor).DMod( (float val) => { return (Player player) => { player.manaCost -= val; }; } , (float val) => { return new MouseTip("-"+Math.Round(val, 2)+"% Mana Cost", true); }, 0.01f, 0.05f),
		new DPrefix("Rejuvenating").Require(armor).DMod( (int v) => { return (Player p) => { p.lifeRegen += v; }; }, (int val) => { return new MouseTip("+"+val+" Life Regen", true); }, 1, 2),
		new DPrefix("Thirsty").Require(armor).DMod( (int v) => { return (Player p) => { p.potionDelayTime -= (v*60); }; }, (int val) => { return new MouseTip("Reduces potion cooldown by "+val+" seconds", true); }, 1, 5),
		new DPrefix("Builder's").Require(armor).DMod( (int v) => { return (Player p) => { p.blockRange += v; }; }, (int val) => { return new MouseTip("Increases range of block placement by "+val, true); }, 1, 2),
		new DPrefix("Magnum").Require(ranged).AddDel( "PreShoot", (float v) => { 
				PreShoot_Del d = (Player P,Vector2 ShootPos,Vector2 ShootVelocity,int projType,int Damage,float knockback,int owner) => 
					{ 
						int num41 = Projectile.NewProjectile(ShootPos.X, ShootPos.Y, ShootVelocity.X, ShootVelocity.Y, projType, Damage, knockback, owner);
						if (projType == 80)
						{
							Main.projectile[num41].ai[0] = (float)Player.tileTargetX;
							Main.projectile[num41].ai[1] = (float)Player.tileTargetY;
						}
						Main.projectile[num41].scale += (1f+v);
						return false; 
					}; return d; }, (float v) => { return new MouseTip("+"+Math.Round((float)(v*100f), 2)+"% Increased Projectile Size", true); }, 0.01f, 0.10f),
		new DPrefix("Vampiric").Require(melee).AddDel( "DealtNPC", (int val) => { DealtNPC_Del d = (Player p, NPC npc, double dmg) => { p.statLife += val; }; return d; }, (int val) => { return new MouseTip("+"+val+" HP On Hit", true); }, 1, 5),
        new DPrefix("Vampiric Ranged").AddAffix("Vampiric").Require(proj)
            .AddDel( "RegisterProjectile", (int val) => {
                Action<Projectile> code = (Projectile p) => { p.RegisterDel(ref p.DealtNPC, (NPC npc, double dmg, Player pl) => { pl.statLife += val; }, "DealtNPC"); };
                return code; }, (int val) => { return new MouseTip("+"+val+" HP On Hit", true); }, 1, 5),
		new DPrefix("Leeching").Require(melee).AddDel( "DealtNPC", (int val) => { DealtNPC_Del d = (Player p, NPC npc, double dmg) => { p.statMana += val; }; return d; }, (int val) => { return new MouseTip("+"+val+" Mana On Hit", true); }, 1, 5),
        new DPrefix("Leeching Ranged").AddAffix("Leeching").Require(proj)
            .AddDel( "RegisterProjectile", (int val) => {
                Action<Projectile> code = (Projectile p) => { p.RegisterDel(ref p.DealtNPC, (NPC npc, double dmg, Player pl) => { pl.statMana += val; }, "DealtNPC"); };
                return code; }, (int val) => { return new MouseTip("+"+val+" HP On Hit", true); }, 1, 5),
		new DPrefix("Stabby").Require(melee).Require((Item i) => { return i.useStyle==1; })
			.DMod( (int v) => { return (Item i) => { i.crit += v; i.useStyle=3; }; }, (int val) => { return new MouseTip("Stick 'em with the pointy end", true); }, 5, 15),
        new DPrefix("of Willpower", true).Require(weapon).Require((Item item) => { return (int)Math.Round((double)((float)item.damage * (0.15f))) > 0; }).DMod( (float v) => { return (Item i) => { i.damage = (int)Math.Round((double)((float)i.damage * v)); i.magic=true; int manaBonus = (int)Math.Round((double)(((float)i.damage * (v-1f))/2f)) + 1; i.mana+=manaBonus; i.toolTip7 = "+"+manaBonus+" mana cost"; }; }, null, 1.15f, 1.25f),
        new DPrefix("Vengeful").Require(melee).Require((Item item) => { return (int)Math.Round((double)((float)item.damage * (0.20f))) > 0; }).AddDel( "DamageNPC", (float val) => { Events.Player.DamageNPC_Del d = (Player p, NPC npc, ref int dmg, ref float k) => { dmg+=(int)Math.Round((double)((float)dmg * ((1f - (p.statLife / p.statLifeMax)) * val))); }; return d; }, (float val) => { return new MouseTip("+"+Math.Round((float)(val*100f), 2)+" Vengeance Damage", true); }, 0.20f, 0.5f),
        /*new DPrefix("Vengeful Ranged").AddAffix("Vengeful").Require(proj).Require((Item item) => { return (int)Math.Round((double)((float)item.damage * (0.20f))) > 0; }).
            AddDel( "RegisterProjectile", (float val) => { Action<Projectile> code = (Projectile pr) => {  pr.RegisterDel(ref pr.DamageNPC, (NPC npc, ref int dmg, ref float k) => { dmg+=(int)Math.Round((double)((float)dmg * ((1f - (p.statLife / p.statLifeMax)) * val))) }, "DamageNPC"); }; return code; }, (float val) => { return new MouseTip("+"+Math.Round((float)(val*100f), 2)+" Vengeance Damage", true); }, 0.20f, 0.5f),
        */

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
        	.AddDel( "PreShoot", (float[] v) => { 
        		//CanUse_Del d =
        		return (PreShoot_Del) ((Player p, Vector2 ShootPos, Vector2 ShootVelocity, int projType, int Damage, float knockback, int owner) => { 
					Item i = p.inventory[p.selectedItem];
        			int amt = (int)Math.Round((double)((float)i.mana / (1f-v[0])));

        			int healthCost = (int)Math.Round((double)(amt * v[1]));
        			//Main.NewText("test2");
        			if(healthCost>p.statLife) return false;

        			float defMod = (p.statDefense/2f);
        			//Main.NewText(healthCost+": "+defMod);
        			int dmg = (int)((healthCost) + defMod);
        			p.Hurt(dmg, 0);
        			return true;
        		}); //return d;
        	})
        	.AddDTip((float[] v) => { 
        		return (Prefix.TipMod) ((Item i) => {
	        		int amt = (int)Math.Round((double)((float)i.mana * v[0]));

	    			int healthCost = (int)Math.Round((double)(amt * v[1]));

	        		return new MouseTip("+"+amt+" Health Cost", true, true); 
        		});
        	}),

            new DPrefix("Kamikaze")
                //Increases damage, reduces defense, by the same percentage
                .Require(armor)
                .AddVal(0.1f, 1f)
                .AddVal(1f, 0.8f)
                .DMod( (float[] v) => {
                    return (Player p) => {
                        //Increase damage
                        p.magicDamage += v[0];
                        p.rangedDamage += v[0];
                        p.meleeDamage += v[0];

                        //Decrease defense
                        p.statDefense = (int)(p.statDefense * (1f-(v[0]*v[1])));
                    }; 
                })
                .AddDTip((float[] v) => {
                    return (Prefix.TipMod) ((Item i) => {
                        return new MouseTip("+"+Math.Round((double)v[0]*100f,2)+"% Damage", true);
                    });
                })
                .AddDTip((float[] v) => {
                    return (Prefix.TipMod) ((Item i) => {
                        return new MouseTip("-"+Math.Round((double)(v[0]*v[1])*100f,2)+"% Defense", true, true);
                    });
                }),

            new DPrefix("Thorned")
                .Require(armor)
                .AddVal(1, 20) //Damage
                .AddVal(0.1f, 2f) //Knockback
                .AddDel( "DealtPlayer", (float[] v) => { 
                    DealtPlayer d = (Player player, double damage, NPC npc) => {
                        npc.StrikeNPC((int)v[0], v[1], npc.direction*-1);
                    }; return d; 
                })
                .AddDTip((float[] v) => { 
                    return (Prefix.TipMod) ((Item i) => {
                        return new MouseTip("+"+(int)v[0]+" Thorns Damage with "+v[1]+" knockback", true);
                    });
                }),

            /*new DPrefix("Dodgy")

                .Require(armor)
                .AddVal(0.01f, 0.03f)
                .AddDel( "DamagePlayer", (float[] v) => { 
                    return (DamagePlayer_Del) ((Player p, ref int damage, NPC npc) => { 
                        //if(ModGeneric.rand.Next() < v[0]) 
                        damage = 0;
                    });
                }),*/
         });

            /*DPrefix colorPrefix = new DPrefix("Colored").DMod( (int index) => { return (Item i) => 
                    {
                        i.color = new Color(ModGeneric.colors[index].R, ModGeneric.colors[index].G, ModGeneric.colors[index].B, ModGeneric.colors[index].A);
                        //i.color = new Color(255, 0, 0, 255);
                    }; }, (int index) => { return "The item appears "+ModGeneric.colorNames[index] + "("+index+")"; }, 0, ModGeneric.colors.Count - 1);
            foreach(string c in colorNames)
            {
                colorPrefix.AddAffix(c);
            }
            prefixes.Add(colorPrefix);*/

            DPrefixGroup heavyOrLight = new DPrefixGroup("Heavy/Light");
            heavyOrLight.AddPrefixes(new DPrefix[]
                {
                    new  DPrefix("Heavy").Require(melee)
			            .DMod( (float v) => { return (Item i) => { i.damage = (int)Math.Round((double)((float)i.damage * (v))); i.knockBack *= (v); }; }, null, 1.05f, 1.30f)
			            .AddDel( "UpdatePlayer", (float val) => { UpdatePlayer_Del d = (Player player) => { player.moveSpeed -= (0.51f - val); }; return d; } , (float val) => { return new MouseTip("-"+Math.Round((float)((0.51f - val)*100f), 2)+"% Movement Speed", true, true); }, 0.01f, 0.50f),
		            new DPrefix("Light").Require(melee)
			            .DMod( (float v) => { return (Item i) => { i.damage = (int)Math.Round((double)((float)i.damage * (1f-v))); i.knockBack *= (1f-v); }; }, null, 0.3f, 0.05f)
			            .AddDel( "UpdatePlayer", (float val) => { UpdatePlayer_Del d = (Player player) => { player.moveSpeed += val; }; return d; } , (float val) => { return new MouseTip("+"+Math.Round((float)(val*100f), 2)+"% Movement Speed", true, false); }, 0.01f, 0.25f),
                }, melee) ;

            ModGeneric.prefixes.Add(heavyOrLight);

            DPrefixGroup debuffImmune = new DPrefixGroup("Suffixes");
            debuffImmune.AddPrefixes(
                new DPrefix[]{
			//Debuff Immunities
			new DPrefix("of Protective Armor", true).Require(armor).Mod( (Player p) => { p.brokenArmor = false; }).AddTip("Provides immunity to Broken Armor debuff"),
			new DPrefix("of Protective Skin", true).Require(armor).Mod( (Player p) => { p.bleed = false; }).AddTip("Provides immunity to Bleeding debuff"),
			new DPrefix("of Protective Mind", true).Require(armor).Mod( (Player p) => { p.confused = false; }).AddTip("Provides immunity to Confused debuff"),
			new DPrefix("of Protective Agility", true).Require(armor).Mod( (Player p) => { p.slow = false; }).AddTip("Provides immunity to Slow debuff"),
			new DPrefix("of Protective Voice", true).Require(armor).Mod( (Player p) => { p.silence = false; }).AddTip("Provides immunity to Silence debuff"),
			new DPrefix("of Protective Antidote", true).Require(armor).Mod( (Player p) => { p.poisoned = false; }).AddTip("Provides immunity to Poison debuff"),
			new DPrefix("of Protective Sight", true).Require(armor).Mod( (Player p) => { p.blind = false; }).AddTip("Provides immunity to Blind debuff"),
			new DPrefix("of Protective Blessing", true).Require(armor).Mod( (Player p) => { p.noItems = false; }).AddTip("Provides immunity to Cursed debuff"),
			new DPrefix("of Protective Flames", true).Require(armor).Mod( (Player p) => { p.onFire = false; p.onFire2 = false; }).AddTip("Provides immunity to Fire debuff"),
			//IDEA: Provide immunity to custom mod debuffs as well by looping through player's buffs and deleting it
			
			//Deadly Class
			new DPrefix("of the Deadly Mage", true).Require(armor).DMod( (float v) => { return (Player p) => { p.magicDamage += v; }; }, (float v) => { return new MouseTip("+"+Math.Round((float)(v*100f), 2)+"% Magic Damage", true); }, 0.02f,0.10f),
			new DPrefix("of the Deadly Warrior", true).Require(armor).DMod( (float v) => { return (Player p) => { p.meleeDamage += v; }; }, (float v) => { return new MouseTip("+"+Math.Round((float)(v*100f), 2)+"% Melee Damage", true); }, 0.02f,0.10f),
			new DPrefix("of the Deadly Rogue", true).Require(armor).DMod( (float v) => { return (Player p) => { p.rangedDamage += v; }; }, (float v) => { return new MouseTip("+"+Math.Round((float)(v*100f), 2)+"% Ranged Damage", true); }, 0.02f,0.10f),

			new DPrefix("of Water Walking", true).Require(armor).Mod( (Player player) => { player.waterWalk=true; } ).AddTip("Gives the ability to walk on water"),

			new DPrefix("of Mage Precision", true).Require(armor).DMod( (int v) => { return (Player p) => { p.magicCrit += v; }; }, (int v) => { return new MouseTip("+"+Math.Round((float)v, 2)+"% Magic Crit Chance", true); }, 2, 10),
			new DPrefix("of Warrior Precision", true).Require(armor).DMod( (int v) => { return (Player p) => { p.meleeCrit += v; }; }, (int v) => { return new MouseTip("+"+Math.Round((float)v, 2)+"% Melee Crit Chance", true); }, 2, 10),
			new DPrefix("of Rogue Precision", true).Require(armor).DMod( (int v) => { return (Player p) => { p.rangedCrit += v; }; }, (int v) => { return new MouseTip("+"+Math.Round((float)v, 2)+"% Ranged Crit Chance", true); }, 2, 10),
		    
		}, armor
                );

            ModGeneric.prefixes.Add(debuffImmune);

            //ModGeneric.prefixes.Add(elementalDamage);
            Codable.RunGlobalMethod("ModGeneric", "AddEpicItemAffixes", ModGeneric.prefixes);
        }
    }
}