using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlotData
{
    public ItemData itemData;
    public int quantity;

    public ItemSlotData(ItemData itemData, int quantity)
    {
        this.itemData = itemData;
        this.quantity = quantity;
        ValidateQuantity();
    }

    //Automatically construct the class with the item data of quantity 1
    public ItemSlotData(ItemData itemData)
    {
        this.itemData=itemData;
        quantity = 1;
        ValidateQuantity();
    }

    //Clones the ItemSlotData
    public ItemSlotData (ItemSlotData slotToClone)
    {
        itemData = slotToClone.itemData;
        quantity = slotToClone.quantity;
    }

    //Stacking System

    //Shortcut function to add 1 to the stack
    public void AddQuantity()
    {
        AddQuantity(1);
    }

    //Add a specified amount to the stack
    public void AddQuantity(int amountToAdd)
    {
        quantity += amountToAdd;
    }

    public void Remove()
    {
        quantity--;
        ValidateQuantity();
    }

    //Compares the item to see if it can be stacked
    public bool Stackable(ItemSlotData slotToCompare)
    {
        return slotToCompare.itemData == itemData;
    }

    //Do checks to see if the values make sense
    private void ValidateQuantity()
    {
        if (quantity <= 0 || itemData == null)
        {
            Empty();
        }
    }

    //Empties out the item slot
    public void Empty()
    {
        itemData = null;
        quantity = 0;
    }

    //Check if the slot is considered 'empty'
    public bool IsEmpty()
    {
        return itemData == null;
    }

    public static ItemSlotSaveData SerializeData(ItemSlotData itemSlot)
    {
        return new ItemSlotSaveData(itemSlot);
    }

    public static ItemSlotData DeserializeData(ItemSlotSaveData itemSaveSlot)
    {
        ItemData item = InventoryManager.Instance.itemIndex.GetItemFromString(itemSaveSlot.itemID);
        return new ItemSlotData(item, itemSaveSlot.quantity);
    }  

    public static ItemSlotSaveData[] SerializeArray(ItemSlotData[] array)
    {
        return Array.ConvertAll(array, new Converter<ItemSlotData, ItemSlotSaveData>(SerializeData));
    }

    public static ItemSlotData[] DeserializeArray(ItemSlotSaveData[] array)
    {
        return Array.ConvertAll(array, new Converter<ItemSlotSaveData, ItemSlotData>(DeserializeData));
    }

    public override string ToString()
    {
        return base.ToString();
    }
}
