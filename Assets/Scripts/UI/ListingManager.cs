using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ListingManager<T> : MonoBehaviour
{
    public GameObject listingEntryPrefab;

    public Transform listingGrid;

    public void Render(List<T> listingItems)
    {
        if (listingGrid.childCount > 0)
        {
            foreach (Transform child in listingGrid)
            {
                Destroy(child.gameObject);
            }
        }

        foreach (T listingItem in listingItems)
        {
            GameObject listingGameObject = Instantiate(listingEntryPrefab, listingGrid);

            DisplayListing(listingItem, listingGameObject);
        }
    }

    protected abstract void DisplayListing(T listingItems, GameObject listingGameObject);
}
