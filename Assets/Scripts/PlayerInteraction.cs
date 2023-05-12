using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    public static PlayerInteraction Instance { get; private set; }

    // SimpleSampleCharacterControl simpleSampleCharacterControl;
    PlayerControllerDemo playerControllerDemo;

    Land selectLand = null;

    //The crop currently planted on the land
    CropBehaviour cropPlanted = null;

    //The interactable object the player is currently selecting
    InteractableObject selectedInteractable = null;

    public GameObject btnHarvestable;

    // Start is called before the first frame update
    void Start()
    {
        btnHarvestable.SetActive(false);
        //Get access to our PlayerController component
        // simpleSampleCharacterControl = transform.parent.GetComponent<SimpleSampleCharacterControl>();
        playerControllerDemo = transform.parent.GetComponent<PlayerControllerDemo>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, 1))
        {
            OnInteractableHit(hit);
        }
    }

    void OnInteractableHit(RaycastHit hit)
    {
        Collider other = hit.collider;

        //Check if the player is going to interact with land
        if (other.tag == "Land")
        {
            //Get the land component
            Land land = other.GetComponent<Land>();
            SelectLand(land);
            return;
        }

        //Check if player is going to interact with an item
        if (other.tag == "Item")
        {
            selectedInteractable = other.GetComponent<InteractableObject>();
            return;
        }

        //Deselect the Interactable if the player is not standing on anything at the moment
        if (selectedInteractable != null)
        {
            selectedInteractable = null;
        }


        //Deselect the land if the player is not standing on any land at the moment
        if (selectLand != null)
        {
            selectLand.Select(false);
            btnHarvestable.SetActive(false);
            selectLand = null;
        }


    }

    //Handles the selection process of the land
    void SelectLand(Land land)
    {
        //Set the previously selected land to false (If any)
        if (selectLand != null)
        {
            selectLand.Select(false);
        }

        selectLand = land;
        btnHarvestable.SetActive(true);
        land.Select(true);
    }

    public void Interact()
    {
        //The player shouldn't be able to use his tool when he has his hands full with an item
        if(InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            return;
        }

        //Check if the player is selecting any land
        if (selectLand != null)
        {
            selectLand.Interact();
            return;
        }
    }

    //Triggered when the player presses the item interact button
    public void ItemInteract()
    {

        //if the player isn't holding anything, pick up an item
        //Check if there is an interactable selected
        if (selectedInteractable != null)
        {
            //Pick it up
            selectedInteractable.PickUp();
        }
    }

    public void CheckIfEquipped() 
    {
        //if the player is holding something, keep it in his inventory
        if(InventoryManager.Instance.SlotEquipped(InventorySlot.InventoryType.Item))
        {
            InventoryManager.Instance.HandToInventory(InventorySlot.InventoryType.Item);
            return;
        }
    }
}
