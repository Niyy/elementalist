
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class SaveSystem 
{
    public static void saveLevels()
    {
        BinaryFormatter format = new BinaryFormatter();
        string path = Application.persistentDataPath + "/levels.u";

        FileStream stream = new FileStream(path, FileMode.Create);

        levelsCompleated level = new levelsCompleated();

        format.Serialize(stream, level);
        stream.Close();
    }







    public static levelsCompleated load()
    {
        string path = Application.persistentDataPath + "/levels.u";
        if(File.Exists(path))
        {
            BinaryFormatter format = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            levelsCompleated data = format.Deserialize(stream) as levelsCompleated;
            stream.Close();

            return data;

        }
        else
        {
            Debug.LogError("Save File not found in " + path);
            return null;
        }
    }



}
