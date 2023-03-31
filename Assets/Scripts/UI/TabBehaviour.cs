using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TabBehaviour : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    public Sprite defaultSprite, selected, hover;
    Image tabImage;
    public UIManager.Tab windowToOpen;

    public static UnityEvent onTabStateChange = new UnityEvent();

    private void Awake()
    {
        tabImage = GetComponent<Image>();

        onTabStateChange.AddListener(RenderTabState);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        onTabStateChange?.Invoke();
        tabImage.sprite = selected;
        UIManager.Instance.OpenWindow(windowToOpen);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        onTabStateChange?.Invoke();
        tabImage.sprite = hover;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        onTabStateChange?.Invoke();
    }

    void RenderTabState()
    {
        if(UIManager.Instance.selectedTab == windowToOpen)
        {
            tabImage.sprite = selected;
            return;
        }
        tabImage.sprite = defaultSprite;
    }
}
