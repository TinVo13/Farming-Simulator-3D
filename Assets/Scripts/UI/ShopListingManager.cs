using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;

public class ShopListingManager : ListingManager<ItemData>
{


    ItemData itemToBuy;
    int quantity;

    [Header("Confirmation Screen")]
    public GameObject confirmationScreen;
    public Text confirmationPrompt;
    public Text quantityText;
    public Text costCalculationText;
    public Button purchaseButton;

    protected override void DisplayListing(ItemData listingItems, GameObject listingGameObject)
    {
        listingGameObject.GetComponent<ShopListing>().Display(listingItems);
    }

    public void OpenConfirmationScreen(ItemData item)
    {
        itemToBuy = item;
        quantity = 1;
        RenderConfirmationScreen();
    }

    public void RenderConfirmationScreen()
    {
        confirmationScreen.SetActive(true);

        confirmationPrompt.text = $"Mua {itemToBuy.name}?";

        quantityText.text = "x" + quantity;

        // int cost = itemToBuy.cost * quantity;
        int purchasePrice = itemToBuy.purchasePrice * quantity;

        // int playerMoneyLeft = PlayerStats.money - cost;
        int playerMoneyLeft = PlayerStats.money - purchasePrice;

        string text = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "EnoughKey");

        if(playerMoneyLeft < 0)
        {
            // costCalculationText.text = "Insufficient funds.";
            costCalculationText.text = text;
            purchaseButton.interactable = false;
            return;
        }
        purchaseButton.interactable = true;

        costCalculationText.text = $"{PlayerStats.money} > {playerMoneyLeft}";
    }

    public void AddQuantity()
    {
        quantity++;
        RenderConfirmationScreen();
    }
    public void SubtractQuantity()
    {
       if(quantity > 1)
       {
            quantity--;
       }
        RenderConfirmationScreen();
    }

    public void ConfirmPurchase()
    {
        Shop.Purchase(itemToBuy, quantity);
        confirmationScreen.SetActive(false);
    }

    public void CancelPurchase()
    {
        confirmationScreen.SetActive(false);
    }
}
