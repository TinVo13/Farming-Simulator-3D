using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShippingBin : MonoBehaviour
{
    /*public static int hourToShip = 0;*/
    /*public static GameTimestamp timeShip = TimeManager.Instance.GetGameTimestamp();*/
    public static ShippingBin Instance { get; private set; }

    public InventorySlot[] itemSlots;

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

    public static List<ItemSlotData> itemsToShip = new List<ItemSlotData>();

    public void PickUp()
    {
        ItemData handSlotItem = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Item);

        if (handSlotItem == null) return;

        UIManager.Instance.TriggerYesNoPrompt($"Do you want to sell {handSlotItem.name} with price {handSlotItem.cost} ? ", PlaceItemInShippingBin);
    }

    void PlaceItemInShippingBin()
    {
        ItemSlotData handSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);

        itemsToShip.Add(new ItemSlotData(handSlot));

        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);

        handSlot.Empty();

        InventoryManager.Instance.RenderHand();

        UIManager.Instance.RenderInventory();

        UIManager.Instance.RenderInventoryPanelSell(inventoryItemSlots, itemSlots);

        foreach (ItemSlotData item in itemsToShip)
        {
            Debug.Log($"{item.itemData.name} x {item.quantity}");
        }
    }

    public static void ShipItems()
    {
        int moneyToReceive = TallyItems(itemsToShip);
        PlayerStats.Earn(moneyToReceive);

        itemsToShip.Clear();
    }

    static int TallyItems(List<ItemSlotData> items)
    {
        int total = 0;
        foreach(ItemSlotData item in items)
        {
            total += item.quantity * item.itemData.cost;
        }
        return total;
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp();
    }

}
