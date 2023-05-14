using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class GetWater : MonoBehaviour
{
    [SerializeField]
    private ItemData water;
    private GameObject canvasJoyStick;
    private GameObject inventoryButton;

    // Start is called before the first frame update
    void Start()
    {
        canvasJoyStick = GameObject.FindWithTag("JoyStick");
        inventoryButton = GameObject.FindWithTag("InventoryButton");
    }

    public void PickUp()
    {
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "GetWaterKey");
        UIManager.Instance.TriggerYesNoPromptCustom(text, GetWaterHandle);
    }

    private void OnTriggerEnter(Collider other)
    {
        PickUp();
    }

    public void GetWaterHandle() 
    {
        // canvasJoyStick.SetActive(false);
        // inventoryButton.SetActive(false);
        ItemSlotData itemWater = new ItemSlotData(water, 5);
        InventoryManager.Instance.ShopToInventory(itemWater);
        InventoryManager.Instance.CheckQuantityWater();
    }

    


}
