using UnityEngine;
using UnityEngine.UI;
using Foxlair.CharacterStats;
using Foxlair.Tools.Events;
using Foxlair.Character;
using System.Linq;
using Foxlair.Character.LevelingSystem;
using Foxlair.Character.Health;

public class InventoryController : MonoBehaviour
{

	[Header("Public")]
	public Inventory Inventory;
	public EquipmentPanel EquipmentPanel;
	public PlayerCharacter PlayerCharacter;

	[Header("Serialize Field")]
	[SerializeField] CraftingWindow craftingWindow;
	[SerializeField] StatPanel statPanel;
	[SerializeField] ItemTooltip itemTooltip;
	[SerializeField] Image draggableItem;
	[SerializeField] DropItemArea dropItemArea;
	[SerializeField] QuestionDialog reallyDropItemDialog;
	[SerializeField] ItemSaveManager itemSaveManager;

	private BaseItemSlot dragItemSlot;

	[Header("Testing Fields")]
	public Item TestItem;
	public LevelingSystem levelingSystem;
	public int xpToAdd;
	public HealthSystem PlayerHealthSystem;
	private void OnValidate()
	{
		if (itemTooltip == null)
			itemTooltip = FindObjectOfType<ItemTooltip>();
	}
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.J))
        {
			Inventory.AddItem(Instantiate(TestItem));

        }
		if (Input.GetKeyDown(KeyCode.L))
		{
			levelingSystem.AddExperience(xpToAdd);
		}
		if (Input.GetKeyDown(KeyCode.B))
		{
			PlayerHealthSystem.TakeDamage(10);
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			PlayerHealthSystem.Heal(10);
		}
	}
    private void Awake()
	{
		FindPlayerCharacter();
		PlayerCharacter.Inventory = Inventory;
		//statPanel.SetStats(Strength, Agility, Intelligence, Vitality);
		statPanel.SetStats(PlayerCharacter.CharacterAttributes.Values.ToArray());
	}

	private void Start()
    {
        SubscribeToEvents();
		FoxlairEventManager.Instance.StatPanel_OnValuesUpdated_Event?.Invoke();

		if (itemSaveManager != null)
        {
            itemSaveManager.LoadEquipment(this);
            itemSaveManager.LoadInventory(this);
        }
    }

    
    private void OnDestroy()
	{
		if (itemSaveManager != null)
		{
			itemSaveManager.SaveEquipment(this);
			itemSaveManager.SaveInventory(this);
		}
		UnsubscribeFromEvents();

	}
	public void Save()
    {
		if (itemSaveManager != null)
		{
			itemSaveManager.SaveEquipment(this);
			itemSaveManager.SaveInventory(this);
		}

	}
	public void Load()
	{
		if (itemSaveManager != null)
		{
			itemSaveManager.LoadEquipment(this);
			itemSaveManager.LoadInventory(this);
		}
	}

	private void InventoryRightClick(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item is EquippableItem)
		{
			Equip((EquippableItem)itemSlot.Item);
		}
		else if (itemSlot.Item is UsableItem)
		{
			UsableItem usableItem = (UsableItem)itemSlot.Item;
			usableItem.Use(PlayerCharacter);

			if (usableItem.IsConsumable)
			{
				itemSlot.Amount--;
				usableItem.Destroy();
			}
		}
	}

	private void EquipmentPanelRightClick(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item is EquippableItem)
		{
			Unequip((EquippableItem)itemSlot.Item);
		}
	}


	private void BeginDrag(BaseItemSlot itemSlot)
	{
		if (itemSlot.Item != null)
		{
			dragItemSlot = itemSlot;
			draggableItem.sprite = itemSlot.Item.Icon;
			draggableItem.transform.position = Input.mousePosition;
			draggableItem.gameObject.SetActive(true);
		}
	}

	private void Drag(BaseItemSlot itemSlot)
	{
		draggableItem.transform.position = Input.mousePosition;
	}

	private void EndDrag(BaseItemSlot itemSlot)
	{
		dragItemSlot = null;
		draggableItem.gameObject.SetActive(false);
	}

	private void Drop(BaseItemSlot dropItemSlot)
	{
		if (dragItemSlot == null) return;

		if (dropItemSlot.CanAddStack(dragItemSlot.Item))
		{
			AddStacks(dropItemSlot);
		}
		else if (dropItemSlot.CanReceiveItem(dragItemSlot.Item) && dragItemSlot.CanReceiveItem(dropItemSlot.Item))
		{
			SwapItems(dropItemSlot);
		}
	}

	private void AddStacks(BaseItemSlot dropItemSlot)
	{
		int numAddableStacks = dropItemSlot.Item.MaximumStacks - dropItemSlot.Amount;
		int stacksToAdd = Mathf.Min(numAddableStacks, dragItemSlot.Amount);

		dropItemSlot.Amount += stacksToAdd;
		dragItemSlot.Amount -= stacksToAdd;
	}

	private void SwapItems(BaseItemSlot dropItemSlot)
	{
		EquippableItem dragEquipItem = dragItemSlot.Item as EquippableItem;
		EquippableItem dropEquipItem = dropItemSlot.Item as EquippableItem;

		if (dropItemSlot is EquipmentSlot)
		{
			if (dragEquipItem != null) dragEquipItem.Equip(PlayerCharacter);
			if (dropEquipItem != null) dropEquipItem.Unequip(PlayerCharacter);
		}
		if (dragItemSlot is EquipmentSlot)
		{
			if (dragEquipItem != null) dragEquipItem.Unequip(PlayerCharacter);
			if (dropEquipItem != null) dropEquipItem.Equip(PlayerCharacter);
		}
		FoxlairEventManager.Instance.StatPanel_OnValuesUpdated_Event?.Invoke();

		Item draggedItem = dragItemSlot.Item;
		int draggedItemAmount = dragItemSlot.Amount;

		dragItemSlot.Item = dropItemSlot.Item;
		dragItemSlot.Amount = dropItemSlot.Amount;

		dropItemSlot.Item = draggedItem;
		dropItemSlot.Amount = draggedItemAmount;
	}

	private void DropItemOutsideUI()
	{
		if (dragItemSlot == null) return;

		reallyDropItemDialog.Show();
		BaseItemSlot slot = dragItemSlot;
		reallyDropItemDialog.OnYesEvent += () => DestroyItemInSlot(slot);
	}

	public void DestroyItemInSlot(BaseItemSlot itemSlot)
	{
		if (itemSlot == null || itemSlot.Item == null) return;
		// If the item is equiped, unequip first
		if (itemSlot is EquipmentSlot)
		{
			EquippableItem equippableItem = (EquippableItem)itemSlot.Item;
			if(equippableItem != null)
			{
				if((itemSlot as EquipmentSlot).EquipmentType == EquipmentType.Weapon)
                {
					//TODO: Send weapon unequipped event
                }
				equippableItem.Unequip(PlayerCharacter);
			}
		}

		itemSlot.Item.Destroy();
		itemSlot.Item = null;
	}

	public BaseItemSlot GetEquipmentSlotByType(EquipmentType equipmentType)
    {
		for (int i = 0; i < EquipmentPanel.EquipmentSlots.Length; i++)
        {
			if (EquipmentPanel.EquipmentSlots[i].EquipmentType == equipmentType)
            {
				return EquipmentPanel.EquipmentSlots[i];
			}
        }
		return null;
    }

	public void Equip(EquippableItem item)
	{
		if (Inventory.RemoveItem(item))
		{
			EquippableItem previousItem;
			if (EquipmentPanel.AddItem(item, out previousItem))
			{
				if (previousItem != null)
				{
					Inventory.AddItem(previousItem);
					previousItem.Unequip(PlayerCharacter);
					FoxlairEventManager.Instance.StatPanel_OnValuesUpdated_Event?.Invoke();
				}
				item.Equip(PlayerCharacter);
				FoxlairEventManager.Instance.StatPanel_OnValuesUpdated_Event?.Invoke();
			}
			else
			{
				Inventory.AddItem(item);
			}
		}
	}

	public void Unequip(EquippableItem item)
	{
		if (Inventory.CanAddItem(item) && EquipmentPanel.RemoveItem(item))
		{
			item.Unequip(PlayerCharacter);
			FoxlairEventManager.Instance.StatPanel_OnValuesUpdated_Event?.Invoke();
			Inventory.AddItem(item);
		}
	}

	private ItemContainer openItemContainer;

	private void TransferToItemContainer(BaseItemSlot itemSlot)
	{
		Item item = itemSlot.Item;
		if (item != null && openItemContainer.CanAddItem(item))
		{
			Inventory.RemoveItem(item);
			openItemContainer.AddItem(item);
		}
	}

	private void TransferToInventory(BaseItemSlot itemSlot)
	{
		Item item = itemSlot.Item;
		if (item != null && Inventory.CanAddItem(item))
		{
			openItemContainer.RemoveItem(item);
			Inventory.AddItem(item);
		}
	}

	public void OpenItemContainer(ItemContainer itemContainer)
	{
		openItemContainer = itemContainer;

		Inventory.OnRightClickEvent -= InventoryRightClick;
		Inventory.OnRightClickEvent += TransferToItemContainer;

		itemContainer.OnRightClickEvent += TransferToInventory;

		//itemContainer.OnPointerEnterEvent += ShowTooltip;
		//itemContainer.OnPointerExitEvent += HideTooltip;
		//itemContainer.OnBeginDragEvent += BeginDrag;
		//itemContainer.OnEndDragEvent += EndDrag;
		//itemContainer.OnDragEvent += Drag;
		//itemContainer.OnDropEvent += Drop;
	}

	public void CloseItemContainer(ItemContainer itemContainer)
	{
		openItemContainer = null;

		Inventory.OnRightClickEvent += InventoryRightClick;
		Inventory.OnRightClickEvent -= TransferToItemContainer;

		itemContainer.OnRightClickEvent -= TransferToInventory;

		//itemContainer.OnPointerEnterEvent -= ShowTooltip;
		//itemContainer.OnPointerExitEvent -= HideTooltip;
		//itemContainer.OnBeginDragEvent -= BeginDrag;
		//itemContainer.OnEndDragEvent -= EndDrag;
		//itemContainer.OnDragEvent -= Drag;
		//itemContainer.OnDropEvent -= Drop;
	}

	public void UpdateStatValuesUI()
	{
		FoxlairEventManager.Instance.StatPanel_OnValuesUpdated_Event?.Invoke();
	}

	private void FindPlayerCharacter()
    {
        if (PlayerCharacter != null) return;

		PlayerCharacter = GameObject.FindWithTag("Player").GetComponent<PlayerCharacter>();
    }

    #region Event Subscribe/Unsubscribe
    private void SubscribeToEvents()
	{
		Inventory.OnRightClickEvent += InventoryRightClick;
		EquipmentPanel.OnRightClickEvent += EquipmentPanelRightClick;
	
		// Begin Drag
		FoxlairEventManager.Instance.Inventory_OnBeginDrag_Event += BeginDrag;
		// End Drag
		FoxlairEventManager.Instance.Inventory_OnEndDrag_Event += EndDrag;
		// Drag
		FoxlairEventManager.Instance.Inventory_OnDrag_Event += Drag;
		// Drop
		FoxlairEventManager.Instance.Inventory_OnDrop_Event += Drop;

		FoxlairEventManager.Instance.DropItemArea_OnDrop_Event += DropItemOutsideUI;
	}

	private void UnsubscribeFromEvents()
	{
		Inventory.OnRightClickEvent -= InventoryRightClick;
		EquipmentPanel.OnRightClickEvent -= EquipmentPanelRightClick;

		// Begin Drag
		FoxlairEventManager.Instance.Inventory_OnBeginDrag_Event -= BeginDrag;
		// End Drag
		FoxlairEventManager.Instance.Inventory_OnEndDrag_Event -= EndDrag;
		// Drag
		FoxlairEventManager.Instance.Inventory_OnDrag_Event -= Drag;
		// Drop
		FoxlairEventManager.Instance.Inventory_OnDrop_Event -= Drop;

		FoxlairEventManager.Instance.DropItemArea_OnDrop_Event -= DropItemOutsideUI;
	}
    #endregion
}
