using Palmmedia.ReportGenerator.Core.Parser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }

    }

    // Start is called before the first frame update
    void Start()
    {
        //Add this to TimeManager's Listener list
        TimeManager.Instance.RegisterTracker(this);
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Update the land and Crop Save states as long as the player is outside of the farm scene
        if (SceneTransitionManager.Instance.currentLocation != SceneTransitionManager.Location.Farm) 
        {
            if (LandManager.farmData == null) return;
            //Retrieve the land and Farm data from the static variable
            List<LandSaveState> landData = LandManager.farmData.Item1;
            List<CropSaveState> cropData = LandManager.farmData.Item2;

            //If there are no crops planted, we don't need to worry abour updating anything
            if (cropData.Count == 0) return;

            for (int i = 0; i < cropData.Count; i++)
            {
                //Get the crop and corresponding land data
                CropSaveState crop = cropData[i];
                LandSaveState land = landData[crop.landID];

                if (crop.cropState == CropBehaviour.CropState.Wilted) continue;

                //Update the Land's state
                land.ClockUpdate(timestamp);
                //Update the crop's state based on the land state
                if(land.landStatus == Land.LandStatus.Watered)
                {
                    crop.Grow();
                } else if(crop.cropState != CropBehaviour.CropState.Seed)
                {
                    crop.Wilther();
                }

                //Update the element in the array
                cropData[i] = crop;
                landData[crop.landID] = land;
            }
            LandManager.farmData.Item2.ForEach((CropSaveState crop) =>
            {
                Debug.Log(crop.seedToGrow + "\n Health: " 
                    + crop.health + "\n Growth: " + crop.growth + "\n State: " 
                    + crop.cropState.ToString());
            });
        }
    }

}
