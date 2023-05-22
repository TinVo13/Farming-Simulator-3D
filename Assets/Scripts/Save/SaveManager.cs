using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class SaveManager : MonoBehaviour
{
    static readonly string FILEPATH = Application.persistentDataPath + "/Save.save";
    static string filePath = Path.Combine(Application.persistentDataPath, "Save.save");
    public static void Save(GameSaveState save)
    {
        //Save as Binary file
        using (FileStream file = File.Create(FILEPATH))
        {
            new BinaryFormatter().Serialize(file, save);
        }
    }

    public static GameSaveState Load()  
    {
        GameSaveState loadedSave = null;

        //Binary method
        if (File.Exists(FILEPATH))
        {
            using (FileStream file = File.Open(FILEPATH, FileMode.Open))
            {
                object loadedData = new BinaryFormatter().Deserialize(file);
                loadedSave = (GameSaveState)loadedData;
            }
        }

        return loadedSave;
    }

    //Returns true if there is a save file
    public static bool HasSave()
    {
        return File.Exists(FILEPATH);
    }

    public static void DeleteDataWhenNewGame() 
    {
        if (File.Exists(filePath))
        {
            File.Delete(filePath);
        }
        else
        {
            Debug.LogWarning("File does not exist.");
        }
    }
}
