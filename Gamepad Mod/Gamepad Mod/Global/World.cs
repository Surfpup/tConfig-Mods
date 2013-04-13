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

        public void PickItem(int num81)
        {
            if (Main.player[Main.myPlayer].selectedItem != num81 || Main.player[Main.myPlayer].itemAnimation <= 0)
            {
                Item item2 = Main.mouseItem;
                Main.mouseItem = Main.player[Main.myPlayer].inventory[num81];
                Main.player[Main.myPlayer].inventory[num81] = item2;
                if (Main.player[Main.myPlayer].inventory[num81].type == 0 || Main.player[Main.myPlayer].inventory[num81].stack < 1)
                {
                    Main.player[Main.myPlayer].inventory[num81] = new Item();
                }
                if (Main.mouseItem.IsTheSameAs(Main.player[Main.myPlayer].inventory[num81]) && Main.player[Main.myPlayer].inventory[num81].stack != Main.player[Main.myPlayer].inventory[num81].maxStack && Main.mouseItem.stack != Main.mouseItem.maxStack)
                {
                    if (Main.mouseItem.stack + Main.player[Main.myPlayer].inventory[num81].stack <= Main.mouseItem.maxStack)
                    {
                        Main.player[Main.myPlayer].inventory[num81].stack += Main.mouseItem.stack;
                        Main.mouseItem.stack = 0;
                    }
                    else
                    {
                        int num82 = Main.mouseItem.maxStack - Main.player[Main.myPlayer].inventory[num81].stack;
                        Main.player[Main.myPlayer].inventory[num81].stack += num82;
                        Main.mouseItem.stack -= num82;
                    }
                }
                if (Main.mouseItem.type == 0 || Main.mouseItem.stack < 1)
                {
                    Main.mouseItem = new Item();
                }
                if (Main.mouseItem.type > 0 || Main.player[Main.myPlayer].inventory[num81].type > 0)
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
            if(ModPlayer.invMenu!=ModPlayer.INVENTORY) return false;

            //Main.inventoryScale = 1.5f;

			this.spriteBatch = s;
			Vector2 origin;
			Color color2 = new Color((int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha));
			string MouseTextString = Main.MouseTextString;
					for (int num77 = 0; num77 < 10; num77++)
                    {
                        for (int num78 = 0; num78 < 4; num78++)
                        {
                            int num79 = (int)(20f + (float)(num77 * 56) * Main.inventoryScale);
                            int num80 = (int)(20f + (float)(num78 * 56) * Main.inventoryScale);
                            int num81 = num77 + num78 * 10;
                            Color white2 = new Color(100, 100, 100, 100);
                            if (InvSlotHighlighted(num77, num78))
                            {
                                Main.player[Main.myPlayer].mouseInterface = true;
                                if (ModPlayer.invSelectItem && ModPlayer.invSelectItemRelease) //(Main.mouseLeftRelease && Main.mouseLeft)
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
                                        PickItem(num81);
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
                                            if (Main.stackSplit <= 1 && Main.mouseRight && Main.player[Main.myPlayer].inventory[num81].maxStack > 1 && Main.player[Main.myPlayer].inventory[num81].type > 0 && (Main.mouseItem.IsTheSameAs(Main.player[Main.myPlayer].inventory[num81]) || Main.mouseItem.type == 0) && (Main.mouseItem.stack < Main.mouseItem.maxStack || Main.mouseItem.type == 0))
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
                                                if (Main.stackSplit == 0)
                                                {
                                                    Main.stackSplit = 15;
                                                }
                                                else
                                                {
                                                    Main.stackSplit = Main.stackDelay;
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
            if(ModPlayer.invMenu!=ModPlayer.ARMOR) return false;

			Main.armorHide = false;
			this.spriteBatch = s;
			Vector2 origin;
			Color color2 = new Color((int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha));
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
				if (num100==ModPlayer.armorSel) //Main.mouseX >= num101 && (float)Main.mouseX <= (float)num101 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num102 && (float)Main.mouseY <= (float)num102 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale)
				{
					Main.armorHide = true;
					Main.player[Main.myPlayer].mouseInterface = true;
					if (ModPlayer.invSelectItem && ModPlayer.invSelectItemRelease && (Main.mouseItem.type == 0 || (Main.mouseItem.headSlot > -1 && num100 == 0) || (Main.mouseItem.bodySlot > -1 && num100 == 1) || (Main.mouseItem.legSlot > -1 && num100 == 2) || (Main.mouseItem.accessory && num100 > 2 && !AccCheck(Main.mouseItem, num100))))
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
				if (num105==ModPlayer.armorSel) //Main.mouseX >= num106 && (float)Main.mouseX <= (float)num106 + (float)Main.inventoryBackTexture.Width * Main.inventoryScale && Main.mouseY >= num107 && (float)Main.mouseY <= (float)num107 + (float)Main.inventoryBackTexture.Height * Main.inventoryScale)
				{
					Main.player[Main.myPlayer].mouseInterface = true;
					Main.armorHide = true;
					if (ModPlayer.invSelectItem && ModPlayer.invSelectItemRelease)
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
			
					if(ModPlayer.invSelectItem && ModPlayer.invSelectItemRelease)
						ModPlayer.invSelectItemRelease=false;
						
			return false;
		}


		
#if DEBUG
    }

    
}
#endif