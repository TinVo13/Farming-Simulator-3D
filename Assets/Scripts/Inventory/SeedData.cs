using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/Seed")]
public class SeedData : ItemData
{
    /*public int daysToGrow;*/

    public int hoursToGrow;

    public ItemData cropToYield;

    //The seedling GameObject;
    public GameObject seedling;

    [Header("Regrowable")]
    //Is the plant able to regrow the crop after being harvested?
    public bool regrowable;
    //Time taken before the plant yields another crop
    /*public int daysToRegrow;*/
    public int hoursToRegrow;

}
