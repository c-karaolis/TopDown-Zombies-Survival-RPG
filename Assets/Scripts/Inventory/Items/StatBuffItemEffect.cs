using System.Collections;
using UnityEngine;
using Foxlair.CharacterStats;
using Foxlair.Character;

[CreateAssetMenu(menuName = "Foxlair/Inventory/Item Effects/Stat Buff")]
public class StatBuffItemEffect : UsableItemEffect
{
	public int AgilityBuff;
	public float Duration;

	public override void ExecuteEffect(UsableItem parentItem, PlayerCharacter character)
	{
		StatModifier statModifier = new StatModifier(AgilityBuff, StatModType.Flat, parentItem);
		character.Agility.AddModifier(statModifier);
		character.InventoryController.UpdateStatValuesUI();
		character.StartCoroutine(RemoveBuff(character, statModifier, Duration));
	}

	public override string GetDescription()
	{
		return "Grants " + AgilityBuff + " Agility for " + Duration + " seconds.";
	}

	private static IEnumerator RemoveBuff(PlayerCharacter character, StatModifier statModifier, float duration)
	{
		yield return new WaitForSeconds(duration);
		character.Agility.RemoveModifier(statModifier);
		character.InventoryController.UpdateStatValuesUI();
	}
}
