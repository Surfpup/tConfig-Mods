#if DEBUG
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Input;
using Terraria;
using System.IO;
using System.Diagnostics;

namespace Terraria_Control
{

    public class ModPlayer
    {
#endif
        //public Dictionary<Microsoft.Xna.Framework.Input.Buttons, int> itemButtons { get; set; }
        //public IEnumerable<KeyValuePair<Microsoft.Xna.Framework.Input.Buttons, int>> ItemButtons { get; set; }
        public Point lastStickPoint = new Point(0,0);//{ get; set; }
        public Vector2 velocity = new Vector2(0f,0f);
        public Vector2 mousePos = new Vector2(0f,0f);
        public Player tPlayer { get; set; }

        private int aimMode = 1;
		//Point stickPoint = new Point(1, 0);
		
		public static int scrollCooldown = 0;
		public const int scrollCooldownAmt = 50;
		
		public int invCool = 0;
		public const int invCoolAmt = 10;
		
		public bool controlInv = false;
		public bool releaseInventory = false;
		
		public static bool invSelectItem = false;
		public static bool invSelectItemRelease = false;
		
		public static bool rightStick = false;
		public static bool useTile=false;
		
		public const int invMenuMax = 2;
		public static int invMenu = 0; //Which section of the inventory menu is selected
		public const int INVENTORY=0;
		public static int invSelectionX = 0;
		public static int invSelectionY = 0;
		
		//public const int COINS=1;
		//public static int coinSel=0;

		public const int ARMOR=1;
		public static int armorSel=0;

		public const int CHEST=2;
		public static int chestSelX = 0;
		public static int chestSelY = 0;

		public const int CRAFT=3;
		public static int craftSel=0;

		//public static int craftScroll=0;
		public static int craftCooldown=0;
		public static int craftCooldownAmt=10;

		public static bool craftBtn=false;

		public static int stackSplit=0;
		public static bool invSelectMore=false;

		public static bool showCraft=false;
		public static bool enableCraft=false;
		
		
		//0: Inventory
		//1: Armor?
		//2: etc
		
		public static int armorSelection = 0;

        public static Microsoft.Xna.Framework.Input.GamePadState padState
        {
            get
            {
                return Microsoft.Xna.Framework.Input.GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
            }
        }


        public void Initialize()
        {

        }

        public static int HandleScroll(Microsoft.Xna.Framework.Input.Buttons left, Microsoft.Xna.Framework.Input.Buttons right)
        { //Moving between items/menus with right trigger/left trigger
        	int ScrollValue = 0;
        	
			if(scrollCooldown<=0) {
				
				if(padState.IsButtonDown(right))
					ScrollValue++;
				else if (padState.IsButtonDown(left))
					ScrollValue--;
			}
			else scrollCooldown--;

			if(!padState.IsButtonDown(right)
				&& !padState.IsButtonDown(left)) scrollCooldown=0;

			return ScrollValue;
        }

