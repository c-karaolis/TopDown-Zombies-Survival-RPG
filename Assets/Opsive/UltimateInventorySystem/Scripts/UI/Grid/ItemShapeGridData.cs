/// ---------------------------------------------
/// Ultimate Inventory System
/// Copyright (c) Opsive. All Rights Reserved.
/// https://www.opsive.com
/// ---------------------------------------------

namespace Opsive.UltimateInventorySystem.UI.Grid
{
    using Opsive.Shared.Utility;
    using Opsive.UltimateInventorySystem.Core.DataStructures;
    using Opsive.UltimateInventorySystem.Core.InventoryCollections;
    using Opsive.UltimateInventorySystem.UI.Item;
    using System;
    using UnityEngine;

    /// <summary>
    /// Item Shape Grid Data.
    /// </summary>
    public class ItemShapeGridData : MonoBehaviour
    {
        /// <summary>
        /// The struct for data of the the grid element.
        /// </summary>
        public struct GridElementData
        {
            public static GridElementData None => new GridElementData();

            public ItemStack ItemStack { get; private set; }
            public bool IsAnchor { get; private set; }
            public bool IsEmpty => ItemStack == null;
            public bool IsOccupied => ItemStack != null;

            /// <summary>
            /// Constructor.
            /// </summary>
            /// <param name="itemStack">The item stack.</param>
            /// <param name="isAnchor">Is the element an anchor.</param>
            public GridElementData(ItemStack itemStack, bool isAnchor)
            {
                //Debug.Log(itemStack);
                ItemStack = itemStack;
                IsAnchor = isAnchor;

                //Debug.Log(IsOccupied);
            }

            /// <summary>
            /// To string.
            /// </summary>
            /// <returns>The string.</returns>
            public override string ToString()
            {
                return $"Grid ElementData : [Is Anchor '{IsAnchor}' -> {ItemStack}]";
            }
        }

        [Tooltip("The ID of the Grid Data.")]
        [SerializeField] internal int m_ID;
        [Tooltip("The Item Collection of the Grid, this is the collection where the items will be added " +
                 "(NONE, means items from all the collections in the inventory can be placed in the grid data.).")]
        [SerializeField] internal ItemCollectionID m_ItemCollectionID;
        [Tooltip("The Item Info Filter used to prevent certain items from being added in the grid.")]
        [SerializeField] internal ItemInfoFilterSorterBase m_ItemInfoFilter;
        [Tooltip("The Grid Size (must match the UI Grid size.)")]
        [SerializeField] internal Vector2Int m_GridSize;
        [Tooltip("Allow items to exchange places when possible or allow items to be moved in empty places only?")]
        [SerializeField] protected bool m_SmartTwoWayMove = false;

        public int GridSizeCount => m_GridSize.x * m_GridSize.y;
        public int GridColumns => m_GridSize.x;
        public int GridRows => m_GridSize.y;
        public string ShapeAttributeName => m_Controller.ShapeAttributeName;

        protected ItemShapeGridController m_Controller;

        protected GridElementData[,] m_ItemStackAnchorGrid;
        protected GridElementData[,] m_TemporaryItemStackAnchorGrid;

        public ItemShapeGridController Controller => m_Controller;
        public Inventory Inventory => m_Controller.Inventory;
        public int ID => m_ID;
        public ItemCollectionID ItemCollectionID {
            get { return m_ItemCollectionID; }
            set { m_ItemCollectionID = value; }
        }

        public Vector2Int GridSize => m_GridSize;
        public IFilterSorter<ItemInfo> FilterSorter => m_ItemInfoFilter;

        /// <summary>
        /// Convert an index to a position in the grid.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <returns>The position.</returns>
        public Vector2Int OneDTo2D(int index)
        {
            if (index < 0 || index >= GridSizeCount) { return new Vector2Int(-1, -1); }

            return new Vector2Int(index % m_GridSize.x, index / m_GridSize.x);
        }

