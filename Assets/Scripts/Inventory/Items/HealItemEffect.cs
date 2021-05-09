using UnityEngine;

[CreateAssetMenu(menuName = "Foxlair/Inventory/Item Effects/Heal")]
public class HealItemEffect : UsableItemEffect
{
	public int HealAmount;

	public override void ExecuteEffect(UsableItem usableItem, Character character)
	{
		character.Health += HealAmount;
	}

	public override string GetDescription()
	{
		return "Heals for " + HealAmount + " health.";
	}
}
