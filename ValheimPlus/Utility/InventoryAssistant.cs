using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using ValheimPlus.Configurations;

namespace ValheimPlus
{
    static class InventoryAssistant
    {

        /// <summary>
        /// Chests the local player may use. Carts and ships are left out when includeVehicles is false.
        /// </summary>
        public static List<Container> GetNearbyChests(GameObject target, float range, bool checkWard = true,
            bool includeVehicles = true)
        {
            // Player features act as the local player, so there is nothing to find without one.
            if (!Player.m_localPlayer) return new List<Container>();

            long playerId = Player.m_localPlayer.GetPlayerID();
            return FindNearbyChests(target, range, includeVehicles, (container, position) =>
                container.CheckAccess(playerId) &&
                (!checkWard || PrivateArea.CheckAccess(position, 0f, false, true)));
        }

        /// <summary>
        /// Chests a machine may use, the same on every peer: public chests not under a ward the machine is outside of.
        /// </summary>
        public static List<Container> GetNearbyChestsForMachine(GameObject machine, float range, bool checkWard = true)
        {
            Vector3 machinePosition = machine.transform.position;
            return FindNearbyChests(machine, range, includeVehicles: true, (container, position) =>
                container.m_privacy == Container.PrivacySetting.Public &&
                (!checkWard || SharesWards(machinePosition, position)));
        }