        /// <summary>
        /// Convert a position to an index.
        /// </summary>
        /// <param name="pos">The position.</param>
        /// <returns>The index.</returns>
        public int TwoDTo1D(Vector2Int pos)
        {
            if (pos.x < 0
                || pos.x >= m_GridSize.x
                || pos.y < 0
                || pos.y >= m_GridSize.y) { return -1; }

            return pos.y * m_GridSize.x + pos.x;
        }

        /// <summary>
        /// Initialize
        /// </summary>
        /// <param name="controller">The item shape inventory grid controller.</param>
        public void Initialize(ItemShapeGridController controller)
        {
            if (m_Controller == controller) {
                return;
            }

            m_Controller = controller;

            m_ItemStackAnchorGrid = new GridElementData[GridColumns, GridRows];
            m_TemporaryItemStackAnchorGrid = new GridElementData[GridColumns, GridRows];
        }

        /// <summary>
        /// Set grid data.
        /// </summary>
        /// <param name="gridElementDatas">The grid element datas.</param>
        public void SetNewGridData(ListSlice<GridElementData> gridElementDatas)
        {
            if (gridElementDatas.Count != GridSizeCount) {
                Debug.LogWarning("The Grid data size limit does not match. The data cannot be set.");
                return;
            }

            for (int i = 0; i < gridElementDatas.Count; i++) {
                var position = OneDTo2D(i);

                m_ItemStackAnchorGrid[position.x, position.y] = gridElementDatas[i];
            }
        }

        /// <summary>
        /// Get the element at the position.
        /// </summary>
        /// <param name="pos">The position of the element.</param>
        /// <returns>The grid element data.</returns>
        public GridElementData GetElementAt(Vector2Int pos)
        {
            return m_ItemStackAnchorGrid[pos.x, pos.y];
        }

        /// <summary>
        /// Get the element at the position.
        /// </summary>
        /// <param name="x">column position.</param>
        /// <param name="y">row position.</param>
        /// <returns>The grid element data.</returns>
        public GridElementData GetElementAt(int x, int y)
        {
            return m_ItemStackAnchorGrid[x, y];
        }

        /// <summary>
        /// Get the element at the position.
        /// </summary>
        /// <param name="index">The element index.</param>
        /// <returns>The grid element data.</returns>
        public GridElementData GetElementAt(int index)
        {
            return GetElementAt(OneDTo2D(index));
        }

        /// <summary>
        /// Get the anchor position of the anchor for the index.
        /// </summary>
        /// <param name="index">index.</param>
        /// <returns>True if the item was found.</returns>
        public int GetAnchorIndex(int index)
        {
            var elementData = GetElementAt(index);

            if (elementData.IsAnchor || elementData.IsEmpty) {
                return index;
            }

            if (TryFindAnchorForItem((ItemInfo)elementData.ItemStack, out var anchorPos)) {
                return TwoDTo1D(anchorPos);
            }

            return -1;
        }

        /// <summary>
        /// Get the anchor position of the anchor for the index.
        /// </summary>
        /// <param name="index">index.</param>
        /// <returns>True if the item was found.</returns>
        public Vector2Int GetAnchorPosition(int index)
        {
            return OneDTo2D(GetAnchorIndex(index));
        }

        /// <summary>
        /// Get the index of the item.
        /// </summary>
        /// <param name="itemInfo">The item info to find the index for.</param>
        /// <returns>The index where the item info is located.</returns>
        public int GetItemIndex(ItemInfo itemInfo)
        {
            return TwoDTo1D(GetItemPos(itemInfo));
        }

        /// <summary>
        /// Get the position of the item.
        /// </summary>
        /// <param name="itemInfo">The item info to find the position for.</param>
        /// <returns>The position of the item within the grid.</returns>
        public Vector2Int GetItemPos(ItemInfo itemInfo)
        {
            for (int row = 0; row < m_ItemStackAnchorGrid.GetLength(1); row++) {
                for (int col = 0; col < m_ItemStackAnchorGrid.GetLength(0); col++) {
                    var element = m_ItemStackAnchorGrid[col, row];
                    if (element.IsAnchor == false) { continue; }

                    if (itemInfo.ItemStack == element.ItemStack) {
                        return new Vector2Int(col, row);
                    }
                }
            }

            return new Vector2Int(-1, -1);
        }

