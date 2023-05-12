using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEditor;
using UnityEngine.UI;

public class Land : MonoBehaviour, ITimeTracker
{
    public int id;

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

    //Obstacles
    public enum FarmObstacleStatus { None, Rock, Wood, Weeds }
    [Header("Obstacles")]
    public FarmObstacleStatus obstacleStatus;
    public GameObject rockPrefab, woodPrefab, weedsPrefab;

    GameObject obstacleObject;

    private string pathToPrefab = "Assets/Prefabs/Essentials.prefab";

    // Start is called before the first frame update
    void Start()
    {
        renderer = GetComponent<Renderer>();

        // Material materialToSwitch = soilMat;
        // //Get the renderer to apply the changes
        // renderer.material = materialToSwitch;

        //Set the land to soil by default

        //Deselect the land by default
        Select(false);

        //Add this to TimeManager's Listener list
        TimeManager.Instance.RegisterTracker(this);
    }

    public void LoadLandData(LandStatus landstatusToSwitch, GameTimestamp lastWatered, FarmObstacleStatus obstacleStatusToSwitch)
    {
        //Set land status accordingly
        landStatus = landstatusToSwitch;
        timeWatered = lastWatered;

        Material materialToSwitch = soilMat;

        //Decide what material to switch to
        switch (landstatusToSwitch)
        {
            case LandStatus.Soil:
                //Switch to the soil material
                materialToSwitch = soilMat;
                break;
            case LandStatus.Farmland:
                //Switch to farmland material 
                materialToSwitch = farmlandMat;
                break;

            case LandStatus.Watered:
                //Switch to watered material
                materialToSwitch = wateredMat;
                break;

        }

        //Get the renderer to apply the changes
        renderer.material = materialToSwitch;

        switch (obstacleStatusToSwitch)
        {
            case FarmObstacleStatus.None:
                if (obstacleObject != null) Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case FarmObstacleStatus.Weeds:
                obstacleObject = Instantiate(weedsPrefab, transform);
                break;
        }

        if (obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x, -0.12f, transform.position.z);

        obstacleStatus = obstacleStatusToSwitch;


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
      
        LandManager.Instance.OnLandStateChange(id, landStatus, timeWatered, obstacleStatus);

    }

    public void SetObstacleStatus(FarmObstacleStatus statusToSwitch)
    {
        switch (statusToSwitch)
        {
            case FarmObstacleStatus.None:
                if (obstacleObject != null) Destroy(obstacleObject);
                break;
            case FarmObstacleStatus.Rock:
                obstacleObject = Instantiate(rockPrefab, transform);
                break;
            case FarmObstacleStatus.Wood:
                obstacleObject = Instantiate(woodPrefab, transform);
                break;
            case FarmObstacleStatus.Weeds:
                obstacleObject = Instantiate(weedsPrefab, transform);
                break;
        }

        if (obstacleObject != null) obstacleObject.transform.position = new Vector3(transform.position.x, -0.12f, transform.position.z);

        obstacleStatus = statusToSwitch;

        LandManager.Instance.OnLandStateChange(id, landStatus, timeWatered, obstacleStatus);
    }

    public void Select(bool toogle)
    {
        select.SetActive(toogle);
    }

    //when the player press the interact button while selecting this land
    public void Interact()
    {
        GameObject rootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(pathToPrefab);
        GameObject male = rootPrefab.transform.Find("Player").gameObject;

        //Check the player's tool slot
        ItemData toolSlot = InventoryManager.Instance.GetEquippedSlotItem(InventorySlot.InventoryType.Tool);

        //if there's nothing equipped, return
        if (!InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Tool)) 
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
                    if(male.activeSelf) 
                    {
                        UIManager.Instance.Dig();
                    }
                    else
                    {
                        UIManager.Instance.DigFemale();
                    }
                    SwitchLandStatus(LandStatus.Farmland);
                    break;
                case EquipmentData.ToolType.WateringCan:
                    if (landStatus != LandStatus.Soil)
                    {
                        if(male.activeSelf) 
                        {
                            UIManager.Instance.WateringCan();
                        }
                        else
                        {
                            UIManager.Instance.WateringCanFemale();
                        }
                        SwitchLandStatus(LandStatus.Watered);
                    }
                    break;
                case EquipmentData.ToolType.Shovel:
                    //Remove the crop from the land
                    if(cropPlanted != null)
                    {
                        /*Destroy(cropPlanted.gameObject);*/
                        if(male.activeSelf) 
                        {
                            UIManager.Instance.Shovel();
                        }
                        else
                        {
                            UIManager.Instance.ShovelFemale();
                        }
                        cropPlanted.RemoveCrop();
                    }

