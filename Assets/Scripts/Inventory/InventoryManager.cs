using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance
    {
        get; private set;
    }

    private void Awake()
    {
        //If there is more than one instance, destroy the extra
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }

    }

    //The full list of items 
    public ItemIndex itemIndex;

    [Header("Tools")]
    //Tool Slots
    [SerializeField] 
    private ItemSlotData[] toolSlots = new ItemSlotData[8];
    //Tool in the player's hand
    [SerializeField] 
    private ItemSlotData equippedToolSlot = null;

    [Header("Items")]
    //Item Slots
    [SerializeField] 
    private ItemSlotData[] itemSlots = new ItemSlotData[8];
    //Item in the player's hand
    [SerializeField] 
    private ItemSlotData equippedItemSlot = null;

    //The transform for the player to hold items in the scene
    public Transform handPoint;

    [SerializeField] 
    private GameObject triggerConfirm;

    [SerializeField] 
    private GameObject joyStick;

    [SerializeField] 
    private GameObject bag;

    public void LoadInventory(ItemSlotData[] toolSlots, ItemSlotData equippedToolSlot, ItemSlotData[] itemSlots, ItemSlotData equippedItemSlot)
    {
        this.toolSlots = toolSlots;
        this.equippedToolSlot = equippedToolSlot;
        this.itemSlots = itemSlots;
        this.equippedItemSlot = equippedItemSlot;

        UIManager.Instance.RenderInventory();
        RenderHand();
    }

    //Equipping


    //Handles movement of item from Inventory to Hand
    public void InventoryToHand(int slotIndex, InventorySlot.InventoryType inventoryType)
    {
        //The slot to equip (Tool by default)
        ItemSlotData handToEquip = equippedToolSlot;
        //The array to change
        ItemSlotData[] inventoryToAlter = toolSlots;

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            handToEquip = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        if (handToEquip.Stackable(inventoryToAlter[slotIndex]))
        {
            ItemSlotData slotToAlter = inventoryToAlter[slotIndex];

            //Add to the hand slot 
            handToEquip.AddQuantity(slotToAlter.quantity);
            //Empty the inventory slot
            slotToAlter.Empty();

        } 
        else
        {
            //Not stackable
            //Cache the Inventory itemSlotData
            ItemSlotData slotToEquip = new ItemSlotData(inventoryToAlter[slotIndex]);

            //Change the inventory slot to hands
            inventoryToAlter[slotIndex] = new ItemSlotData(handToEquip);

            if (slotToEquip.IsEmpty())
            {
                handToEquip.Empty();
            } else
            {
                EquipHandSlot(slotToEquip);
            }
        }

        //Update the changes in the scene
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            RenderHand();
        }

        //Update the changes to the UI
        UIManager.Instance.RenderInventory();
    }

    //Handles movement of item from Hand to Inventory
    public void HandToInventory(InventorySlot.InventoryType inventoryType)
    {
        //The slot to move from (Tool by deafault) 
        ItemSlotData handSlot = equippedToolSlot;
        //The array to change
        ItemSlotData[] inventoryToAlter = toolSlots;

        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            handSlot = equippedItemSlot;
            inventoryToAlter = itemSlots;
        }

        //Try stacking the hand slot
        //Check if the operation failed
        if (!StackItemToInventory(handSlot, inventoryToAlter))
        {
            //Find an empty slot to put the item in
            //Interate through each inventory slot and find an empty slot
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    //Send the equipped item over to its new slot
                    inventoryToAlter[i] = new ItemSlotData(handSlot);
                    //Remove the item from hand
                    handSlot.Empty();
                    break;
                }
            }
        }

        //Update the changes in the scene
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            RenderHand();
        }
        //Update the changes to the UI
        UIManager.Instance.RenderInventory();
    }

    //Iterate through each of the items in the inventory to see if it can be stacked
    //Will perform the operation if found, returns false if unsuccessful
    public bool StackItemToInventory(ItemSlotData itemSlot, ItemSlotData[] inventoryArray)
    {
        for (int i = 0; i < inventoryArray.Length; i++)
        {
            if (inventoryArray[i].Stackable(itemSlot))
            {
                //Add to the inventory slot's stack
                inventoryArray[i].AddQuantity(itemSlot.quantity);
                //Empty the item slot
                itemSlot.Empty();
                return true;
            }
        }
        //Can't find any slot that can be s
        return false;
    }

    public void ShopToInventory(ItemSlotData itemSlotToMove)
    {
        ItemSlotData[] inventoryToAlter = IsTool(itemSlotToMove.itemData) ? toolSlots : itemSlots;


        //Try stacking the hand slot
        //Check if the operation failed
        if (!StackItemToInventory(itemSlotToMove, inventoryToAlter))
        {
            //Find an empty slot to put the item in
            //Interate through each inventory slot and find an empty slot
            for (int i = 0; i < inventoryToAlter.Length; i++)
            {
                if (inventoryToAlter[i].IsEmpty())
                {
                    //Send the equipped item over to its new slot
                    inventoryToAlter[i] = new ItemSlotData(itemSlotToMove);
                    break;
                }
            }
        }
        //Update the changes to the UI
        UIManager.Instance.RenderInventory();
        RenderHand();
    }

    //Render changes in the inventory
    public void RenderHand()
    {
        if (handPoint.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
        }

        if (SlotEquipped(InventorySlot.InventoryType.Item))
        {
            //Instantiate the game model on the player's hand and put it on the scene 22.357f
            Instantiate(GetEquippedSlotItem(InventorySlot.InventoryType.Item).gameModel, new Vector3(transform.position.x, 1000.357f, transform.position.z), quaternion.identity);
        }

    }

    //Inventory Slot Data   
    #region Get and Checks
    //Get the slot item (ItemData)
    public ItemData GetEquippedSlotItem(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot.itemData;

        }
        return equippedToolSlot.itemData;
    }

    //Get function for the slots (ItemSlotData) 
    public ItemSlotData GetEquippedSlot(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return equippedItemSlot;
        }
        return equippedToolSlot;
    }

    //Get function for the inventory slots
    public ItemSlotData[] GetInventorySlots(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return itemSlots;
        }
        return toolSlots;
    }


    //Check if a hand slot has an item
    public bool SlotEquipped(InventorySlot.InventoryType inventoryType)
    {
        if (inventoryType == InventorySlot.InventoryType.Item)
        {
            return !equippedItemSlot.IsEmpty();
        }
        return !equippedToolSlot.IsEmpty();
    }

    public bool IsTool(ItemData item)
    {
        //Is it equipment?
        //Try to cast it as equipment first
        EquipmentData equipment = item as EquipmentData;
        if(equipment != null)
        {
            return true;
        }

        //Is it a seed?
        //Try to cast it as a seed 
        SeedData seed = item as SeedData;
        //If the seed is not null it is a seed
        return seed != null;
    }

    #endregion

    //Equip the hand slot with an ItemData (Will overwrite the slot)
    public void EquipHandSlot(ItemData item)
    {
       if (IsTool(item))
       {
            equippedToolSlot = new ItemSlotData(item);
       } 
       else
       {
            equippedItemSlot = new ItemSlotData(item);
       }
    }

    public void EquipHandSlot(ItemSlotData itemSlot)
    {
        //Get the item data from the slot
        ItemData item = itemSlot.itemData;

        if (IsTool(item))
        {
            equippedToolSlot = new ItemSlotData(itemSlot);
        }
        else
        {
            equippedItemSlot = new ItemSlotData(itemSlot);
        }

    }

    public void ConsumeItem(ItemSlotData itemSlot)
    {
        if(itemSlot.IsEmpty())
        {
            Debug.LogError("There is nothing to consume!");
            return;
        }

        //Use up one of the item slots
        itemSlot.Remove();
        //Refresh inventory
        RenderHand();
        UIManager.Instance.RenderInventory();
    }

    public void ConsumeItemHandleQuantity(ItemSlotData itemSlot)
    {
        if(itemSlot.IsEmpty())
        {
            Debug.LogError("There is nothing to consume!");
            return;
        }

        for(int i = 0; i < 5; i++) 
        {
            //Use up one of the item slots
             itemSlot.Remove();
        }
        //Refresh inventory
        RenderHand();
        UIManager.Instance.RenderInventory();
    }

    #region Inventory Slot Validation
    private void OnValidate()
    {
        //Validate the hand slots
        ValidateInventorySlot(equippedToolSlot);
        ValidateInventorySlot(equippedItemSlot);

        //Validate the slots in the 
        ValidateInventorySlots(itemSlots);
        ValidateInventorySlots(toolSlots);
    }


    //When giving the itemData value in the inspector, automatically set the quantity to 1
    void ValidateInventorySlot(ItemSlotData slot)
    {
        if (slot.itemData != null && slot.quantity == 0)
        {
            slot.quantity = 1;
        }
    }

    //Validate arrays 
    void ValidateInventorySlots(ItemSlotData[] array)
    {
        foreach (ItemSlotData slot in array)
        {
            ValidateInventorySlot(slot);
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void CheckQuantityWater()
    {
         ItemData item = InventoryManager.Instance.itemIndex.GetItemFromString("Bình tưới nước");
         string text = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "QuantityWaterKey");
         string textSuccess = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "AddQuantityWater");
         foreach(ItemSlotData itemSlotData in toolSlots)
         {
            if(itemSlotData.itemData == item)
            {
                if(itemSlotData.quantity > 20) 
                {
                    triggerConfirm.SetActive(false);
                    UIManager.Instance.TriggerConfirm(text);
                    ConsumeItemHandleQuantity(itemSlotData);
                    joyStick.SetActive(true);
                    bag.SetActive(true);
                }
                if(equippedToolSlot != null) 
                {
                    if((equippedToolSlot.quantity + itemSlotData.quantity) > 20)
                    {
                        triggerConfirm.SetActive(false);
                        UIManager.Instance.TriggerConfirm(text);
                        ConsumeItemHandleQuantity(itemSlotData);
                        joyStick.SetActive(true);
                        bag.SetActive(true);
                    }
                }
                else if(itemSlotData.quantity < 20)
                {
                    triggerConfirm.SetActive(false);
                    UIManager.Instance.TriggerConfirm(textSuccess);
                    joyStick.SetActive(true);
                    bag.SetActive(true);
                }
            }
         }
    }
}