        public void HandleMovement(Player player)
        {
			if (padState.DPad.Up == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				player.controlUp = true;
			}

			if (padState.DPad.Down == Microsoft.Xna.Framework.Input.ButtonState.Pressed
					|| padState.ThumbSticks.Left.Y < 0)
			{
				player.controlDown = true;
			}

			if (padState.DPad.Left == Microsoft.Xna.Framework.Input.ButtonState.Pressed 
				|| (/* padState.Triggers.Right == 0 && */ padState.ThumbSticks.Left.X < 0))
			{
				player.controlLeft = true;
			}

			if (padState.DPad.Right == Microsoft.Xna.Framework.Input.ButtonState.Pressed
				|| (/* padState.Triggers.Right == 0 && */ padState.ThumbSticks.Left.X > 0))
			{
				player.controlRight = true;
			}

			if (padState.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			{
				player.controlJump = true;
			}
        }

        public void PreUpdatePlayer(Player player)
        {
        	if (ModPlayer.stackSplit > 0)
				ModPlayer.stackSplit--;

            tPlayer = player;
			
			controlInv = false;
			//invSelectItem = false;
			invSelectItemRelease = false;
			//craftScroll = 0;

            if (!Main.gameMenu && (Main.netMode != 2))
            {
				if(invCool>0) invCool--;
				
                if (padState.IsConnected)
                {
                	int ScrollValue = HandleScroll(Microsoft.Xna.Framework.Input.Buttons.LeftTrigger, 
                		Microsoft.Xna.Framework.Input.Buttons.RightTrigger);

					//Open Inventory
					if (padState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.Y))
					{
						controlInv = true;
					}

					if (padState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.RightStick))
					{
						rightStick=true;
					}
					else if(rightStick)
					{
						rightStick=false;
						aimMode++;
						if(aimMode>1) aimMode=0; 
					}
					
					if (!Main.playerInventory)
					{
						//Use Tile
						if (padState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.B))
						{
							useTile=true;
						}
						else if(useTile)
						{
							useTile=false;
							player.controlUseTile=true;
						}

						HandleMovement(player);

						//Aiming
						if(aimMode==0)
						{
							Point stickPoint = new Point(1, 0);
	                   
							Vector2 stick = padState.ThumbSticks.Right;
							if (stick.X != 0 || stick.Y != 0)
							{
								stickPoint = updateStick(stick, (float)Player.tileRangeX);
							}
							else
							{
								stickPoint = new Point(1, 0);
								stick = padState.ThumbSticks.Left;
								if (stick.Y == 0)
								{
									stick.X = ((float)1 * (float)tPlayer.direction);
								}
								stickPoint = updateStick(stick, 2F);
							}

							if (Math.Abs(stickPoint.X) + Math.Abs(stickPoint.Y) > 0)
							{
								if (stickPoint.X != lastStickPoint.X || stickPoint.Y != lastStickPoint.Y)
								{
									Microsoft.Xna.Framework.Input.Mouse.SetPosition(stickPoint.X, stickPoint.Y);
									lastStickPoint.X = (int) stickPoint.X;
									lastStickPoint.Y = (int) stickPoint.Y;
								}
							}
						}
						else if(aimMode==1) {
							Vector2 stick = padState.ThumbSticks.Right;
							Point stickPoint = new Point(0, 0);
							//Add to velocity of mouse
							velocity = padState.ThumbSticks.Right;

							mousePos.X += (velocity.X/2);
							mousePos.Y += (velocity.Y/2);

							if(mousePos.X > (float)Player.tileRangeX) mousePos.X = (float)Player.tileRangeX;
							if(mousePos.X < (float)Player.tileRangeX*-1) mousePos.X = (float)Player.tileRangeX*-1;
							if(mousePos.Y > (float)Player.tileRangeY) mousePos.Y = (float)Player.tileRangeY;
							if(mousePos.Y < (float)Player.tileRangeY*-1) mousePos.Y = (float)Player.tileRangeY*-1;

							//Main.NewText(velocity.X+","+velocity.Y);
							
							int X = (int)((float)(Main.screenWidth / 2) + mousePos.X  * 16f); //* (float)Player.tileRangeX
               				int Y = (int)((float)(Main.screenHeight / 2) - mousePos.Y * 16f); //* 2F
							Microsoft.Xna.Framework.Input.Mouse.SetPosition(X, Y);
						}

						//Throw an Item!
						if (padState.Buttons.X == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
						//padState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.X)) //
						{
							player.controlThrow = true;
							//player.releaseThrow = true;
						}
						
						//Use an item!
						if (padState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.RightShoulder))
						{
							player.controlUseItem = true;
						}
						if(ScrollValue!=0) {
							player.selectedItem+=ScrollValue;
							
							if (ScrollValue != 0) {
								Main.PlaySound(12, -1, -1, 1);
								scrollCooldown = scrollCooldownAmt;
							}

							if (player.changeItem >= 0)
							{
								if (player.selectedItem != tPlayer.changeItem)
									Main.PlaySound(12, -1, -1, 1);

								player.selectedItem = player.changeItem;
								player.changeItem = -1;
							}

							while (player.selectedItem > 9)
								player.selectedItem -= 10;

							while (player.selectedItem < 0)
								player.selectedItem += 10;
						}
					}
					else {
						if (padState.Buttons.Back == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
						{
							Main.menuMode = 10;
							WorldGen.SaveAndQuit();
						}

			            if (padState.Buttons.Start == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
						{
							enableCraft=true;
						} else if(enableCraft)
						{
							showCraft=!showCraft;
							enableCraft=false;
						}

						if (padState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.LeftShoulder))
						{
							craftBtn=true;
						} else {
							craftBtn=false;
							//ModPlayer.stackSplit=0;
						}

						if(ScrollValue!=0) {
							invMenu+=ScrollValue;
							scrollCooldown = scrollCooldownAmt;
							if(invMenu==CHEST && Main.player[Main.myPlayer].chest==-1) invMenu+=ScrollValue;

							if(invMenu>invMenuMax) invMenu=0;
							if(invMenu<0) invMenu=invMenuMax;

							UpdateInvMouse();
						}

						int craftScroll=0;
						//Handle craft selection
						if(craftCooldown<=0) {
				
							if(padState.ThumbSticks.Right.Y < 0)
								craftScroll++;
							else if (padState.ThumbSticks.Right.Y > 0)
								craftScroll--;

							craftCooldown=craftCooldownAmt;
						}
						else craftCooldown--;

						Main.focusRecipe+=craftScroll;
						if(Main.focusRecipe<0) Main.focusRecipe=0;
						if(Main.focusRecipe>Main.numAvailableRecipes) Main.focusRecipe=Main.numAvailableRecipes;

						//if(!padState.IsButtonDown(right)
						//	&& !padState.IsButtonDown(left)) scrollCooldown=0;

						
						if (padState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.A))
						{
							invSelectItem = true;
						}
						else if(invSelectItem) {
							invSelectItem=false;
							invSelectItemRelease = true;
						}

						if (padState.IsButtonDown(Microsoft.Xna.Framework.Input.Buttons.RightShoulder))
						{
							invSelectMore = true;
						}
						else {
							invSelectMore=false;
							//ModPlayer.stackSplit=0;
						}

						if(invCool<=0) {							
							Point stickPoint = new Point(0, 0);
								
							Vector2 stick = padState.ThumbSticks.Left;
							if (stick.X != 0 || stick.Y != 0)
							{
								stickPoint.X = (int) stick.X;
								stickPoint.Y = (int) stick.Y;
							}
							if(Math.Abs(stickPoint.X) + Math.Abs(stickPoint.Y) > 0) {
								if(invMenu==INVENTORY) { //Main inventory of items

									//Inventory selection
							  
									int oldSelectionX = invSelectionX;
									int oldSelectionY = invSelectionY;
									if(stickPoint.X > 0) {
										invSelectionX++;
									}
									else if(stickPoint.X < 0) {
										invSelectionX--;
									}
									else if (stickPoint.Y > 0) {
										invSelectionY--;
									}
									else if (stickPoint.Y < 0) {
										invSelectionY++;
									}
									if(invSelectionX>11) invSelectionX = 11;
									if(invSelectionX<0) invSelectionX = 0;
									if(invSelectionY>3) {
										if(Main.player[Main.myPlayer].chest!=-1) {
											invMenu=CHEST;
											chestSelY=0;
											chestSelX=invSelectionX-1;
											if(chestSelX>4) chestSelX=4;

											invCool = invCoolAmt;
											UpdateInvMouse();
										}
										else invSelectionY = 3;
									}
									if(invSelectionY<0) invSelectionY = 0;
									if(oldSelectionX != invSelectionX || oldSelectionY != invSelectionY) {
										invCool = invCoolAmt;
										UpdateInvMouse();
									}
								}
								else if(invMenu==CHEST)
								{
									if(Main.player[Main.myPlayer].chest==-1) {
										invMenu=0;
										UpdateInvMouse();
									} else {
										int oldSelectionX = chestSelX;
										int oldSelectionY = chestSelY;
										if(stickPoint.X > 0) {
											chestSelX++;
										}
										else if(stickPoint.X < 0) {
											chestSelX--;
										}
										else if (stickPoint.Y > 0) {
											chestSelY--;
										}
										else if (stickPoint.Y < 0) {
											chestSelY++;
										}
										if(chestSelX>5) chestSelX = 5;
										if(chestSelX<0) chestSelX = 0;
										if(chestSelY>3) chestSelY = 3;
										if(chestSelY<0) {
											invMenu=ModPlayer.INVENTORY;
											chestSelY = 0;
											invSelectionY = 3;
											invSelectionX = chestSelX+1;
											invCool = invCoolAmt;
										}
										if(oldSelectionX != chestSelX || oldSelectionY != chestSelY) {
											invCool = invCoolAmt;
											UpdateInvMouse();
										}
									}
								}
								/*else if(invMenu==COINS)
								{
									int prev = coinSel;
									if (stickPoint.Y > 0) {
										coinSel--;
									}
									else if (stickPoint.Y < 0) {
										coinSel++;
									}
									if(coinSel!=prev) {
										invCool = invCoolAmt;
									}
								}*/
								else if(invMenu==ARMOR) {			
									//Inventory selection
							  
									int oldSelectionX = armorSel;
									//int oldSelectionY = invSelectionY;
									if(stickPoint.Y > 0) {
										armorSel--;
									}
									else if(stickPoint.Y < 0) {
										armorSel++;
									}
									else if (stickPoint.X > 0 && armorSel>=8) {
										armorSel-=8;
									}
									else if (stickPoint.X < 0 && armorSel<8) {
										armorSel+=8;
									}
									if(armorSel>10) armorSel = 10;
									if(armorSel<0) armorSel = 0;
									//if(invSelectionY>3) invSelectionY = 3;
									//if(invSelectionY<0) invSelectionY = 0;
									if(oldSelectionX != armorSel) { // || oldSelectionY != invSelectionY) {
										invCool = invCoolAmt;
										UpdateInvMouse();
									}
								}
							}
						}
					}
					
					if (controlInv)
					{
						if (releaseInventory) {
							player.toggleInv();
							if(Main.playerInventory) UpdateInvMouse();
						}

						releaseInventory = false;
					}
					else releaseInventory = true;

					if (player.delayUseItem)
					{
						if (!player.controlUseItem)
							player.delayUseItem = false;

						player.controlUseItem = false;
					}

					if (player.itemAnimation == 0 && player.itemTime == 0)
					{
						player.dropItemCheck();
					}
                }
            }

