SpriteBatch spriteBatch;
public bool ItemMouseText(SpriteBatch s,string cursorText,int rare,byte diff,string[] array,bool[] array2, bool[]array3)
{
	this.spriteBatch = s;
	int num4=array.Length;

	int num = (int) (Main.screenWidth - 200); //Main.mouseX + 10;
	int num2 = (int) (200); //Main.mouseY + 10;
	int num22 = 0;
	float num20 = (float)Main.mouseTextColor / 255f;
	Color color = new Color((int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor, (int)Main.mouseTextColor);
	Vector2 origin;
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