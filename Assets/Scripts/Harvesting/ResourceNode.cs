using Foxlair.Inventory;
using System.Collections.Generic;
using UnityEngine;

namespace Foxlair.Harvesting
{

    public class ResourceNode : MonoBehaviour
    {
        public bool isHarvested = false;
        public float respawnTime = 120f;
        public float timeSinceLastHarvest = 0f;

        //Maybe only get resource when fully harvested
        public int resourceDropRatePerTick = 1;

        public List<CharacterController> allowedHarvesters;

        //public WeaponItem requiredHarvestTool;
        public Item inventoryResource;
        //public ItemPicker inventoryItemPicker;
        //public Health harvestResourceNodeHealth;

        public Animator resourceNodeAnimator;
        public GameObject meshRendererGameObject;
        public ResourceType resource;


        public virtual void Awake()
        {
            //harvestResourceNodeHealth.OnDeath += OnHarvested;
            meshRendererGameObject = GetComponentInChildren<MeshRenderer>().gameObject;
        }

        public virtual void Update()
        {
            HandleRespawnTimer();
        }

        public virtual void StartHarvesting()
        {

        }


        public virtual void OnHarvested()
        {
            meshRendererGameObject.SetActive(false);
            isHarvested = true;
        }

        private void HandleRespawnTimer()
        {
            if (!isHarvested) return;
            timeSinceLastHarvest += Time.deltaTime;
            if (timeSinceLastHarvest >= respawnTime)
            {
                RespawnResourceNode();
            }
        }

        private void RespawnResourceNode()
        {
            //harvestResourceNodeHealth.Revive();
            meshRendererGameObject.SetActive(true);
            timeSinceLastHarvest = 0f;
            isHarvested = false;
        }

        //public virtual InventoryItem HarvestHit(CharacterController _harvester)
        //{
        //    if (_harvester.GetComponent<CharacterHandleWeapon>().CurrentWeapon == requiredHarvestTool
        //        && isHarvested == false
        //        && (allowedHarvesters.Count == 0 || allowedHarvesters.Contains(_harvester)))
        //    {
        //        inventoryResource.Quantity = resourceDropRatePerTick;
        //        return inventoryResource;
        //    }
        //    return null;
        //}
    }

    
}