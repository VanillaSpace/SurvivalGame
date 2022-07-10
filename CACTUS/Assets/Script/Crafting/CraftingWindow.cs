using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingWindow : MonoBehaviour
{
    public CraftingUI[] recipeUIs;

    public static CraftingWindow instance;

    void Awake()
    {
        instance = this;
    }

    void OnEnable()
    {
        Inventory.instance.onOpenInventory.AddListener(OnOpenInventory);
    }

    void OnDisable()
    {
        Inventory.instance.onOpenInventory.RemoveListener(OnOpenInventory);
    }

    void OnOpenInventory()
    {
        gameObject.SetActive(false);
    }

    // called when we click on a crafting recipe to craft it
    public void Craft(CraftingRecipies recipe)
    {
        // remove the items it costs to craft from our inventory
        for (int i = 0; i < recipe.cost.Length; i++)
        {
            for (int x = 0; x < recipe.cost[i].quantity; x++)
            {
                Inventory.instance.RemoveItem(recipe.cost[i].item);
            }
        }

        // add the crafted item to our inventory
        Inventory.instance.AddItem(recipe.itemToCraft);

        // update the recipe UIs
        for (int i = 0; i < recipeUIs.Length; i++)
        {
            recipeUIs[i].UpdateCanCraft();
        }
    }
}
