using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    public List<ItemData> shopItems;

    public static void Purchase(ItemData item, int quantity)
    {
        int totalCost = item.cost * quantity;

        if (PlayerStats.money >= totalCost)
        {
            PlayerStats.Spend(totalCost);

            ItemSlotData purchasedItem = new ItemSlotData(item, quantity);

            InventoryManager.Instance.ShopToInventory(purchasedItem);
        }
    }

    public override void PickUp()
    {
        Debug.Log("Purchasing");
        Purchase(item, 2);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp();
    }
}
