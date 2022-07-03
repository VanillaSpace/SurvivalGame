using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ItemSlotUI : MonoBehaviour
{
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;

    private ItemSlot curSlot;
    private Outline outline;

    public int index;
    public bool equipped;

    void Awake()
    {
        outline = GetComponent<Outline>();
    }
    void OnEnable()
    {
        outline.enabled = equipped;
    }

    // sets the item to be displayed in the slot
    public void Set(ItemSlot slot)
    {
        curSlot = slot;
        icon.gameObject.SetActive(true);
        icon.sprite = slot.item.icon;
        quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : string.Empty;
        if (outline != null)
        {
            outline.enabled = equipped;
        }
    }

    // clears the item slot
    public void Clear()
    {
        curSlot = null;
        icon.gameObject.SetActive(false);
        quantityText.text = string.Empty;
    }

    // called when we click on the slot
    public void OnButtonClick()
    {

    }

}
