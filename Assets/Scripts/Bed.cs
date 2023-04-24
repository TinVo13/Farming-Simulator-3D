using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class Bed : MonoBehaviour
{
    [SerializeField]
    private GameObject panel;
    private void OnTriggerEnter(Collider other)
    {
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "sleepKey");
        UIManager.Instance.TriggerYesNoPrompt(text, GameStateManager.Instance.Sleep);
    }
    private void OnTriggerExit(Collider other)
    {
        panel.SetActive(false);
    }

    public void TestSave() {
         string text = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "sleepKey");
        UIManager.Instance.TriggerYesNoPrompt(text, GameStateManager.Instance.Sleep);
    }
}
