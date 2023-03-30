using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : InteractableObject
{
    public List<ItemData> shopItems;
    private GameObject canvasJoyStick;
    private GameObject inventoryButton;

    [Header("Dialogue")]
    public List<DialogueLine> dialogueOnShopOpen;

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
        DialogueManager.Instance.StartDialogue(dialogueOnShopOpen, OpenShop);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp();
    }

    void OpenShop()
    {
        canvasJoyStick.SetActive(false);
        inventoryButton.SetActive(false);
        UIManager.Instance.OpenShop(shopItems);
    }
}
