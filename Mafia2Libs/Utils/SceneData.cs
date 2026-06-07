using ResourceTypes.FrameResource;
using ResourceTypes.FrameNameTable;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Utils.Settings;
using Utils.Language;
using Gibbed.IO;
using ResourceTypes.BufferPools;
using ResourceTypes.City;
using ResourceTypes.ItemDesc;
using ResourceTypes.Materials;
using ResourceTypes.Sound;
using ResourceTypes.Actors;
using ResourceTypes.Collisions;
using ResourceTypes.Navigation;
using ResourceTypes.Navigation.Traffic;
using ResourceTypes.Translokator;
using ResourceTypes.Prefab;
using ResourceTypes.Misc;
using Utils.Types;
using System.Diagnostics;
using Utils.Models;
using System.Linq;
using Utils.Logging;

namespace Mafia2Tool
{
    public class SceneData
    {
        public FrameNameTable FrameNameTable;
        public FrameResource FrameResource;
        public VertexBufferManager VertexBufferPool;
        public IndexBufferManager IndexBufferPool;
        public SoundSectorResource SoundSector;
        public Actor[] Actors;
        public ItemDescLoader[] ItemDescs;
        public Collision Collisions;
        public CityAreas CityAreas;
        public CityShops CityShops;
        public IRoadmap roadMap;
        public string roadMapFilePath;
        public AnimalTrafficLoader ATLoader;
        public NAVData[] AIWorlds;
        public NAVData[] OBJData;
        public HPDData HPDData;
        public TranslokatorLoader Translokator;
        public PrefabLoader Prefabs;
        public string ScenePath = "";
        public ulong FrameRef;


        public SDSContentFile sdsContent;
        private bool isBigEndian;

        private FileInfo BuildFileInfo(string name)
        {
            var file = name;
            var info = new FileInfo(file);
            ToolkitAssert.Ensure(info.Exists, "File [{0}] does not exist!", name);
            return info;
        }

