using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;


public class UIManager : MonoBehaviour, ITimeTracker
{
    public static UIManager Instance { get; private set; }

    [Header("Screen Management")]
    public GameObject menuScreen;

    //Check if the screen has finished fading out
    bool screenFadeOut;
    public enum Tab
    {
        Inventory, Relationships
    }
    public Tab selectedTab;

    [Header("Status Bar")]
    public Image toolEquipSlot;

    //Tool Quantity text on the status bar
    public Text toolQuantityText;

    public Text timeText;
    public Text dateText;

    [Header("Inventory System")]
    public GameObject inventoryPanel;


    public HandInventorySlot toolHandSlot;

    public InventorySlot[] toolSlots;

    public HandInventorySlot itemHandSlot;

    public SellItem itemSell;
    public InventorySlot[] itemSellSlots;

    public InventorySlot[] itemSlots;

    [Header("Item info box")]
    public GameObject itemInfoBox;
    public Text itemNameText;
    public Text itemDescriptionText;

    [Header("Screen Transitions")]
    public GameObject fadeIn;
    public GameObject fadeOut;

    [Header("Yes No Prompt")]
    public YesNoPrompt yesNoPrompt;
    public YesNoPromptCustom yesNoPromptCustom;
    public InfomationConfirm confirm;

    [Header("Player Stats")]
    public Text moneyText;

    [Header("Shop")]
    public ShopListingManager shopListingManager;

    [Header("Relationships")]
    public RelationshipListingManager relationshipListingManager;

    [Header("Exit")]
    public GameObject exitButton;

    [Header("OptionBuyOrSell")]
    public GameObject option;

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

    // Start is called before the first frame update
    private void Start()
    {
        RenderInventory();
        AssignIndexes();
        RenderPlayerStats();
        DisplayItemInfo(null);

        TimeManager.Instance.RegisterTracker(this);
    }

