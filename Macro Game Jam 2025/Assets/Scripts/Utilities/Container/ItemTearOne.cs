using UnityEngine;
using UnityEngine.Events;

public enum itemType
{
    Type_a,
    Type_b,
}

[System.Serializable]
public class ItemTearOne
{
    public GameObject itemPrefab;

    [Header("Item Displays")]
    public string itemName;
    public string itemDiscription;
    public itemType type;
    public Sprite itemIcon;

    [Header("Item Events")]
    public UnityEvent onUse;
}
