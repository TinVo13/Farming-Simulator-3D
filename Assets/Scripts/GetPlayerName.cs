using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetPlayerName : MonoBehaviour
{
    [SerializeField]
    private InputField inputField;
    [SerializeField]
    private Text textPlayerName;
    public void GetName()
    {
        string playerName = inputField.text;
        PlayerPrefs.SetString("PlayerName", playerName);
        PlayerPrefs.Save();
        textPlayerName.text = playerName;
    }
}
