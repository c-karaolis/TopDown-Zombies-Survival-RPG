using Foxlair.Character;
using UnityEngine;

public abstract class UsableItemEffect : ScriptableObject
{
	public abstract void ExecuteEffect(UsableItem parentItem, PlayerCharacter character);

	public abstract string GetDescription();
}