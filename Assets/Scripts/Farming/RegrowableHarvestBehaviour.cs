using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RegrowableHarvestBehaviour : InteractableObject
{
    CropBehaviour parrentCrop;

    //Sets the parrent crop
    public void SetParrent(CropBehaviour parrentCrop)
    {
        this.parrentCrop = parrentCrop;
    }

    public override void PickUp()
    {
        //Set the player's inventory to the item
        InventoryManager.Instance.EquipHandSlot(item);

        //Update the changes in the scene
        InventoryManager.Instance.RenderHand();

        //Set the parrent  crop back to seeding to regrow it
        parrentCrop.Regrow();
    }
}
