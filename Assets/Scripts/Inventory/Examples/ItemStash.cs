using UnityEngine;

public class ItemStash : ItemContainer
{
	[SerializeField] Transform itemsParent;
	[SerializeField] SpriteRenderer spriteRenderer;
	[SerializeField] KeyCode openKeyCode = KeyCode.E;

	private bool isOpen;
	private bool isInRange;

	private InventoryController inventoryController;

	protected override void OnValidate()
	{
		if (itemsParent != null)
			itemsParent.GetComponentsInChildren(includeInactive: true, result: ItemSlots);

		if (spriteRenderer == null)
			spriteRenderer = GetComponentInChildren<SpriteRenderer>(includeInactive: true);

		spriteRenderer.enabled = false;
	}

	protected override void Awake()
	{
		base.Awake();
		itemsParent.gameObject.SetActive(false);
	}

	private void Update()
	{
		if (isInRange && Input.GetKeyDown(openKeyCode))
		{
			isOpen = !isOpen;
			itemsParent.gameObject.SetActive(isOpen);

			if (isOpen)
				inventoryController.OpenItemContainer(this);
			else
				inventoryController.CloseItemContainer(this);
		}
	}

	private void OnTriggerEnter(Collider other)
	{
		CheckCollision(other.gameObject, true);
	}

	private void OnTriggerExit(Collider other)
	{
		CheckCollision(other.gameObject, false);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		CheckCollision(collision.gameObject, true);
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		CheckCollision(collision.gameObject, false);
	}

	private void CheckCollision(GameObject gameObject, bool state)
	{
		if (gameObject.CompareTag("Player"))
		{
			isInRange = state;
			spriteRenderer.enabled = state;

			if (!isInRange && isOpen)
			{
				isOpen = false;
				itemsParent.gameObject.SetActive(false);
				inventoryController.CloseItemContainer(this);
			}

			if (isInRange)
				inventoryController = gameObject.GetComponent<InventoryController>();
			else
				inventoryController = null;
		}
	}
}
