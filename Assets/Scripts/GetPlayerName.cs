using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Settings;
// using UnityEditor;

public class GetPlayerName : MonoBehaviour
{
    [SerializeField]
    public InputField inputField;
    // [SerializeField]
    // private Text textPlayerName;

    [SerializeField]
    private InfomationConfirm confirm;
    [SerializeField]
    private GameObject confirmName;
    [SerializeField]
    private GameObject choosePanel;
    [SerializeField]
    private GameObject customOption;
    private Text textPlayerName;

    // private string pathToPrefabPlayerNamePanel = "Assets/Prefabs/PlayerNamePanel.prefab";
    private string pathToPrefabPlayerNamePanel = "PlayerNamePanel";

    private void Start() 
    {
        // GameObject rootPrefabPlayerName = AssetDatabase.LoadAssetAtPath<GameObject>(pathToPrefabPlayerNamePanel);
        GameObject rootPrefabPlayerName = Resources.Load<GameObject>(pathToPrefabPlayerNamePanel);
        rootPrefabPlayerName.SetActive(true);

        GameObject convertText = rootPrefabPlayerName.transform.Find("Player Name Text").gameObject;

        // Get the Text component from the object
        textPlayerName = convertText.GetComponent<Text>();
    }

    public void GetName()
    {
        string playerName = inputField.text;
        string textRegex = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "RegexNameKey");

        if(playerName == "") 
        {
            InfomationConfirm(textRegex);
            confirmName.SetActive(true);
            choosePanel.SetActive(false);
            customOption.SetActive(false);
        }
        else 
        {
            string textConfirm = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "ConfirmNameKey");
            InfomationConfirm(textConfirm);
            confirmName.SetActive(false);
            choosePanel.SetActive(false);
            customOption.SetActive(true);
            PlayerPrefs.SetString("PlayerName", playerName);
            PlayerPrefs.Save();
            textPlayerName.text = playerName;
        }
    }

    public void InfomationConfirm(string message)
    {
        //Set active the gameObject of the Yes No Prompt
        confirm.gameObject.SetActive(true);

        confirm.CreatePrompt(message);

        StartCoroutine(DisableAfterDelay(10.0f));
    }

    IEnumerator DisableAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        confirm.gameObject.SetActive(false);
    }
}
