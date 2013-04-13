#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Terraria;

namespace Terraria_Control
{

    public class ModWorld
    {
#endif
		private SpriteBatch spriteBatch;
        public static Vector2 origin;
        //public static string MouseTextString;
        public static Color color2 = new Color((int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha));


        public void PickItem(Item[] inv, int index)
        { //Main.player[Main.myPlayer].inventory[num81]
            if (Main.player[Main.myPlayer].selectedItem != index || Main.player[Main.myPlayer].itemAnimation <= 0)
            {
                Item item2 = Main.mouseItem;
                Main.mouseItem = inv[index];
                inv[index] = item2;
                if (inv[index].type == 0 || inv[index].stack < 1)
                {
                    inv[index] = new Item();
                }
                if (Main.mouseItem.IsTheSameAs(inv[index]) && inv[index].stack != inv[index].maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack)
                {
                    if (Main.mouseItem.stack + inv[index].stack <= Main.mouseItem.maxStack)
                    {
                        inv[index].stack += Main.mouseItem.stack;
                        Main.mouseItem.stack = 0;
                    }
                    else
                    {
                        int num82 = Main.mouseItem.maxStack - inv[index].stack;
                        inv[index].stack += num82;
                        Main.mouseItem.stack -= num82;
                    }
                }
                if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
                {
                    Main.mouseItem = new Item();
                }
                if (Main.mouseItem.type > 0 || inv[index].type > 0)
                {
                    Recipe.FindRecipes();
                    Main.PlaySound(7, -1, -1, 1);
                }
            }
        }


        public bool InvSlotHighlighted(int x, int y)
        { //Returns true if the mouse is highlighting the given item slot
            //OR if the gamepad has it selected!
            /*
            return Main.mouseX >= x
                && (float)Main.mouseX <= (float)x + (float)Main.inventoryBackTexture.Width * Main.inventoryScale
                && Main.mouseY >= y 
                && (float)Main.mouseY <= (float)y + (float)Main.inventoryBackTexture.Height * Main.inventoryScale;*/
            return ModPlayer.invSelectionX == x && ModPlayer.invSelectionY == y;
        }

