using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Crafting Recipe", menuName = "New Crafting Recipe")]
public class CraftingRecipies : ScriptableObject
{
    public ItemData itemToCraft;
    public ResourceCost[] cost;
}

[System.Serializable]
public class ResourceCost
{
    public ItemData item;
    public int quantity;
}
