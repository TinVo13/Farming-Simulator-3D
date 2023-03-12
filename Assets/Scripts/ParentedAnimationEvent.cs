using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParentedAnimationEvent : MonoBehaviour
{
    //Send the message upwards
    public void NotifyAncestors(string message)
    {
        SendMessageUpwards(message);
    }
}
