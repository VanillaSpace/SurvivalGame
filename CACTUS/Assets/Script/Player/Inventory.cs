using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.InputSystem;
using UnityEngine.Events;

public class Inventory : MonoBehaviour
{
    public ItemSlotUI[] uiSlots;
    public ItemSlot[] slots;

    public GameObject inventoryWindow;
    public Transform dropPosition;

    [Header("Selected Item")]
    private ItemSlot selectedItem;
    private int selectedItemIndex;
    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public TextMeshProUGUI selectedItemStatNames;
    public TextMeshProUGUI selectedItemStatValues;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unEquipButton;
    public GameObject dropButton;

    private int curEquipIndex;

    // components
    private PlayerController controller;
    private PlayerNeeds needs;

    [Header("Events")]
    public UnityEvent onOpenInventory;
    public UnityEvent onCloseInventory;

    // singleton
    public static Inventory instance;

    void Awake()
    {
        instance = this;
        controller = GetComponent<PlayerController>();
        needs = GetComponent<PlayerNeeds>();
    }

    void Start()
    {
        inventoryWindow.SetActive(false);
        slots = new ItemSlot[uiSlots.Length];

        // initialize the slots
        for (int x = 0; x < slots.Length; x++)
        {
            slots[x] = new ItemSlot();
            uiSlots[x].index = x;
            uiSlots[x].Clear();
        }

        ClearSelectedItemWindow();
    }

    // called when we give an inventory input - managed by the Input System
    public void OnInventoryButton(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            Toggle();
        }
    }

    // opens or closes the inventory
    public void Toggle()
    {
        if (inventoryWindow.activeInHierarchy)
        {
            inventoryWindow.SetActive(false);
            onCloseInventory.Invoke();
            controller.ToggleCursor(false);
        }
        else
        {
            inventoryWindow.SetActive(true);
            onOpenInventory.Invoke();
            ClearSelectedItemWindow();
            controller.ToggleCursor(true);
        }
    }

    // is the inventory currently open?
    public bool IsOpen()
    {
        return inventoryWindow.activeInHierarchy;
    }

    // adds the requested item to the player's inventory
    public void AddItem(ItemData item)
    {
        if (item.canStack)
        {
            ItemSlot slotToStackTo = GetItemStack(item);

            if (slotToStackTo != null)
            {
                slotToStackTo.quantity++;
                UpdateUI();
                return;
            }
        }

        ItemSlot emptySlot = GetEmptySlot();

        if (emptySlot != null)
        {
            emptySlot.item = item;
            emptySlot.quantity = 1;
            UpdateUI();
            return;
        }

        ThrowItem(item);
    }

    // spawns the item infront of the player
    void ThrowItem(ItemData item)
    {
        Instantiate(item.dropPrefab, dropPosition.position, Quaternion.Euler(Vector3.one * Random.value * 360.0f));
    }


    // updates the UI slots
    void UpdateUI()
    {
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item != null)
            {
                uiSlots[x].Set(slots[x]);
            }
            else
            {
                uiSlots[x].Clear();
            }
        }
    }

    // returns the item slot that the requested item can be stacked on
    // returns null if there is no stack available
    ItemSlot GetItemStack(ItemData item)
    {
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item == item && slots[x].quantity < item.maxStackAmount)
            {
                return slots[x];
            }
        }

        return null;
    }

    // returns an empty slot in the inventory
    // if there are no empty slots - return null
    ItemSlot GetEmptySlot()
    {
        for (int x = 0; x < slots.Length; x++)
        {
            if (slots[x].item == null)
            {
                return slots[x];
            }
        }

        return null;
    }

    // called when we click on an item slot
    public void SelectItem(int index)
    {
        // we can't select the slot if there's no item
        if (slots[index].item == null)
        {
            return;
        }

        // set the selected item preview window
        selectedItem = slots[index];
        selectedItemIndex = index;

        selectedItemName.text = selectedItem.item.displayName;
        selectedItemDescription.text = selectedItem.item.description;

        // set stat values and stat names
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;

        for (int x = 0; x < selectedItem.item.consumables.Length; x++)
        {
            selectedItemStatNames.text += selectedItem.item.consumables[x].type.ToString() + "\n";
            selectedItemStatValues.text += selectedItem.item.consumables[x].value.ToString() + "\n";
        }

        useButton.SetActive(selectedItem.item.type == ItemType.Consumable);
        equipButton.SetActive(selectedItem.item.type == ItemType.Equipable && !uiSlots[index].equipped);
        unEquipButton.SetActive(selectedItem.item.type == ItemType.Equipable && uiSlots[index].equipped);
        dropButton.SetActive(true);
    }


    // called when the inventory opens or the currently selected item has depleted
    void ClearSelectedItemWindow()
    {
        // clear the text elements
        selectedItem = null;
        selectedItemName.text = string.Empty;
        selectedItemDescription.text = string.Empty;
        selectedItemStatNames.text = string.Empty;
        selectedItemStatValues.text = string.Empty;

        // disable buttons
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unEquipButton.SetActive(false);
        dropButton.SetActive(false);
    }

    // called when the "Use" button is pressed
    public void OnUseButton()
    {
        // is the selected item a consumable?
        if (selectedItem.item.type == ItemType.Consumable)
        {
            // loop through all the needs it will contribute to
            for (int x = 0; x < selectedItem.item.consumables.Length; x++)
            {
                switch (selectedItem.item.consumables[x].type)
                {
                    case ConsumableType.Health: needs.Heal(selectedItem.item.consumables[x].value); break;
                    case ConsumableType.Hunger: needs.Eat(selectedItem.item.consumables[x].value); break;
                    case ConsumableType.Thirst: needs.Drink(selectedItem.item.consumables[x].value); break;
                    case ConsumableType.Sleep: needs.Sleep(selectedItem.item.consumables[x].value); break;
                }
            }
        }

        RemoveSelectedItem();
    }

    // called when the "Equip" button is pressed
    public void OnEquipButton()
    {
    }
    // unequips the requested item
    void UnEquip(int index)
    {
    }
    // called when the "UnEquip" button is pressed
    public void OnUnEquipButton()
    {
    }
    // called when the "Drop" button is pressed
    public void OnDropButton()
    {
        ThrowItem(selectedItem.item);
        RemoveSelectedItem();
    }

    // removes the currently selected item
    void RemoveSelectedItem()
    {
        selectedItem.quantity--;
        // have we dropped all of this stack?
        if (selectedItem.quantity == 0)
        {
            if (uiSlots[selectedItemIndex].equipped == true)
                UnEquip(selectedItemIndex);
            selectedItem.item = null;
            ClearSelectedItemWindow();
        }

        UpdateUI();
    }

    public void RemoveItem(ItemData item)
    {

    }

    // does the player have "quantity" amount of "item"s?
    public bool HasItems(ItemData item, int quantity)
    {
        return false;
    }


}

public class ItemSlot
{
    public ItemData item;
    public int quantity;
}
