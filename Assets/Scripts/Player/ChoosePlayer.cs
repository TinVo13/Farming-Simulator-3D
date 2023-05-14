using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using UnityEngine.Localization.Settings;

public class ChoosePlayer : MonoBehaviour
{
    public GameObject myPrefab;
    private string pathToPrefab = "Assets/Prefabs/Essentials.prefab";

    [SerializeField]
    private GameObject loadingScene;
    [SerializeField]
    private Slider loadingBar;

    [SerializeField]
    private InfomationConfirm confirm;

    public void OnClickUpdatePrefabPlayerFemale()
    {

        GameObject newObj = Instantiate(myPrefab);
        Debug.Log(newObj);
        GameObject rootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(pathToPrefab);
        Debug.Log(rootPrefab);

        GameObject female = newObj.transform.Find("Player Female").gameObject;
        
        Debug.Log(female);
        GameObject male = newObj.transform.Find("Player").gameObject;

        if(rootPrefab != null && female != null)
        {
            female.SetActive(true);
            male.SetActive(false);
            newObj.SetActive(true);
            PrefabUtility.ReplacePrefab(newObj, rootPrefab, ReplacePrefabOptions.ConnectToPrefab);
            myPrefab.SetActive(false);
            Debug.Log("Prefab updated successfully!");
        }
        else
        {
            Debug.LogWarning("female or male not found in prefab.");
        }

        Destroy(newObj);
    }

    public void OnClickUpdatePrefabPlayerMale()
    {

        GameObject newObj = Instantiate(myPrefab);
        Debug.Log(newObj);
        GameObject rootPrefab = AssetDatabase.LoadAssetAtPath<GameObject>(pathToPrefab);
        Debug.Log(rootPrefab);

        GameObject female = newObj.transform.Find("Player Female").gameObject;
        
        Debug.Log(female);
        GameObject male = newObj.transform.Find("Player").gameObject;

        if(rootPrefab != null && male != null)
        {
            male.SetActive(true);
            female.SetActive(false);
            newObj.SetActive(true);
            PrefabUtility.ReplacePrefab(newObj, rootPrefab, ReplacePrefabOptions.ConnectToPrefab);
            myPrefab.SetActive(false);
            Debug.Log("Prefab updated successfully!");
        }
        else
        {
            Debug.LogWarning("female or male not found in prefab.");
        }

        Destroy(newObj);
    }

    public void OnClickOptionMale() 
    {
        string textConfirm = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "OptionMaleKey");
        InfomationConfirm(textConfirm);
    }

    public void OnClickOptionFemale() 
    {
        string textConfirm = LocalizationSettings.StringDatabase.GetLocalizedString("LanguageTable", "OptionFemaleKey");
        InfomationConfirm(textConfirm);
    }

    public void PlayGame() 
    {
        StartCoroutine(LoadGameAsync(SceneTransitionManager.Location.PlayerHome, null));
    }

    IEnumerator LoadGameAsync(SceneTransitionManager.Location scene, Action onFirstFrameLoad)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scene.ToString(),LoadSceneMode.Single);
        loadingScene.SetActive(true);
        DontDestroyOnLoad(gameObject);
        while (!asyncLoad.isDone)
        {
            loadingBar.value = asyncLoad.progress;
            yield return null;
            Debug.Log("Loading!");
        }

        //Scene load
        Debug.Log("Loaded!");

        yield return new WaitForEndOfFrame();
        Debug.Log("First frame is loaded");
        onFirstFrameLoad?.Invoke();

        Destroy(gameObject);
    }

    public void InfomationConfirm(string message)
    {
        //Set active the gameObject of the Yes No Prompt
        confirm.gameObject.SetActive(true);

        confirm.CreatePrompt(message);

        StartCoroutine(DisableAfterDelay(10.0f));
    }

    IEnumerator DisableAfterDelay(float delay) {
        yield return new WaitForSeconds(delay);
        confirm.gameObject.SetActive(false);
    }
}
