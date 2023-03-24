using Palmmedia.ReportGenerator.Core.Parser;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameStateManager : MonoBehaviour, ITimeTracker
{
    public static GameStateManager Instance { get; private set; }

    //Check if the screen has finished fading out
    bool screenFadeOut;

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

    public void Sleep()
    {
        //Call a fadeout
        UIManager.Instance.FadeOutScreen();
        screenFadeOut = false;
        StartCoroutine(TransitionTime());
    }

    IEnumerator TransitionTime()
    {
        //Calculate how many ticks we need to advance the time to 6am

        //Get the time stamp of 6am the next day
        GameTimestamp timestampOfNextDay = TimeManager.Instance.GetGameTimestamp();
        timestampOfNextDay.day += 1;
        timestampOfNextDay.hour = 6;
        timestampOfNextDay.minute = 0;
        Debug.Log(timestampOfNextDay.day + " " + timestampOfNextDay.hour + ":" + timestampOfNextDay.minute);

        
        //Wait for the scene to finish fading out before loading the next scene
        while (!screenFadeOut)
        {
            yield return new WaitForSeconds(1f);
        }
        TimeManager.Instance.SkipTime(timestampOfNextDay);
        //Save game
        SaveManager.Save(ExportSaveState());
        //Reset the boolean
        screenFadeOut = false;
        UIManager.Instance.ResetFadeDeafaults();
    }

    //Call when the screen has faded out
    public void OnFadeOutComplete()
    {
        screenFadeOut = true;
    }

    public GameSaveState ExportSaveState()
    {
        //Retrieve Farm Data
        List<LandSaveState> landData = LandManager.farmData.Item1;
        List<CropSaveState> cropData = LandManager.farmData.Item2;

        //Retrieve Inventory Data
        ItemSlotData[] toolSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Tool);
        ItemSlotData[] itemSlots = InventoryManager.Instance.GetInventorySlots(InventorySlot.InventoryType.Item);

        ItemSlotData equippedItemSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Item);
        ItemSlotData equippedToolSlot = InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool);

        //Time
        GameTimestamp timestamp = TimeManager.Instance.GetGameTimestamp();
        return new GameSaveState(landData, cropData, toolSlots, itemSlots, equippedItemSlot, equippedToolSlot, timestamp);
    }

    public void LoadSave()
    {
        GameSaveState save = SaveManager.Load();

        TimeManager.Instance.LoadTime(save.timestamp);

        ItemSlotData[] toolSlots = ItemSlotData.DeserializeArray(save.toolSlots);
        ItemSlotData equippedToolSlot = ItemSlotData.DeserializeData(save.equippedToolSlot);
        ItemSlotData[] itemSlots = ItemSlotData.DeserializeArray(save.itemSlots); 
        ItemSlotData equippedItemSlot = ItemSlotData.DeserializeData(save.equippedItemSlot);
        InventoryManager.Instance.LoadInventory(toolSlots, equippedToolSlot, itemSlots, equippedItemSlot);

        //Farming data
        LandManager.farmData = new System.Tuple<List<LandSaveState>, List<CropSaveState>>(save.landData, save.cropData);
    }
}
