using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class YesNoPrompt : MonoBehaviour
{
    [SerializeField]
    Text promptText;
    Action onYesSelected;
    [SerializeField]
    private GameObject panel;

    public void CreatePrompt(string message, Action onYesSelected)
    {
        this.onYesSelected = onYesSelected;

        promptText.text = message;
    }

    public void Answer(bool yes)
    {
        if (yes && onYesSelected != null)
        {
            onYesSelected();
        }

        onYesSelected = null;

        gameObject.SetActive(false);
    }
    public void Yes()
    {
        GameStateManager.Instance.Sleep();
        panel.SetActive(false);
    }
    public void No()
    {
        panel.SetActive(false);
    }

}
