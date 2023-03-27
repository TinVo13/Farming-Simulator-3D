using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class ShopListing : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public Image itemThumbnail;
    public Text nameText;
    public Text costText;

    ItemData itemData;

    public void Display(ItemData itemData)
    {
        this.itemData = itemData;
        itemThumbnail.sprite = itemData.thumbnail;
        nameText.text = itemData.name;
        costText.text = itemData.cost + PlayerStats.CURRENCY;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        UIManager.Instance.shopListingManager.OpenConfirmationScreen(itemData);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(itemData);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UIManager.Instance.DisplayItemInfo(null);
    }
}