        public void BuildData(bool forceBigEndian)
        {
            List<FileInfo> vbps = new List<FileInfo>();
            List<FileInfo> ibps = new List<FileInfo>();
            List<ItemDescLoader> ids = new List<ItemDescLoader>();
            List<Actor> act = new List<Actor>();
            List<NAVData> aiw = new List<NAVData>();
            List<NAVData> obj = new List<NAVData>();

            isBigEndian = forceBigEndian;
            VertexTranslator.IsBigEndian = forceBigEndian;

            if (isBigEndian)
            {
                MessageBox.Show("Detected 'Big Endian' formats. This will severely effect functionality!", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }

            DirectoryInfo dirInfo = new DirectoryInfo(ScenePath);
            sdsContent = new SDSContentFile();
            sdsContent.ReadFromFile(new FileInfo(Path.Combine(ScenePath + "/SDSContent.xml")));

            //IndexBuffers
            if (ToolkitSettings.LoadFrameResource)
            {
                var paths = sdsContent.GetResourceFiles("IndexBufferPool", true);
                foreach (var item in paths)
                    ibps.Add(BuildFileInfo(item));

                paths = sdsContent.GetResourceFiles("VertexBufferPool", true);
                foreach (var item in paths)
                    vbps.Add(BuildFileInfo(item));
            }

            //Actors
            if (ToolkitSettings.LoadActors && !isBigEndian)
            {
                var paths = sdsContent.GetResourceFiles("Actors", true);
                foreach (var item in paths)
                {
                    try
                    {
                        if (File.Exists(item))
                            act.Add(new Actor(new FileInfo(item)));
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Failed to read actor {0}", item);
                    }
                }
            }

            //FrameResource
            if (ToolkitSettings.LoadFrameResource && sdsContent.HasResource("FrameResource"))
            {
                var name = sdsContent.GetResourceFiles("FrameResource", true)[0];
                FrameResource = new FrameResource(name, this, isBigEndian);
            }

            //Item Desc
            if (ToolkitSettings.LoadItemDescs && !isBigEndian)
            {
                var paths = sdsContent.GetResourceFiles("ItemDesc", true);
                foreach (var item in paths)
                    ids.Add(new ItemDescLoader(item));
            }

            //FrameNameTable
            if (ToolkitSettings.LoadFrameResource && sdsContent.HasResource("FrameNameTable"))
            {
                var name = sdsContent.GetResourceFiles("FrameNameTable", true)[0];
                FrameNameTable = new FrameNameTable(name, isBigEndian);
            }

            //Collisions
            if (ToolkitSettings.LoadCollisions && !isBigEndian && sdsContent.HasResource("Collisions"))
            {
                var name = sdsContent.GetResourceFiles("Collisions", true)[0];
                Collisions = new Collision(name);
            }

            //~ENABLE THIS SECTION AT YOUR OWN RISK
            //AnimalTrafficPaths
            if (ToolkitSettings.LoadATP && !isBigEndian && sdsContent.HasResource("AnimalTrafficPaths"))
            {
                var name = sdsContent.GetResourceFiles("AnimalTrafficPaths", true)[0];
                try
                {
                    ATLoader = new AnimalTrafficLoader(new FileInfo(name));
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Failed to read AnimalTrafficPaths {0}", ex.Message);
                }
            }
            //~ENABLE THIS SECTION AT YOUR OWN RISK


            if (ToolkitSettings.LoadItemDescs && !isBigEndian && sdsContent.HasResource("PREFAB"))
            {
                var name = sdsContent.GetResourceFiles("PREFAB", true)[0];
                PrefabLoader loader = new PrefabLoader(new FileInfo(name));
                Prefabs = loader;
            }


            //RoadMap

            if (ToolkitSettings.LoadRoads && !isBigEndian)
            {
                var paths = sdsContent.GetResourceFiles("MemFile", true);
                foreach (var item in paths)
                {
                    if (item.Contains("RoadMap") || item.Contains("roadmap"))
                    {
                        using (FileStream RoadmapStream = File.Open(item, FileMode.Open))
                        {
                            roadMap = new RoadmapCe();
                            roadMap.Read(RoadmapStream);
                            roadMapFilePath = item;
                        }
                    }
                }
            }
            // DEBUG

            //Translokator
            if (ToolkitSettings.LoadTranslokator && !isBigEndian && sdsContent.HasResource("Translokator"))
            {
                var name = sdsContent.GetResourceFiles("Translokator", true)[0];
                Translokator = new TranslokatorLoader(new FileInfo(name));
            }

            // Kynapse Navigation
            if (ToolkitSettings.bNavigation)
            {
                if (ToolkitSettings.LoadOBJData && !isBigEndian)
                {
                    var paths = sdsContent.GetResourceFiles("NAV_OBJ_DATA", true);
                    foreach (var item in paths)
                        obj.Add(new NAVData(new FileInfo(item)));
                    OBJData = obj.ToArray();
                }

                if (ToolkitSettings.LoadAIWorld && !isBigEndian)
                {
                    var paths = sdsContent.GetResourceFiles("NAV_AIWORLD_DATA", true);
                    foreach (var item in paths)
                        aiw.Add(new NAVData(new FileInfo(item)));
                    AIWorlds = aiw.ToArray();
                }

                if (ToolkitSettings.LoadHPD && !isBigEndian && sdsContent.HasResource("NAV_HPD_DATA"))
                {
                    var name = sdsContent.GetResourceFiles("NAV_HPD_DATA", true)[0];
                    var data = new NAVData(new FileInfo(name));
                    HPDData = (data.Data as HPDData);
                }
            }

            IndexBufferPool = (ibps.Count > 0) ? new IndexBufferManager(ibps, dirInfo, isBigEndian) : new IndexBufferManager(new List<FileInfo>(), dirInfo, isBigEndian);
            VertexBufferPool = (vbps.Count > 0) ? new VertexBufferManager(vbps, dirInfo, isBigEndian) : new VertexBufferManager(new List<FileInfo>(), dirInfo, isBigEndian);
            ItemDescs = ids.ToArray();
            Actors = act.ToArray();
        }

        public void UpdateResourceType()
        {
            sdsContent.CreateFileFromFolder();
            sdsContent.WriteToFile();
        }

        public void SaveRoadMap()
        {
            if (roadMap == null)
            {
                MessageBox.Show("No road map data loaded.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(roadMapFilePath))
            {
                MessageBox.Show("Road map file path is unknown. Cannot save.", "Toolkit", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            try
            {
                using (FileStream fs = new FileStream(roadMapFilePath, FileMode.Create))
                {
                    roadMap.Write(fs, Endian.Little);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save road map: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
        }
        public void SavePrefabs()
        {
            if (Prefabs == null || Prefabs.Prefabs.Length == 0)
            {
                return;
            }

            string prefabPath = null;
            try
            {
                var existingFiles = sdsContent.GetResourceFiles("PREFAB", true);
                if (existingFiles.Length > 0)
                    prefabPath = existingFiles[0];
                else
                    prefabPath = Path.Combine(ScenePath, "Prefabs_0.prf");

                Prefabs.WriteToFile(new FileInfo(prefabPath));
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save prefab file: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        public Actor CreateNewActor()
        {
            string DirectoryAndName = string.Format("{0}/Actors_{1}.act", ScenePath, Actors.Length);
            Actor NewActorFile = new Actor(DirectoryAndName);

            // TODO: Terrible code, but this whole class is going to be re-written anyway.
            List<Actor> ActorFiles = Actors.ToList();
            ActorFiles.Add(NewActorFile);
            Actors = ActorFiles.ToArray();

            return NewActorFile;
        }

        public void CleanData()
        {
            FrameNameTable = null;
            FrameResource = null;
            VertexBufferPool = null;
            IndexBufferPool = null;
            SoundSector = null;
            Actors = null;
            ItemDescs = null;
            Collisions = null;
            CityAreas = null;
            CityShops = null;
            roadMap = null;
            ATLoader = null;
            AIWorlds = null;
            OBJData = null;
        }

        //for foolfroofing, maybe the imported textures could be cached until save, then reset it, and if user doesn't save and exit, all cached textures would be deleted
        public void ImportTextures(List<string> textures, string ImportScenePath)
        {
            foreach (var texture in textures)
            {
                if (TextureCheck(texture, ImportScenePath))
                {
                    CopyTexture(texture, ImportScenePath);
                }

                string mipTexture = "MIP_" + texture;
                if (TextureCheck(mipTexture, ImportScenePath))
                {
                    CopyTexture(mipTexture, ImportScenePath);
                }
            }

        }
        public void ImportItemDescForNode(TreeNode frnode, SceneData importedScene)
        {
            if (importedScene.ItemDescs == null || importedScene.ItemDescs.Length == 0)
                return;

            HashSet<ulong> hashes = new HashSet<ulong>();
            CollectCollisionHashes(frnode, hashes);

            if (hashes.Count == 0)
                return;

            var filtered = importedScene.ItemDescs.Where(id => hashes.Contains(id.FrameRef)).ToList();

            if (filtered.Count == 0)
                return;

            var groupedByFile = filtered.GroupBy(id => id.FileName);

            foreach (var group in groupedByFile)
            {
                string outFile = Path.Combine(ScenePath, group.Key);

                if (File.Exists(outFile))
                    continue;

                using (BinaryWriter writer = new BinaryWriter(File.Open(outFile, FileMode.Create)))
                {
                    foreach (var item in group)
                        item.WriteToFile(writer);
                }
            }

            UpdateResourceType();
        }

        private void CollectCollisionHashes(TreeNode node, HashSet<ulong> hashes)
        {
            if (node?.Tag is FrameObjectCollision collision)
            {
                if (collision.Hash != 0)
                    hashes.Add(collision.Hash);
            }

            foreach (TreeNode child in node.Nodes)
                CollectCollisionHashes(child, hashes);
        }


        private bool TextureCheck(string importTextureName, string ImportScenePath)//done like this in case sdscontent wasn't updated, accurate option
        {
            //checking if importing texture exists
            string texPath = Path.Combine(ImportScenePath, importTextureName);
            if (!File.Exists(texPath))
            {
                return false;
            }
            //checking if the texture is already present
            texPath = Path.Combine(ScenePath, importTextureName);
            if (File.Exists(texPath))
            {
                return false;
            }

            return true;
        }

        private void CopyTexture(string texture, string ImportScenePath)
        {
            string importPath = Path.Combine(ImportScenePath, texture);
            string destinationPath = Path.Combine(ScenePath, texture);

            try
            {
                File.Copy(importPath, destinationPath);
            }
            catch (IOException ex)
            {
                Console.WriteLine($"Error copying texture: {ex.Message}");
            }
        }
    }

    public static class MaterialData
    {
        public static bool HasLoaded = false;

        public static void Load()
        {
            MaterialsManager.ClearLoadedMTLs();

            try
            {
                MaterialsManager.ReadMatFiles(GameStorage.Instance.GetSelectedGame().Materials.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                HasLoaded = true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(Language.GetString("$ERROR_DIDNT_FIND_MTL") + ex.Message, Language.GetString("$ERROR_TITLE"), MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