            //UpdateButtons();
        }
		
		private void UpdateInvMouse() {
			int x = 0;
			int y = 0;
			if(invMenu==INVENTORY) {
				x = (int)(28f + (float)(invSelectionX * 80) * Main.inventoryScale);
				y = (int)(28f + (float)(invSelectionY * 80) * Main.inventoryScale);
				if(invSelectionX>=10) {
					y=(int)(85f + (float)(invSelectionY * 80) * Main.inventoryScale);
				}
				
			}
			else if(invMenu==ARMOR) {
				if(armorSel<8) {
					x = Main.screenWidth - 64 - 28;
					y = (int)(174f + (float)(armorSel * 80) * Main.inventoryScale);
				}
				else {
					x = Main.screenWidth - 64 - 28 - 47;
					y = (int)(174f + (float)((armorSel - 8) * 80) * Main.inventoryScale);
				}
			}
			else if (invMenu==CHEST)
			{
				x = (int)(73f + (float)(chestSelX * 56) * Main.inventoryScale);
                y = (int)(210f + (float)(chestSelY * 56) * Main.inventoryScale);
			}
			Microsoft.Xna.Framework.Input.Mouse.SetPosition(x, y);
		}

        private Point updateStick(Vector2 stick, float range)
        {
            return new Point
            {                
                X = (int)((float)(Main.screenWidth / 2) + stick.X * range * 16f),
                Y = (int)((float)(Main.screenHeight / 2) - stick.Y * range * 16f)
            };
        }


        public void UpdateButtons()
        {
			if(!Main.playerInventory) {
                /*if (ItemButtons == null)
                {
                    ItemButtons = new Dictionary<Microsoft.Xna.Framework.Input.Buttons, int>()
                {
                    {Microsoft.Xna.Framework.Input.Buttons.X, 0},
                    {Microsoft.Xna.Framework.Input.Buttons.Y, 1},
                    {Microsoft.Xna.Framework.Input.Buttons.B, 2},
                    {Microsoft.Xna.Framework.Input.Buttons.LeftShoulder, 3},
                    {Microsoft.Xna.Framework.Input.Buttons.RightShoulder, 4},
                    {Microsoft.Xna.Framework.Input.Buttons.LeftTrigger, 5},
                    {Microsoft.Xna.Framework.Input.Buttons.RightTrigger, 6},
                };
                }

                foreach (KeyValuePair<Microsoft.Xna.Framework.Input.Buttons, int> playerButtons in ItemButtons)
                {
                    // if the right trigger and another button is held
                    if (padState.IsButtonDown(playerButtons.Key))
                    {
                        tPlayer.selectedItem = playerButtons.Value;
                        if (playerButtons.Key == Microsoft.Xna.Framework.Input.Buttons.X)
                        {
                            tPlayer.controlUseTile = true;
                        }

                        tPlayer.controlUseItem = true;
                    }
                }*/
            }
            else if (Main.playerInventory)
            {

            }
        }
		
		


#if DEBUG
    }

}
#endif
