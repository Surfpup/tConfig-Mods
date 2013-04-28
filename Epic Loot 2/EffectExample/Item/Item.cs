public void Initialize()
{
	Effects.Items.HealthCost effect = new Effects.Items.HealthCost(this.item, 20);
	effect.Initialize(); //Add health cost of 20 HP

	//Add 20 defense
	Effects.Items.Defense effect2 = new Effects.Items.Defense(this.item,  20);
	effect2.Initialize();
}