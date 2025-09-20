using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public static class JsonSave
{
    public static string gameConfigsFolderPath = Application.dataPath + @"\configs";
    public static string gameSaveFolderPath = Application.dataPath + @"\saves";

    public static void SaveToFile<T>(T dataToSave,string folderPath,string fileName)
    {
        string json = JsonUtility.ToJson(dataToSave);

        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }
        File.WriteAllText($"{folderPath}\\{fileName}.json", json);
    }

    public static T GetDataFromJson<T>(string folderPath,string filename)
    {
        T data = default;
        string json;
        string path = $"{folderPath}\\{filename}.json";
        if (File.Exists(path))
        {
            json = File.ReadAllText(path);
            data = JsonUtility.FromJson<T>(json);
        }
        else Logger.Error($"No data to load at {path}");
        return data;
    }

}
