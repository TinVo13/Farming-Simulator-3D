using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    PlayerController playerController;

    Land selectLand = null;

    // Start is called before the first frame update
    void Start()
    {
        playerController = transform.parent.GetComponent<PlayerController>();
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
              
               if (other.tag == "Land")
               {
                    Land land = other.GetComponent<Land>();
                    SelectLand(land);
                    return;
               }

               if (selectLand != null) 
               {
                    selectLand.Select(false);
                    selectLand = null;
               }
    }
    
    void SelectLand(Land land)
    {
        if (selectLand != null)
        {
            selectLand.Select(false);
        }


        selectLand = land;
        land.Select(true);
    }

    public void Interact()
    {
        if (selectLand != null)
        {
            selectLand.Interact();  
            return;
        }

       
    }
}
