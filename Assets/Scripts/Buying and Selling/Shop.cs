using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shop : MonoBehaviour
{
    public static Shop Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

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

    public void PickUp()
    {
        /*DialogueManager.Instance.StartDialogue(dialogueOnShopOpen, OpenShop);*/
        DialogueManager.Instance.StartDialogue(dialogueOnShopOpen, OpenOption);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp();
    }

    public void OpenShop()
    {
        canvasJoyStick.SetActive(false);
        inventoryButton.SetActive(false);
        UIManager.Instance.OpenShop(shopItems);
    }

    void OpenOption()
    {
        canvasJoyStick.SetActive(false);
        inventoryButton.SetActive(false);
        UIManager.Instance.OpenOption();
    }
}
