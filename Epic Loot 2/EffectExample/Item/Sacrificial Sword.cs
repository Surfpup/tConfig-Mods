Effects.Items.HealthCost effect;
public void Initialize()
{
	effect = new Effects.Items.HealthCost(this.item);
	effect.Load(20); //Add health cost of 20 HP
}