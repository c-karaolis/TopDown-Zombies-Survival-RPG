using UnityEngine;
using Foxlair.CharacterStats;
using Foxlair.Tools.Events;
using System;

public class StatPanel : MonoBehaviour
{
	[SerializeField] StatDisplay[] statDisplays;
	[SerializeField] string[] statNames;

	private CharacterStat[] stats;

	private void OnValidate()
	{
		statDisplays = GetComponentsInChildren<StatDisplay>();
		UpdateStatNames();
	}

    private void Start()
    {
		SubscribeToEvents();
    }

    private void OnDestroy()
    {
		UnsubscribeFromEvents();
	}

    private void UnsubscribeFromEvents()
    {
		FoxlairEventManager.Instance.StatPanel_OnValuesUpdated_Event -= UpdateStatValuesUI;
	}

	private void SubscribeToEvents()
    {
		FoxlairEventManager.Instance.StatPanel_OnValuesUpdated_Event += UpdateStatValuesUI;
	}

	public void SetStats(params CharacterStat[] charStats)
	{
		stats = charStats;

		if (stats.Length > statDisplays.Length)
		{
			Debug.LogError("Not Enough Stat Displays!");
			return;
		}

		for (int i = 0; i < statDisplays.Length; i++)
		{
			statDisplays[i].gameObject.SetActive(i < stats.Length);

			if (i < stats.Length)
			{
				statDisplays[i].Stat = stats[i];
			}
		}
	}

	public void UpdateStatValuesUI()
	{
		for (int i = 0; i < stats.Length; i++)
		{
			statDisplays[i].UpdateStatValue();
		}
	}

	public void UpdateStatNames()
	{
		for (int i = 0; i < statNames.Length; i++)
		{
			statDisplays[i].Name = statNames[i];
		}
	}
}