                    if (obstacleStatus == FarmObstacleStatus.Weeds) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
                case EquipmentData.ToolType.Axe:
                    if (obstacleStatus == FarmObstacleStatus.Wood) SetObstacleStatus(FarmObstacleStatus.None);
                    break;
                case EquipmentData.ToolType.Pickaxe:
                    if (obstacleStatus == FarmObstacleStatus.Rock) SetObstacleStatus(FarmObstacleStatus.None);
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
        ///4: There are no obstacles
        if (seedTool != null && landStatus != LandStatus.Soil && cropPlanted == null && obstacleStatus == FarmObstacleStatus.None) 
        {
            /*  //Instantiate the crop object parented to the land 
              GameObject cropObject = Instantiate(cropPrefab, transform);
              //Move the crop object to the top of the land gameobject 
              *//*cropObject.transform.position = new Vector3(transform.position.x, 22.357f, transform.position.z);*//*
              cropObject.transform.position = new Vector3(transform.position.x, -0.11f, transform.position.z);
              //Access the CropBehaviour of the crop we're going to plant
              cropPlanted = cropObject.GetComponent<CropBehaviour>();*/
            SpawnCrop();
            //Plant it with the seed's inform
            cropPlanted.Plant(id, seedTool);

            //Comsume the item
            InventoryManager.Instance.ConsumeItem(InventoryManager.Instance.GetEquippedSlot(InventorySlot.InventoryType.Tool));
        }
    }

    public CropBehaviour SpawnCrop()
    {
        GameObject rootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(pathToPrefab);
        GameObject male = rootPrefab.transform.Find("Player").gameObject;

        if(male.activeSelf) 
        {
            UIManager.Instance.SeedSack();
        }
        else
        {
            UIManager.Instance.SeedSackFemale();
        }

        //Instantiate the crop object parented to the land
        GameObject cropObject = Instantiate(cropPrefab, transform);
        //Move the crop object to the top of the land gameobject
        cropObject.transform.position = new Vector3(transform.position.x, -0.12f, transform.position.z);

        //Access the CropBehaviour of the crop we're going to plant
        cropPlanted = cropObject.GetComponent<CropBehaviour>();
        return cropPlanted;
    }

    public void ClockUpdate(GameTimestamp timestamp)
    {
        //Checked if 1 hours has passed since last watered
        if (landStatus == LandStatus.Watered)
        {
            //Hours since the land was watered 
            int hoursElapsed = GameTimestamp.CompareTimestamps(timeWatered, timestamp);
            Debug.Log(hoursElapsed + " hour a since this was watered");

            //Grow the planted crop, if any
            if (cropPlanted != null)
            {
                cropPlanted.Grow();
            }

            /*if(hoursElapsed > 24)
            {
                SwitchLandStatus(LandStatus.Farmland);
            }*/
            if (hoursElapsed > 1)
            {
                //Dry up (Switch back to farmland)
                SwitchLandStatus(LandStatus.Farmland);
                
            }
        }
        //Handle the wilting of the plant when land is not watered
        if (landStatus != LandStatus.Watered && cropPlanted != null)
        {
            if (cropPlanted.cropState != CropBehaviour.CropState.Seed)
            {
                cropPlanted.Wilther();
            }
        }
    }


    private void OnDestroy()
    {
        //Unsubscribe from the list on destroy
        TimeManager.Instance.UnregisterTracker(this); 
    }

}
