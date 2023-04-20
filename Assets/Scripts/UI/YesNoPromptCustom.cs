using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class YesNoPromptCustom : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI promptText;
    Action onYesSelected;

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
}