    public void SaveData()
    {
        // Debug.Log(SceneTransitionManager.Instance.currentLocation);
        // GameTimestamp timestamp = TimeManager.Instance.GetGameTimestamp();
        // GameStateManager.Instance.ClockUpdate(timestamp);
        // TimeManager.Instance.Tick();
        // TimeManager.Instance.UpdateTime();
        // SaveManager.Save(GameStateManager.Instance.ExportSaveState());
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "SaveTrueKey");
        if(SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.Farm) 
        {
            GameStateManager.Instance.SaveGame();
            TriggerConfirm(text);
        }
        else 
        {
            LandManager.Instance.SaveLandAndCropData();
            GameStateManager.Instance.SaveGame();
            TriggerConfirm(text);
        }

    }


        /*SaveManager.Save(GameStateManager.Instance.ExportSaveState());*/


        public void TriggerYesNoPrompt(string message, System.Action onYesCallback)
    {
        //Set active the gameObject of the Yes No Prompt
        yesNoPrompt.gameObject.SetActive(true);

        yesNoPrompt.CreatePrompt(message, onYesCallback);

        StartCoroutine(DisableAfterDelay(5.0f));
    }

    public void TriggerYesNoPromptCustom(string message, System.Action onYesCallback)
    {
        //Set active the gameObject of the Yes No Prompt
        yesNoPromptCustom.gameObject.SetActive(true);

        yesNoPromptCustom.CreatePrompt(message, onYesCallback);

        StartCoroutine(DisableAfterDelay(5.0f));
    }

    public void TriggerConfirm(string message)
    {
        //Set active the gameObject of the Yes No Prompt
        confirm.gameObject.SetActive(true);

        confirm.CreatePrompt(message);

        StartCoroutine(DisableAfterDelay(5.0f));
    }

     IEnumerator DisableAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        confirm.gameObject.SetActive(false);
        yesNoPromptCustom.gameObject.SetActive(false);
        yesNoPrompt.gameObject.SetActive(false);
    }

    #region Tab Management
    public void ToggleMenuPanel()
    {
        menuScreen.SetActive(!menuScreen.activeSelf);

        OpenWindow(selectedTab);

        TabBehaviour.onTabStateChange?.Invoke();
    }

    public void OpenWindow(Tab windowToOpen)
    {
        relationshipListingManager.gameObject.SetActive(false);
        inventoryPanel.SetActive(false);

        switch(windowToOpen)
        {
            case Tab.Inventory:
                inventoryPanel.SetActive(true);
                RenderInventory();
                break;
            case Tab.Relationships:
                relationshipListingManager.gameObject.SetActive(true);
                relationshipListingManager.Render(RelationshipStats.relationships);
                break;
        }

        selectedTab = windowToOpen;
    }
    #endregion

    #region Fadein Fadeout Transitions

    public void FadeOutScreen()
    {
        fadeOut.SetActive(true);
    }

    public void FadeInScreen()
    {
        fadeIn.SetActive(true);
    }

    public void OnFadeInComplete()
    {
        //Disable Fade in Screen when animation is completed
        fadeIn.SetActive(false);
    }

    //Reset the fadein fadeout screens to their default positions
    public void ResetFadeDeafaults()
    {  
        fadeOut.SetActive(false);
        fadeIn.SetActive(false);
    }

    #endregion

    #region Inventory
    //Iterate through the slot UI elements and assign it is reference slot index
    public void AssignIndexes()
    {
        for (int i = 0; i < toolSlots.Length; i++)
        {
            toolSlots[i].AssignIndex(i);
            itemSlots[i].AssignIndex(i);
            itemSellSlots[i].AssignIndex(i);
        }
    }


    public void RenderInventory()
    {
        //Get the respective slots to process
        ItemSlotData[] inventoryToolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] inventoryItemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);


        RenderInventoryPanel(inventoryToolSlots, toolSlots);

        RenderInventoryPanel(inventoryItemSlots, itemSlots);
        RenderInventoryPanelSell(inventoryItemSlots, itemSellSlots);

        toolHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        itemHandSlot.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));

        itemSell.Display(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item));

        ItemData equippedTool = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        //Text should be empty by deafault
        toolQuantityText.text = "";

        if (equippedTool != null)
        {
            //Switch the thumbnail over
            toolEquipSlot.sprite = equippedTool.thumbnail;

            toolEquipSlot.gameObject.SetActive(true);

            //Get quantity
            int quantity = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool).quantity;
            if (quantity > 1)
            {
                toolQuantityText.text = quantity.ToString();
            }

            return;
        }
        toolEquipSlot.gameObject.SetActive(false);
    }

    public void RenderInventoryPanel(ItemSlotData[] slots, InventorySlot[] uiSlots)
    {
        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
        }
    }

    public void RenderInventoryPanelSell(ItemSlotData[] slots, InventorySlot[] uiSlots)
    {
        uiSlots = itemSellSlots;

        for (int i = 0; i < uiSlots.Length; i++)
        {
            uiSlots[i].Display(slots[i]);
        }
    }

    public void ToogleInventoryPanel()
    {
        //If the panel is hidden, show it and vice versa
        inventoryPanel.SetActive(!inventoryPanel.activeSelf);

        RenderInventory();
    }

    public void DisplayItemInfo(ItemData data)
    {
        if (data == null)
        {
            itemNameText.text = "";
            itemDescriptionText.text = "";
            itemInfoBox.SetActive(false);
            return;
        }
        itemInfoBox.SetActive(true);
        itemNameText.text = data.name;
        itemDescriptionText.text = data.description;

    }
    #endregion

    #region Time
    public void ClockUpdate(GameTimestamp timestamp)
    {
        int hours = timestamp.hour;
        int minutes = timestamp.minute;

        string prefix = " AM";

        if(hours > 12)
        {
            prefix = " PM";

            hours -= 12;
        }

        timeText.text = hours + ":" + minutes.ToString("00") + prefix;

        int day = timestamp.day;
        string season = timestamp.season.ToString();
        string dayOfTheWeek = timestamp.GetDayOfTheWeek().ToString();

        dateText.text = season + " " + day + " (" + dayOfTheWeek + ")";

    }
    #endregion

    public void RenderPlayerStats()
    {
        moneyText.text = PlayerStats.money + PlayerStats.CURRENCY;
    }

    public void OpenShop(List<ItemData> shopItems)
    {
        exitButton.SetActive(true);
        shopListingManager.gameObject.SetActive(true);
        shopListingManager.Render(shopItems);
    }

    public void OpenOption()
    {
        exitButton.SetActive(true);
        option.SetActive(true);
    }

    public void ToggleRelationshipPanel()
    {
        GameObject panel = relationshipListingManager.gameObject;
        panel.SetActive(!panel.activeSelf);

        if(panel.activeSelf)
        {
            relationshipListingManager.Render(RelationshipStats.relationships);
        }
    }

    public void OpenBuyOption()
    {
        Shop.Instance.OpenShop();
    }
}
