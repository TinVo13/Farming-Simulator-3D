using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Land : MonoBehaviour, ITimeTracker
{
    public enum LandStatus  
    {
        Soil, Farmland, Watered
    }

    public LandStatus landStatus;



    public Material soilMat, farmlandMat, wateredMat;

    new Renderer renderer;

    //The selection gameobject to enable when player is selecting the land
    public GameObject select;

    //Cache the time the land was watered
    GameTimestamp timeWatered;

    [Header("Crops")]
    //The crop prefab to instantiate
    public GameObject cropPrefab;

    //The crop currently planted on the land
    CropBehaviour cropPlanted = null;

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        SwitchLandStatus(LandStatus.Soil);

        //Deselect the land by default
        Select(false);

        //Add this to TimeManager's Listener list
        TimeManager.Instance.RegisterTracker(this);
    }

    public void SwitchLandStatus(LandStatus statusToSwitch)
    {
        //Set land status accordingly
        landStatus = statusToSwitch;

        Material materialToSwitch = soilMat;

        //Decide what material to switch to
        switch(landStatus)
        {
            case LandStatus.Soil:
                //Switch to the soil material
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                materialToSwitch = farmlandMat;
                break;
            case LandStatus.Watered:
                materialToSwitch = wateredMat;

                //Cache the time it was watered
                timeWatered = TimeManager.Instance.GetGameTimestamp();
                break;
        }

        //Get the renderer to apply the changes
        renderer.material = materialToSwitch;
    }

    public void Select(bool toogle)
    {
        select.SetActive(toogle);  
    }

    //when the player press the interact button while selecting this land
    public void Interact()
    {
        //Check the player's tool slot
        ItemData toolSlot = InventoryManager.Instance.equippedTool;

        //if there's nothing equipped, return
        if (toolSlot == null)
        {
            return;
        }

        //Try casting the itemdata in the toolslot as EquipmentData
        EquipmentData equipmentTool = toolSlot as EquipmentData;

        //Check if it is of type EquipmentData
        if (equipmentTool != null)
        {
            //Get the tool type
            EquipmentData.ToolType toolType = equipmentTool.toolType;

            switch (toolType)
            {
             
                case EquipmentData.ToolType.Hoe:
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    SwitchLandStatus(LandStatus.Watered);
                    break;
            }

            //we don't need to check for seeds if we have already confirmed the tool to be an equipment
            return;

        }

        //Try casting the itemdata in the toolslot as SeedData
        SeedData seedTool = toolSlot as SeedData;

        ///Conditions for the player to be able to plant a seed
        ///1: He is holding a tool of type SeedData
        ///2: The land State must be either watered of farmland
        ///3: There isn't already a crop that has been planted 
        if (seedTool != null && landStatus != LandStatus.Soil && cropPlanted == null)
        {
            //Instantiate the crop object parented to the land 
            GameObject cropObject = Instantiate(cropPrefab, transform);
            //Move the crop object to the top of the land gameobject 
            cropObject.transform.position = new Vector3(transform.position.x, 22.357f, transform.position.z);

            //Access the CropBehaviour of the crop we're going to plant
            cropPlanted = cropObject.GetComponent<CropBehaviour>();
            //Plant it with the seed's inform
            cropPlanted.Plant(seedTool);
        }
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        if (landStatus == LandStatus.Watered)
        {
            int hoursElapsed = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log(hoursElapsed + " houra since this was watered");

            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }

            if(hoursElapsed > 24)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }
        }
    }
}
