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

/*
This is the complete re-write of Terraria's Prefix class that I wrote for the sake of sanity.

It is included here due to some minor changes to the class in tConfig's codebase that don't really fit with Epic Loot's needs.
*/

using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;
using Gajatko.IniFiles;
using System.Diagnostics;
using Terraria;

namespace Epic_Loot
{
    /*
    
    Prefixes need to do two main things:
     -Check for Requirements
     -Apply Stat modifications

    Prefixes are static, for now. Perhaps a separate class/type of prefixes could be made that are dynamic and act as global item classes.
    */
    public interface IPrefix
    {
        void Apply(Item item); //Apply changes
        bool Check(Item item); //Check requirements
        void Apply(Player player);
       // void Save(BinaryWriter writer);
       // void Load(BinaryReader reader, int version);
    }

    public interface IPrefixDefiner
    {
        List<Prefix> DefinePrefixes();
    }

    public class Prefix : IPrefix
    {
        public static List<Prefix> prefixes; //All the loaded prefixes
        public static Dictionary<string, int> ID; //Get Prefix ID by mod+name
        public static Dictionary<int, string> playerLoad; //Maps saved IDs to names
        public static Dictionary<int, string> worldLoad; //Maps saved IDs to names

        public static int saveVersion = 0; //For save/load

        public delegate bool Requirement(Item item);
        public delegate void ItemMod(Item item);
        public delegate void PlayerMod(Player player);

        public string affix; //Name of prefix; appears in front of item name
        public bool suffix = false; //If true, the 'prefix' is a suffix
        public string identifier; //Identifier of prefix, must be unique.
        public IPrefix code = null;
        public string modname = ""; //Mod that made this prefix
        public List<MouseTip> toolTips;

        public class ItemVals<T>
        {
            public T damage;
            //public T rare;
            public T scale;
            public T speed;
            public T mana;
            public T knockback;
            public T shootSpeed;
            public T crit;
            public T defense;

            public bool melee;
            public bool accessory;
            public bool ranged;
            public bool magic;
            public bool armor;
            public bool legArmor;
            public bool bodyArmor;
            public bool headArmor;
            public bool notVanity;
        }

        public class PlayerVals<T>
        {
            public T defense;
            public T crit;
            public T mana;
            public T damage;
            public T moveSpeed;
            public T meleeSpeed;

            public T meleeDamage;
            public T rangedDamage;
            public T magicDamage;
            public T meleeCrit;
            public T rangedCrit;
            public T magicCrit;
        }
        public ItemVals<int> add;
        public ItemVals<float> multiply;
        public ItemVals<float> requirements;

        public PlayerVals<float> pAdd;

        public List<Requirement> customRequirements;
        public List<ItemMod> itemMods;
        public List<PlayerMod> playerMods;

        public static void LoadPrefixes(string modpack, BinaryReader r, string modversion)
        {
            if (!Config.CheckVersion(modversion, "0.27.4")) return;
            int version = r.ReadInt32(); //This contains the version of the prefix system

            int amt = r.ReadInt32();
            for (int i = 0; i < amt; i++)
            {
                Prefix p = new Prefix(r, version);
                p.modname = modpack;
                prefixes.Add(p);
                ID[modpack + ":" + p.identifier] = prefixes.Count - 1;
            }

            if (Config.codeExists)
            {
                Assembly assembly = Config.modDLL[modpack];
                //Load custom prefix via code?
                IPrefixDefiner builder = (IPrefixDefiner) assembly.CreateInstance(Config.namespaces[assembly.FullName]+".Global_Prefix");
                if (builder != null)
                {
                    foreach (Prefix p in builder.DefinePrefixes())
                    {
                        p.modname = modpack;
                        prefixes.Add(p);
                        ID[modpack + ":" + p.identifier] = prefixes.Count - 1;
                    }
                }
            }
        }

        public static void LoadPrefixNames(string type, MemoryStream stream)
        {
            Dictionary<int, string> dict;
            if (type == "player")
            {
                playerLoad = new Dictionary<int, string>();
                dict = playerLoad;
            }
            else if (type == "world")
            {
                worldLoad = new Dictionary<int, string>();
                dict = worldLoad;
            }
            else return;

            try
            {
                
                IniFileReader r = new IniFileReader(stream);
                IniFile file = IniFile.FromStream(r);
                System.Collections.ObjectModel.ReadOnlyCollection<string> items = file["Prefixes"].GetKeys();
                foreach (string section in items)
                {
                    int id = Convert.ToInt32(section);
                    string name = file["Prefixes"][section];
                    dict[id] = name;
                }
            }
            catch (Exception)
            {
                //Debug.WriteLine("Error in LoadItemNames: " + e);
            }
        }
        public static void SavePrefixNames(MemoryStream stream)
        {
            IniFileWriter w = new IniFileWriter(stream);
            IniFile file = new IniFile();
            //IniFile file = IniFile.FromStream(r);
            //for (int i = Main.maxItemTypes; i < Main.maxItemTypes + customItemsAmt; i++)
            for (int i=0;i<prefixes.Count;i++)
            {
                int netID = i;
                file["Prefixes"][netID.ToString()] = prefixes[i].modname+":"+prefixes[i].identifier;
            }
            //file.Save(path);
            w.WriteIniFile(file);
            w.Flush();
            //Debug.WriteLine("ITEMS! " + stream.Length);
        }
        public static void SavePrefix(BinaryWriter w, Item item)
        { //Handle saving of a prefix.
            w.Write((byte)item.prefix);
            if (item.prefix == ID[":Mysterious"])
            {
                w.Write(item.unloadedPrefix); //Write prefix name
            }
        }
        public static void LoadPrefix(BinaryReader r, Item item, string type)
        {
            //Debug.WriteLine("Loading prefix");
            Dictionary<int, string> dict;
            if (type == "player")
            {
                dict = playerLoad;
            }
            else if (type == "world")
            {
                dict = worldLoad;
            }
            else return;

            int prefix = r.ReadByte();
            if (prefix == 0)
            {
                item.Prefix(0);
                return;
            }

            if (prefix > ID[":Mysterious"])
            {
                if (dict.ContainsKey((int)prefix))
                {
                    string pname = dict[(int)prefix];
                    //Debug.WriteLine("Loading prefix " + pname);
                    try
                    {
                        prefix = ID[pname];
                    }
                    catch (Exception)
                    {
                        //Debug.WriteLine("Failed to load prefix, setting as an unloaded");
                        item.Prefix((byte)(ID[":Mysterious"]));
                        item.unloadedPrefix = pname;
                        return;
                    }
                }
                else //else if(!(prefix <= 84 && dict.Count == 0))
                {
                    //Debug.WriteLine("Prefix " + prefix + " epic failed, ID invalid, setting prefix to zero");
                    prefix = 0;
                    item.Prefix(0);
                    return;
                }
            }

            if (prefix == ID[":Mysterious"])
            {
                string name = r.ReadString();
                //Debug.WriteLine("Found unloaded prefix " + name + ". Attempting to load");
                try
                {
                    prefix = Prefix.ID[name];
                    item.Prefix(prefix);
                }
                catch (Exception)
                {
                    //Debug.WriteLine("Failed to load custom prefix");
                    item.unloadedPrefix = name;
                    item.Prefix((byte)(ID[":Mysterious"]));
                }
            }
            else item.Prefix(prefix);

        }

