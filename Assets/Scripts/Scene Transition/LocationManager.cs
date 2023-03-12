using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocationManager : MonoBehaviour
{
    public static LocationManager Instance
    {
        get; private set;
    }

    public List<StartPoint> startPoints;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }

    }

    //Find the player's start position based on where he's coming from
    public Transform GetPlayerStartingPosition(SceneTransitionManager.Location enteringFrom)
    {
        //Tries to find the matching startpoint based on the Location given
        StartPoint startingPoitn = startPoints.Find(x => x.enteringFrom == enteringFrom);

        //Return the transform
        return startingPoitn.playerStart;
    }
}
