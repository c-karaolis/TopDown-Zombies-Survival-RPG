using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using Foxlair.CharacterStats;

public class StatDisplay : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
	private CharacterStat _stat;
	public CharacterStat Stat {
		get { return _stat; }
		set {
			_stat = value;
			UpdateStatValue();
		}
	}

	private string _name;
	public string Name {
		get { return _name; }
		set {
			_name = value;
			nameText.text = _name.ToLower();
		}
	}

	[SerializeField] Text nameText;
	[SerializeField] Text valueText;
	[SerializeField] StatTooltip tooltip;

	private bool showingTooltip;

	private void OnValidate()
	{
		Text[] texts = GetComponentsInChildren<Text>();
		nameText = texts[0];
		valueText = texts[1];

		if (tooltip == null)
			tooltip = FindObjectOfType<StatTooltip>();
	}

	public void OnPointerEnter(PointerEventData eventData)
	{
		tooltip.ShowTooltip(Stat, Name);
		showingTooltip = true;
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		tooltip.HideTooltip();
		showingTooltip = false;
	}

	public void UpdateStatValue()
	{
		valueText.text = _stat.Value.ToString();
		if (showingTooltip) {
			tooltip.ShowTooltip(Stat, Name);
		}
	}
}