        /// <summary>
        /// Try to add an item in the position.
        /// </summary>
        /// <param name="info">The item info.</param>
        /// <param name="position">The position.</param>
        /// <returns>True if the item was added.</returns>
        public virtual bool TryPlaceItemToPosition(ItemInfo info, Vector2Int position)
        {
            //Debug.Log($"Try Add: {position} + {info}");
            var x = position.x;
            var y = position.y;

            if (info.Item.TryGetAttributeValue<ItemShape>(ShapeAttributeName, out var shape) == false
                || shape.Count <= 1) {

                // Item takes a 1x1 shape.
                if (m_ItemStackAnchorGrid[x, y].IsOccupied) {
                    return false;
                }

                m_ItemStackAnchorGrid[x, y] = new GridElementData(info.ItemStack, true);
                //Debug.Log("Value 1x1 is now set: "+m_ItemStackAnchorGrid[x, y].IsOccupied);
                return true;
            }

            var anchorWithOffset = new Vector2Int(
                x - shape.Anchor.x,
                y - shape.Anchor.y);

            // Out of range
            if (anchorWithOffset.x < 0 || anchorWithOffset.x + shape.Cols > GridColumns) { return false; }
            if (anchorWithOffset.y < 0 || anchorWithOffset.y + shape.Rows > GridRows) { return false; }

            // Check if the item fits
            for (int row = 0; row < shape.Rows; row++) {
                for (int col = 0; col < shape.Cols; col++) {
                    var gridElementData = m_ItemStackAnchorGrid[anchorWithOffset.x + col, anchorWithOffset.y + row];

                    if (!gridElementData.IsOccupied || !shape.IsIndexOccupied(col, row)) { continue; }

                    if (CanItemStack(info, gridElementData)) { continue; }

                    return false;
                }
            }

            // Item fits, place it
            for (int row = 0; row < shape.Rows; row++) {
                for (int col = 0; col < shape.Cols; col++) {
                    if (shape.IsIndexOccupied(col, row)) {
                        m_ItemStackAnchorGrid[anchorWithOffset.x + col, anchorWithOffset.y + row] =
                            new GridElementData(info.ItemStack, shape.IsAnchor(col, row));
                    }
                }
            }

            //Debug.Log("Value mxn is now set: "+m_ItemStackAnchorGrid[x, y].IsOccupied);
            return true;
        }

        /// <summary>
        /// Can the item be stacked.
        /// </summary>
        /// <param name="info">The item info.</param>
        /// <param name="gridElementData">The grid element data.</param>
        /// <returns>True if the item can stacked to the item in the grid element.</returns>
        protected virtual bool CanItemStack(ItemInfo info, GridElementData gridElementData)
        {
            return info.Item.IsUnique == false && info.Item.StackableEquivalentTo(gridElementData.ItemStack.Item);
        }

        /// <summary>
        /// Try to find a position available for the item info.
        /// </summary>
        /// <param name="info">The item info.</param>
        /// <param name="position">The position which fits the item.</param>
        /// <returns>True if a position was found.</returns>
        public bool TryFindAvailablePosition(ItemInfo info, out Vector2Int position)
        {
            position = Vector2Int.zero;
            if (info.Item == null) { return false; }

            for (int y = 0; y < GridRows; y++) {
                for (int x = 0; x < GridColumns; x++) {

                    position = new Vector2Int(x, y);
                    if (IsPositionAvailable(info, position)) {
                        return true;
                    }
                }
            }

            position = new Vector2Int(-1, -1);
            return false;
        }

        /// <summary>
        /// Is the position within the grid size.
        /// </summary>
        /// <param name="position">The Position.</param>
        /// <returns>True if the position is available.</returns>
        public bool IsPositionValid(Vector2Int position)
        {
            return IsPositionValid(position.x, position.y);
        }

