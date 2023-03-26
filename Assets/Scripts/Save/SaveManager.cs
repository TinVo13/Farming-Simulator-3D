using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static readonly string FILEPATH = Application.persistentDataPath + "/Save.json";
    public static void Save(GameSaveState save)
    {
        string json = JsonUtility.ToJson(save);
        File.WriteAllText(FILEPATH, json);

        /*//Save as Binary file
        using (FileStream file = File.Create(FILEPATH))
        {
            new BinaryFormatter().Serialize(file, save);
        }*/
    }

    public static GameSaveState Load()  
    {
        GameSaveState loadedSave = null;
        //JSON
        if (File.Exists(FILEPATH))
        {
            string json = File.ReadAllText(FILEPATH);
            loadedSave = JsonUtility.FromJson<GameSaveState>(json);
        }

        //Binary method
       /* if (File.Exists(FILEPATH))
        {
            using (FileStream file = File.Open(FILEPATH, FileMode.Open))
            {
                object loadedData = new BinaryFormatter().Deserialize(file);
                loadedSave = (GameSaveState)loadedData;
            }
        }*/

        return loadedSave;
    }

    //Returns true if there is a save file
    public static bool HasSave()
    {
        return File.Exists(FILEPATH);
    }
}
