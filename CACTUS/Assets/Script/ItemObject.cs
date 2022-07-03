using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData item;

    public string GetInteractPrompt()
    {
        return string.Format("Pickup {0}", item.displayName);
    }
    public void OnInteract()
    {
        Destroy(gameObject);
    }
}
