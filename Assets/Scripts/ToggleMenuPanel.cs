using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;

public class ToggleMenuPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;
    public void OpenPanel()
    {
        bool active = Panel.activeSelf;
        Panel.SetActive(!active);
    }

    public void Quit()
    {
        string text = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "ExitGameKey");

        UIManager.Instance.TriggerYesNoPromptCustom(text, QuitGameOption);
    }

    void QuitGameOption() 
    {
        Application.Quit();
    }
}
