using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneTransitionManager : MonoBehaviour
{
    public static SceneTransitionManager Instance;

    public enum Location { Farm, PlayerHome, Village }
    public Location currentLocation;

    //list of all the place that are to be considered indoor
    static readonly Location[] indoor = { Location.Farm };

    //The player's transform
    Transform playerPoint;

    //Check if the screen has finished fading out
    bool screenFadeOut;

    private void Awake()
    {
        //If there is more than 1 instance, destroy GameObject 
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            //Set the static instance to this instance
            Instance = this;
        }

        //Make the gameobject persistent across scenes
        DontDestroyOnLoad(gameObject);

        //OnlLocationLoad will be called when the scene is loaded
        SceneManager.sceneLoaded += OnLocationLoad;

        //Find the player's transform
        playerPoint = FindObjectOfType<SimpleSampleCharacterControl>().transform;
    }
    //check if the current location indoor
    public bool CurrentlyIndoor()
    {
        return indoor.Contains(currentLocation);
    }
    //Switch the player to another scene
    public void SwitchLocation(Location locationToSwitch)
    {
        //Call a fadeout
        UIManager.Instance.FadeOutScreen();
        screenFadeOut = false;
        StartCoroutine(ChangeScene(locationToSwitch));
    }

    IEnumerator ChangeScene(Location locationToSwitch)
    {
        //Disable the player's CharacterController component
        /*CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
        playerCharacter.enabled = false;*/

        //Wait for the scene to finish fading out before loading the next scene
        while (!screenFadeOut)
        {
            yield return new WaitForSeconds(0.1f);
        }
        //Reset the boolean
        screenFadeOut = false;
        UIManager.Instance.ResetFadeDeafaults();
        SceneManager.LoadScene(locationToSwitch.ToString(),LoadSceneMode.Single);
    }

    //Call when the screen has faded out
    public void OnFadeOutComplete()
    {
        screenFadeOut = true;
    }



    //Called when a scene is loaded
    public void OnLocationLoad(Scene scene, LoadSceneMode mode)
    {
        //The location the player is coming from when the scene loads
        Location oldLocation = currentLocation;

        //Get the new location by converting the string of our current scene into a Location enum value
        Location newLocation =  (Location) Enum.Parse(typeof(Location), scene.name);

        //If the player is not coming from any new place, stop code function
        if (currentLocation == newLocation) return;

        //Find the start point
        Transform startPoint = LocationManager.Instance.GetPlayerStartingPosition(oldLocation);

        //Disable the player's CharacterController component
        /* CharacterController playerCharacter = playerPoint.GetComponent<CharacterController>();
         playerCharacter.enabled = false;*/
        if (playerPoint == null) return;
            //Change the player's position to the start point
            playerPoint.position = startPoint.position;
            playerPoint.rotation = startPoint.rotation;

        

        //Re-enable player character controller so he can move
        /* playerCharacter.enabled = true;*/

        //Save the current location we just switched to
        currentLocation = newLocation;

    }

}
