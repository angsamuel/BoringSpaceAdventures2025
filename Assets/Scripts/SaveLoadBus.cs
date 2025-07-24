using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public static class SaveLoadBus
{

    public static string fileName = "save.txt";
    public static string saveFolder = "saves";
    public static char saveToken = '=';
    public static string directoryPath;


    public static Dictionary<string, string> dataDict;

    static SaveLoadBus(){
        dataDict = new Dictionary<string, string>();
        directoryPath = Directory.GetParent(Application.dataPath).FullName;
        directoryPath += "/" + saveFolder;
        Debug.Log(directoryPath);

        if(!Directory.Exists(directoryPath)){
            Directory.CreateDirectory(directoryPath);
        }
        LoadFromFile();
    }


    public static void SaveFloat(string key, float value){
        dataDict[key] = value.ToString();

    }
    public static float LoadFloat(string key, float defaultValue){
        if(!dataDict.ContainsKey(key)){
            return defaultValue;
        }
        return float.Parse(dataDict[key]);
    }

    public static void SaveVector3(string key, Vector3 value){
        dataDict[key] = value.x.ToString() + "," + value.y.ToString() + "," + value.z.ToString();
    }

    public static Vector3 LoadVector3(string key){
        if(!dataDict.ContainsKey(key)){
            return Vector3.zero;
        }
        Vector3 output = Vector3.zero;
        string[] parts = dataDict[key].Split(',');
        output.x = float.Parse(parts[0]);
        output.y = float.Parse(parts[1]);
        output.z = float.Parse(parts[2]);
        return output;
    }

    public static void SaveToFile(){
        string filePath = directoryPath + "/" + fileName;

        try {
            using (StreamWriter sw = new StreamWriter(filePath, false)){
                foreach(KeyValuePair<string,string> pair in dataDict){
                    sw.WriteLine($"{pair.Key}{saveToken}{pair.Value}");
                }
            }
            Debug.Log($"Saved to file {filePath}");
        }catch{
            Debug.Log($"Error! D: Could not save to file {filePath}");
        }
    }

    public static void LoadFromFile(){
        string filePath = directoryPath + "/" + fileName;
        dataDict.Clear();
        if(File.Exists(filePath)){
            string[] lines = File.ReadAllLines(filePath);

            foreach(string line in lines){
                string[] parts = line.Split(saveToken);
                if(parts.Length == 2){
                    dataDict[parts[0]] = parts[1];
                }
            }
        }
    }
}