        /// <summary>
        /// Is the position within the grid size.
        /// </summary>
        /// <param name="x">The column.</param>
        /// <param name="y">The row.</param>
        /// <returns>True if the position is available.</returns>
        public bool IsPositionValid(int x, int y)
        {
            if (x < 0 || y < 0 || x >= m_GridSize.x || y >= m_GridSize.y) {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Is the position available for the item.
        /// </summary>
        /// <param name="info">The item info.</param>
        /// <param name="position">The position.</param>
        /// <param name="canIgnore">A function of whether the grid element should be ignored.</param>
        /// <returns>True if the position is available.</returns>
        public virtual bool IsPositionAvailable(ItemInfo info, Vector2Int position, Func<Vector2Int, bool> canIgnore = null)
        {
            var x = position.x;
            var y = position.y;

            if (info.Item.TryGetAttributeValue<ItemShape>(ShapeAttributeName, out var shape) == false
                || shape.Count <= 1) {
                // Item takes a 1x1 shape.
                return IsSingularPositionAvailable(info, position, canIgnore);
            }

            // Out of range
            if (x - shape.Anchor.x < 0 || x - shape.Anchor.x + shape.Cols > GridColumns) { return false; }
            if (y - shape.Anchor.y < 0 || y - shape.Anchor.y + shape.Rows > GridRows) { return false; }

            // Check if the item fits
            for (int row = 0; row < shape.Rows; row++) {
                for (int col = 0; col < shape.Cols; col++) {

                    if (!shape.IsIndexOccupied(col, row)) { continue; }

                    var innerPosition = new Vector2Int(
                        x - shape.Anchor.x + col,
                        y - shape.Anchor.y + row);

                    if (IsSingularPositionAvailable(info, innerPosition, canIgnore)) {
                        continue;
                    }

                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Is the exact position available for the item to be placed.
        /// </summary>
        /// <param name="info">The item to check the position for.</param>
        /// <param name="position">The position to check.</param>
        /// <param name="canIgnore">Can certain positions be ignored.</param>
        /// <returns>True if the position is available for that particular item.</returns>
        protected virtual bool IsSingularPositionAvailable(ItemInfo info, Vector2Int position, Func<Vector2Int, bool> canIgnore = null)
        {
            if (canIgnore?.Invoke(position) ?? false) { return true; }

            var gridElementData = m_ItemStackAnchorGrid[position.x, position.y];

            if (!gridElementData.IsOccupied) { return true; }

            if (CanItemStack(info, gridElementData)) { return true; }

            return false;
        }

        /// <summary>
        /// Is the position available for the item.
        /// </summary>
        /// <param name="info">The item info.</param>
        /// <param name="x">The column.</param>
        /// <param name="y">The row.</param>
        /// <param name="itemStacksToIgnore">A list of item stacks to ignore.</param>
        /// <returns>True if the position is available.</returns>
        public bool IsPositionAvailable(ItemInfo info, int x, int y, ListSlice<ItemStack> itemStacksToIgnore)
        {
            if (info.Item.TryGetAttributeValue<ItemShape>(ShapeAttributeName, out var shape) == false
                || shape.Count <= 1) {

                // Item takes a 1x1 shape.
                if (m_ItemStackAnchorGrid[x, y].IsOccupied && itemStacksToIgnore.Contains(m_ItemStackAnchorGrid[x, y].ItemStack) == false) {
                    return false;
                }
                return true;
            }

            // Out of range
            if (x - shape.Anchor.x < 0 || x - shape.Anchor.x + shape.Cols > GridColumns) { return false; }
            if (y - shape.Anchor.y < 0 || y - shape.Anchor.y + shape.Rows > GridRows) { return false; }

            // Check if the item fits
            for (int row = 0; row < shape.Rows; row++) {
                for (int col = 0; col < shape.Cols; col++) {

                    var localX = x - shape.Anchor.x + col;
                    var localY = y - shape.Anchor.y + row;

                    if (m_ItemStackAnchorGrid[localX, localY].IsOccupied &&
                        shape.IsIndexOccupied(col, row) &&
                        itemStacksToIgnore.Contains(m_ItemStackAnchorGrid[localX, localY].ItemStack) == false) {

                        return false;

                    }
                }
            }

            return true;
        }

        /// <summary>
        /// An item was added, checking if it needs a position within the grid.
        /// </summary>
        /// <param name="originItemInfo">The original item info.</param>
        /// <param name="itemStackAdded">The item stack of the added item.</param>
        public void OnItemAdded(ItemInfo originItemInfo, ItemStack itemStackAdded)
        {
            if (m_Controller.IsCollectionIgnored(itemStackAdded.ItemCollection)) { return; }
            
            var itemInfoAdded = (ItemInfo)itemStackAdded;
            Vector2Int position = new Vector2Int(-1, -1);

            // First check if the item stack already exists in the grid
            if (TryFindAnchorForItem(itemInfoAdded, out position)) {
                // The item is already placed in the grid.
                return;
            }

            if (TryFindAvailablePosition(itemInfoAdded, out position) == false) {
                Debug.LogWarning($"An Item '{itemStackAdded}' was added to the inventory but there is no place for it in the grid!");
                return;
            }

            //Place the item in the grid
            if (TryPlaceItemToPosition(itemInfoAdded, position) == false) {
                Debug.LogError("This should never happen, item could fit but was not added: " + itemInfoAdded);
                return;
            };

            //Debug.Log("Added without any issues");

        }

        /// <summary>
        /// Try to find the anchor of the item info.
        /// </summary>
        /// <param name="itemInfo">The item info.</param>
        /// <param name="position">The position where the item is located.</param>
        /// <returns>True if the item was found.</returns>
        public bool TryFindAnchorForItem(ItemInfo itemInfo, out Vector2Int position)
        {
            var itemStack = itemInfo.ItemStack;
            if (itemStack == null) {
                position = new Vector2Int(-1, -1);
                return false;
            }

            for (int y = 0; y < GridRows; y++) {
                for (int x = 0; x < GridColumns; x++) {
                    if (m_ItemStackAnchorGrid[x, y].IsAnchor && m_ItemStackAnchorGrid[x, y].ItemStack == itemStack) {
                        position = new Vector2Int(x, y);
                        return true;
                    }
                }
            }

            position = new Vector2Int(-1, -1);
            return false;
        }

        /// <summary>
        /// An item was removed, therefore remove it from the grid.
        /// </summary>
        /// <param name="itemInfoRemoved">The item info was removed.</param>
        public void OnItemRemoved(ItemInfo itemInfoRemoved)
        {
            //Clean up the grid from null items when an item is removed.
            //This works because the item stack is reset if an item is removed from the inventory.
            for (int row = 0; row < m_GridSize.y; row++) {
                for (int col = 0; col < m_GridSize.x; col++) {
                    if (m_ItemStackAnchorGrid[col, row].ItemStack?.Item == null) {
                        m_ItemStackAnchorGrid[col, row] = GridElementData.None;
                    }
                }
            }
        }

        /// <summary>
        /// Remove the item from the position.
        /// </summary>
        /// <param name="position">The position.</param>
        /// <returns>True if an item was removed.</returns>
        public bool RemoveItemFromPosition(Vector2Int position)
        {
            var x = position.x;
            var y = position.y;

            if (m_ItemStackAnchorGrid[x, y].ItemStack == null) { return false; }

            if (m_ItemStackAnchorGrid[x, y].IsAnchor == false) {

                if (TryFindAnchorForItem((ItemInfo)m_ItemStackAnchorGrid[x, y].ItemStack, out var anchor) == false) { return false; }

                x = anchor.x;
                y = anchor.y;
            }

            var itemStack = m_ItemStackAnchorGrid[x, y].ItemStack;

            if (itemStack?.Item == null
                || itemStack.Item.TryGetAttributeValue<ItemShape>(ShapeAttributeName, out var shape) == false
                || shape.Count <= 1) {

                // Item takes a 1x1 shape.
                m_ItemStackAnchorGrid[x, y] = GridElementData.None;
                return true;
            }

            // Remove item from each grid element from the shape.
            for (int row = 0; row < shape.Rows; row++) {
                for (int col = 0; col < shape.Cols; col++) {
                    if (shape.IsIndexOccupied(col, row)) {
                        m_ItemStackAnchorGrid[x - shape.Anchor.x + col, y - shape.Anchor.y + row] = GridElementData.None;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Add the item at the index.
        /// </summary>
        /// <param name="itemInfo">The item info.</param>
        /// <param name="positon">The position where the item should be added.</param>
        /// <returns>The added item.</returns>
        public virtual ItemInfo AddItemToPosition(ItemInfo itemInfo, Vector2Int position)
        {
            if (IsPositionAvailable(itemInfo, position) == false) {
                return ItemInfo.None;
            }

            var itemCollection = Inventory.GetItemCollection(m_ItemCollectionID);

            ItemInfo addedItem;
            if (itemCollection == null) {
                addedItem = Inventory.AddItem(itemInfo);
            } else {
                addedItem = itemCollection.AddItem(itemInfo);
            }

            var tempItemPos = GetItemPos(addedItem);

            RemoveItemFromPosition(tempItemPos);
            var placedItem = TryPlaceItemToPosition(addedItem, position);

            return addedItem;
        }

        /// <summary>
        /// Add the item at the index.
        /// </summary>
        /// <param name="itemInfo">The item info.</param>
        /// <param name="index">The index of the item.</param>
        /// <returns>The added item.</returns>
        public virtual ItemInfo AddItemToPosition(ItemInfo itemInfo, int index)
        {
            var wantedItemPos = OneDTo2D(index);
            return AddItemToPosition(itemInfo, wantedItemPos);
        }

        /// <summary>
        /// Can the grid contain the item.
        /// </summary>
        /// <param name="originalItemInfo">The item to preview.</param>
        /// <param name="receivingCollection">The receiving Item Collection.</param>
        /// <returns>True if the item fits.</returns>
        public virtual bool CanAddItem(ItemInfo originalItemInfo, ItemCollection receivingCollection)
        {
            if (originalItemInfo.Item == null) { return false; }
            if (m_Controller.IsCollectionIgnored(receivingCollection)) { return true; }

            var collectionIsNone =
                m_ItemCollectionID.Purpose == ItemCollectionPurpose.None
                && string.IsNullOrWhiteSpace(m_ItemCollectionID.Name);
            var collectionMatch = 
                m_ItemCollectionID.Compare(receivingCollection) 
                || (receivingCollection == null && m_ItemCollectionID.Purpose == ItemCollectionPurpose.Main);

            if (!collectionIsNone && !collectionMatch) { return false; }

            if (m_ItemInfoFilter == null) { return true; }

            // set the item info as if it was already added in the receiving collection to pass the conditions.
            var previewItemInfo =
                new ItemInfo(originalItemInfo.ItemAmount, receivingCollection, originalItemInfo.ItemStack);

            return m_ItemInfoFilter.CanContain(previewItemInfo);
        }

        /// <summary>
        /// Can this grid contain an item that was added in the inventory?
        /// </summary>
        /// <param name="itemInfo">The itemInfo.</param>
        /// <returns>True if the item is valid for the grid data.</returns>
        public virtual bool IsItemValidForGridData(ItemInfo itemInfo)
        {
            return CanAddItem(itemInfo, itemInfo.ItemCollection);
        }

        /// <summary>
        /// Can the item be moved from the source position to the destination position.
        /// </summary>
        /// <param name="sourcePos">The source position.</param>
        /// <param name="destinationPos">The destination position.</param>
        /// <returns>True of the item can be moved.</returns>
        public bool CanMoveIndex(Vector2Int sourcePos, Vector2Int destinationPos)
        {
            // save current layout.
            Copy(m_ItemStackAnchorGrid, m_TemporaryItemStackAnchorGrid);

            var result = TryMoveIndex(sourcePos, destinationPos);

            //Return to previous state.
            Copy(m_TemporaryItemStackAnchorGrid, m_ItemStackAnchorGrid);

            return result;
        }

        /// <summary>
        /// Try to move an item from one index to the other.
        /// </summary>
        /// <param name="sourcePos">The source position.</param>
        /// <param name="destinationPos">The destination position.</param>
        /// <returns>True if the item was moved.</returns>
        public bool TryMoveIndex(Vector2Int sourcePos, Vector2Int destinationPos)
        {
            // Copy values to temporary slot.
            Copy(m_ItemStackAnchorGrid, m_TemporaryItemStackAnchorGrid);

            var sourceElement = GetElementAt(sourcePos);
            var destinationElement = GetElementAt(destinationPos);

            if (sourceElement.IsEmpty) { return false; }

            TryFindAnchorForItem((ItemInfo)sourceElement.ItemStack, out var sourceAnchor);
            TryFindAnchorForItem((ItemInfo)destinationElement.ItemStack, out var destinationAnchor);

            var sourceOffset = sourceAnchor - sourcePos;
            var destinationOffset = destinationAnchor - destinationPos;

            var sourcePosWithOffset = sourcePos + destinationOffset;
            var destinationPosWithOffset = destinationPos + sourceOffset;

            // Remove source item.
            if (RemoveItemFromPosition(sourceAnchor) == false) {
                Debug.LogError("Nothing should be preventing it to be removed");
                //Return to previous state.
                Copy(m_TemporaryItemStackAnchorGrid, m_ItemStackAnchorGrid);
                return false;
            }

            var isOneWayMove = (destinationElement.ItemStack == sourceElement.ItemStack || destinationElement.IsEmpty);

            if (isOneWayMove || m_SmartTwoWayMove == false) {

                if (TryPlaceItemToPosition((ItemInfo)sourceElement.ItemStack, destinationPosWithOffset) == false) {
                    //Return to previous state.
                    Copy(m_TemporaryItemStackAnchorGrid, m_ItemStackAnchorGrid);
                    return false;
                }

                return true;
            }

            if (RemoveItemFromPosition(destinationAnchor) == false) {
                Debug.LogError("Nothing should be preventing it to be removed");
                //Return to previous state.
                Copy(m_TemporaryItemStackAnchorGrid, m_ItemStackAnchorGrid);
                return false;
            }

            if (TryPlaceItemToPosition((ItemInfo)sourceElement.ItemStack, destinationPosWithOffset) == false) {
                //Return to previous state.
                Copy(m_TemporaryItemStackAnchorGrid, m_ItemStackAnchorGrid);
                return false;
            }

            if (TryPlaceItemToPosition((ItemInfo)destinationElement.ItemStack, sourcePosWithOffset) == false) {
                //Return to previous state.
                Copy(m_TemporaryItemStackAnchorGrid, m_ItemStackAnchorGrid);
                return false;
            }

            return true;
        }

        /// <summary>
        /// Get the anchor offset specific for an item compared to a position.
        /// </summary>
        /// <param name="itemInfo"></param>
        /// <param name="pos"></param>
        /// <returns></returns>
        public bool TryGetAnchorOffset(ItemInfo itemInfo, Vector2Int pos, out Vector2Int anchorOffset)
        {
            if (TryFindAnchorForItem(itemInfo, out var anchor) == false) {
                anchorOffset = new Vector2Int(-1, -1);
                return false;
            }

            anchorOffset = anchor - pos;
            return true;
        }

        /// <summary>
        /// Copy the data from another grid data.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="destination">The destination.</param>
        public void Copy(GridElementData[,] source, GridElementData[,] destination)
        {
            for (int row = 0; row < GridRows; row++) {
                for (int col = 0; col < GridColumns; col++) { destination[col, row] = source[col, row]; }
            }
        }

        /// <summary>
        /// Print the Grid Array.
        /// </summary>
        public void PrintGridArray()
        {
            var printMessage = "";
            for (int y = 0; y < m_ItemStackAnchorGrid.GetLength(1); y++) {
                printMessage += "| ";
                for (int x = 0; x < m_ItemStackAnchorGrid.GetLength(0); x++) {
                    printMessage += (m_ItemStackAnchorGrid[x, y].IsOccupied ? "x" : "o") + " | ";
                }
                printMessage += "\n";
            }

            Debug.Log(printMessage);
        }
    }
}