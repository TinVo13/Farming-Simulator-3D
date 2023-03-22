using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bed : MonoBehaviour
{
    /*   public override void PickUp()
       {
           UIManager.Instance.TriggerYesNoPrompt("Do you want to sleep?", GameStateManager.Instance.Sleep);
       }*/
    private void OnTriggerEnter(Collider other)
    {
        UIManager.Instance.TriggerYesNoPrompt("Do you want to sleep?", GameStateManager.Instance.Sleep);
    }
}
