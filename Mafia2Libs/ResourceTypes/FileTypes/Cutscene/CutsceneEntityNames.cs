using Newtonsoft.Json;
using ResourceTypes.Cutscene.AnimEntities;
using System;
using System.Collections.Generic;
using System.IO;

public class CutsceneEntityNames
{
    private static string NamesDirectory => Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "CutsceneEntityNames");
    public Dictionary<int, string> GameEntityDisplayNames { get; set; } = new Dictionary<int, string>();
    public Dictionary<int, string> SoundEntityDisplayNames { get; set; } = new Dictionary<int, string>();

    private string currentCutsceneName;
    private bool currentIsSound;

    public static string GetNamesFilePath(string cutsceneName, bool isSound = false)
    {
        if (!Directory.Exists(NamesDirectory))
        {
            Directory.CreateDirectory(NamesDirectory);
        }

        string typeSuffix = isSound ? "_sound" : "_game";
        string safeFileName = MakeValidFileName(cutsceneName + typeSuffix);
        return Path.Combine(NamesDirectory, safeFileName + ".json");
    }

    private static string MakeValidFileName(string name)
    {
        string invalidChars = System.Text.RegularExpressions.Regex.Escape(
            new string(Path.GetInvalidFileNameChars()));
        string invalidRegStr = string.Format(@"([{0}]*\.+$)|([{0}]+)", invalidChars);

        return System.Text.RegularExpressions.Regex.Replace(name, invalidRegStr, "_");
    }

    public void Save(string cutsceneName, bool isSound = false)
    {
        currentCutsceneName = cutsceneName;
        currentIsSound = isSound;

        string filePath = GetNamesFilePath(cutsceneName, isSound);

        var dataToSave = new Dictionary<string, Dictionary<int, string>>();

        if (isSound)
        {
            dataToSave["SoundEntityDisplayNames"] = SoundEntityDisplayNames;
        }
        else
        {
            dataToSave["GameEntityDisplayNames"] = GameEntityDisplayNames;
        }

        string json = JsonConvert.SerializeObject(dataToSave, Formatting.Indented);
        File.WriteAllText(filePath, json);
    }

    public static CutsceneEntityNames Load(string cutsceneName, bool isSound = false)
    {
        string filePath = GetNamesFilePath(cutsceneName, isSound);

        CutsceneEntityNames result = new CutsceneEntityNames();
        result.currentCutsceneName = cutsceneName;
        result.currentIsSound = isSound;

        if (!File.Exists(filePath))
        {
            LoadBothFiles(cutsceneName, result);
            return result;
        }

        try
        {
            string json = File.ReadAllText(filePath);
            var data = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, string>>>(json);

            if (data != null)
            {
                if (data.TryGetValue("GameEntityDisplayNames", out var gameNames))
                {
                    result.GameEntityDisplayNames = gameNames;
                }

                if (data.TryGetValue("SoundEntityDisplayNames", out var soundNames))
                {
                    result.SoundEntityDisplayNames = soundNames;
                }
            }

            LoadBothFiles(cutsceneName, result);

            return result;
        }
        catch
        {
            return result;
        }
    }

    private static void LoadBothFiles(string cutsceneName, CutsceneEntityNames result)
    {
        string gameFilePath = GetNamesFilePath(cutsceneName, false);
        if (File.Exists(gameFilePath))
        {
            try
            {
                string gameJson = File.ReadAllText(gameFilePath);
                var gameData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, string>>>(gameJson);

                if (gameData != null && gameData.TryGetValue("GameEntityDisplayNames", out var gameNames))
                {
                    result.GameEntityDisplayNames = gameNames;
                }
            }
            catch { }
        }

        string soundFilePath = GetNamesFilePath(cutsceneName, true);
        if (File.Exists(soundFilePath))
        {
            try
            {
                string soundJson = File.ReadAllText(soundFilePath);
                var soundData = JsonConvert.DeserializeObject<Dictionary<string, Dictionary<int, string>>>(soundJson);

                if (soundData != null && soundData.TryGetValue("SoundEntityDisplayNames", out var soundNames))
                {
                    result.SoundEntityDisplayNames = soundNames;
                }
            }
            catch { }
        }
    }

    public string GetDisplayName(int entityIndex, AnimEntityTypes entityType, string defaultName, bool isSound = false)
    {
        Dictionary<int, string> targetDict = isSound ? SoundEntityDisplayNames : GameEntityDisplayNames;

        if (targetDict.ContainsKey(entityIndex))
            return targetDict[entityIndex];

        return defaultName;
    }

    public void SetDisplayName(int entityIndex, string displayName, string cutsceneName, bool isSound = false)
    {
        Dictionary<int, string> targetDict = isSound ? SoundEntityDisplayNames : GameEntityDisplayNames;
        targetDict[entityIndex] = displayName;
        Save(cutsceneName, isSound);
    }

    public void RemoveDisplayName(int entityIndex, string cutsceneName, bool isSound = false)
    {
        Dictionary<int, string> targetDict = isSound ? SoundEntityDisplayNames : GameEntityDisplayNames;

        if (targetDict.ContainsKey(entityIndex))
        {
            targetDict.Remove(entityIndex);
            Save(cutsceneName, isSound);
        }
    }

    public void ReindexAfterDeletion(int deletedIndex, string cutsceneName, bool isSound = false)
    {
        Dictionary<int, string> targetDict = isSound ? SoundEntityDisplayNames : GameEntityDisplayNames;
        var newDict = new Dictionary<int, string>();

        foreach (var kvp in targetDict)
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

        if (isSound)
            SoundEntityDisplayNames = newDict;
        else
            GameEntityDisplayNames = newDict;

        Save(cutsceneName, isSound);
    }

    public void InsertDisplayName(int originalIndex, int newIndex, string cutsceneName, bool isSound = false)
    {
        Dictionary<int, string> targetDict = isSound ? SoundEntityDisplayNames : GameEntityDisplayNames;
        var newDict = new Dictionary<int, string>();

        foreach (var kvp in targetDict)
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

        if (targetDict.ContainsKey(originalIndex))
        {
            newDict[newIndex] = targetDict[originalIndex] + " (Copy)";
        }

        if (isSound)
            SoundEntityDisplayNames = newDict;
        else
            GameEntityDisplayNames = newDict;

        Save(cutsceneName, isSound);
    }
}