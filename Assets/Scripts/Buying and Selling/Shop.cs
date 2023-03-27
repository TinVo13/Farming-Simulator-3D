using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    public List<ItemData> shopItems;
    private GameObject canvasJoyStick;
    private GameObject inventoryButton;

    private void Start()
    {
        canvasJoyStick = GameObject.FindWithTag("JoyStick");
        inventoryButton = GameObject.FindWithTag("InventoryButton");
    }

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
        UIManager.Instance.OpenShop(shopItems);
        canvasJoyStick.SetActive(false);
        inventoryButton.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp();
    }
}
