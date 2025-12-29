using Newtonsoft.Json;
using ResourceTypes.Cutscene.AnimEntities;
using System;
using System.Collections.Generic;
using System.IO;

public class CutsceneEntityNames
{
    private static string NamesDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CutsceneEntityNames");

    public Dictionary<int, string> EntityDisplayNames { get; set; } = new Dictionary<int, string>();

    public static string GetNamesFilePath(string cutsceneName)
    {
        if (!Directory.Exists(NamesDirectory))
        {
            Directory.CreateDirectory(NamesDirectory);
        }
        string safeFileName = MakeValidFileName(cutsceneName);
        return Path.Combine(NamesDirectory, safeFileName + ".json");
    }

    private static string MakeValidFileName(string name)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(
            new string(Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
    }

    public void Save(string cutsceneName)
    {
        string filePath = GetNamesFilePath(cutsceneName);
        string json = JsonConvert.SerializeObject(this, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static CutsceneEntityNames Load(string cutsceneName)
    {
        string filePath = GetNamesFilePath(cutsceneName);

        if (!File.Exists(filePath))
            return new CutsceneEntityNames();

        try
        {
            string json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<CutsceneEntityNames>(json);
        }
        catch
        {
            return new CutsceneEntityNames();
        }
    }

    public string GetDisplayName(int entityIndex, AnimEntityTypes entityType, string defaultName)
    {
        if (EntityDisplayNames.ContainsKey(entityIndex))
            return EntityDisplayNames[entityIndex];

        return defaultName;
    }

    public void SetDisplayName(int entityIndex, string displayName, string cutsceneName)
    {
        EntityDisplayNames[entityIndex] = displayName;
        Save(cutsceneName);
    }

    public void RemoveDisplayName(int entityIndex, string cutsceneName)
    {
        if (EntityDisplayNames.ContainsKey(entityIndex))
        {
            EntityDisplayNames.Remove(entityIndex);
            Save(cutsceneName);
        }
    }

    public void ReindexAfterDeletion(int deletedIndex, string cutsceneName)
    {
        var newDict = new Dictionary<int, string>();

        foreach (var kvp in EntityDisplayNames)
        {
            if (kvp.Key < deletedIndex)
            {
                newDict[kvp.Key] = kvp.Value;
            }
            else if (kvp.Key > deletedIndex)
            {
                newDict[kvp.Key - 1] = kvp.Value;
            }
        }

        EntityDisplayNames = newDict;
        Save(cutsceneName);
    }

    public void InsertDisplayName(int originalIndex, int newIndex, string cutsceneName)
    {
        var newDict = new Dictionary<int, string>();

        foreach (var kvp in EntityDisplayNames)
        {
            if (kvp.Key <= originalIndex)
            {
                newDict[kvp.Key] = kvp.Value;
            }
            else if (kvp.Key > originalIndex)
            {
                newDict[kvp.Key + 1] = kvp.Value;
            }
        }

        if (EntityDisplayNames.ContainsKey(originalIndex))
        {
            newDict[newIndex] = EntityDisplayNames[originalIndex] + " (Copy)";
        }

        EntityDisplayNames = newDict;
        Save(cutsceneName);
    }
}