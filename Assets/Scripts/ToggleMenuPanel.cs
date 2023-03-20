using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleMenuPanel : MonoBehaviour
{
    [SerializeField]
    private GameObject Panel;
    public void OpenPanel()
    {
        bool active = Panel.activeSelf;
        Panel.SetActive(!active);
    }
}