		public bool PreDrawInventorySlots(SpriteBatch s) {
            //if(ModPlayer.invMenu!=ModPlayer.INVENTORY) return false;

            //Main.inventoryScale = 1.5f;

			this.spriteBatch = s;
			string MouseTextString = Main.MouseTextString;
					for (int num77 = 0; num77 < 10; num77++)
                    {
                        for (int num78 = 0; num78 < 4; num78++)
                        {
                            int num79 = (int)(20f + (float)(num77 * 56) * Main.inventoryScale);
                            int num80 = (int)(20f + (float)(num78 * 56) * Main.inventoryScale);
                            int num81 = num77 + num78 * 10;
                            Color white2 = new Color(100, 100, 100, 100);
                            if (ModPlayer.invMenu==ModPlayer.INVENTORY && InvSlotHighlighted(num77, num78))
                            {
                                Main.player[Main.myPlayer].mouseInterface = true;
                                if (ModPlayer.invSelectItemRelease) //(Main.mouseLeftRelease && Main.mouseLeft)
                                {
                                    /*if (Main.keyState.IsKeyDown(Keys.LeftShift))
                                    {
                                        if (Main.player[Main.myPlayer].inventory[num81].type > 0)
                                        {
                                            if (Main.npcShop > 0)
                                            {
                                                if (Main.player[Main.myPlayer].SellItem(Main.player[Main.myPlayer].inventory[num81].value, Main.player[Main.myPlayer].inventory[num81].stack))
                                                {
                                                    this.shop[Main.npcShop].AddShop(Main.player[Main.myPlayer].inventory[num81]);
                                                    Main.player[Main.myPlayer].inventory[num81].SetDefaults(0, false);
                                                    Main.PlaySound(18, -1, -1, 1);
                                                }
                                                else
                                                {
                                                    if (Main.player[Main.myPlayer].inventory[num81].value == 0)
                                                    {
                                                        this.shop[Main.npcShop].AddShop(Main.player[Main.myPlayer].inventory[num81]);
                                                        Main.player[Main.myPlayer].inventory[num81].SetDefaults(0, false);
                                                        Main.PlaySound(7, -1, -1, 1);
                                                    }
                                                }
                                            }
                                            else
                                            {
                                                Main.PlaySound(7, -1, -1, 1);
                                                Main.trashItem = (Item)Main.player[Main.myPlayer].inventory[num81].Clone();
                                                Main.player[Main.myPlayer].inventory[num81].SetDefaults(0, false);
                                                Recipe.FindRecipes();
                                            }
                                        }
                                    }
                                    else
                                    {*/
                                        PickItem(Main.player[Main.myPlayer].inventory, num81);
                                    //}
                                }
                                else
                                {
                                    if (Main.mouseRight && Main.mouseRightRelease && Main.player[Main.myPlayer].inventory[num81].type > 0 && !Main.player[Main.myPlayer].inventory[num81].InvRightClicked(Main.player[Main.myPlayer], Main.myPlayer, num81))
                                    {

                                    }
                                    else if (Main.mouseRight && Main.mouseRightRelease && (Main.player[Main.myPlayer].inventory[num81].type == 599 || Main.player[Main.myPlayer].inventory[num81].type == 600 || Main.player[Main.myPlayer].inventory[num81].type == 601))
                                    {
                                        Main.PlaySound(7, -1, -1, 1);
                                        Main.stackSplit = 30;
                                        Main.mouseRightRelease = false;
                                        int num83 = Main.rand.Next(14);
                                        if (num83 == 0 && Main.hardMode)
                                        {
                                            Main.player[Main.myPlayer].inventory[num81].SetDefaults(602, false);
                                        }
                                        else
                                        {
                                            if (num83 <= 7)
                                            {
                                                Main.player[Main.myPlayer].inventory[num81].SetDefaults(586, false);
                                                Main.player[Main.myPlayer].inventory[num81].stack = Main.rand.Next(20, 50);
                                            }
                                            else
                                            {
                                                Main.player[Main.myPlayer].inventory[num81].SetDefaults(591, false);
                                                Main.player[Main.myPlayer].inventory[num81].stack = Main.rand.Next(20, 50);
                                            }
                                        }
                                    }
                                    else
                                    {
                                        if (Main.mouseRight && Main.mouseRightRelease && Main.player[Main.myPlayer].inventory[num81].maxStack == 1)
                                        {
                                            Main.player[Main.myPlayer].inventory[num81] = Main.armorSwap(Main.player[Main.myPlayer].inventory[num81]);
                                        }
                                        else
                                        {
                                            if (ModPlayer.stackSplit <= 1 && ModPlayer.invSelectMore && Main.player[Main.myPlayer].inventory[num81].maxStack > 1 && Main.player[Main.myPlayer].inventory[num81].type > 0 && (Main.mouseItem.IsTheSameAs(Main.player[Main.myPlayer].inventory[num81]) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
                                            {
                                                if (Main.mouseItem.type == 0)
                                                {
                                                    Main.mouseItem = (Item)Main.player[Main.myPlayer].inventory[num81].Clone();
                                                    Main.mouseItem.stack = 0;
                                                }
                                                Main.mouseItem.stack++;
                                                Main.player[Main.myPlayer].inventory[num81].stack--;
                                                if (Main.player[Main.myPlayer].inventory[num81].stack <= 0)
                                                {
                                                    Main.player[Main.myPlayer].inventory[num81] = new Item();
                                                }
                                                Recipe.FindRecipes();
                                                Main.soundInstanceMenuTick.Stop();
                                                Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
                                                Main.PlaySound(12, -1, -1, 1);
                                                if (ModPlayer.stackSplit == 0)
                                                {
                                                    ModPlayer.stackSplit = 15;
                                                }
                                                else
                                                {
                                                    ModPlayer.stackSplit = Main.stackDelay;
                                                }
                                            }
                                        }
                                    }
                                }
                                MouseTextString = Main.player[Main.myPlayer].inventory[num81].name;
                                Main.toolTip = Main.player[Main.myPlayer].inventory[num81];
                                if (Main.player[Main.myPlayer].inventory[num81].stack > 1)
                                {
                                    object obj = MouseTextString;
                                    MouseTextString = string.Concat(new object[]
								{
									obj, 
									" (", 
									Main.player[Main.myPlayer].inventory[num81].stack, 
									")"
								});
                                }
                            }
							
                            if (num78 != 0)
                            {
								Texture2D back = Main.inventoryBackTexture;
								if(ModPlayer.invMenu==ModPlayer.INVENTORY && num77==ModPlayer.invSelectionX && num78==ModPlayer.invSelectionY) back = Main.inventoryBack6Texture;
							
                                SpriteBatch arg_38F2_0 = this.spriteBatch;
                                Texture2D arg_38F2_1 = back;
                                Vector2 arg_38F2_2 = new Vector2((float)num79, (float)num80);
                                Rectangle? arg_38F2_3 = new Rectangle?(new Rectangle(0, 0, back.Width, back.Height));
                                Color arg_38F2_4 = color2;
                                float arg_38F2_5 = 0f;
                                origin = default(Vector2);
                                arg_38F2_0.Draw(arg_38F2_1, arg_38F2_2, arg_38F2_3, arg_38F2_4, arg_38F2_5, origin, Main.inventoryScale, SpriteEffects.None, 0f);
                            }
                            else
                            {
								Texture2D back = Main.inventoryBack9Texture;
								if(ModPlayer.invMenu==ModPlayer.INVENTORY && num77==ModPlayer.invSelectionX && num78==ModPlayer.invSelectionY) back = Main.inventoryBack6Texture;
								
                                SpriteBatch arg_394F_0 = this.spriteBatch;
                                Texture2D arg_394F_1 = back;
                                Vector2 arg_394F_2 = new Vector2((float)num79, (float)num80);
                                Rectangle? arg_394F_3 = new Rectangle?(new Rectangle(0, 0, back.Width, back.Height));
                                Color arg_394F_4 = color2;
                                float arg_394F_5 = 0f;
                                origin = default(Vector2);
                                arg_394F_0.Draw(arg_394F_1, arg_394F_2, arg_394F_3, arg_394F_4, arg_394F_5, origin, Main.inventoryScale, SpriteEffects.None, 0f);
                            }
                            white2 = Color.White;
                            if (Main.player[Main.myPlayer].inventory[num81].type > 0 && Main.player[Main.myPlayer].inventory[num81].stack > 0)
                            {
                                float num84 = 1f;
                                try
                                {
                                    if (Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Width > 32 || Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Height > 32)
                                    {
                                        if (Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Width > Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Height)
                                        {
                                            num84 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Width;
                                        }
                                        else
                                        {
                                            num84 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Height;
                                        }
                                    }
                                }
                                catch (Exception e)
                                {
                                    //Debug.WriteLine(Main.player[Main.myPlayer].inventory[num81].type + "," + Main.player[Main.myPlayer].inventory[num81].name);
                                    throw e;
                                }
                                num84 *= Main.inventoryScale;
                                SpriteBatch arg_3BC5_0 = this.spriteBatch;
                                Texture2D arg_3BC5_1 = Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type];
                                Vector2 arg_3BC5_2 = new Vector2((float)num79 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Width * 0.5f * num84, (float)num80 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Height * 0.5f * num84);
                                Rectangle? arg_3BC5_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Width, Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Height));
                                Color arg_3BC5_4 = Main.player[Main.myPlayer].inventory[num81].GetAlpha(white2);
                                float arg_3BC5_5 = 0f;
                                origin = default(Vector2);
                                arg_3BC5_0.Draw(arg_3BC5_1, arg_3BC5_2, arg_3BC5_3, arg_3BC5_4, arg_3BC5_5, origin, num84, SpriteEffects.None, 0f);
                                Color arg_3BF0_0 = Main.player[Main.myPlayer].inventory[num81].color;
                                Color b = default(Color);
                                if (arg_3BF0_0 != b)
                                {
                                    SpriteBatch arg_3D24_0 = this.spriteBatch;
                                    Texture2D arg_3D24_1 = Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type];
                                    Vector2 arg_3D24_2 = new Vector2((float)num79 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Width * 0.5f * num84, (float)num80 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Height * 0.5f * num84);
                                    Rectangle? arg_3D24_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Width, Main.itemTexture[Main.player[Main.myPlayer].inventory[num81].type].Height));
                                    Color arg_3D24_4 = Main.player[Main.myPlayer].inventory[num81].GetColor(white2);
                                    float arg_3D24_5 = 0f;
                                    origin = default(Vector2);
                                    arg_3D24_0.Draw(arg_3D24_1, arg_3D24_2, arg_3D24_3, arg_3D24_4, arg_3D24_5, origin, num84, SpriteEffects.None, 0f);
                                }
                                if (Main.player[Main.myPlayer].inventory[num81].stack > 1)
                                {
                                    SpriteBatch arg_3DB1_0 = this.spriteBatch;
                                    SpriteFont arg_3DB1_1 = Main.fontItemStack;
                                    string arg_3DB1_2 = string.Concat(Main.player[Main.myPlayer].inventory[num81].stack);
                                    Vector2 arg_3DB1_3 = new Vector2((float)num79 + 10f * Main.inventoryScale, (float)num80 + 26f * Main.inventoryScale);
                                    Color arg_3DB1_4 = white2;
                                    float arg_3DB1_5 = 0f;
                                    origin = default(Vector2);
                                    arg_3DB1_0.DrawString(arg_3DB1_1, arg_3DB1_2, arg_3DB1_3, arg_3DB1_4, arg_3DB1_5, origin, num84, SpriteEffects.None, 0f);
                                }
                            }
                            if (num78 == 0)
                            {
                                string text16 = string.Concat(num81 + 1);
                                if (text16 == "10")
                                {
                                    text16 = "0";
                                }
                                Color color4 = color2;
                                if (Main.player[Main.myPlayer].selectedItem == num81)
                                {
                                    color4.R = 0;
                                    color4.B = 0;
                                    color4.G = 255;
                                    color4.A = 50;
                                }
                                SpriteBatch arg_3E61_0 = this.spriteBatch;
                                SpriteFont arg_3E61_1 = Main.fontItemStack;
                                string arg_3E61_2 = text16;
                                Vector2 arg_3E61_3 = new Vector2((float)(num79 + 6), (float)(num80 + 4));
                                Color arg_3E61_4 = color4;
                                float arg_3E61_5 = 0f;
                                origin = default(Vector2);
                                arg_3E61_0.DrawString(arg_3E61_1, arg_3E61_2, arg_3E61_3, arg_3E61_4, arg_3E61_5, origin, Main.inventoryScale * 0.8f, SpriteEffects.None, 0f);
                            }
                        }
                    }
					
			Main.MouseTextString = MouseTextString;
			return false;
		}
		
		private static bool AccCheck(Item newItem, int slot)
		{
			if (Main.player[Main.myPlayer].armor[slot].IsTheSameAs(newItem))
			{
				return false;
			}
            if (newItem.AccCheck != null) return !newItem.AccCheck(Main.player[Main.myPlayer], slot);
			for (int i = 0; i < Main.player[Main.myPlayer].armor.Length; i++)
			{
				if (newItem.IsTheSameAs(Main.player[Main.myPlayer].armor[i]))
				{
					return true;
				}
			}
			return false;
		}
		
		public bool PreDrawPlayerEquipment(SpriteBatch s) {
            //if(ModPlayer.invMenu!=ModPlayer.ARMOR) return false;

			Main.armorHide = false;
			this.spriteBatch = s;
			string MouseTextString = Main.MouseTextString;
			Color color5 = new Color((int)(Main.mouseTextColor * Main.armorAlpha), (int)(Main.mouseTextColor * Main.armorAlpha),
                                         (int)(Main.mouseTextColor * Main.armorAlpha), (int)(Main.mouseTextColor * Main.armorAlpha));
										 
			Vector2 vector5 = Main.fontMouseText.MeasureString("Equip");
			Vector2 vector6 = Main.fontMouseText.MeasureString(Lang.inter[45]);
			float num99 = vector5.X / vector6.X;
			SpriteBatch arg_4692_0 = this.spriteBatch;
			SpriteFont arg_4692_1 = Main.fontMouseText;
			string arg_4692_2 = Lang.inter[45];
			Vector2 arg_4692_3 = new Vector2((float)(Main.screenWidth - 64 - 28 + 4), 152f + (vector5.Y - vector5.Y * num99) / 2f);
			Color arg_4692_4 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
			float arg_4692_5 = 0f;
			origin = default(Vector2);
			arg_4692_0.DrawString(arg_4692_1, arg_4692_2, arg_4692_3, arg_4692_4, arg_4692_5, origin, 0.8f * num99, SpriteEffects.None, 0f);
			if (Main.mouseX > Main.screenWidth - 64 - 28 && Main.mouseX < (int)((float)(Main.screenWidth - 64 - 28) + 56f * Main.inventoryScale) && Main.mouseY > 174 && Main.mouseY < (int)(174f + 448f * Main.inventoryScale))
			{
				Main.player[Main.myPlayer].mouseInterface = true;
			}
			for (int num100 = 0; num100 < 8; num100++)
			{
				int num101 = Main.screenWidth - 64 - 28;
				int num102 = (int)(174f + (float)(num100 * 56) * Main.inventoryScale);
				Color white4 = new Color(100, 100, 100, 100);
				string text18 = "";
				if (num100 == 3)
				{
					text18 = Lang.inter[9];
				}
				if (num100 == 7)
				{
					text18 = Main.player[Main.myPlayer].statDefense + " " + Lang.inter[10];
				}
				Vector2 vector7 = Main.fontMouseText.MeasureString(text18);
				SpriteBatch arg_47F0_0 = this.spriteBatch;
				SpriteFont arg_47F0_1 = Main.fontMouseText;
				string arg_47F0_2 = text18;
				Vector2 arg_47F0_3 = new Vector2((float)num101 - vector7.X - 10f, (float)num102 + (float)Main.inventoryBackTexture.Height * 0.5f - vector7.Y * 0.5f);
				Color arg_47F0_4 = color5;
				float arg_47F0_5 = 0f;
				origin = default(Vector2);
				arg_47F0_0.DrawString(arg_47F0_1, arg_47F0_2, arg_47F0_3, arg_47F0_4, arg_47F0_5, origin, 1f, SpriteEffects.None, 0f);
				if (ModPlayer.invMenu==ModPlayer.ARMOR && num100==ModPlayer.armorSel) //Main.mouseX >= num101 && (float)Main.mouseX <= (float)num101 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num102 && (float)Main.mouseY <= (float)num102 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale)
				{
					Main.armorHide = true;
					Main.player[Main.myPlayer].mouseInterface = true;
					if (ModPlayer.invSelectItemRelease && (Main.mouseItem.type == 0 || (Main.mouseItem.headSlot > -1 && num100 == 0) || (Main.mouseItem.bodySlot > -1 && num100 == 1) || (Main.mouseItem.legSlot > -1 && num100 == 2) || (Main.mouseItem.accessory && num100 > 2 && !AccCheck(Main.mouseItem, num100))))
					{
						if (!Main.mouseItem.CanEquip(Main.player[Main.myPlayer], num100))
						{

						}
						else
						{
							Item item3 = Main.mouseItem;
							Main.mouseItem = Main.player[Main.myPlayer].armor[num100];
							Main.mouseItem.social=false;
							Main.mouseItem.OnUnequip(Main.player[Main.myPlayer], num100);
							Main.player[Main.myPlayer].armor[num100] = item3;
							Main.player[Main.myPlayer].armor[num100].OnEquip(Main.player[Main.myPlayer], num100);
							if (Main.player[Main.myPlayer].armor[num100].type == 0 || Main.player[Main.myPlayer].armor[num100].stack < 1)
							{
								Main.player[Main.myPlayer].armor[num100] = new Item();
							}
							if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
							{
								Main.mouseItem = new Item();
							}
							if (Main.mouseItem.type > 0 || Main.player[Main.myPlayer].armor[num100].type > 0)
							{
								Recipe.FindRecipes();
								Main.PlaySound(7, -1, -1, 1);
							}
						}
					}
					MouseTextString = Main.player[Main.myPlayer].armor[num100].name;
					Main.toolTip = Main.player[Main.myPlayer].armor[num100];
					Main.toolTip.social = false;
					if (num100 <= 2)
					{
						Main.toolTip.wornArmor = true;
					}
					if (Main.player[Main.myPlayer].armor[num100].stack > 1)
					{
						object obj = MouseTextString;
						MouseTextString = string.Concat(new object[]
					{
						obj, 
						" (", 
						Main.player[Main.myPlayer].armor[num100].stack, 
						")"
					});
					}
				}
				SpriteBatch arg_4AD6_0 = this.spriteBatch;
				Texture2D back = Main.inventoryBack3Texture;
				if(ModPlayer.invMenu==ModPlayer.ARMOR && num100==ModPlayer.armorSel) back = Main.inventoryBack6Texture;
								
				Texture2D arg_4AD6_1 = back;
				Vector2 arg_4AD6_2 = new Vector2((float)num101, (float)num102);
				Rectangle? arg_4AD6_3 = new Rectangle?(new Rectangle(0, 0, Main.inventoryBackTexture.Width, Main.inventoryBackTexture.Height));
				Color arg_4AD6_4 = color2;
				float arg_4AD6_5 = 0f;
				origin = default(Vector2);
				arg_4AD6_0.Draw(arg_4AD6_1, arg_4AD6_2, arg_4AD6_3, arg_4AD6_4, arg_4AD6_5, origin, Main.inventoryScale, SpriteEffects.None, 0f);
				white4 = Color.White;
				if (Main.player[Main.myPlayer].armor[num100].type > 0 && Main.player[Main.myPlayer].armor[num100].stack > 0)
				{
					float num103 = 1f;
					if (Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Width > 32 || Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Height > 32)
					{
						if (Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Width > Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Height)
						{
							num103 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Width;
						}
						else
						{
							num103 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Height;
						}
					}
					num103 *= Main.inventoryScale;
					SpriteBatch arg_4D4C_0 = this.spriteBatch;
					Texture2D arg_4D4C_1 = Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type];
					Vector2 arg_4D4C_2 = new Vector2((float)num101 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Width * 0.5f * num103, (float)num102 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Height * 0.5f * num103);
					Rectangle? arg_4D4C_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Width, Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Height));
					Color arg_4D4C_4 = Main.player[Main.myPlayer].armor[num100].GetAlpha(white4);
					float arg_4D4C_5 = 0f;
					origin = default(Vector2);
					arg_4D4C_0.Draw(arg_4D4C_1, arg_4D4C_2, arg_4D4C_3, arg_4D4C_4, arg_4D4C_5, origin, num103, SpriteEffects.None, 0f);
					Color arg_4D77_0 = Main.player[Main.myPlayer].armor[num100].color;
					Color b = default(Color);
					if (arg_4D77_0 != b)
					{
						SpriteBatch arg_4EAB_0 = this.spriteBatch;
						Texture2D arg_4EAB_1 = Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type];
						Vector2 arg_4EAB_2 = new Vector2((float)num101 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Width * 0.5f * num103, (float)num102 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Height * 0.5f * num103);
						Rectangle? arg_4EAB_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Width, Main.itemTexture[Main.player[Main.myPlayer].armor[num100].type].Height));
						Color arg_4EAB_4 = Main.player[Main.myPlayer].armor[num100].GetColor(white4);
						float arg_4EAB_5 = 0f;
						origin = default(Vector2);
						arg_4EAB_0.Draw(arg_4EAB_1, arg_4EAB_2, arg_4EAB_3, arg_4EAB_4, arg_4EAB_5, origin, num103, SpriteEffects.None, 0f);
					}
					if (Main.player[Main.myPlayer].armor[num100].stack > 1)
					{
						SpriteBatch arg_4F38_0 = this.spriteBatch;
						SpriteFont arg_4F38_1 = Main.fontItemStack;
						string arg_4F38_2 = string.Concat(Main.player[Main.myPlayer].armor[num100].stack);
						Vector2 arg_4F38_3 = new Vector2((float)num101 + 10f * Main.inventoryScale, (float)num102 + 26f * Main.inventoryScale);
						Color arg_4F38_4 = white4;
						float arg_4F38_5 = 0f;
						origin = default(Vector2);
						arg_4F38_0.DrawString(arg_4F38_1, arg_4F38_2, arg_4F38_3, arg_4F38_4, arg_4F38_5, origin, num103, SpriteEffects.None, 0f);
					}
				}
			}
			Vector2 vector8 = Main.fontMouseText.MeasureString("Social");
			Vector2 vector9 = Main.fontMouseText.MeasureString(Lang.inter[11]);
			float num104 = vector8.X / vector9.X;
			SpriteBatch arg_5000_0 = this.spriteBatch;
			SpriteFont arg_5000_1 = Main.fontMouseText;
			string arg_5000_2 = Lang.inter[11];
			Vector2 arg_5000_3 = new Vector2((float)(Main.screenWidth - 64 - 28 - 44), 152f + (vector8.Y - vector8.Y * num104) / 2f);
			Color arg_5000_4 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
			float arg_5000_5 = 0f;
			origin = default(Vector2);
			arg_5000_0.DrawString(arg_5000_1, arg_5000_2, arg_5000_3, arg_5000_4, arg_5000_5, origin, 0.8f * num104, SpriteEffects.None, 0f);
			if (Main.mouseX > Main.screenWidth - 64 - 28 - 47 && Main.mouseX < (int)((float)(Main.screenWidth - 64 - 20 - 47) + 56f * Main.inventoryScale) && Main.mouseY > 174 && Main.mouseY < (int)(174f + 168f * Main.inventoryScale))
			{
				Main.player[Main.myPlayer].mouseInterface = true;
			}
			for (int num105 = 8; num105 < 11; num105++)
			{
				int num106 = Main.screenWidth - 64 - 28 - 47;
				int num107 = (int)(174f + (float)((num105 - 8) * 56) * Main.inventoryScale);
				Color white5 = new Color(100, 100, 100, 100);
				string text19 = "";
				if (num105 == 8)
				{
					text19 = Lang.inter[12];
				}
				else
				{
					if (num105 == 9)
					{
						text19 = Lang.inter[13];
					}
					else
					{
						if (num105 == 10)
						{
							text19 = Lang.inter[14];
						}
					}
				}
				Vector2 vector10 = Main.fontMouseText.MeasureString(text19);
				SpriteBatch arg_515F_0 = this.spriteBatch;
				SpriteFont arg_515F_1 = Main.fontMouseText;
				string arg_515F_2 = text19;
				Vector2 arg_515F_3 = new Vector2((float)num106 - vector10.X - 10f, (float)num107 + (float)Main.inventoryBackTexture.Height * 0.5f - vector10.Y * 0.5f);
				Color arg_515F_4 = color5;
				float arg_515F_5 = 0f;
				origin = default(Vector2);
				arg_515F_0.DrawString(arg_515F_1, arg_515F_2, arg_515F_3, arg_515F_4, arg_515F_5, origin, 1f, SpriteEffects.None, 0f);
				if (ModPlayer.invMenu==ModPlayer.ARMOR && num105==ModPlayer.armorSel) //Main.mouseX >= num106 && (float)Main.mouseX <= (float)num106 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num107 && (float)Main.mouseY <= (float)num107 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale)
				{
					Main.player[Main.myPlayer].mouseInterface = true;
					Main.armorHide = true;
					if (ModPlayer.invSelectItemRelease)
					{
						if (Main.mouseItem.type == 0 || (Main.mouseItem.headSlot > -1 && num105 == 8) || (Main.mouseItem.bodySlot > -1 && num105 == 9) || (Main.mouseItem.legSlot > -1 && num105 == 10))
						{
							Item item4 = Main.mouseItem;
							Main.mouseItem = Main.player[Main.myPlayer].armor[num105];
							Main.mouseItem.social=false;
							Main.player[Main.myPlayer].armor[num105] = item4;
							if (Main.player[Main.myPlayer].armor[num105].type == 0 || Main.player[Main.myPlayer].armor[num105].stack < 1)
							{
								Main.player[Main.myPlayer].armor[num105] = new Item();
							}
							if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
							{
								Main.mouseItem = new Item();
							}
							if (Main.mouseItem.type > 0 || Main.player[Main.myPlayer].armor[num105].type > 0)
							{
								Recipe.FindRecipes();
								Main.PlaySound(7, -1, -1, 1);
							}
						}
					}
					else
					{
						if (Main.mouseRight && Main.mouseRightRelease && Main.player[Main.myPlayer].armor[num105].maxStack == 1)
						{
							Main.player[Main.myPlayer].armor[num105] = Main.armorSwap(Main.player[Main.myPlayer].armor[num105]);
						}
					}
					MouseTextString = Main.player[Main.myPlayer].armor[num105].name;
					Main.toolTip = Main.player[Main.myPlayer].armor[num105];
					Main.toolTip.social = true;
					if (num105 <= 2)
					{
						Main.toolTip.wornArmor = true;
					}
					if (Main.player[Main.myPlayer].armor[num105].stack > 1)
					{
						object obj = MouseTextString;
						MouseTextString = string.Concat(new object[]
					{
						obj, 
						" (", 
						Main.player[Main.myPlayer].armor[num105].stack, 
						")"
					});
					}
				}
				SpriteBatch arg_5487_0 = this.spriteBatch;
				
				Texture2D back = Main.inventoryBack8Texture;
				if(ModPlayer.invMenu==ModPlayer.ARMOR && num105==ModPlayer.armorSel) back = Main.inventoryBack6Texture;
				
				Texture2D arg_5487_1 = back;
				Vector2 arg_5487_2 = new Vector2((float)num106, (float)num107);
				Rectangle? arg_5487_3 = new Rectangle?(new Rectangle(0, 0, Main.inventoryBackTexture.Width, Main.inventoryBackTexture.Height));
				Color arg_5487_4 = color2;
				float arg_5487_5 = 0f;
				origin = default(Vector2);
				arg_5487_0.Draw(arg_5487_1, arg_5487_2, arg_5487_3, arg_5487_4, arg_5487_5, origin, Main.inventoryScale, SpriteEffects.None, 0f);
				white5 = Color.White;
				if (Main.player[Main.myPlayer].armor[num105].type > 0 && Main.player[Main.myPlayer].armor[num105].stack > 0)
				{
					float num108 = 1f;
					if (Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Width > 32 || Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Height > 32)
					{
						if (Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Width > Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Height)
						{
							num108 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Width;
						}
						else
						{
							num108 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Height;
						}
					}
					num108 *= Main.inventoryScale;
					SpriteBatch arg_56FD_0 = this.spriteBatch;
					Texture2D arg_56FD_1 = Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type];
					Vector2 arg_56FD_2 = new Vector2((float)num106 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Width * 0.5f * num108, (float)num107 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Height * 0.5f * num108);
					Rectangle? arg_56FD_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Width, Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Height));
					Color arg_56FD_4 = Main.player[Main.myPlayer].armor[num105].GetAlpha(white5);
					float arg_56FD_5 = 0f;
					origin = default(Vector2);
					arg_56FD_0.Draw(arg_56FD_1, arg_56FD_2, arg_56FD_3, arg_56FD_4, arg_56FD_5, origin, num108, SpriteEffects.None, 0f);
					Color arg_5728_0 = Main.player[Main.myPlayer].armor[num105].color;
					Color b = default(Color);
					if (arg_5728_0 != b)
					{
						SpriteBatch arg_585C_0 = this.spriteBatch;
						Texture2D arg_585C_1 = Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type];
						Vector2 arg_585C_2 = new Vector2((float)num106 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Width * 0.5f * num108, (float)num107 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Height * 0.5f * num108);
						Rectangle? arg_585C_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Width, Main.itemTexture[Main.player[Main.myPlayer].armor[num105].type].Height));
						Color arg_585C_4 = Main.player[Main.myPlayer].armor[num105].GetColor(white5);
						float arg_585C_5 = 0f;
						origin = default(Vector2);
						arg_585C_0.Draw(arg_585C_1, arg_585C_2, arg_585C_3, arg_585C_4, arg_585C_5, origin, num108, SpriteEffects.None, 0f);
					}
					if (Main.player[Main.myPlayer].armor[num105].stack > 1)
					{
						SpriteBatch arg_58E9_0 = this.spriteBatch;
						SpriteFont arg_58E9_1 = Main.fontItemStack;
						string arg_58E9_2 = string.Concat(Main.player[Main.myPlayer].armor[num105].stack);
						Vector2 arg_58E9_3 = new Vector2((float)num106 + 10f * Main.inventoryScale, (float)num107 + 26f * Main.inventoryScale);
						Color arg_58E9_4 = white5;
						float arg_58E9_5 = 0f;
						origin = default(Vector2);
						arg_58E9_0.DrawString(arg_58E9_1, arg_58E9_2, arg_58E9_3, arg_58E9_4, arg_58E9_5, origin, num108, SpriteEffects.None, 0f);
					}
				}
			}
			Main.MouseTextString = MouseTextString;
			
			/*if(ModPlayer.invSelectItem)
				ModPlayer.invSelectItemRelease=false;
            else ModPlayer.invSelectItemRelease=true;*/
			return false;
		}

        public bool PreDrawInventoryCoins(SpriteBatch s) {
            string MouseTextString = Main.MouseTextString;
            this.spriteBatch=s;

            Vector2 vector11 = Main.fontMouseText.MeasureString("Coins");
            Vector2 vector12 = Main.fontMouseText.MeasureString(Lang.inter[26]);
            float num144 = vector11.X / vector12.X;
            SpriteBatch arg_857B_0 = this.spriteBatch;
            SpriteFont arg_857B_1 = Main.fontMouseText;
            string arg_857B_2 = Lang.inter[26];
            Vector2 arg_857B_3 = new Vector2(496f, 84f + (vector11.Y - vector11.Y * num144) / 2f);
            Color arg_857B_4 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
            float arg_857B_5 = 0f;
            origin = default(Vector2);
            arg_857B_0.DrawString(arg_857B_1, arg_857B_2, arg_857B_3, arg_857B_4, arg_857B_5, origin, 0.75f * num144, SpriteEffects.None, 0f);
            Main.inventoryScale = 0.6f;
            for (int num145 = 0; num145 < 4; num145++)
            {
                int num146 = 497;
                int num147 = (int)(85f + (float)(num145 * 56) * Main.inventoryScale + 20f);
                int num148 = num145 + 40;
                Color white11 = new Color(100, 100, 100, 100);
                bool selected=(ModPlayer.invMenu==ModPlayer.INVENTORY && ModPlayer.invSelectionX==10 && ModPlayer.invSelectionY==num145);
                if (selected) //Main.mouseX >= num146 && (float)Main.mouseX <= (float)num146 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num147 && (float)Main.mouseY <= (float)num147 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale)
                {
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (ModPlayer.invSelectItemRelease)
                    {
                        /*if (Main.keyState.IsKeyDown(Keys.LeftShift))
                        {
                            if (Main.player[Main.myPlayer].inventory[num148].type > 0)
                            {
                                if (Main.npcShop > 0)
                                {
                                    if (Main.player[Main.myPlayer].SellItem(Main.player[Main.myPlayer].inventory[num148].value, Main.player[Main.myPlayer].inventory[num148].stack))
                                    {
                                        this.shop[Main.npcShop].AddShop(Main.player[Main.myPlayer].inventory[num148]);
                                        Main.player[Main.myPlayer].inventory[num148].SetDefaults(0, false);
                                        Main.PlaySound(18, -1, -1, 1);
                                    }
                                    else
                                    {
                                        if (Main.player[Main.myPlayer].inventory[num148].value == 0)
                                        {
                                            this.shop[Main.npcShop].AddShop(Main.player[Main.myPlayer].inventory[num148]);
                                            Main.player[Main.myPlayer].inventory[num148].SetDefaults(0, false);
                                            Main.PlaySound(7, -1, -1, 1);
                                        }
                                    }
                                }
                                else
                                {
                                    Main.PlaySound(7, -1, -1, 1);
                                    Main.trashItem = (Item)Main.player[Main.myPlayer].inventory[num148].Clone();
                                    Main.player[Main.myPlayer].inventory[num148].SetDefaults(0, false);
                                    Recipe.FindRecipes();
                                }
                            }
                        }
                        else
                        {*/

                            if ((Main.player[Main.myPlayer].selectedItem != num148 || Main.player[Main.myPlayer].itemAnimation <= 0) && (Main.mouseItem.type == 0 || Main.mouseItem.type == 71 || Main.mouseItem.type == 72 || Main.mouseItem.type == 73 || Main.mouseItem.type == 74))
                            {
                                PickItem(Main.player[Main.myPlayer].inventory, num148);
                            }
                        //}
                    }
                    else
                    {
                        if (ModPlayer.stackSplit <= 1 && ModPlayer.invSelectMore && (Main.mouseItem.IsTheSameAs(Main.player[Main.myPlayer].inventory[num148]) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
                        {
                            if (Main.mouseItem.type == 0)
                            {
                                Main.mouseItem = (Item)Main.player[Main.myPlayer].inventory[num148].Clone();
                                Main.mouseItem.stack = 0;
                            }
                            Main.mouseItem.stack++;
                            Main.player[Main.myPlayer].inventory[num148].stack--;
                            if (Main.player[Main.myPlayer].inventory[num148].stack <= 0)
                            {
                                Main.player[Main.myPlayer].inventory[num148] = new Item();
                            }
                            Recipe.FindRecipes();
                            Main.soundInstanceMenuTick.Stop();
                            Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
                            Main.PlaySound(12, -1, -1, 1);
                            if (ModPlayer.stackSplit == 0)
                            {
                                ModPlayer.stackSplit = 15;
                            }
                            else
                            {
                                ModPlayer.stackSplit = Main.stackDelay;
                            }
                        }
                    }
                    MouseTextString = Main.player[Main.myPlayer].inventory[num148].name;
                    Main.toolTip = (Item)Main.player[Main.myPlayer].inventory[num148].ShallowClone();
                    if (Main.player[Main.myPlayer].inventory[num148].stack > 1)
                    {
                        object obj = MouseTextString;
                        MouseTextString = string.Concat(new object[]
                    {
                        obj, 
                        " (", 
                        Main.player[Main.myPlayer].inventory[num148].stack, 
                        ")"
                    });
                    }
                }

                Texture2D back = Main.inventoryBackTexture;
                if(selected) back = Main.inventoryBack6Texture;
                            

                SpriteBatch arg_8C9C_0 = this.spriteBatch;
                Texture2D arg_8C9C_1 = back;
                Vector2 arg_8C9C_2 = new Vector2((float)num146, (float)num147);
                Rectangle? arg_8C9C_3 = new Rectangle?(new Rectangle(0, 0, Main.inventoryBackTexture.Width, Main.inventoryBackTexture.Height));
                Color arg_8C9C_4 = color2;
                float arg_8C9C_5 = 0f;
                origin = default(Vector2);
                arg_8C9C_0.Draw(arg_8C9C_1, arg_8C9C_2, arg_8C9C_3, arg_8C9C_4, arg_8C9C_5, origin, Main.inventoryScale, SpriteEffects.None, 0f);
                white11 = Color.White;
                if (Main.player[Main.myPlayer].inventory[num148].type > 0 && Main.player[Main.myPlayer].inventory[num148].stack > 0)
                {
                    float num150 = 1f;
                    if (Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Width > 32 || Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Height > 32)
                    {
                        if (Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Width > Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Height)
                        {
                            num150 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Width;
                        }
                        else
                        {
                            num150 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Height;
                        }
                    }
                    num150 *= Main.inventoryScale;
                    SpriteBatch arg_8F12_0 = this.spriteBatch;
                    Texture2D arg_8F12_1 = Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type];
                    Vector2 arg_8F12_2 = new Vector2((float)num146 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Width * 0.5f * num150, (float)num147 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Height * 0.5f * num150);
                    Rectangle? arg_8F12_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Width, Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Height));
                    Color arg_8F12_4 = Main.player[Main.myPlayer].inventory[num148].GetAlpha(white11);
                    float arg_8F12_5 = 0f;
                    origin = default(Vector2);
                    arg_8F12_0.Draw(arg_8F12_1, arg_8F12_2, arg_8F12_3, arg_8F12_4, arg_8F12_5, origin, num150, SpriteEffects.None, 0f);
                    Color arg_8F3D_0 = Main.player[Main.myPlayer].inventory[num148].color;
                    Color b = default(Color);
                    if (arg_8F3D_0 != b)
                    {
                        SpriteBatch arg_9071_0 = this.spriteBatch;
                        Texture2D arg_9071_1 = Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type];
                        Vector2 arg_9071_2 = new Vector2((float)num146 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Width * 0.5f * num150, (float)num147 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Height * 0.5f * num150);
                        Rectangle? arg_9071_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Width, Main.itemTexture[Main.player[Main.myPlayer].inventory[num148].type].Height));
                        Color arg_9071_4 = Main.player[Main.myPlayer].inventory[num148].GetColor(white11);
                        float arg_9071_5 = 0f;
                        origin = default(Vector2);
                        arg_9071_0.Draw(arg_9071_1, arg_9071_2, arg_9071_3, arg_9071_4, arg_9071_5, origin, num150, SpriteEffects.None, 0f);
                    }
                    if (Main.player[Main.myPlayer].inventory[num148].stack > 1)
                    {
                        SpriteBatch arg_90FE_0 = this.spriteBatch;
                        SpriteFont arg_90FE_1 = Main.fontItemStack;
                        string arg_90FE_2 = string.Concat(Main.player[Main.myPlayer].inventory[num148].stack);
                        Vector2 arg_90FE_3 = new Vector2((float)num146 + 10f * Main.inventoryScale, (float)num147 + 26f * Main.inventoryScale);
                        Color arg_90FE_4 = white11;
                        float arg_90FE_5 = 0f;
                        origin = default(Vector2);
                        arg_90FE_0.DrawString(arg_90FE_1, arg_90FE_2, arg_90FE_3, arg_90FE_4, arg_90FE_5, origin, num150, SpriteEffects.None, 0f);
                    }
                }
            }

            Main.MouseTextString = MouseTextString;

            return false;
        }

        public void DrawHeader(string str1, string str2)
        {
            Vector2 vector13 = Main.fontMouseText.MeasureString(str1);
            Vector2 vector14 = Main.fontMouseText.MeasureString(str2);
            float num151 = vector13.X / vector14.X;
            spriteBatch.DrawString(
                    Main.fontMouseText,
                    str2, 
                    new Vector2(532f, 84f + (vector13.Y - vector13.Y * num151) / 2f),
                    new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor), 
                    0f, 
                    default(Vector2),
                     0.75f * num151, 
                     SpriteEffects.None, 
                     0f);

        }
        public bool PreDrawInventoryAmmo(SpriteBatch s) {
            string MouseTextString = Main.MouseTextString;
            this.spriteBatch=s;

            DrawHeader("Ammo", Lang.inter[27]);

            Main.inventoryScale = 0.6f;
            for (int num152 = 0; num152 < 4; num152++)
            {
                int num153 = 534;
                int num154 = (int)(85f + (float)(num152 * 56) * Main.inventoryScale + 20f);
                int num155 = 44 + num152;
                Color white12 = new Color(100, 100, 100, 100);
                bool selected=(ModPlayer.invMenu==ModPlayer.INVENTORY && ModPlayer.invSelectionX==11 && ModPlayer.invSelectionY==num152);
                if (selected) //Main.mouseX >= num153 && (float)Main.mouseX <= (float)num153 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num154 && (float)Main.mouseY <= (float)num154 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale)
                {
                    Main.player[Main.myPlayer].mouseInterface = true;
                    if (ModPlayer.invSelectItemRelease)
                    {
                        /*if (Main.keyState.IsKeyDown(Keys.LeftShift))
                        {
                            if (Main.player[Main.myPlayer].inventory[num155].type > 0)
                            {
                                if (Main.npcShop > 0)
                                {
                                    if (Main.player[Main.myPlayer].SellItem(Main.player[Main.myPlayer].inventory[num155].value, Main.player[Main.myPlayer].inventory[num155].stack))
                                    {
                                        this.shop[Main.npcShop].AddShop(Main.player[Main.myPlayer].inventory[num155]);
                                        Main.player[Main.myPlayer].inventory[num155].SetDefaults(0, false);
                                        Main.PlaySound(18, -1, -1, 1);
                                    }
                                    else
                                    {
                                        if (Main.player[Main.myPlayer].inventory[num155].value == 0)
                                        {
                                            this.shop[Main.npcShop].AddShop(Main.player[Main.myPlayer].inventory[num155]);
                                            Main.player[Main.myPlayer].inventory[num155].SetDefaults(0, false);
                                            Main.PlaySound(7, -1, -1, 1);
                                        }
                                    }
                                }
                                else
                                {
                                    Main.PlaySound(7, -1, -1, 1);
                                    Main.trashItem = (Item)Main.player[Main.myPlayer].inventory[num155].Clone();
                                    Main.player[Main.myPlayer].inventory[num155].SetDefaults(0, false);
                                    Recipe.FindRecipes();
                                }
                            }
                        }
                        else
                        {*/
                            if ((Main.player[Main.myPlayer].selectedItem != num155 || Main.player[Main.myPlayer].itemAnimation <= 0) && (Main.mouseItem.type == 0 || Main.mouseItem.ammo > 0 || Main.mouseItem.type == 530))
                            {
                                PickItem(Main.player[Main.myPlayer].inventory, num155);
                            }
                        //}
                    }
                    else
                    {
                        if (ModPlayer.stackSplit <= 1 && ModPlayer.invSelectMore && (Main.mouseItem.IsTheSameAs(Main.player[Main.myPlayer].inventory[num155]) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
                        {
                            if (Main.mouseItem.type == 0)
                            {
                                Main.mouseItem = (Item)Main.player[Main.myPlayer].inventory[num155].Clone();
                                Main.mouseItem.stack = 0;
                            }
                            Main.mouseItem.stack++;
                            Main.player[Main.myPlayer].inventory[num155].stack--;
                            if (Main.player[Main.myPlayer].inventory[num155].stack <= 0)
                            {
                                Main.player[Main.myPlayer].inventory[num155] = new Item();
                            }
                            Recipe.FindRecipes();
                            Main.soundInstanceMenuTick.Stop();
                            Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
                            Main.PlaySound(12, -1, -1, 1);
                            if (ModPlayer.stackSplit == 0)
                            {
                                ModPlayer.stackSplit = 15;
                            }
                            else
                            {
                                ModPlayer.stackSplit = Main.stackDelay;
                            }
                        }
                    }
                    MouseTextString = Main.player[Main.myPlayer].inventory[num155].name;
                    Main.toolTip = (Item)Main.player[Main.myPlayer].inventory[num155].ShallowClone();
                    if (Main.player[Main.myPlayer].inventory[num155].stack > 1)
                    {
                        object obj = MouseTextString;
                        MouseTextString = string.Concat(new object[]
                    {
                        obj, 
                        " (", 
                        Main.player[Main.myPlayer].inventory[num155].stack, 
                        ")"
                    });
                    }
                }

                Texture2D back = Main.inventoryBackTexture;
                if(selected) back = Main.inventoryBack6Texture;

                SpriteBatch arg_98C3_0 = this.spriteBatch;
                Texture2D arg_98C3_1 = back;
                Vector2 arg_98C3_2 = new Vector2((float)num153, (float)num154);
                Rectangle? arg_98C3_3 = new Rectangle?(new Rectangle(0, 0, Main.inventoryBackTexture.Width, Main.inventoryBackTexture.Height));
                Color arg_98C3_4 = color2;
                float arg_98C3_5 = 0f;
                origin = default(Vector2);
                arg_98C3_0.Draw(arg_98C3_1, arg_98C3_2, arg_98C3_3, arg_98C3_4, arg_98C3_5, origin, Main.inventoryScale, SpriteEffects.None, 0f);
                white12 = Color.White;
                if (Main.player[Main.myPlayer].inventory[num155].type > 0 && Main.player[Main.myPlayer].inventory[num155].stack > 0)
                {
                    float num157 = 1f;
                    if (Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Width > 32 || Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Height > 32)
                    {
                        if (Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Width > Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Height)
                        {
                            num157 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Width;
                        }
                        else
                        {
                            num157 = 32f / (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Height;
                        }
                    }
                    num157 *= Main.inventoryScale;
                    SpriteBatch arg_9B39_0 = this.spriteBatch;
                    Texture2D arg_9B39_1 = Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type];
                    Vector2 arg_9B39_2 = new Vector2((float)num153 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Width * 0.5f * num157, (float)num154 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Height * 0.5f * num157);
                    Rectangle? arg_9B39_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Width, Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Height));
                    Color arg_9B39_4 = Main.player[Main.myPlayer].inventory[num155].GetAlpha(white12);
                    float arg_9B39_5 = 0f;
                    origin = default(Vector2);
                    arg_9B39_0.Draw(arg_9B39_1, arg_9B39_2, arg_9B39_3, arg_9B39_4, arg_9B39_5, origin, num157, SpriteEffects.None, 0f);
                    Color arg_9B64_0 = Main.player[Main.myPlayer].inventory[num155].color;
                    Color b = default(Color);
                    if (arg_9B64_0 != b)
                    {
                        SpriteBatch arg_9C98_0 = this.spriteBatch;
                        Texture2D arg_9C98_1 = Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type];
                        Vector2 arg_9C98_2 = new Vector2((float)num153 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Width * 0.5f * num157, (float)num154 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Height * 0.5f * num157);
                        Rectangle? arg_9C98_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Width, Main.itemTexture[Main.player[Main.myPlayer].inventory[num155].type].Height));
                        Color arg_9C98_4 = Main.player[Main.myPlayer].inventory[num155].GetColor(white12);
                        float arg_9C98_5 = 0f;
                        origin = default(Vector2);
                        arg_9C98_0.Draw(arg_9C98_1, arg_9C98_2, arg_9C98_3, arg_9C98_4, arg_9C98_5, origin, num157, SpriteEffects.None, 0f);
                    }
                    if (Main.player[Main.myPlayer].inventory[num155].stack > 1)
                    {
                        SpriteBatch arg_9D25_0 = this.spriteBatch;
                        SpriteFont arg_9D25_1 = Main.fontItemStack;
                        string arg_9D25_2 = string.Concat(Main.player[Main.myPlayer].inventory[num155].stack);
                        Vector2 arg_9D25_3 = new Vector2((float)num153 + 10f * Main.inventoryScale, (float)num154 + 26f * Main.inventoryScale);
                        Color arg_9D25_4 = white12;
                        float arg_9D25_5 = 0f;
                        origin = default(Vector2);
                        arg_9D25_0.DrawString(arg_9D25_1, arg_9D25_2, arg_9D25_3, arg_9D25_4, arg_9D25_5, origin, num157, SpriteEffects.None, 0f);
                    }
                }
            }

            Main.MouseTextString = MouseTextString;

            return false;
        }

        public bool PreDrawChestButtons(SpriteBatch s)
        {
            this.spriteBatch = s;

            Main.inventoryScale = 0.75f;

            if (Main.mouseX > 73 && Main.mouseX < (int)(73f + 280f * Main.inventoryScale) && Main.mouseY > 210 && Main.mouseY < (int)(210f + 224f * Main.inventoryScale))
                Main.player[Main.myPlayer].mouseInterface = true;
            
            for (int num164 = 0; num164 < 3; num164++)
            {
                int num165 = 286;
                int num166 = 250;
                float num167 = Config.mainInstance.chestLootScale;
                string text23 = Lang.inter[29];
                if (num164 == 1)
                {
                    num166 += 26;
                    num167 = Config.mainInstance.chestDepositScale;
                    text23 = Lang.inter[30];
                }
                else
                {
                    if (num164 == 2)
                    {
                        num166 += 52;
                        num167 = Config.mainInstance.chestStackScale;
                        text23 = Lang.inter[31];
                    }
                }
                Vector2 vector15 = Main.fontMouseText.MeasureString(text23) / 2f;
                Color color8 = new Color((int)((byte)((float)Main.mouseTextColor * num167)), (int)((byte)((float)Main.mouseTextColor * num167)), (int)((byte)((float)Main.mouseTextColor * num167)), (int)((byte)((float)Main.mouseTextColor * num167)));
                num165 += (int)(vector15.X * num167);
                Config.mainInstance.spriteBatch.DrawString(Main.fontMouseText, text23, new Vector2((float)num165, (float)num166), color8, 0f, vector15, num167, SpriteEffects.None, 0f);
                vector15 *= num167;
                if (ModPlayer.invMenu==ModPlayer.CHEST && ModPlayer.chestSelX==5 && ModPlayer.chestSelY==num164) //(float)Main.mouseX > (float)num165 - vector15.X && (float)Main.mouseX < (float)num165 + vector15.X && (float)Main.mouseY > (float)num166 - vector15.Y && (float)Main.mouseY < (float)num166 + vector15.Y)
                {
                    if (num164 == 0)
                    {
                        if (!Config.mainInstance.chestLootHover)
                        {
                            Main.PlaySound(12, -1, -1, 1);
                        }
                        Config.mainInstance.chestLootHover = true;
                    }
                    else
                    {
                        if (num164 == 1)
                        {
                            if (!Config.mainInstance.chestDepositHover)
                            {
                                Main.PlaySound(12, -1, -1, 1);
                            }
                            Config.mainInstance.chestDepositHover = true;
                        }
                        else
                        {
                            if (!Config.mainInstance.chestStackHover)
                            {
                                Main.PlaySound(12, -1, -1, 1);
                            }
                            Config.mainInstance.chestStackHover = true;
                        }
                    }
                    Main.player[Main.myPlayer].mouseInterface = true;
                    num167 += 0.05f;
                    if (ModPlayer.invSelectItemRelease)
                    {
                        Interface.chestFunc(Main.player[Main.myPlayer].chest, num164);
                        Recipe.FindRecipes();
                    }
                }
                else
                {
                    num167 -= 0.5f;
                    if (num164 == 0)
                    {
                        Config.mainInstance.chestLootHover = false;
                    }
                    else
                    {
                        if (num164 == 1)
                        {
                            Config.mainInstance.chestDepositHover = false;
                        }
                        else
                        {
                            Config.mainInstance.chestStackHover = false;
                        }
                    }
                }
                if ((double)num167 < 0.75)
                {
                    num167 = 0.75f;
                }
                if (num167 > 1.5f)
                {
                    num167 = 1.5f;
                }
                if (num164 == 0)
                {
                    Config.mainInstance.chestLootScale = num167;
                }
                else
                {
                    if (num164 == 1)
                    {
                        Config.mainInstance.chestDepositScale = num167;
                    }
                    else
                    {
                        Config.mainInstance.chestStackScale = num167;
                    }
                }
            }

            drawChest(Main.player[Main.myPlayer].chest, ref Main.MouseTextString);

            return false;
        }

       public void drawChest(int type, ref string mouseTip)
        {
            if (type == -1) return;
            string name = null;
            if(type>-1) name= Main.chestText; //Always "Chest" ?
            if (type == -2) name = Lang.inter[32]; //Piggy Bank
            if (type == -3) name = Lang.inter[33]; //Safe*/
            Item[] chest = null;
            if (type > -1 && Main.chest[Main.player[Main.myPlayer].chest]!=null) chest = Main.chest[Main.player[Main.myPlayer].chest].item;
            if (type == -2) chest = Main.player[Main.myPlayer].bank;
            if (type == -3) chest = Main.player[Main.myPlayer].bank2;
            bool netUpdate = false;
            if (type > -1) netUpdate = true;
            //Debug.WriteLine("drawChest:" + name + "," + chest.Length + "," + netUpdate);

            Texture2D invTexture = null;
            if (type > -1) invTexture = Main.inventoryBack5Texture;
            if (type == -2 || type==-3) invTexture = Main.inventoryBack2Texture;

            if(name!=null && chest!=null && invTexture!=null)
                drawChest(name, chest, invTexture, netUpdate, ref mouseTip);
        }
        public void drawChest(string chestName, Item[] chest, Texture2D invTexture, bool netUpdate, ref string mouseTip)
        { //type = Main.player[Main.myPlayer].chest
            SpriteBatch spriteBatch = Interface.main.spriteBatch;
            Color color2 = new Color((int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha));
            //SpriteBatch spriteBatch = Config.mainInstance.spriteBatch;
            SpriteFont spriteFont = Main.fontMouseText;
            string text=chestName;
            /*if(type>-1) text= Main.chestText; //Always "Chest" ?
            if (type == -2) text = Lang.inter[32]; //Piggy Bank
            if (type == -3) text = Lang.inter[33]; //Safe*/
            Vector2 position = new Vector2(284f, 210f);
            Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
            float rotation = 0f;
            Vector2 origin = default(Vector2);
            spriteBatch.DrawString(spriteFont, text, position, color, rotation, origin, 1f, SpriteEffects.None, 0f);
            Main.inventoryScale = 0.75f;
            if (Main.mouseX > 73 && Main.mouseX < (int)(73f + 280f * Main.inventoryScale) && Main.mouseY > 210 && Main.mouseY < (int)(210f + 224f * Main.inventoryScale))
            {
                Main.player[Main.myPlayer].mouseInterface = true;
            }
            //Item[] chest = null;
            /*if (type > -1) chest = Main.chest[Main.player[Main.myPlayer].chest].item;
            if (type == -2) chest = Main.player[Main.myPlayer].bank;
            if (type == -3) chest = Main.player[Main.myPlayer].bank2;*/

            for (int num188 = 0; num188 < 5; num188++)
            {
                for (int num189 = 0; num189 < 4; num189++)
                {
                    int num190 = (int)(73f + (float)(num188 * 56) * Main.inventoryScale);
                    int num191 = (int)(210f + (float)(num189 * 56) * Main.inventoryScale);
                    int num192 = num188 + num189 * 5;
                    Color white14 = new Color(100, 100, 100, 100);
                    bool selected=(ModPlayer.invMenu==ModPlayer.CHEST && ModPlayer.chestSelX==num188 && ModPlayer.chestSelY==num189);
                    if (selected) //Main.mouseX >= num190 && (float)Main.mouseX <= (float)num190 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num191 && (float)Main.mouseY <= (float)num191 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale)
                    {
                        Main.player[Main.myPlayer].mouseInterface = true;
                        /*if (ModPlayer.invSelectItemRelease)
                        {

                            if (Main.player[Main.myPlayer].selectedItem != num192 || Main.player[Main.myPlayer].itemAnimation <= 0)
                            {
                                PickItem(chest, num192);
                                if (netUpdate && Main.netMode == 1) //Only occurs with physical tile chests
                                {
                                    NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)num192, 0f, 0f, 0);
                                }
                            }
                        }
                        else
                        {*/
                            if (Main.mouseRight && Main.mouseRightRelease && chest[num192].maxStack == 1)
                            {
                                chest[num192] = Main.armorSwap(chest[num192]);
                                if (netUpdate && Main.netMode == 1)
                                {
                                    NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)num192, 0f, 0f, 0);
                                }
                            }
                            else
                            {
                                if (ModPlayer.stackSplit <= 1 && ModPlayer.invSelectMore && chest[num192].maxStack > 1 && (Main.mouseItem.IsTheSameAs(chest[num192]) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
                                {
                                    if (Main.mouseItem.type == 0)
                                    {
                                        Main.mouseItem = (Item)chest[num192].Clone();
                                        Main.mouseItem.stack = 0;
                                    }
                                    Main.mouseItem.stack++;
                                    chest[num192].stack--;
                                    if (chest[num192].stack <= 0)
                                    {
                                        chest[num192] = new Item();
                                    }
                                    Recipe.FindRecipes();
                                    Main.soundInstanceMenuTick.Stop();
                                    Main.soundInstanceMenuTick = Main.soundMenuTick.CreateInstance();
                                    Main.PlaySound(12, -1, -1, 1);
                                    if (ModPlayer.stackSplit == 0)
                                    {
                                        ModPlayer.stackSplit = 15;
                                    }
                                    else
                                    {
                                        ModPlayer.stackSplit = Main.stackDelay;
                                    }
                                    if (netUpdate && Main.netMode == 1)
                                    {
                                        NetMessage.SendData(32, -1, -1, "", Main.player[Main.myPlayer].chest, (float)num192, 0f, 0f, 0);
                                    }
                                }
                            }
                        //}
                        mouseTip = chest[num192].name;
                        Main.toolTip = (Item)chest[num192].ShallowClone();
                        if (chest[num192].stack > 1)
                        {
                            object obj = mouseTip;
                            mouseTip = string.Concat(new object[]
                                    {
                                        obj, 
                                        " (", 
                                        chest[num192].stack, 
                                        ")"
                                    });
                        }
                    }
                    Texture2D back;
                    if(selected) back = Main.inventoryBack6Texture;
                    else back = invTexture;
                    Interface.DrawItem(back, chest[num192], num190, num191);
                }
            }
        }

        public bool PreDrawAvailableRecipes(SpriteBatch s)
        {
            this.spriteBatch = s;
            int num109 = (Main.screenHeight - 600) / 2;
            int num110 = (int)((float)Main.screenHeight / 600f * 250f);
            string MouseTextString = Main.MouseTextString;

            Color color6 = new Color((int)((byte)((float)Main.mouseTextColor * Main.craftingAlpha)), (int)((byte)((float)Main.mouseTextColor * Main.craftingAlpha)), (int)((byte)((float)Main.mouseTextColor * Main.craftingAlpha)), (int)((byte)((float)Main.mouseTextColor * Main.craftingAlpha)));

            if (Main.numAvailableRecipes > 0)
            {
                SpriteBatch arg_7137_0 = this.spriteBatch;
                SpriteFont arg_7137_1 = Main.fontMouseText;
                string arg_7137_2 = Lang.inter[25];
                Vector2 arg_7137_3 = new Vector2(76f, (float)(414 + num109));
                Color arg_7137_4 = color6;
                float arg_7137_5 = 0f;
                origin = default(Vector2);
                arg_7137_0.DrawString(arg_7137_1, arg_7137_2, arg_7137_3, arg_7137_4, arg_7137_5, origin, 1f, SpriteEffects.None, 0f);
            }
            for (int num132 = 0; num132 < Recipe.maxRecipes; num132++)
            {
                Main.inventoryScale = 100f / (Math.Abs(Main.availableRecipeY[num132]) + 100f);
                if ((double)Main.inventoryScale < 0.75)
                {
                    Main.inventoryScale = 0.75f;
                }
                if (Main.availableRecipeY[num132] < (float)((num132 - Main.focusRecipe) * 65))
                {
                    if (Main.availableRecipeY[num132] == 0f)
                    {
                        Main.PlaySound(12, -1, -1, 1);
                    }
                    Main.availableRecipeY[num132] += 6.5f;
                }
                else
                {
                    if (Main.availableRecipeY[num132] > (float)((num132 - Main.focusRecipe) * 65))
                    {
                        if (Main.availableRecipeY[num132] == 0f)
                        {
                            Main.PlaySound(12, -1, -1, 1);
                        }
                        Main.availableRecipeY[num132] -= 6.5f;
                    }
                }
                if (num132 < Main.numAvailableRecipes && Math.Abs(Main.availableRecipeY[num132]) <= (float)num110)
                {
                    int num133 = (int)(46f - 26f * Main.inventoryScale);
                    int num134 = (int)(410f + Main.availableRecipeY[num132] * Main.inventoryScale - 30f * Main.inventoryScale + (float)num109);
                    double num135 = (double)(color2.A + 50);
                    double num136 = 255.0;
                    if (Math.Abs(Main.availableRecipeY[num132]) > (float)(num110 - 100))
                    {
                        num135 = (double)(150f * (100f - (Math.Abs(Main.availableRecipeY[num132]) - (float)(num110 - 100)))) * 0.01;
                        num136 = (double)(255f * (100f - (Math.Abs(Main.availableRecipeY[num132]) - (float)(num110 - 100)))) * 0.01;
                    }
                    new Color((int)((byte)num135), (int)((byte)num135), (int)((byte)num135), (int)((byte)num135));
                    Color color7 = new Color((int)((byte)num136), (int)((byte)num136), (int)((byte)num136), (int)((byte)num136));
                    //if (Main.mouseX >= num133 && (float)Main.mouseX <= (float)num133 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num134 && (float)Main.mouseY <= (float)num134 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale)
                    //{
                        Main.player[Main.myPlayer].mouseInterface = true;
                        if (Main.focusRecipe == num132 && Main.guideItem.type == 0)
                        {
                            if (Main.mouseItem.type == 0 || (Main.mouseItem.IsTheSameAs(Main.recipe[Main.availableRecipe[num132]].createItem) && Main.mouseItem.stack + Main.recipe[Main.availableRecipe[num132]].createItem.stack <= Main.mouseItem.maxStack))
                            {
                                /*if (ModPlayer.craftBtn)
                                {
                                    int stack = Main.mouseItem.stack;
                                    Main.mouseItem = (Item)Main.recipe[Main.availableRecipe[num132]].createItem.Clone();
                                    Main.mouseItem.Prefix(-1);
                                    Main.mouseItem.stack += stack;
                                    Main.mouseItem.position.X = Main.player[Main.myPlayer].position.X + (float)(Main.player[Main.myPlayer].width / 2) - (float)(Main.mouseItem.width / 2);
                                    Main.mouseItem.position.Y = Main.player[Main.myPlayer].position.Y + (float)(Main.player[Main.myPlayer].height / 2) - (float)(Main.mouseItem.height / 2);
                                    ItemText.NewText(Main.mouseItem, Main.recipe[Main.availableRecipe[num132]].createItem.stack);
                                    Main.recipe[Main.availableRecipe[num132]].Create(Main.mouseItem);
                                    if (Main.mouseItem.type > 0 || Main.recipe[Main.availableRecipe[num132]].createItem.type > 0)
                                    {
                                        Main.PlaySound(7, -1, -1, 1);
                                    }
                                }
                                else
                                {*/
                                    if (ModPlayer.stackSplit <= 1 && ModPlayer.craftBtn && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
                                    {
                                        if (ModPlayer.stackSplit == 0)
                                        {
                                            ModPlayer.stackSplit = 15;
                                        }
                                        else
                                        {
                                            ModPlayer.stackSplit = Main.stackDelay;
                                        }
                                        int stack2 = Main.mouseItem.stack;
                                        Main.mouseItem = (Item)Main.recipe[Main.availableRecipe[num132]].createItem.Clone();
                                        Main.mouseItem.Prefix(-1);
                                        Main.mouseItem.stack += stack2;
                                        Main.mouseItem.position.X = Main.player[Main.myPlayer].position.X + (float)(Main.player[Main.myPlayer].width / 2) - (float)(Main.mouseItem.width / 2);
                                        Main.mouseItem.position.Y = Main.player[Main.myPlayer].position.Y + (float)(Main.player[Main.myPlayer].height / 2) - (float)(Main.mouseItem.height / 2);
                                        ItemText.NewText(Main.mouseItem, Main.recipe[Main.availableRecipe[num132]].createItem.stack);
                                        Main.recipe[Main.availableRecipe[num132]].Create(Main.mouseItem);
                                        if (Main.mouseItem.type > 0 || Main.recipe[Main.availableRecipe[num132]].createItem.type > 0)
                                        {
                                            Main.PlaySound(7, -1, -1, 1);
                                        }
                                    }
                                //}
                            }
                        }
                        else
                        {
                            if (Main.mouseLeftRelease && Main.mouseLeft)
                            {
                                Main.focusRecipe = num132;
                            }
                        }
                        Main.craftingHide = true;
                        MouseTextString = Main.recipe[Main.availableRecipe[num132]].createItem.name;
                        Main.toolTip = (Item)Main.recipe[Main.availableRecipe[num132]].createItem.ShallowClone();
                        if (Main.recipe[Main.availableRecipe[num132]].createItem.stack > 1)
                        {
                            object obj = MouseTextString;
                            MouseTextString = string.Concat(new object[]
                        {
                            obj, 
                            " (", 
                            Main.recipe[Main.availableRecipe[num132]].createItem.stack, 
                            ")"
                        });
                        }
                    //}
                    if (Main.numAvailableRecipes > 0)
                    {
                        num135 -= 50.0;
                        if (num135 < 0.0)
                        {
                            num135 = 0.0;
                        }
                        SpriteBatch arg_784D_0 = this.spriteBatch;
                        Texture2D arg_784D_1 = Main.inventoryBack4Texture;
                        Vector2 arg_784D_2 = new Vector2((float)num133, (float)num134);
                        Rectangle? arg_784D_3 = new Rectangle?(new Rectangle(0, 0, Main.inventoryBackTexture.Width, Main.inventoryBackTexture.Height));
                        Color arg_784D_4 = new Color((int)((byte)num135), (int)((byte)num135), (int)((byte)num135), (int)((byte)num135));
                        float arg_784D_5 = 0f;
                        origin = default(Vector2);
                        arg_784D_0.Draw(arg_784D_1, arg_784D_2, arg_784D_3, arg_784D_4, arg_784D_5, origin, Main.inventoryScale, SpriteEffects.None, 0f);
                        if (Main.recipe[Main.availableRecipe[num132]].createItem.type > 0 && Main.recipe[Main.availableRecipe[num132]].createItem.stack > 0)
                        {
                            float num137 = 1f;
                            if (Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Width > 32 || Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Height > 32)
                            {
                                if (Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Width > Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Height)
                                {
                                    num137 = 32f / (float)Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Width;
                                }
                                else
                                {
                                    num137 = 32f / (float)Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Height;
                                }
                            }
                            num137 *= Main.inventoryScale;
                            SpriteBatch arg_7ABC_0 = this.spriteBatch;
                            Texture2D arg_7ABC_1 = Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type];
                            Vector2 arg_7ABC_2 = new Vector2((float)num133 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Width * 0.5f * num137, (float)num134 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Height * 0.5f * num137);
                            Rectangle? arg_7ABC_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Width, Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Height));
                            Color arg_7ABC_4 = Main.recipe[Main.availableRecipe[num132]].createItem.GetAlpha(color7);
                            float arg_7ABC_5 = 0f;
                            origin = default(Vector2);
                            arg_7ABC_0.Draw(arg_7ABC_1, arg_7ABC_2, arg_7ABC_3, arg_7ABC_4, arg_7ABC_5, origin, num137, SpriteEffects.None, 0f);
                            Color arg_7AE7_0 = Main.recipe[Main.availableRecipe[num132]].createItem.color;
                            Color b = default(Color);
                            if (arg_7AE7_0 != b)
                            {
                                SpriteBatch arg_7C1B_0 = this.spriteBatch;
                                Texture2D arg_7C1B_1 = Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type];
                                Vector2 arg_7C1B_2 = new Vector2((float)num133 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Width * 0.5f * num137, (float)num134 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Height * 0.5f * num137);
                                Rectangle? arg_7C1B_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Width, Main.itemTexture[Main.recipe[Main.availableRecipe[num132]].createItem.type].Height));
                                Color arg_7C1B_4 = Main.recipe[Main.availableRecipe[num132]].createItem.GetColor(color7);
                                float arg_7C1B_5 = 0f;
                                origin = default(Vector2);
                                arg_7C1B_0.Draw(arg_7C1B_1, arg_7C1B_2, arg_7C1B_3, arg_7C1B_4, arg_7C1B_5, origin, num137, SpriteEffects.None, 0f);
                            }
                            if (Main.recipe[Main.availableRecipe[num132]].createItem.stack > 1)
                            {
                                SpriteBatch arg_7CA8_0 = this.spriteBatch;
                                SpriteFont arg_7CA8_1 = Main.fontItemStack;
                                string arg_7CA8_2 = string.Concat(Main.recipe[Main.availableRecipe[num132]].createItem.stack);
                                Vector2 arg_7CA8_3 = new Vector2((float)num133 + 10f * Main.inventoryScale, (float)num134 + 26f * Main.inventoryScale);
                                Color arg_7CA8_4 = color7;
                                float arg_7CA8_5 = 0f;
                                origin = default(Vector2);
                                arg_7CA8_0.DrawString(arg_7CA8_1, arg_7CA8_2, arg_7CA8_3, arg_7CA8_4, arg_7CA8_5, origin, num137, SpriteEffects.None, 0f);
                            }
                        }
                    }
                }
            }
            if (Main.numAvailableRecipes > 0)
            {
                int num138 = 0;
                while (num138 < Recipe.maxRequirements && Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type != 0)
                {
                    int num139 = 80 + num138 * 40;
                    int num140 = 380 + num109;
                    double num141 = (double)(color2.A + 50);
                    double num142 = 255.0;
                    Color white9 = Color.White;
                    Color white10 = Color.White;
                    num141 = (double)((float)(color2.A + 50) - Math.Abs(Main.availableRecipeY[Main.focusRecipe]) * 2f);
                    num142 = (double)(255f - Math.Abs(Main.availableRecipeY[Main.focusRecipe]) * 2f);
                    if (num141 < 0.0)
                    {
                        num141 = 0.0;
                    }
                    if (num142 < 0.0)
                    {
                        num142 = 0.0;
                    }
                    white9.R = (byte)num141;
                    white9.G = (byte)num141;
                    white9.B = (byte)num141;
                    white9.A = (byte)num141;
                    white10.R = (byte)num142;
                    white10.G = (byte)num142;
                    white10.B = (byte)num142;
                    white10.A = (byte)num142;
                    Main.inventoryScale = 0.6f;
                    if (num141 == 0.0)
                    {
                        break;
                    }
                    if (Main.mouseX >= num139 && (float)Main.mouseX <= (float)num139 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num140 && (float)Main.mouseY <= (float)num140 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale)
                    {
                        Main.craftingHide = true;
                        Main.player[Main.myPlayer].mouseInterface = true;
                        MouseTextString = Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].name;
                        Main.toolTip = (Item)Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].ShallowClone();
                        if (Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].stack > 1)
                        {
                            object obj = MouseTextString;
                            MouseTextString = string.Concat(new object[]
                        {
                            obj, 
                            " (", 
                            Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].stack, 
                            ")"
                        });
                        }
                    }
                    num141 -= 50.0;
                    if (num141 < 0.0)
                    {
                        num141 = 0.0;
                    }
                    SpriteBatch arg_7FD4_0 = this.spriteBatch;
                    Texture2D arg_7FD4_1 = Main.inventoryBack4Texture;
                    Vector2 arg_7FD4_2 = new Vector2((float)num139, (float)num140);
                    Rectangle? arg_7FD4_3 = new Rectangle?(new Rectangle(0, 0, Main.inventoryBackTexture.Width, Main.inventoryBackTexture.Height));
                    Color arg_7FD4_4 = new Color((int)((byte)num141), (int)((byte)num141), (int)((byte)num141), (int)((byte)num141));
                    float arg_7FD4_5 = 0f;
                    origin = default(Vector2);
                    arg_7FD4_0.Draw(arg_7FD4_1, arg_7FD4_2, arg_7FD4_3, arg_7FD4_4, arg_7FD4_5, origin, Main.inventoryScale, SpriteEffects.None, 0f);
                    if (Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type > 0 && Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].stack > 0)
                    {
                        float num143 = 1f;
                        if (Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Width > 32 || Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Height > 32)
                        {
                            if (Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Width > Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Height)
                            {
                                num143 = 32f / (float)Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Width;
                            }
                            else
                            {
                                num143 = 32f / (float)Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Height;
                            }
                        }
                        num143 *= Main.inventoryScale;
                        SpriteBatch arg_8297_0 = this.spriteBatch;
                        Texture2D arg_8297_1 = Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type];
                        Vector2 arg_8297_2 = new Vector2((float)num139 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Width * 0.5f * num143, (float)num140 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Height * 0.5f * num143);
                        Rectangle? arg_8297_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Width, Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Height));
                        Color arg_8297_4 = Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].GetAlpha(white10);
                        float arg_8297_5 = 0f;
                        origin = default(Vector2);
                        arg_8297_0.Draw(arg_8297_1, arg_8297_2, arg_8297_3, arg_8297_4, arg_8297_5, origin, num143, SpriteEffects.None, 0f);
                        Color arg_82C8_0 = Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].color;
                        Color b = default(Color);
                        if (arg_82C8_0 != b)
                        {
                            SpriteBatch arg_8420_0 = this.spriteBatch;
                            Texture2D arg_8420_1 = Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type];
                            Vector2 arg_8420_2 = new Vector2((float)num139 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Width * 0.5f * num143, (float)num140 + 26f * Main.inventoryScale - (float)Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Height * 0.5f * num143);
                            Rectangle? arg_8420_3 = new Rectangle?(new Rectangle(0, 0, Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Width, Main.itemTexture[Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].type].Height));
                            Color arg_8420_4 = Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].GetColor(white10);
                            float arg_8420_5 = 0f;
                            origin = default(Vector2);
                            arg_8420_0.Draw(arg_8420_1, arg_8420_2, arg_8420_3, arg_8420_4, arg_8420_5, origin, num143, SpriteEffects.None, 0f);
                        }
                        if (Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].stack > 1)
                        {
                            SpriteBatch arg_84B9_0 = this.spriteBatch;
                            SpriteFont arg_84B9_1 = Main.fontItemStack;
                            string arg_84B9_2 = string.Concat(Main.recipe[Main.availableRecipe[Main.focusRecipe]].requiredItem[num138].stack);
                            Vector2 arg_84B9_3 = new Vector2((float)num139 + 10f * Main.inventoryScale, (float)num140 + 26f * Main.inventoryScale);
                            Color arg_84B9_4 = white10;
                            float arg_84B9_5 = 0f;
                            origin = default(Vector2);
                            arg_84B9_0.DrawString(arg_84B9_1, arg_84B9_2, arg_84B9_3, arg_84B9_4, arg_84B9_5, origin, num143, SpriteEffects.None, 0f);
                        }
                    }
                    num138++;
                }
            }

            Main.MouseTextString = MouseTextString;

            return false;
        }
		
#if DEBUG
    }

    
}
#endif