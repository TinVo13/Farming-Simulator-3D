using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Bed : MonoBehaviour
{
    // [SerializeField]
    // private GameObject panel;
    private void OnTriggerEnter(Collider other)
    {
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "sleepKey");
        GameTimestamp timestampNow = TimeManager.Instance.GetGameTimestamp();
        string textSaveFalse = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "SaveFalseKey");
        if(timestampNow.hour < 18) 
        {
           UIManager.Instance.TriggerConfirm(textSaveFalse);
        }
        else
        {
            UIManager.Instance.TriggerYesNoPrompt(text, GameStateManager.Instance.Sleep);
        }

    }
    // private void OnTriggerExit(Collider other)
    // {
    //     panel.SetActive(false);
    // }

}