        // A warded chest is off-limits unless the machine is under the same wards. Location only, so ward
        // access removed later, or a ward overlapping the chest from outside, isn't accounted for.
        private static bool SharesWards(Vector3 machine, Vector3 chest)
        {
            foreach (PrivateArea area in PrivateArea.m_allAreas)
            {
                if (area && area.IsEnabled() && area.IsInside(chest, 0f) && !area.IsInside(machine, 0f))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Removes up to amount of the item from the given chests, in order.
        /// </summary>
        public static int RemoveItemInAmountFromChests(List<Container> chests, ItemDrop.ItemData needle, int amount)
        {
            int removedTotal = 0;
            foreach (Container chest in chests)
            {
                if (amount <= 0) break;
                int removed = RemoveItemFromChest(chest, needle, amount);
                removedTotal += removed;
                amount -= removed;
            }

            return removedTotal;
        }

        private static List<Container> FindNearbyChests(GameObject target, float range, bool includeVehicles,
            Func<Container, Vector3, bool> hasAccessAt)
        {
            // item == cart layermask
            // vehicle == cart&ship layermask

            string[] layerMask = { "piece" };

            if (includeVehicles &&
                (Configuration.Current.CraftFromChest.allowCraftingFromCarts || Configuration.Current.CraftFromChest.allowCraftingFromShips))
                layerMask = new string[] { "piece", "item", "vehicle" };

            Collider[] hitColliders = Physics.OverlapSphere(target.transform.position, range, LayerMask.GetMask(layerMask));

            // Order the found objects to select the nearest first instead of the farthest inventory.
            IOrderedEnumerable<Collider> orderedColliders = hitColliders.OrderBy(x => Vector3.Distance(x.gameObject.transform.position, target.transform.position));

            List<Container> validContainers = new List<Container>();
            foreach (var hitCollider in orderedColliders)
            {
                try
                {
                    Container foundContainer = hitCollider.GetComponentInParent<Container>();

                    if (!foundContainer)
                        continue;
                    
                    // Verify the container has loaded its inventory from the ZDO at least once.
                    // Without this, we could perform operations on a container that has not yet loaded its inventory,
                    // which could result in overwriting the container's saved contents with an empty inventory.
                    // Value starts at uint.MaxValue, the first revision is set to 0 and monotonically increases by 1.
                    if (foundContainer.m_lastRevision == uint.MaxValue)
                        continue;
                    
                    if (validContainers.Contains(foundContainer))
                        continue;

                    bool hasAccess = hasAccessAt(foundContainer, hitCollider.gameObject.transform.position);
                    var piece = foundContainer.GetComponentInParent<Piece>();
                    var isVagon = foundContainer.GetComponentInParent<Vagon>() != null;
                    var isShip = foundContainer.GetComponentInParent<Ship>() != null;

                    if (piece != null
                        /*&& piece.IsPlacedByPlayer() Prevents detection of ship storage */
                        && hasAccess
                        && foundContainer.GetInventory() != null)
                    {

                        if (isVagon && (!includeVehicles || !Configuration.Current.CraftFromChest.allowCraftingFromCarts))
                            continue;
                        if (isShip && (!includeVehicles || !Configuration.Current.CraftFromChest.allowCraftingFromShips))
                            continue;

                        if (piece.IsPlacedByPlayer() || (isShip && Configuration.Current.CraftFromChest.allowCraftingFromShips))
                            validContainers.Add(foundContainer);
                    }
                }
                catch (Exception e)
                {
                    // Skip the container rather than abandon the search, but say what went wrong.
                    ValheimPlusPlugin.Logger.LogDebug(
                        $"GetNearbyChests skipped '{hitCollider.gameObject.name}': {e}");
                }
            }

            return validContainers;
        }

        /// <summary>
        /// Get a chests that contain the specified itemInfo.m_shared.m_name (item name)
        /// </summary>
        public static List<Container> GetNearbyChestsWithItem(GameObject target, float range, ItemDrop.ItemData itemInfo, bool checkWard = true)
        {
            List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);

            List<Container> validChests = new List<Container>();
            foreach (Container chest in nearbyChests)
            {
                if (ChestContainsItem(chest, itemInfo))
                {
                    validChests.Add(chest);
                }
            }

            return validChests;
        }

        /// <summary>
        /// Check if the container contains the itemInfo.m_shared.name (item name)
        /// </summary>
        public static bool ChestContainsItem(Container chest, ItemDrop.ItemData needle)
        {
            List<ItemDrop.ItemData> items = chest.GetInventory().GetAllItems();

            foreach (ItemDrop.ItemData item in items)
            {
                if (item.m_shared.m_name == needle.m_shared.m_name)
                    return true;
            }

            return false;
        }

        public static bool ChestContainsItem(Container chest, string needle)
        {
            List<ItemDrop.ItemData> items = chest.GetInventory().GetAllItems();

            foreach (ItemDrop.ItemData item in items)
            {
                if (item.m_shared.m_name == needle)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// function to get all items in nearby chests by range
        /// </summary>
        public static List<ItemDrop.ItemData> GetNearbyChestItems(GameObject target, float range = 10, bool checkWard = true)
        {
            List<ItemDrop.ItemData> itemList = new List<ItemDrop.ItemData>();
            List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);

            foreach (Container chest in nearbyChests)
            {
                List<ItemDrop.ItemData> chestItemList = chest.GetInventory().GetAllItems();
                foreach (ItemDrop.ItemData item in chestItemList)
                    itemList.Add(item);
            }

            return itemList;
        }

        /// <summary>
        /// function to get all items in chest list
        /// </summary>
        public static List<ItemDrop.ItemData> GetNearbyChestItemsByContainerList(List<Container> nearbyChests)
        {
            List<ItemDrop.ItemData> itemList = new List<ItemDrop.ItemData>();

            foreach (Container chest in nearbyChests)
            {
                List<ItemDrop.ItemData> chestItemList = chest.GetInventory().GetAllItems();
                foreach (ItemDrop.ItemData item in chestItemList)
                    itemList.Add(item);
            }

            return itemList;
        }

        /// <summary>
        /// function to get the amount of a specific item in a list of ItemDrop.ItemData
        /// </summary>
        public static int GetItemAmountInItemList(List<ItemDrop.ItemData> itemList, ItemDrop.ItemData item,
            int quality = -1, bool matchWorldLevel = true) => itemList
            .Where(current => current.m_shared.m_name == item.m_shared.m_name &&
                              (quality < 0 || quality == current.m_quality) &&
                              (!matchWorldLevel || current.m_worldLevel >= Game.m_worldLevel))
            .Sum(current => current.m_stack);

        public static int GetItemAmountInItemList(List<ItemDrop.ItemData> itemList, string name,
            int quality = -1, bool matchWorldLevel = true) => itemList
            .Where(current => current.m_shared.m_name == name &&
                              (quality < 0 || quality == current.m_quality) &&
                              (!matchWorldLevel || current.m_worldLevel >= Game.m_worldLevel))
            .Sum(current => current.m_stack);

        // function to remove items in the amount from all nearby chests
        public static int RemoveItemInAmountFromAllNearbyChests(GameObject target, float range, ItemDrop.ItemData needle, int amount, bool checkWard = true)
        {
            List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);

            // check if there are enough items nearby
            List<ItemDrop.ItemData> allItems = GetNearbyChestItemsByContainerList(nearbyChests);

            // get amount of item
            int availableAmount = GetItemAmountInItemList(allItems, needle);

            // check if there are enough items
            if (amount == 0)
                return 0;

            // iterate all chests and remove as many items as possible for the respective chest
            int itemsRemovedTotal = 0;
            foreach (Container chest in nearbyChests)
            {
                if (itemsRemovedTotal != amount)
                {
                    int removedItems = RemoveItemFromChest(chest, needle, amount);
                    itemsRemovedTotal += removedItems;
                    amount -= removedItems;
                }
            }

            return itemsRemovedTotal;
        }

        public static int RemoveItemInAmountFromAllNearbyChests(GameObject target, float range, string needle, int amount, bool checkWard = true)
        {
            List<Container> nearbyChests = GetNearbyChests(target, range, checkWard);

            // check if there are enough items nearby
            List<ItemDrop.ItemData> allItems = GetNearbyChestItemsByContainerList(nearbyChests);

            // get amount of item
            int availableAmount = GetItemAmountInItemList(allItems, needle);

            // check if there are enough items
            if (amount == 0)
                return 0;

            // iterate all chests and remove as many items as possible for the respective chest
            int itemsRemovedTotal = 0;
            foreach (Container chest in nearbyChests)
            {
                if (itemsRemovedTotal != amount)
                {
                    int removedItems = RemoveItemFromChest(chest, needle, amount);
                    itemsRemovedTotal += removedItems;
                    amount -= removedItems;
                }
            }

            return itemsRemovedTotal;
        }

        // function to add a item by name/ItemDrop.ItemData to a specified chest

        /// <summary>
        /// Removes the specified amount of a item found by m_shared.m_name by the declared amount
        /// </summary>
        public static int RemoveItemFromChest(Container chest, ItemDrop.ItemData needle, int amount = 1)
        {
            if (!ChestContainsItem(chest, needle))
            {
                return 0;
            }

            int totalRemoved = 0;
            // find item
            List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
            foreach (ItemDrop.ItemData itemData in allItems)
            {
                if (itemData.m_shared.m_name == needle.m_shared.m_name)
                {
                    int num = Mathf.Min(itemData.m_stack, amount);
                    itemData.m_stack -= num;
                    amount -= num;
                    totalRemoved += num;
                    if (amount <= 0)
                    {
                        break;
                    }
                }
            }

            // We don't want to send chest content through network
            if (totalRemoved == 0) return 0;

            allItems.RemoveAll((ItemDrop.ItemData x) => x.m_stack <= 0);
            chest.m_inventory.m_inventory = allItems;

            ConveyContainerToNetwork(chest);

            return totalRemoved;
        }

        public static int RemoveItemFromChest(Container chest, string needle, int amount = 1)
        {
            if (!ChestContainsItem(chest, needle))
            {
                return 0;
            }

            int totalRemoved = 0;
            // find item
            List<ItemDrop.ItemData> allItems = chest.GetInventory().GetAllItems();
            foreach (ItemDrop.ItemData itemData in allItems)
            {
                if (itemData.m_shared.m_name == needle)
                {
                    int num = Mathf.Min(itemData.m_stack, amount);
                    itemData.m_stack -= num;
                    amount -= num;
                    totalRemoved += num;
                    if (amount <= 0)
                    {
                        break;
                    }
                }
            }

            // We don't want to send chest content through network
            if (totalRemoved == 0) return 0;

            allItems.RemoveAll((ItemDrop.ItemData x) => x.m_stack <= 0);
            chest.m_inventory.m_inventory = allItems;

            ConveyContainerToNetwork(chest);

            return totalRemoved;
        }

        /// <summary>
        /// Function to convey the changes of a container to the network
        /// </summary>
        public static void ConveyContainerToNetwork(Container c)
        {
            c.Save();
            c.GetInventory().Changed();
        }
    }
}
