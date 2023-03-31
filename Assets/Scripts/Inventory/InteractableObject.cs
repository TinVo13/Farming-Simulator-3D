using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class InteractableObject : MonoBehaviour
{
    //The item information the GameObject is supposed to represent
    public ItemData item;
    public UnityEvent onInteract = new UnityEvent();

    public virtual void PickUp()
    {
        onInteract?.Invoke();

        //Set the player's inventory to the item
        InventoryManager.Instance.EquipHandSlot(item);

        //Update the changes in the scene
        InventoryManager.Instance.RenderHand();
        //Destroy this instance so as to not have multiple copies
        Destroy(gameObject);

    }
}
