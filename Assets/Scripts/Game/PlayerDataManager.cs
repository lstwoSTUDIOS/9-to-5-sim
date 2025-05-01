using System.Collections.Generic;
using System.IO;
using Jobs;
using UnityEngine;
using System.Linq;

public static class PlayerDataManager
{
    public static PlayerData playerData = new();
    public static Dictionary<string, JobScriptableObject> jobs;
    
    private static string _saveDataPath = Application.persistentDataPath + "/player.data";

    public static void SaveData()
    {
        var json = JsonUtility.ToJson(playerData);
        File.WriteAllText(_saveDataPath, json);
    }

    public static void LoadData()
    {
        try
        {
            var json = File.ReadAllText(_saveDataPath);
            playerData = JsonUtility.FromJson<PlayerData>(json);
        } 
        catch {}
    }
}