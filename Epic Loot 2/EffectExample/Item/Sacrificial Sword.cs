Effects.Items.HealthCost effect;
public void Initialize()
{
	effect = new Effects.Items.HealthCost(this.item);
	effect.Load(20); //Add health cost of 20 HP

	Delegate addDel = (Func<Player, int, bool>) effect.CanUse;
	Delegate curDel = null;
    item.delegates.TryGetValue("CanUse", out curDel);
    if (curDel != null)
    {
        Delegate newDel = Delegate.Combine(curDel, addDel);
        item.delegates["CanUse"] = newDel;
    }
    else item.delegates["CanUse"] = addDel;

}

public bool CanUse(Player p, int i)
{
	return effect.CanUse(p, i);
}