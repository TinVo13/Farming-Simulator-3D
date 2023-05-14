using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerInfomation : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI textPlayerName;
    [SerializeField]
    private GameObject male;
    [SerializeField]
    private GameObject maleInfomation;
    [SerializeField]
    private GameObject femaleInfomation;

    // Start is called before the first frame update
    void Start()
    {
        if(male.activeSelf)
        {
           maleInfomation.SetActive(true);
           femaleInfomation.SetActive(false);
           string playerName = PlayerPrefs.GetString("PlayerName");
           textPlayerName.text = playerName; 
        }
        else 
        {
           femaleInfomation.SetActive(true);
           maleInfomation.SetActive(false);
           string playerName = PlayerPrefs.GetString("PlayerName");
           textPlayerName.text = playerName; 
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
