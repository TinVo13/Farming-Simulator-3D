using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class NPCRelationshipState
{
    public string name;
    public int friendshipPoints;

    public bool hasTalkedToday;
    public bool giftGivenToday;


    public NPCRelationshipState(string name, int friendshipPoints)
    {
        this.name = name;
        this.friendshipPoints = friendshipPoints;
    }

    public NPCRelationshipState(string name)
    {
        this.name = name;
        friendshipPoints = 0;
    }

    public float Hearts()
    {
        return friendshipPoints / 250;
    }
}
