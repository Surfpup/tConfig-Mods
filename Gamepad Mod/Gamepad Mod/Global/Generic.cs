SpriteBatch spriteBatch;
public bool ItemMouseText(SpriteBatch s,string cursorText,int rare,byte diff,string[] array,bool[] array2, bool[]array3)
{
	this.spriteBatch = s;
	int num4=array.Length;
	Vector2 origin;

	int num = Main.mouseX + 10; //(int) (Main.screenWidth - 250); //Main.mouseX + 10;
	int num2 = Main.mouseY + 10; //(int) (25); //Main.mouseY + 10;

	//Draw background texture
	/*float scale=6f;
	Color color20 = new Color((int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha), (int)((byte)Main.invAlpha));
	SpriteBatch arg_5487_0 = this.spriteBatch;
	
	Texture2D back = Main.inventoryBackTexture;
	
	Texture2D arg_5487_1 = back;
	Vector2 arg_5487_2 = new Vector2((float)num-25, (float)num2-25);
	Rectangle? arg_5487_3 = new Rectangle?(new Rectangle(0, 0, Main.inventoryBackTexture.Width, Main.inventoryBackTexture.Height));
	Color arg_5487_4 = color20;
	float arg_5487_5 = 0f;
	origin = default(Vector2);
	arg_5487_0.Draw(arg_5487_1, arg_5487_2, arg_5487_3, arg_5487_4, arg_5487_5, origin, scale, SpriteEffects.None, 0f);
	*/
	int num22 = 0;
	float num20 = (float)Main.mouseTextColor / 255f;
	Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
	
	for (int k = 0; k < num4; k++)
	{
		if(array[k]==null) continue;
		for (int l = 0; l < 5; l++)
		{
			int num23 = num;
			int num24 = num2 + num22;
			Color color2 = Color.Black;
			if (l == 0)
			{
				num23 -= 2;
			}
			else
			{
				if (l == 1)
				{
					num23 += 2;
				}
				else
				{
					if (l == 2)
					{
						num24 -= 2;
					}
					else
					{
						if (l == 3)
						{
							num24 += 2;
						}
						else
						{
							color2 = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
							if (k == 0)
							{
								if (rare >= -1 && rare <= 6)
								{
									color2 = new Color((int)((byte)(Main.colorRarity[rare][0] * num20)), (int)((byte)(Main.colorRarity[rare][1] * num20)), (int)((byte)(Main.colorRarity[rare][2] * num20)), (int)Main.mouseTextColor);
								}
								if (diff == 1)
								{
									color2 = new Color((int)((byte)((float)Main.mcColor.R * num20)), (int)((byte)((float)Main.mcColor.G * num20)), (int)((byte)((float)Main.mcColor.B * num20)), (int)Main.mouseTextColor);
								}
								if (diff == 2)
								{
									color2 = new Color((int)((byte)((float)Main.hcColor.R * num20)), (int)((byte)((float)Main.hcColor.G * num20)), (int)((byte)((float)Main.hcColor.B * num20)), (int)Main.mouseTextColor);
								}
							}
							else
							{
								if (array2[k])
								{
									if (array3[k])
									{
										color2 = new Color((int)((byte)(190f * num20)), (int)((byte)(120f * num20)), (int)((byte)(120f * num20)), (int)Main.mouseTextColor);
									}
									else
									{
										color2 = new Color((int)((byte)(120f * num20)), (int)((byte)(190f * num20)), (int)((byte)(120f * num20)), (int)Main.mouseTextColor);
									}
								}
								else
								{
									if (k == num4 - 1)
									{
										color2 = color;
									}
								}
							}
						}
					}
				}
			}
			SpriteBatch arg_19D3_0 = this.spriteBatch;
			SpriteFont arg_19D3_1 = Main.fontMouseText;
			string arg_19D3_2 = array[k];
			Vector2 arg_19D3_3 = new Vector2((float)num23, (float)num24);
			Color arg_19D3_4 = color2;
			float arg_19D3_5 = 0f;
			origin = default(Vector2);
			arg_19D3_0.DrawString(arg_19D3_1, arg_19D3_2, arg_19D3_3, arg_19D3_4, arg_19D3_5, origin, 1f, SpriteEffects.None, 0f);
		}
		num22 += (int)(Main.fontMouseText.MeasureString(array[k]).Y); // + (float)num21
	}
	return false;
}

public Microsoft.Xna.Framework.Input.GamePadState padState
{
    get
    {
        return Microsoft.Xna.Framework.Input.GamePad.GetState(Microsoft.Xna.Framework.PlayerIndex.One);
    }
}

int selectedMenu = 0;
bool upPressed=false;
bool controlUp=false;
bool downPressed=false;
bool controlDown=false;
bool enterPressed=false;
bool backPressed=false;
//int prevMenu=0;

int scrollCooldown=0;
public const int scrollCooldownAmt = 10;

public bool OverrideMenuSelection(int prevFocus, int numItems, Main main)
{ //Handle main menu stuff
    //if(gameMenu) {
    	/*Microsoft.Xna.Framework.Input.Keys[] pressedKeys = Main.keyState.GetPressedKeys();

		for (int i = 0; i < pressedKeys.Length; i++)
		{
			string a = pressedKeys[i].ToString();

			if (a == Main.cUp)
				controlUp = true;
			else if (a == Main.cDown)
				controlDown = true;
		}*/

		int ScrollValue = 0;
		Vector2 stick=padState.ThumbSticks.Left;
		if(scrollCooldown<=0) {
			
			if(padState.DPad.Down == Microsoft.Xna.Framework.Input.ButtonState.Pressed
				|| stick.Y < 0)
				ScrollValue++;
			else if (padState.DPad.Up == Microsoft.Xna.Framework.Input.ButtonState.Pressed
				|| stick.Y > 0)
				ScrollValue--;
		}
		else scrollCooldown--;

		if(!(padState.DPad.Down == Microsoft.Xna.Framework.Input.ButtonState.Pressed)
			&& !(padState.DPad.Up == Microsoft.Xna.Framework.Input.ButtonState.Pressed) && stick.Y==0) scrollCooldown=0;

		if(ScrollValue!=0)
			scrollCooldown = scrollCooldownAmt;

		selectedMenu+=ScrollValue;
        /*if () {
            upPressed=true;
        } else if(upPressed) {
            selectedMenu--;
            upPressed=false;
        }

        if () {
            downPressed=true; 
        } else if(downPressed) {
            downPressed=false;
            selectedMenu++;
        }*/

        if (padState.Buttons.A == Microsoft.Xna.Framework.Input.ButtonState.Pressed) {
            enterPressed=true;
        } else if(enterPressed) {
            enterPressed=false;
            //prevMenu=Main.menuMode;
            main.selectedMenu=selectedMenu;
        }

        if (padState.Buttons.B == Microsoft.Xna.Framework.Input.ButtonState.Pressed) {
            backPressed=true;
        } else if(backPressed) {
            backPressed=false;
            Main.menuMode=0;
            selectedMenu=0;
        }

        if(selectedMenu<0) selectedMenu=numItems-1;
       	if(selectedMenu>numItems-1) selectedMenu=0;

        main.focusMenu=selectedMenu;
    //}
    return false;
}