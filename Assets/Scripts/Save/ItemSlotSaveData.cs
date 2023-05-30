using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotSaveData
{
    public string itemID;
    public int quantity;

    public ItemSlotSaveData(ItemSlotData data)
    {
        if (data.IsEmpty())
        {
            itemID = null;
            quantity = 0;
            return;
        }

        itemID = data.itemData.name;
        quantity = data.quantity;
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
