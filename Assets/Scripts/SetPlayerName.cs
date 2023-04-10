using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SetPlayerName : MonoBehaviour
{
    [SerializeField]
    private Text text;
    private void Start()
    {
        SetName();
    }
    public void SetName()
    {
        string playerName = PlayerPrefs.GetString("PlayerName");
        text.text = playerName;
    }
}
