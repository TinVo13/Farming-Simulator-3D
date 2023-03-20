using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct CropSaveState 
{
    public int landID;

    public string seedToGrow;
    public CropBehaviour.CropState cropState;
    public int growth;
    public int health;

    public CropSaveState(int landID, string seedToGrow, CropBehaviour.CropState cropState, int growth, int health)
    {
        this.landID = landID;
        this.seedToGrow = seedToGrow;
        this.cropState = cropState;
        this.growth = growth;
        this.health = health;
    }

    //The crop will grow when watered
    public void Grow()
    {
        //Increase the growth point by 1
        growth++;

        //Convert the seedToGrow string into SeedData
        SeedData seedInfo =  (SeedData) InventoryManager.Instance.itemIndex.GetItemFromString(seedToGrow);
        //Get the maxHealth and maxGrowth from the seed data
        int maxGrowth = GameTimestamp.HoursToMinutes(seedInfo.hoursToGrow);
        int maxHealth = GameTimestamp.HoursToMinutes(2);

        //Restore the health of the plant when it is watered
        if (health < maxHealth)
        {
            health++;
        }

        //The sees will sprout into a seeding when the growth is at 1/2
        if (growth >= maxGrowth / 2 && cropState == CropBehaviour.CropState.Seed)
        {
            cropState = CropBehaviour.CropState.Seedling;
        }

        //Grow from seeding to harvestable
        if (growth >= maxGrowth && cropState == CropBehaviour.CropState.Seedling)
        {
           cropState = CropBehaviour.CropState.Harvestable;
        }
    }

    //The crop will progress wither when the soil is dry
    public void Wilther()
    {
        health--;

        //If the health is below 0 and the crop has germinated, kill it
        if (health <= 0 && cropState != CropBehaviour.CropState.Seed)
        {
            cropState = CropBehaviour.CropState.Wilted;
        }
    }
}
