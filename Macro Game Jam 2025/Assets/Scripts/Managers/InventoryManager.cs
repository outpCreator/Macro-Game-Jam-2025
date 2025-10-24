using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager instance { get; private set; }

    [Header("Inventory Settings")]
    public int maxInventorySize = 20;
    [HideInInspector] public int currentInventorySlots = 0;
    

    [Header("Tear One Items")]
    public List<ItemTearOne> tearOneItems = new List<ItemTearOne>();

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public bool AddItemToInventory(ItemTearOne newItem)
    {
        if (currentInventorySlots < maxInventorySize)
        {
            tearOneItems.Add(newItem);
            currentInventorySlots++;
            Debug.Log($"Item {newItem.itemName} added to inventory. Current size: {currentInventorySlots}/{maxInventorySize}");
            return true;
        }
        else
        {
            Debug.LogWarning("Inventory is full! Cannot add more items.");
            return false;
        }
    }

    public bool RemoveItemFromInventory(ItemTearOne itemToRemove)
    {
        if (tearOneItems.Remove(itemToRemove))
        {
            currentInventorySlots--;
            Debug.Log($"Item {itemToRemove.itemName} removed from inventory. Current size: {currentInventorySlots}/{maxInventorySize}");
            return true;
        }
        else
        {
            Debug.LogWarning($"Item {itemToRemove.itemName} not found in inventory.");
            return false;
        }
    }


}