        public void Initialize()
        {
            this.add = new ItemVals<int>();
            this.multiply = new ItemVals<float>();
            //Initialize multiply floats to 1f
            multiply.defense = 1f;
            multiply.crit = 1f; //Crit is added to item variable, not multiplied. Misleading, yea.
            //Actually, multiply.crit isn't used. Add.crit is used instead.
            multiply.mana = 1f;
            multiply.damage = 1f;
            multiply.scale = 1f;
            multiply.knockback = 1f;
            multiply.shootSpeed = 1f;
            multiply.speed = 1f;

            this.requirements = new ItemVals<float>();
            this.pAdd = new PlayerVals<float>();
            this.customRequirements = new List<Requirement>();
            this.itemMods = new List<ItemMod>();
            this.playerMods = new List<PlayerMod>();
            this.toolTips = new List<MouseTip>();
        }
        public Prefix(string name, bool suffix=false, IPrefix code=null) {
            this.affix=name;
            this.suffix=suffix;
            this.code = code;
            this.identifier = name;
            this.Initialize();
        }
        public Prefix(string name, string id, bool suffix = false)
        {
            this.affix = name;
            this.suffix = suffix;
            this.identifier = id;
            this.Initialize();
        }
        public Prefix(BinaryReader r, int version)
        {
            this.Initialize();
            Load(r, version);
        }
        public Prefix(IniFile ini, string identifier)
        {
            this.identifier = identifier;

            if (String.IsNullOrEmpty(ini["Stats"]["name"])) throw new Exception("Name required for prefix!");
            else this.affix = ini["Stats"]["name"];

            //Debug.WriteLine("Created prefix " + affix);

            if (!String.IsNullOrEmpty(ini["Stats"]["suffix"])) this.suffix = Convert.ToBoolean(ini["Stats"]["suffix"]);

            this.add = new ItemVals<int>();
            this.multiply = new ItemVals<float>();
            //Initialize multiply floats to 1f
            multiply.defense = 1f;
            multiply.crit = 1f; //Crit is added to item variable, not multiplied. Misleading, yea.
            //Actually, multiply.crit isn't used. Add.crit is used instead.
            multiply.mana = 1f;
            multiply.damage = 1f;
            multiply.scale = 1f;
            multiply.knockback = 1f;
            multiply.shootSpeed = 1f;
            multiply.speed = 1f;

           // if (String.IsNullOrEmpty(ini["Multiply"]["defense"])) multiply.defense = Convert.ToSingle(ini["Multiply"]["defense"]);
           // if (String.IsNullOrEmpty(ini["Multiply"]["crit"])) multiply.crit = Convert.ToSingle(ini["Multiply"]["crit"]);
            if (!String.IsNullOrEmpty(ini["Item"]["manaCost"])) multiply.mana = Convert.ToSingle(ini["Item"]["manaCost"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            if (!String.IsNullOrEmpty(ini["Item"]["damage"])) multiply.damage = Convert.ToSingle(ini["Item"]["damage"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            if (!String.IsNullOrEmpty(ini["Item"]["scale"])) multiply.scale = Convert.ToSingle(ini["Item"]["scale"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            if (!String.IsNullOrEmpty(ini["Item"]["knockback"])) multiply.knockback = Convert.ToSingle(ini["Item"]["knockback"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            if (!String.IsNullOrEmpty(ini["Item"]["shootSpeed"])) multiply.shootSpeed = Convert.ToSingle(ini["Item"]["shootSpeed"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            if (!String.IsNullOrEmpty(ini["Item"]["speed"])) MultiplySpeedInverted(Convert.ToSingle(ini["Item"]["speed"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat));

            this.requirements = new ItemVals<float>();
            this.pAdd = new PlayerVals<float>();

            if (!String.IsNullOrEmpty(ini["Item"]["defense"])) add.defense = Convert.ToInt32(ini["Item"]["defense"]);
            if (!String.IsNullOrEmpty(ini["Item"]["crit"])) add.crit = Convert.ToInt32(ini["Item"]["crit"]);
           // if (String.IsNullOrEmpty(ini["Add"]["mana"])) add.mana = Convert.ToInt32(ini["Add"]["mana"]);
           // if (String.IsNullOrEmpty(ini["Add"]["damage"])) add.damage = Convert.ToInt32(ini["Add"]["damage"]);
            //if (String.IsNullOrEmpty(ini["Add"]["scale"])) add.scale = Convert.ToInt32(ini["Add"]["scale"]);
            //if (String.IsNullOrEmpty(ini["Add"]["knockback"])) add.knockback = Convert.ToInt32(ini["Add"]["knockback"]);
            //if (String.IsNullOrEmpty(ini["Add"]["shootSpeed"])) add.shootSpeed = Convert.ToInt32(ini["Add"]["shootSpeed"]);

            if (!String.IsNullOrEmpty(ini["Player"]["defense"])) pAdd.defense = Convert.ToInt32(ini["Player"]["defense"]);
            if (!String.IsNullOrEmpty(ini["Player"]["crit"])) pAdd.crit = Convert.ToInt32(ini["Player"]["crit"]);
            if (!String.IsNullOrEmpty(ini["Player"]["mana"])) pAdd.mana = Convert.ToInt32(ini["Player"]["mana"]);
            if (!String.IsNullOrEmpty(ini["Player"]["damage"])) pAdd.damage = Convert.ToSingle(ini["Player"]["damage"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            if (!String.IsNullOrEmpty(ini["Player"]["moveSpeed"])) pAdd.moveSpeed = Convert.ToSingle(ini["Player"]["moveSpeed"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);
            if (!String.IsNullOrEmpty(ini["Player"]["meleeSpeed"])) pAdd.meleeSpeed = Convert.ToSingle(ini["Player"]["meleeSpeed"], System.Globalization.CultureInfo.InvariantCulture.NumberFormat);

            //Requirements
            if (!String.IsNullOrEmpty(ini["Requirements"]["melee"])) requirements.melee = Convert.ToBoolean(ini["Requirements"]["melee"]);
            if (!String.IsNullOrEmpty(ini["Requirements"]["magic"])) requirements.magic = Convert.ToBoolean(ini["Requirements"]["magic"]);
            if (!String.IsNullOrEmpty(ini["Requirements"]["ranged"])) requirements.ranged = Convert.ToBoolean(ini["Requirements"]["ranged"]);
            if (!String.IsNullOrEmpty(ini["Requirements"]["accessory"])) requirements.accessory = Convert.ToBoolean(ini["Requirements"]["accessory"]);
        }

        public virtual void Save(BinaryWriter writer)
        {
            writer.Write(this.identifier);
            writer.Write(this.affix);
            writer.Write(this.suffix);
            writer.Write(multiply.defense);
            writer.Write(multiply.crit);
            writer.Write(multiply.mana);
            writer.Write(multiply.damage);
            writer.Write(multiply.scale);
            writer.Write(multiply.knockback);
            writer.Write(multiply.shootSpeed);
            writer.Write(multiply.speed);

            writer.Write(add.defense);
            writer.Write(add.crit);
            writer.Write(add.mana);
            writer.Write(add.damage);
            writer.Write(add.scale);
            writer.Write(add.knockback);
            writer.Write(add.shootSpeed);
            writer.Write(add.speed);

            writer.Write(pAdd.defense);
            writer.Write((int)pAdd.crit);
            writer.Write((int)pAdd.mana);
            writer.Write(pAdd.damage);
            writer.Write(pAdd.moveSpeed);
            writer.Write(pAdd.meleeSpeed);

            writer.Write(pAdd.meleeDamage);
            writer.Write(pAdd.rangedDamage);
            writer.Write(pAdd.magicDamage);
            writer.Write((int) pAdd.meleeCrit);
            writer.Write((int)pAdd.rangedCrit);
            writer.Write((int)pAdd.magicCrit);

            writer.Write(requirements.accessory);
            writer.Write(requirements.melee);
            writer.Write(requirements.ranged);
            writer.Write(requirements.magic);
            writer.Write(requirements.armor);
            writer.Write(requirements.legArmor);
            writer.Write(requirements.bodyArmor);
            writer.Write(requirements.headArmor);
            writer.Write(requirements.notVanity);
        }
        public virtual void Load(BinaryReader r, int version)
        {
            this.identifier = r.ReadString();
            this.affix = r.ReadString();
            this.suffix = r.ReadBoolean();

            multiply.defense = r.ReadSingle();
            multiply.crit = r.ReadSingle();
            multiply.mana = r.ReadSingle();
            multiply.damage = r.ReadSingle();
            multiply.scale = r.ReadSingle();
            multiply.knockback = r.ReadSingle();
            multiply.shootSpeed = r.ReadSingle();
            multiply.speed = r.ReadSingle();

            add.defense = r.ReadInt32();
            add.crit = r.ReadInt32();
            add.mana = r.ReadInt32();
            add.damage = r.ReadInt32();
            add.scale = r.ReadInt32();
            add.knockback = r.ReadInt32();
            add.shootSpeed = r.ReadInt32();
            add.speed = r.ReadInt32();

            pAdd.defense = r.ReadSingle();
            pAdd.crit = r.ReadInt32();
            pAdd.mana = r.ReadInt32();
            pAdd.damage = r.ReadSingle();
            pAdd.moveSpeed = r.ReadSingle();
            pAdd.meleeSpeed = r.ReadSingle();

            pAdd.meleeDamage = r.ReadSingle();
            pAdd.rangedDamage = r.ReadSingle();
            pAdd.magicDamage = r.ReadSingle();
            pAdd.meleeCrit = r.ReadInt32();
            pAdd.rangedCrit = r.ReadInt32();
            pAdd.magicCrit = r.ReadInt32();

            requirements.accessory = r.ReadBoolean();
            requirements.melee = r.ReadBoolean();
            requirements.ranged = r.ReadBoolean();
            requirements.magic = r.ReadBoolean();
            requirements.armor = r.ReadBoolean();
            requirements.legArmor = r.ReadBoolean();
            requirements.bodyArmor = r.ReadBoolean();
            requirements.headArmor = r.ReadBoolean();
            requirements.notVanity = r.ReadBoolean();
        }

        public Prefix Require(Requirement req)
        {
            customRequirements.Add(req);
            return this;
        }
        public Prefix Mod(ItemMod m)
        {
            itemMods.Add(m);
            return this;
        }
        public Prefix Mod(PlayerMod m)
        {
            //Debug.WriteLine("Adding playermod");
            playerMods.Add(m);
            return this;
        }
        public Prefix AddTip(MouseTip tip)
        {
            toolTips.Add(tip);
            return this;
        }
        public Prefix AddTip(string text)
        {
            toolTips.Add(new MouseTip(text));
            return this;
        }
        public Prefix SetName(string name)
        {
            this.affix = name;
            return this;
        }
        public Prefix SetCode(IPrefix code)
        {
            this.code = code;
            return this;
        }
        public Prefix RequireMelee()
        {
            requirements.melee = true;
            return this;
        }
        public Prefix RequireRanged()
        {
            requirements.ranged = true;
            return this;
        }
        public Prefix RequireMagic()
        {
            requirements.magic = true;
            return this;
        }
        public Prefix RequireAccessory()
        {
            requirements.accessory = true;
            return this;
        }
        public Prefix RequireDamage(int dmg)
        {
            requirements.damage = dmg;
            return this;
        }
        public Prefix AddDamage(int dmg)
        {
            add.damage += dmg;
            return this;
        }
        public Prefix AddPlayerDefense(int def)
        {
            pAdd.defense += def;
            return this;
        }
        public Prefix AddPlayerCrit(int crit)
        {
            pAdd.crit += crit;
            return this;
        }
        public Prefix AddPlayerMana(int mana)
        {
            pAdd.mana += mana;
            return this;
        }
        public Prefix AddPlayerDmg(float dmg)
        {
            pAdd.damage+=dmg;
            return this;
        }
        public Prefix AddPlayerMeleeDmg(float dmg)
        {
            pAdd.meleeDamage += dmg;
            return this;
        }
        public Prefix AddPlayerRangedDmg(float dmg)
        {
            pAdd.rangedDamage += dmg;
            return this;
        }
        public Prefix AddPlayerMagicDmg(float dmg)
        {
            pAdd.magicDamage += dmg;
            return this;
        }
        public Prefix AddPlayerMeleeCrit(int amt)
        {
            pAdd.meleeCrit += amt;
            return this;
        }
        public Prefix AddPlayerRangedCrit(int amt)
        {
            pAdd.rangedCrit += amt;
            return this;
        }
        public Prefix AddPlayerMagicCrit(int amt)
        {
            pAdd.magicCrit += amt;
            return this;
        }
        public Prefix AddPlayerMovespeed(float speed)
        {
            pAdd.moveSpeed += speed;
            return this;
        }
        public Prefix AddPlayerMeleespeed(float speed)
        {
            pAdd.meleeSpeed += speed;
            return this;
        }
       /* public Prefix AddRarity(int rare)
        {
            add.rare += rare;
            return this;
        }*/
        public Prefix AddSpeed(int speed)
        {
            add.speed += speed;
            return this;
        }
        public Prefix AddManaCost(int mana)
        {
            add.mana += mana;
            return this;
        }
        public Prefix AddCrit(int crit)
        {
            add.crit += crit;
            return this;
        }
        public Prefix MultiplyManaCost(float mana)
        {
            multiply.mana = mana;
            return this;
        }
        public Prefix MultiplyDmg(float dmg)
        {
            multiply.damage = dmg;
            return this;
        }
        public Prefix MultiplyScale(float scale)
        {
            multiply.scale = scale;
            return this;
        }
        public Prefix MultiplySpeed(float speed)
        { //Note that the higher this value is, the slower the speed. And vice versa, the lower it is, the higher the speed.
            multiply.speed = speed;
            return this;
        }
        public Prefix MultiplySpeedInverted(float speed)
        { //More consistent with other methods. 1.25f should result in 25% increase of speed, while 0.75 decreases speed by 25%
            IncreaseSpeed(speed - 1f);
            return this;
        }
        public Prefix IncreaseSpeed(float amt)
        { //Less confusing function. Increase speed by a percentange
            multiply.speed -= amt; //Multiplier is decreased to increase speed.
            return this;
        }
        public Prefix MultiplyKnockback(float kb)
        {
            multiply.knockback = kb;
            return this;
        }
        public Prefix MultiplyShootspeed(float s)
        {
            multiply.shootSpeed = s;
            return this;
        }
        public virtual void Apply(Item item) //Apply changes
        {
            item.damage = (int)Math.Round((double)((float)item.damage * multiply.damage));

            item.useAnimation = (int)Math.Round((double)((float)item.useAnimation * multiply.speed));
            item.useTime = (int)Math.Round((double)((float)item.useTime * multiply.speed));
            item.reuseDelay = (int)Math.Round((double)((float)item.reuseDelay * multiply.speed));

            item.mana = (int)Math.Round((double)((float)item.mana * multiply.mana));
            item.knockBack *= multiply.knockback;
            item.scale *= multiply.scale;
            item.shootSpeed *= multiply.shootSpeed;
            item.crit += add.crit;
            float num14 = 1f * multiply.damage * (2f - multiply.speed) * (2f - multiply.mana) * multiply.scale * multiply.knockback * multiply.shootSpeed * (1f + (float)item.crit * 0.02f);

            //Add additional rarity/value based on player modified stats
            num14 *= (1f + (0.05f * pAdd.defense)) * (1f + pAdd.damage) * (1f + (5 * pAdd.moveSpeed)) * (1f + (5 * pAdd.meleeSpeed));

            if ((double)num14 >= 1.2)
            {
                item.rare += 2;
            }
            else
            {
                if ((double)num14 >= 1.05)
                {
                    item.rare++;
                }
                else
                {
                    if ((double)num14 <= 0.8)
                    {
                        item.rare -= 2;
                    }
                    else
                    {
                        if ((double)num14 <= 0.95)
                        {
                            item.rare--;
                        }
                    }
                }
            }
            num14 *= num14;
            item.value = (int)((float)item.value * num14);

            foreach (ItemMod m in itemMods)
            {
                m(item);
            }
            if (code != null) code.Apply(item);
        }
        public virtual string AffixName(Item item)
        { //Return the item's name with the affix applied to it
            string name = item.name;
            if (!this.suffix)
                name = this.affix + " " + name;
            else name = name + " " + this.affix;

            return name;
        }
        public virtual bool Check(Item item)
        { //Check requirements
            if (item.maxStack > 1) return false; //Only non-stackables get prefixes...
            if (requirements.melee && !item.melee) return false;
            if (requirements.accessory && !item.accessory) return false;
            if (item.damage>=0 && item.damage < requirements.damage) return false;
            if (requirements.ranged && !item.ranged) return false;
            if (requirements.magic && !item.magic) return false;

            //Check stat requirements based on modifiers. A prefix that modifies damage can only appear on items that have damage!
            if ((multiply.damage != 1f || add.damage != 0 || add.crit != 0) && item.damage == -1) return false;
            if (multiply.mana != 1f && item.mana == 0) return false;

            //Weapon-only stuff
            if ((multiply.speed != 1f || add.crit > 0 || multiply.damage != 1f || multiply.knockback != 1f) && (!item.magic && !item.ranged && !item.melee)) return false;

            //Armor?
            if (requirements.armor && item.bodySlot == -1 && item.legSlot == -1 && item.headSlot == -1) return false;
            if (requirements.legArmor && item.legSlot == -1) return false;
            if (requirements.bodyArmor && item.bodySlot == -1) return false;
            if (requirements.headArmor && item.headSlot == -1) return false;
            if (requirements.notVanity && item.vanity) return false;

            foreach (Requirement r in customRequirements)
            {
                if (!r(item)) return false;
            }
            if (code != null) return code.Check(item);
            return true;
        }
        public virtual void Apply(Player player)
        {
            player.statDefense += (int) pAdd.defense;

            player.meleeCrit += (int) pAdd.crit;
            player.rangedCrit += (int) pAdd.crit;
            player.magicCrit += (int) pAdd.crit;

            player.statManaMax2 += (int) pAdd.mana;

            player.meleeDamage += pAdd.damage;
            player.rangedDamage += pAdd.damage;
            player.magicDamage += pAdd.damage;

            player.moveSpeed += pAdd.moveSpeed;

            player.meleeSpeed += pAdd.meleeSpeed;

            foreach (PlayerMod m in playerMods)
            {
                //Debug.WriteLine("Running playermod");
                m(player);
            }
            if (code != null) code.Apply(player);
        }
        public MouseTip[] UpdateTooltip()
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
        }

        /*
        public virtual void Save(BinaryWriter writer)
        {
        }
        public virtual void Load(BinaryReader reader, int version)
        {
        }*/

        public static void DefineDefaults() {
            ID = new Dictionary<string, int>();

            List<Prefix> pref = new List<Prefix>();
            //Default prefixes: http://wiki.terrariaonline.com/Reforge

            pref.AddRange(
                new Prefix[]{
                    new Prefix("None"),
		            new Prefix("Large") 
					            .RequireMelee()
					            .MultiplyScale(1.12f),
		            new Prefix("Massive").RequireMelee().MultiplyScale(1.18f),
		            new Prefix("Dangerous").RequireMelee().MultiplyScale(1.05f).MultiplyDmg(1.05f).AddCrit(2),
		            new Prefix("Savage").RequireMelee().MultiplyScale(1.10f).MultiplyDmg(1.10f).MultiplyKnockback(1.10f),
		            new Prefix("Sharp").RequireMelee().MultiplyDmg(1.15f),
		            new Prefix("Pointy").RequireMelee().MultiplyDmg(1.10f),
		            new Prefix("Tiny").RequireMelee().MultiplyScale(1f-0.18f),
		            new Prefix("Terrible").RequireMelee().MultiplyScale(1f-0.13f).MultiplyDmg(1f-0.15f).MultiplyKnockback(1f-0.15f),
		            new Prefix("Small").RequireMelee().MultiplyScale(1f-0.10f),
		            new Prefix("Dull").RequireMelee().MultiplyDmg(1f-0.15f),
		            new Prefix("Unhappy").RequireMelee().MultiplyScale(1f-0.10f).IncreaseSpeed(-0.10f).MultiplyKnockback(1f-0.10f),
		            new Prefix("Bulky").RequireMelee().MultiplyDmg(1.05f).IncreaseSpeed(-0.15f).MultiplyScale(1.10f).MultiplyKnockback(1.10f),
		            new Prefix("Shameful").RequireMelee().MultiplyDmg(1f-0.10f).MultiplyScale(1.10f).MultiplyKnockback(1f-0.2f),
		            new Prefix("Heavy").RequireMelee().IncreaseSpeed(-0.10f).MultiplyKnockback(1.15f),
		            new Prefix("Light").RequireMelee().IncreaseSpeed(0.15f).MultiplyKnockback(1f-0.10f),
		            new Prefix("Sighted").RequireRanged().MultiplyDmg(1.10f).AddCrit(3),
		            new Prefix("Rapid").RequireRanged().IncreaseSpeed(0.15f).MultiplyShootspeed(1.10f), //velocity
		            new Prefix("Hasty").RequireRanged().IncreaseSpeed(0.10f).MultiplyShootspeed(1.15f),
		            new Prefix("Intimidating").RequireRanged().MultiplyShootspeed(1.05f).MultiplyKnockback(1.15f),
		            new Prefix("Deadly").RequireRanged().MultiplyDmg(1.1f).IncreaseSpeed(0.05f).AddCrit(2).MultiplyShootspeed(1.05f).MultiplyKnockback(1.05f),
		            new Prefix("Staunch").RequireRanged().MultiplyDmg(1.1f).MultiplyKnockback(1.15f),
		            new Prefix("Awful").RequireRanged().MultiplyDmg(1f-0.1f).MultiplyShootspeed(1f-0.1f).MultiplyKnockback(1f-0.1f),
		            new Prefix("Lethargic").RequireRanged().IncreaseSpeed(-0.15f).MultiplyShootspeed(1f-0.1f),
		            new Prefix("Awkward").RequireRanged().IncreaseSpeed(-0.1f).MultiplyKnockback(1f-0.2f),
		            new Prefix("Powerful").RequireRanged().MultiplyDmg(1.15f).IncreaseSpeed(-0.1f).AddCrit(1), //25
		            new Prefix("Mystic").RequireMagic().MultiplyDmg(1.1f).MultiplyManaCost(1f-0.15f),
		            new Prefix("Adept").RequireMagic().MultiplyManaCost(1f-0.15f),
		            new Prefix("Masterful").RequireMagic().MultiplyDmg(1.15f).MultiplyManaCost(1f-0.2f).MultiplyKnockback(1.05f),
		            new Prefix("Inept").RequireMagic().MultiplyManaCost(1.1f),
		            new Prefix("Ignorant").RequireMagic().MultiplyDmg(1f-0.1f).MultiplyManaCost(1.2f), //30
		            new Prefix("Deranged").RequireMagic().MultiplyDmg(1f-0.1f).MultiplyKnockback(1f-0.1f),
		            new Prefix("Intense").RequireMagic().MultiplyDmg(1.1f).MultiplyManaCost(1.15f),
		            new Prefix("Taboo").RequireMagic().IncreaseSpeed(0.1f).MultiplyManaCost(1.1f).MultiplyKnockback(1.1f),
		            new Prefix("Celestial").RequireMagic().MultiplyDmg(1.1f).IncreaseSpeed(-0.1f).MultiplyManaCost(1f-0.1f).MultiplyKnockback(1.1f),
		            new Prefix("Furious").RequireMagic().MultiplyDmg(1.15f).MultiplyManaCost(1.2f).MultiplyKnockback(1.15f), //35
		            new Prefix("Keen").AddCrit(3),
		            new Prefix("Superior").MultiplyDmg(1.1f).AddCrit(3).MultiplyKnockback(1.1f),
		            new Prefix("Forceful").MultiplyKnockback(1.15f), //38
		            new Prefix("Broken").MultiplyDmg(1f-0.3f).MultiplyKnockback(1f-0.2f), //39
		            new Prefix("Damaged").MultiplyDmg(1f-0.15f), //40
		            new Prefix("Shoddy").MultiplyDmg(1f-0.1f).MultiplyKnockback(1f-0.15f), //41
		            new Prefix("Quick").IncreaseSpeed(0.1f),
		            new Prefix("Deadly").MultiplyDmg(1.1f).IncreaseSpeed(0.1f),
		            new Prefix("Agile").IncreaseSpeed(0.1f).AddCrit(3),
		            new Prefix("Nimble").IncreaseSpeed(0.05f), //45
		            new Prefix("Murderous").MultiplyDmg(1.07f).IncreaseSpeed(0.06f).AddCrit(3),
		            new Prefix("Slow").IncreaseSpeed(-0.15f),
		            new Prefix("Sluggish").IncreaseSpeed(-0.2f),
		            new Prefix("Lazy").IncreaseSpeed(-0.08f),
		            new Prefix("Annoying").MultiplyDmg(1f-0.2f).IncreaseSpeed(-0.15f),
		            new Prefix("Nasty").MultiplyDmg(1.05f).IncreaseSpeed(0.1f).AddCrit(2).MultiplyKnockback(1f-0.1f), //51
		            new Prefix("Manic").RequireMagic().MultiplyDmg(1f-0.1f).IncreaseSpeed(0.1f).MultiplyManaCost(1f-0.1f), // 52
		            new Prefix("Hurtful").MultiplyDmg(1.1f), //53
		            new Prefix("Strong").MultiplyKnockback(1.15f), //54
		            new Prefix("Unpleasant").MultiplyDmg(1.05f).MultiplyKnockback(1.15f), //55
		            new Prefix("Weak").MultiplyKnockback(1f-0.2f), //56
		            new Prefix("Ruthless").MultiplyDmg(1.18f).MultiplyKnockback(1f-0.1f), //57
		            new Prefix("Frenzying").RequireRanged().MultiplyDmg(1f-0.15f).IncreaseSpeed(0.15f), //58
		            new Prefix("Godly").MultiplyDmg(1.15f).AddCrit(5).MultiplyKnockback(1.15f),
		            new Prefix("Demonic").MultiplyDmg(1.15f).AddCrit(5),
		            new Prefix("Zealous").AddCrit(5), //61
		            new Prefix("Hard").RequireAccessory().AddPlayerDefense(1),
		            new Prefix("Guarding").RequireAccessory().AddPlayerDefense(2),
		            new Prefix("Armored").RequireAccessory().AddPlayerDefense(3),
		            new Prefix("Warding").RequireAccessory().AddPlayerDefense(4),
		            new Prefix("Arcane").RequireAccessory().AddPlayerMana(20),
		            new Prefix("Precise").RequireAccessory().AddPlayerCrit(1),
		            new Prefix("Lucky").RequireAccessory().AddPlayerCrit(2),
		            new Prefix("Jagged").RequireAccessory().AddPlayerDmg(0.01f),
		            new Prefix("Spiked").RequireAccessory().AddPlayerDmg(0.02f), //70
		            new Prefix("Angry").RequireAccessory().AddPlayerDmg(0.03f),
		            new Prefix("Menacing").RequireAccessory().AddPlayerDmg(0.04f),
		            new Prefix("Brisk").RequireAccessory().AddPlayerMovespeed(0.01f),
		            new Prefix("Fleeting").RequireAccessory().AddPlayerMovespeed(0.02f),
		            new Prefix("Hasty").RequireAccessory().AddPlayerMovespeed(0.03f),
		            new Prefix("Quick").RequireAccessory().AddPlayerMovespeed(0.04f),
		            new Prefix("Wild").RequireAccessory().AddPlayerMeleespeed(0.01f),
		            new Prefix("Rash").RequireAccessory().AddPlayerMeleespeed(0.02f),
		            new Prefix("Intrepid").RequireAccessory().AddPlayerMeleespeed(0.03f),
		            new Prefix("Violent").RequireAccessory().AddPlayerMeleespeed(0.04f),
		            new Prefix("Legendary").RequireMelee().MultiplyDmg(1.15f).IncreaseSpeed(0.10f).AddCrit(5).MultiplyScale(1.10f).MultiplyKnockback(1.15f),
		            new Prefix("Unreal").RequireRanged().MultiplyDmg(1.15f).IncreaseSpeed(0.1f).AddCrit(5).MultiplyShootspeed(1.1f).MultiplyKnockback(1.15f),
		            new Prefix("Mythical").RequireMagic().MultiplyDmg(1.15f).IncreaseSpeed(0.1f).AddCrit(5).MultiplyManaCost(1f-0.1f).MultiplyKnockback(1.15f), //83
                    new UnloadedPrefix("Mysterious") //84
                });

            prefixes = pref;
            for(int i=0;i<prefixes.Count;i++)
                ID[":"+prefixes[i].identifier] = i;
        }
    }

    public class UnloadedPrefix : Prefix
    {
        public UnloadedPrefix(string name, bool suffix=false, IPrefix code=null) : base(name, suffix, code)
        {
            this.affix = name;
        }
        public override void Apply(Item item)
        {
            //Do nothing
        }
        public override void Apply(Player player)
        {
            //Do nothing
        }
        public override bool Check(Item item)
        { //This prefix won't be applied to anything randomly
            return false;
        }
    }
}
 

    /*public class Prefix : IPrefix
    {
        //[7:50:55 PM] Joshua Collins: Have everything method that sets a variable return "this", being the object you're using to set with.  That way you remain in scope while setting
        //[7:51:22 PM] Joshua Collins: new Prefix().setName("Something").setProperty("Prop");
        public static List<Prefix> prefixes; //All the loaded prefixes
        string prefix; //Name of prefix; appears in front of item name
        bool suffix = false; //If true, the 'prefix' is a suffix
        public IPrefix handler=null;
        public delegate void Operation(Item item); //Apply changes
        public delegate bool Requirements(Item item); //Check requirements
        public delegate void OperationPlayer(Player player); //For equipment
        public Operation operation=null;
        public Requirements check = null;
        public OperationPlayer playerOp = null;

        public Prefix(string prefix, Requirements re, Operation op, OperationPlayer pOp, bool suffix=false) {
            operation = op;
            check = re;
            playerOp = pOp;
            this.prefix = prefix;
            this.suffix = suffix;
        }
        public Prefix(string prefix, Operation op, bool suffix = false)
        {
            operation = op;
            check = null;
            playerOp = null;
            this.prefix = prefix;
            this.suffix = suffix;
        }
        public Prefix(string prefix, OperationPlayer pOp, bool suffix = false)
        {
            operation = null;
            check = null;
            playerOp = pOp;
            this.prefix = prefix;
            this.suffix = suffix;
        }
        public Prefix(string prefix, Requirements re, Operation op, bool suffix = false)
        {
            operation = op;
            check = re;
            playerOp = null;
            this.prefix = prefix;
            this.suffix = suffix;
        }
        public Prefix(string prefix, Requirements re, OperationPlayer pOp, bool suffix = false)
        {
            operation = null;
            check = re;
            playerOp = pOp;
            this.prefix = prefix;
            this.suffix = suffix;
        }
        public Prefix(string prefix, IPrefix p, bool suffix = false)
        {
            this.handler = p;
            this.prefix = prefix;
            this.suffix = suffix;
        }
        public bool Check(Item item)
        { //Determine whether the prefix can be applied
            if (check != null)
            {
                return check(item);
            }
            else if (handler != null)
                return handler.Check(item);

            else return true;
        }
        public void Apply(Item item)
        {
            if (operation != null)
            {
                 operation(item);
            }
            else if(handler!=null) 
            {
                handler.Apply(item);
            }
            item.name = this.prefix + " " + item.name;
        }
        public void Apply(Player player)
        {
            if (playerOp != null)
            {
                playerOp(player);
            }
            else if(handler!=null)
            {
                handler.Apply(player);
            }
        }

        public class I
        {
            public delegate Prefix.Operation Multiple(params Prefix.Operation[] parameters);
            public delegate Prefix.Operation Generic(Prefix.Modifier op, params object[] parameters);
            public delegate Prefix.Operation Single(params object[] parameters);

            public static Single Multiply = delegate(object[] parameters)
            {
                return I.Mod(Prefix.Multiply, parameters);
            };

            public static Single Add = delegate(object[] parameters)
            {
                return I.Mod(Prefix.Add, parameters);
            };

            public static Generic Mod = delegate(Prefix.Modifier op, object[] parameters)
            {
                Prefix.Operation effect = delegate(Item item)
                {
                    for (int i = 0; i < parameters.Length; i += 2)
                    {
                        if ((string)parameters[i] == "speed")
                        {
                            op(item, "useAnimation", parameters[i + 1]);
                            op(item, "useTime", parameters[i + 1]);
                            op(item, "reuseDelay", parameters[i + 1]);
                        }
                        else op(item, (string)parameters[i], parameters[i + 1]);
                    }
                };
                return effect;
            };

            public static Multiple ManyMod = delegate(Prefix.Operation[] parameters)
            {
                Prefix.Operation effect = delegate(Item item)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i](item);
                    }
                };
                return effect;
            };
        }

        public class P
        {
            public delegate Prefix.OperationPlayer Multiple(params Prefix.OperationPlayer[] parameters);
            public delegate Prefix.OperationPlayer Generic(Prefix.Modifier op, params object[] parameters);
            public delegate Prefix.OperationPlayer Single(params object[] parameters);

            public static Single Multiply = delegate(object[] parameters)
            {
                return P.Mod(Prefix.Multiply, parameters);
            };

            public static Single Add = delegate(object[] parameters)
            {
                return P.Mod(Prefix.Add, parameters);
            };

            public static Generic Mod = delegate(Prefix.Modifier op, object[] parameters)
            {
                Prefix.OperationPlayer effect = delegate(Player player)
                {
                    for (int i = 0; i < parameters.Length; i += 2)
                    {
                        op(player, (string)parameters[i], parameters[i + 1]);
                    }
                };
                return effect;
            };

            public static Multiple ManyMod = delegate(Prefix.OperationPlayer[] parameters)
            {
                Prefix.OperationPlayer effect = delegate(Player player)
                {
                    for (int i = 0; i < parameters.Length; i++)
                    {
                        parameters[i](player);
                    }
                };
                return effect;
            };
        }

        public delegate void Modifier(object obj, string stat, object value);
        public static Prefix.Modifier Add = delegate(object item, string stat, object value)
        {
            Type objType = item.GetType();
            
            FieldInfo f = objType.GetField(stat);

            string[] stuff = f.ToString().Split(' ');
            string type = stuff[0];
            string name = stuff[1];

            if (type == "Int32") f.SetValue(item, Convert.ToInt32(f.GetValue(item)) + Convert.ToInt32(value));
            else if (type == "Single") f.SetValue(item, Convert.ToSingle(f.GetValue(item)) + Convert.ToSingle(value));
        };

        public static Prefix.Modifier Multiply = delegate(object item, string stat, object value)
        {
            Type objType = item.GetType();
            FieldInfo f = objType.GetField(stat);
            string[] stuff = f.ToString().Split(' ');
            string type = stuff[0];
            string name = stuff[1];

            if (type == "Int32") f.SetValue(item, (int)Math.Round((float)(Convert.ToInt32(f.GetValue(item))) * Convert.ToSingle(value)));
            else if (type == "Single") f.SetValue(item, Convert.ToSingle(f.GetValue(item)) * Convert.ToSingle(value));
        };


        public static void DefineDefaults() {
            List<Prefix> pref = new List<Prefix>();
            //Default prefixes: http://wiki.terrariaonline.com/Reforge
            Prefix.Requirements isAccessory = (Item item) => { return item.accessory; };
            Prefix.Requirements isMelee = (Item item) => { return item.melee; };

            pref.AddRange(
                new Prefix[]{
                    new Prefix("Large", 
                            new Regular()
                                .RequireMelee()
                                .MultiplyScale(1.12f)
                            ),
                   new Prefix("Large", 
                            (Item item) => { return item.melee; },
                            (Item item) => { item.scale *=1.12f; item.rare++; }),
                    new Prefix("Large", isMelee, I.ManyMod(
                            I.Multiply("scale", 1.12f),
                            I.Add("rare", 1))),
                    new Prefix("Massive", isMelee, I.ManyMod(
                            I.Multiply("scale", 1.18f),
                            I.Add( "rare", 1))),
                    new Prefix("Dangerous", isMelee, I.ManyMod(
                            I.Multiply("scale", 1.05f),
                            I.Multiply("damage", 1.05f),
                            I.Add("crit", 2),
                            I.Add("rare", 1)
                            )),
                    new Prefix("Savage", isMelee, I.ManyMod(
                            I.Mod(Multiply, "scale", 1.10f, "damage", 1.10f, "knockBack", 1.10f),
                            I.Mod(Add, "rare", 2)
                            )),
                    new Prefix("Sharp", isMelee, I.ManyMod(
                            I.Mod(Multiply, "damage", 1.15f),
                            I.Mod(Add, "rare", 1)
                            )),
                    new Prefix("Pointy", isMelee, I.ManyMod(I.Multiply("damage", 1.10f), I.Add("rare", 1))),
                    new Prefix("Tiny", isMelee, I.ManyMod(I.Multiply("scale", 1f-0.18f), I.Add("rare", -1))),
                    new Prefix("Terrible", isMelee, I.ManyMod(I.Mod(Multiply, "scale", 1f-0.13f, "damage", 1f-0.15f, "knockBack", 1f-0.15f), I.Add("rare", -2))),
                    new Prefix("Small", isMelee, I.ManyMod(I.Multiply("scale", 1f-0.10f), I.Add("rare", -1))),
                    new Prefix("Dull", isMelee, I.ManyMod(I.Multiply("damage", 1f-0.15f), I.Add("rare", -1))),
                    new Prefix("Unhappy", isMelee, I.ManyMod(
                            I.Mod(Multiply, "scale", 1f-0.10f, "speed", 1f-0.10f, "knockBack", 1f-0.10f),
                            I.Add("rare", -2))),
                });

            /*if (this.prefix == 12)
            {
                text = "Bulky";
            }
            if (this.prefix == 13)
            {
                text = "Shameful";
            }
            if (this.prefix == 14)
            {
                text = "Heavy";
            }
            if (this.prefix == 15)
            {
                text = "Light";
            }
            if (this.prefix == 16)
            {
                text = "Sighted";
            }
            if (this.prefix == 17)
            {
                text = "Rapid";
            }
            if (this.prefix == 18)
            {
                text = "Hasty";
            }
            if (this.prefix == 19)
            {
                text = "Intimidating";
            }
            if (this.prefix == 20)
            {
                text = "Deadly";
            }
            if (this.prefix == 21)
            {
                text = "Staunch";
            }
            if (this.prefix == 22)
            {
                text = "Awful";
            }
            if (this.prefix == 23)
            {
                text = "Lethargic";
            }
            if (this.prefix == 24)
            {
                text = "Awkward";
            }
            if (this.prefix == 25)
            {
                text = "Powerful";
            }
            if (this.prefix == 58)
            {
                text = "Frenzying";
            }
            if (this.prefix == 26)
            {
                text = "Mystic";
            }
            if (this.prefix == 27)
            {
                text = "Adept";
            }
            if (this.prefix == 28)
            {
                text = "Masterful";
            }
            if (this.prefix == 29)
            {
                text = "Inept";
            }
            if (this.prefix == 30)
            {
                text = "Ignorant";
            }
            if (this.prefix == 31)
            {
                text = "Deranged";
            }
            if (this.prefix == 32)
            {
                text = "Intense";
            }
            if (this.prefix == 33)
            {
                text = "Taboo";
            }
            if (this.prefix == 34)
            {
                text = "Celestial";
            }
            if (this.prefix == 35)
            {
                text = "Furious";
            }
            if (this.prefix == 52)
            {
                text = "Manic";
            }
            if (this.prefix == 36)
            {
                text = "Keen";
            }
            if (this.prefix == 37)
            {
                text = "Superior";
            }
            if (this.prefix == 38)
            {
                text = "Forceful";
            }
            if (this.prefix == 53)
            {
                text = "Hurtful";
            }
            if (this.prefix == 54)
            {
                text = "Strong";
            }
            if (this.prefix == 55)
            {
                text = "Unpleasant";
            }
            if (this.prefix == 39)
            {
                text = "Broken";
            }
            if (this.prefix == 40)
            {
                text = "Damaged";
            }
            if (this.prefix == 56)
            {
                text = "Weak";
            }
            if (this.prefix == 41)
            {
                text = "Shoddy";
            }
            if (this.prefix == 57)
            {
                text = "Ruthless";
            }
            if (this.prefix == 42)
            {
                text = "Quick";
            }
            if (this.prefix == 43)
            {
                text = "Deadly";
            }
            if (this.prefix == 44)
            {
                text = "Agile";
            }
            if (this.prefix == 45)
            {
                text = "Nimble";
            }
            if (this.prefix == 46)
            {
                text = "Murderous";
            }
            if (this.prefix == 47)
            {
                text = "Slow";
            }
            if (this.prefix == 48)
            {
                text = "Sluggish";
            }
            if (this.prefix == 49)
            {
                text = "Lazy";
            }
            if (this.prefix == 50)
            {
                text = "Annoying";
            }
            if (this.prefix == 51)
            {
                text = "Nasty";
            }
            if (this.prefix == 59)
            {
                text = "Godly";
            }
            if (this.prefix == 60)
            {
                text = "Demonic";
            }
            if (this.prefix == 61)
            {
                text = "Zealous";
            }

            //Accessory prefixes;
            pref.Add(new Prefix("Hard", isAccessory, I.Mod(Add, "defense", 1)));

            pref.Add(new Prefix("Guarding", isAccessory, I.Mod(Add, "defense", 2, "rare", 1)));

            pref.Add(new Prefix("Armored", isAccessory, I.Mod(Add, "defense", 3, "rare", 1)));

            pref.Add(new Prefix("Warding", isAccessory, I.Mod(Add, "defense", 4, "rare", 2)));

            pref.Add(new Prefix("Arcane", isAccessory, I.Mod(Add, "mana", 20, "rare", 1)));

            pref.Add(new Prefix("Precise", isAccessory, I.Mod(Add, "crit", 1, "rare", 1)));

            pref.Add(new Prefix("Lucky", isAccessory, I.Mod(Add, "crit", 2, "rare", 2)));

            pref.Add(new Prefix("Jagged", isAccessory, I.Mod(Multiply, "damage", 1.01)));

            pref.Add(new Prefix("Spiked", isAccessory, I.ManyMod(I.Mod(Multiply, "damage", 1.02), I.Mod(Add, "rare", 1))));

            pref.Add(new Prefix("Angry", isAccessory, I.ManyMod(I.Mod(Multiply, "damage", 1.03), I.Mod(Add, "rare", 1))));

            pref.Add(new Prefix("Menacing", isAccessory, I.ManyMod(I.Mod(Multiply, "damage", 1.04), I.Mod(Add, "rare", 2))));

            pref.Add(new Prefix("Brisk", isAccessory, P.Mod(Add, "moveSpeed", 0.01f)));

            pref.Add(new Prefix("Fleeting", isAccessory, P.Mod(Add, "moveSpeed", 0.02f, "rare", 1)));

            pref.Add(new Prefix("Hasty", isAccessory, P.Mod(Add, "moveSpeed", 0.03f, "rare", 1)));

            pref.Add(new Prefix("Quick", isAccessory, P.Mod(Add, "moveSpeed", 0.04f, "rare", 2)));

            pref.Add(new Prefix("Wild", isAccessory, P.Mod(Add, "meleeSpeed", 0.01f)));

            pref.Add(new Prefix("Rash", isAccessory, P.Mod(Add, "meleeSpeed", 0.02f, "rare", 1)));

            pref.Add(new Prefix("Intrepid", isAccessory, P.Mod(Add, "meleeSpeed", 0.03f, "rare", 1)));

            pref.Add(new Prefix("Violent", isAccessory, P.Mod(Add, "meleeSpeed", 0.04f, "rare", 2)));


           /* if (this.prefix == 81)
            {
                text = "Legendary";
            }
            if (this.prefix == 82)
            {
                text = "Unreal";
            }
            if (this.prefix == 83)
            {
                text = "Mythical";
            }

            prefixes = pref;
        }
    }

    */
    /*
	public class Prefix
	{
        public static Prefix[] prefixes; //All the loaded prefixes

        string prefix; //Name of prefix; appears in front of item name
        string suffix; //Appears after item name
        public PrefixStat[] stats; //Stat additions or multipliers

        public Prefix(int size)
        { //Size is # of stats to modify
            stats = new PrefixStat[size];
        }
        public Prefix(string stat, object value, int operation=0)
        {
            stats = new PrefixStat[1];
            stats[0] = new PrefixStat(stat, value, operation);
        }
	}
    public class PrefixStat
    {
        string stat; //Name of stat to modify
        object value; //Multiplier or addition value
        byte operation; //Determines whether it's a multiplier, adder, setter, subtracter, etc.

        public static const byte ADD = 0;
        public static const byte MULTIPLY = 1;
        public static const byte SET = 2;
       // delegate void Operation(object value);
        //Operation operation;
        public PrefixStat(string stat, object value, int operation)
        {
            this.stat = stat;
            this.value = value;
            //this.operation = op;
            this.operation = (byte)operation;
        }
        public void Apply(Item item)
        {
            Type objType = item.GetType();
            FieldInfo f = objType.GetField(stat);


            string[] stuff = f.ToString().Split(' ');
            string type = stuff[0];
            string name = stuff[1];


          //  MethodInfo castMethod = f.GetValue(item).GetType().GetMethod("Cast").MakeGenericMethod(t);
           // object castedObject = castMethod.Invoke(null, new object[] { obj });

            if (type == "Int32")
            {
                if(operation==ADD)
                    f.SetValue(item, (int) f.GetValue(item) + (int) value);
                else if (operation == MULTIPLY)
                {
                    f.SetValue(item, (int)f.GetValue(item) * (int)value);
                }
                else if (operation == SET)
                {
                    f.SetValue(item, (int)value);
                }
            }
            else if (type == "Single")
            {
                if (operation == ADD)
                    f.SetValue(item, (float)f.GetValue(item) + (float)value);
                else if (operation == MULTIPLY)
                {
                    f.SetValue(item, (float)f.GetValue(item) * (float)value);
                }
                else if (operation == SET)
                {
                    f.SetValue(item, (float)value);
                }
            }
            else if (type == "Boolean")
            {
                f.SetValue(item, (bool)value);
            }
        }
    }*/

    /*public class Regular : IPrefix
 {
     List<string> funcName;
     List<List<object>> parameters;

     List<string> RfuncName;
     List<List<object>> Rparameters;

     List<string> PfuncName;
     List<List<object>> Pparameters;

     public Regular()
     {
         funcName = new List<string>();
         parameters = new List<List<object>>();
         RfuncName = new List<string>();
         Rparameters = new List<List<object>>();
         PfuncName = new List<string>();
         Pparameters = new List<List<object>>();
     }
     public Regular AddDamage(int val)
     {
         funcName.Add("AddDamage");
         parameters.Add(new List<object>(new object[]{val}));
         return this;
     }
     public Regular AddRarity(int val)
     {
         funcName.Add("AddRarity");
         parameters.Add(new List<object>(new object[] { val }));
         return this;
     }
     public Regular MultiplyDmg(float val)
     {
         funcName.Add("MultiplyDmg");
         parameters.Add(new List<object>(new object[] { val }));
         return this;
     }
     public Regular MultiplyScale(float val)
     {
         funcName.Add("MultiplyScale");
         parameters.Add(new List<object>(new object[] { val }));
         return this;
     }
     public Regular RequireMelee()
     {
         RfuncName.Add("RequireMelee");
         Rparameters.Add(new List<object>());
         return this;
     }
     public Regular RequireAccessory()
     {
         RfuncName.Add("RequireAccessory");
         Rparameters.Add(new List<object>());
         return this;
     }
     void Apply(Item item) //Apply changes
     {
         ItemMod mod = new ItemMod(item);
         for(int i=0;i<funcName.Count;i++)
         {
             Codable.RunSpecifiedMethod("Item Modifier", mod, funcName[i], parameters[i].ToArray());
         }
     }
     bool Check(Item item)
     { //Check requirements
         ItemMod mod = new ItemMod(item);
         for (int i = 0; i < RfuncName.Count; i++)
         {
             if(Codable.RunSpecifiedMethod("Item Modifier Check", mod, RfuncName[i], Rparameters[i].ToArray()) && (bool)Codable.customMethodReturn==false)
                 return false;
         }
         return true;
     }
     void Apply(Player player)
     {
     }
 }
 public class ItemMod
 {
     Item item;
     public ItemMod(Item item)
     {
         this.item = item;
     }
     public ItemMod AddDmg(int dmg)
     {
         item.damage += dmg;
         return this;
     }
     public ItemMod AddRarity(int rare)
     {
         item.rare += rare;
         return this;
     }
     public ItemMod MultiplyDmg(float dmg)
     {
         item.damage = (int)Math.Round((double)((float)item.damage * dmg));
         return this;
     }
     public ItemMod MultiplyScale(float scale)
     {
         item.scale *= scale;
         return this;
     }
     public bool RequireAccessory()
     {
         return item.accessory;
     }
     public bool RequireMelee()
     {
         return item.melee;
     }
     public Item Finish()
     {
         return this.item;
     }
 }
}*/