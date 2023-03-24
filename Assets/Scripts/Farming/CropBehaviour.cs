using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CropBehaviour : MonoBehaviour
{
    //The ID of the land the crop belongs to
    int landID;

    //Infomation on what the crop will grow into
    SeedData seedToGrow;

    PlayerInteraction playerInteraction;

    [Header("Stages of life")]
    public GameObject seed;
    public GameObject wilted;
    private GameObject seedling;
    private GameObject harvestable;

    public void Start()
    {
        playerInteraction = GetComponent<PlayerInteraction>();
    }

    //The growth points of the crop
    int growth;
    //How many growth points it takes before it becomes harvestable 
    int maxGrowth;

    int health;
    //The crop can stay alive for 2 hours without water before it dies
    int maxHealth = GameTimestamp.HoursToMinutes(2);


    public enum CropState
    {
        Seed, Seedling, Harvestable, Wilted
    }

    //The current stage in the crop's growth
    public CropState cropState;

    //Initialisation for the crop GameObject
    //Called when the player plants a seed
    public void Plant(int landID, SeedData seedToGrow)
    {
        /*   //Save the seed infomation
           this.seedToGrow = seedToGrow;

           //Instantiate the seedling and harvestable GameObjects
           seedling = Instantiate(seedToGrow.seedling, transform);

           //Access the crop item data
           ItemData cropToYield = seedToGrow.cropToYield;

           //Instantiate the harvestable crop
           harvestable = Instantiate(cropToYield.gameModel, transform);

           //Convert Days To Grow into hours 
           *//* int hoursToGrow = GameTimestamp.DaysToHours(seedToGrow.daysToGrow);*/
        /*maxGrowth = GameTimestamp.HoursToMinutes(hoursToGrow);*//*
        int minutesToGrow = GameTimestamp.HoursToMinutes(seedToGrow.hoursToGrow);
        //Convert it to minutes


        maxGrowth = minutesToGrow;

        //Check if it is regrowable
        if (seedToGrow.regrowable)
        {
            //Get the RegrowableHarvestBehaviour from the GameObject
            RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();

            //Initialise the harvestable
            regrowableHarvest.SetParrent(this);
        }

        //Set the initial state to seed
        SwitchState(CropState.Seed);*/
        LoadCrop(landID, seedToGrow, CropState.Seed, 0, 0);
        LandManager.Instance.RegisterCrop(landID, seedToGrow, cropState, growth, health);
    }

     public void LoadCrop(int landID, SeedData seedToGrow, CropState cropState, int growth, int health)
    {
        this.landID = landID;
        //Save the seed information
        this.seedToGrow = seedToGrow;

        //Instantiate the seedling and harvestable GameObjects
        seedling = Instantiate(seedToGrow.seedling, transform);

        //Access the crop item data
        ItemData cropToYield = seedToGrow.cropToYield;

        //Instantiate the harvestable crop
        harvestable = Instantiate(cropToYield.gameModel, transform);

        //Convert Hours To Grow into minutes
        int minutesToGrow = GameTimestamp.HoursToMinutes(seedToGrow.hoursToGrow);
        //Convert it to minutes
        maxGrowth = minutesToGrow;

        //Set the growth and health accordingly
        this.growth = growth;
        this.health = health;

        //Check if it is regrowable
       /* if (seedToGrow.regrowable)
        {
            //Get the RegrowableHarvestBehaviour from the GameObject
            RegrowableHarvestBehaviour regrowableHarvest = harvestable.GetComponent<RegrowableHarvestBehaviour>();

            //Initialise the harvestable 
            regrowableHarvest.SetParent(this);
        }*/

        //Set the initial state to Seed
        SwitchState(cropState);

    }

    //The crop will grow when watered
    public void Grow()
    {
        //Increase the growth point by 1
        growth++;

        //Restore the health of the plant when it is watered
        if(health < maxHealth)
        {
            health++;
        }

        //The sees will sprout into a seeding when the growth is at 1/2
        if (growth >= maxGrowth / 2 && cropState == CropState.Seed)
        {
            SwitchState(CropState.Seedling);
        }

        //Grow from seeding to harvestable
        if (growth >= maxGrowth && cropState == CropState.Seedling)
        {
            SwitchState(CropState.Harvestable);
        }

        //Inform LandManager on the changes
        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }

    //The crop will progress wither when the soil is dry
    public void Wilther()
    {
        health--;

        //If the health is below 0 and the crop has germinated, kill it
        if(health <= 0 && cropState != CropState.Seed)
        {
            SwitchState(CropState.Wilted);
        }

        //Inform LandManager on the changes
        LandManager.Instance.OnCropStateChange(landID, cropState, growth, health);
    }

    //Function to handle the state changes
    void SwitchState(CropState stateToSwitch)
    {
        //Reset everything and set all GameObjects to inactive
        seed.SetActive(false);
        seedling.SetActive(false);
        harvestable.SetActive(false);
        wilted.SetActive(false);

        switch (stateToSwitch)
        {
            case CropState.Seed:
                //Enable the Seed GameObject
                seed.SetActive(true);
                break;
            case CropState.Seedling:
                //Enable the Seedling GameObject
                seedling.SetActive(true);

                health = maxHealth;
                break;
            case CropState.Harvestable:
                //Enable the Harvestable GameObject
                harvestable.SetActive(true);

                //If the seed is not regrowable, detach the harvestable from this crop gameobject and destroy it.
                if (!seedToGrow.regrowable)
                {
                    //Unparent it to the crop
                    harvestable.transform.parent = null;
                    RemoveCrop();
                    /*Destroy(gameObject);*/
                }

                break;
            case CropState.Wilted:
                wilted.SetActive(true);
                break;
        }

        //Set the current crop state to the state we're switching to 
        cropState = stateToSwitch;

    }

    //Destroys and Deregisters the Crop
    public void RemoveCrop()
    {
        LandManager.Instance.DeregisterCrop(landID);
        Destroy(gameObject);
    }

    //Called when the player harvests a regrowable crop. Resets the state to seeding
/*    public void Regrow()
    {
        //Reset the growth
        //Get the regrowth time in minutes
        int minutesToRegrow = GameTimestamp.HoursToMinutes(seedToGrow.hoursToRegrow);
        growth = maxGrowth - minutesToRegrow;
*//*        int hoursToRegrow = GameTimestamp.DaysToHours(seedToGrow.daysToRegrow);
        growth = maxGrowth - GameTimestamp.HoursToMinutes(hoursToRegrow);*//*

        //Switch the state back to seeding
        SwitchState(CropState.Seedling);
    }*/
}
