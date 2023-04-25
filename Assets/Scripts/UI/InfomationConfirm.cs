using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InfomationConfirm : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI promptText;

    public void CreatePrompt(string message)
    {
        promptText.text = message;
    }

    public void Answer(bool yes)
    {
        if (yes)
        {
            gameObject.SetActive(false);
            return;
        }

        gameObject.SetActive(false);
    }

}
