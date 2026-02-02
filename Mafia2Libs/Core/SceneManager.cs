using Mafia2Tool;
using Rendering.Core;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;

namespace Toolkit.Core
{
    public enum SceneLayer
    {
        Primary,
        Reference,
        Import
    }

    public class ManagedScene
    {
        public string SceneID { get; set; }
        public SceneData SceneData { get; set; }
        public SceneLayer Layer { get; set; }
        public bool IsVisible { get; set; } = true;
        public bool IsEditable { get; set; }
        public int RefIDOffset { get; set; }
        public Color SceneTint { get; set; }
        public SpatialPartition SpatialIndex { get; set; }
    }

    public class SceneManager
    {
        private Dictionary<string, ManagedScene> scenes = new Dictionary<string, ManagedScene>();
        private string activeSceneID;
        private int nextRefIDOffset = 0;
        private const int REF_ID_NAMESPACE_SIZE = 100000;

        public ManagedScene ActiveScene
        {
            get
            {
                if (string.IsNullOrEmpty(activeSceneID) || !scenes.ContainsKey(activeSceneID))
                    return null;
                return scenes[activeSceneID];
            }
        }

        public IReadOnlyCollection<ManagedScene> AllScenes => scenes.Values;

        public string AddScene(SceneData sceneData, SceneLayer layer)
        {
            string sceneID = Guid.NewGuid().ToString();
            int offset = (layer == SceneLayer.Primary) ? 0 : nextRefIDOffset;

            scenes[sceneID] = new ManagedScene
            {
                SceneID = sceneID,
                SceneData = sceneData,
                Layer = layer,
                IsEditable = (layer == SceneLayer.Primary),
                RefIDOffset = offset,
                SceneTint = GetLayerTint(layer),
                SpatialIndex = new SpatialPartition()
            };

            if (layer != SceneLayer.Primary)
                nextRefIDOffset += REF_ID_NAMESPACE_SIZE;

            if (activeSceneID == null)
                activeSceneID = sceneID;

            return sceneID;
        }

        public void RemoveScene(string sceneID)
        {
            if (!scenes.ContainsKey(sceneID))
                return;

            // Don't allow removing the active scene if it's the only one
            if (sceneID == activeSceneID && scenes.Count == 1)
                return;

            scenes.Remove(sceneID);

            // If we removed the active scene, set a new one
            if (sceneID == activeSceneID)
            {
                activeSceneID = scenes.Keys.FirstOrDefault();
            }
        }

        public void SetActiveScene(string sceneID)
        {
            if (!scenes.ContainsKey(sceneID))
                throw new ArgumentException($"Scene with ID {sceneID} does not exist");

            activeSceneID = sceneID;

            // Update RefManager namespace for the active scene
            var scene = scenes[sceneID];
            RefManager.SetNamespaceOffset(scene.RefIDOffset);
        }

        public ManagedScene GetScene(string sceneID)
        {
            if (!scenes.ContainsKey(sceneID))
                return null;

            return scenes[sceneID];
        }

        public int TranslateRefID(string sceneID, int localRefID)
        {
            if (!scenes.ContainsKey(sceneID))
                throw new ArgumentException($"Scene with ID {sceneID} does not exist");

            return scenes[sceneID].RefIDOffset + localRefID;
        }

        public (string sceneID, int localRefID) ReverseTranslateRefID(int globalRefID)
        {
            // Find the scene that owns this RefID
            foreach (var kvp in scenes)
            {
                int offset = kvp.Value.RefIDOffset;
                int nextOffset = offset + REF_ID_NAMESPACE_SIZE;

                if (globalRefID >= offset && globalRefID < nextOffset)
                {
                    int localRefID = globalRefID - offset;
                    return (kvp.Key, localRefID);
                }
            }

            // If not found in any namespace, assume it's in the primary scene
            if (activeSceneID != null)
            {
                return (activeSceneID, globalRefID);
            }

            throw new ArgumentException($"RefID {globalRefID} does not belong to any loaded scene");
        }

        private Color GetLayerTint(SceneLayer layer)
        {
            switch (layer)
            {
                case SceneLayer.Primary:
                    return Color.White;
                case SceneLayer.Reference:
                    return Color.LightBlue;
                case SceneLayer.Import:
                    return Color.LightYellow;
                default:
                    return Color.White;
            }
        }

        // Scene Merging Functionality
        public string MergeScenes(string[] sceneIDs, MergeOptions options)
        {
            if (sceneIDs == null || sceneIDs.Length < 2)
                throw new ArgumentException("At least two scenes are required for merging");

            // Create new merged scene
            SceneData mergedScene = new SceneData();
            mergedScene.ScenePath = options.TargetSceneName;

            // Merge frame resources
            MergeFrameResources(sceneIDs, mergedScene, options);

            // Merge other resources
            MergeCollisions(sceneIDs, mergedScene);
            MergeActors(sceneIDs, mergedScene);
            MergeBufferPools(sceneIDs, mergedScene);

            // Add merged scene as primary
            string mergedSceneID = AddScene(mergedScene, SceneLayer.Primary);
            return mergedSceneID;
        }

        private void MergeFrameResources(string[] sceneIDs, SceneData target, MergeOptions options)
        {
            Dictionary<ulong, ResourceTypes.FrameResource.FrameGeometry> geometryHash =
                new Dictionary<ulong, ResourceTypes.FrameResource.FrameGeometry>();

            foreach (string sceneID in sceneIDs)
            {
                var scene = GetScene(sceneID);
                if (scene == null)
                    continue;

                // Simple merge - copy all frame objects
                // In a full implementation, this would handle deduplication and hierarchy
                foreach (var kvp in scene.SceneData.FrameResource.FrameObjects)
                {
                    // Note: This is a simplified implementation
                    // Full implementation would properly handle RefID remapping and deduplication
                }
            }
        }

        private void MergeCollisions(string[] sceneIDs, SceneData target)
        {
            // Merge collision data from all scenes
            foreach (string sceneID in sceneIDs)
            {
                var scene = GetScene(sceneID);
                if (scene == null || scene.SceneData.Collisions == null)
                    continue;

                // Simple merge - in full implementation would handle proper collision merging
            }
        }

        private void MergeActors(string[] sceneIDs, SceneData target)
        {
            List<ResourceTypes.Actors.Actor> mergedActors = new List<ResourceTypes.Actors.Actor>();

            foreach (string sceneID in sceneIDs)
            {
                var scene = GetScene(sceneID);
                if (scene == null || scene.SceneData.Actors == null)
                    continue;

                mergedActors.AddRange(scene.SceneData.Actors);
            }

            target.Actors = mergedActors.ToArray();
        }

        private void MergeBufferPools(string[] sceneIDs, SceneData target)
        {
            // Merge vertex and index buffer pools
            // This is a placeholder for the full implementation
            foreach (string sceneID in sceneIDs)
            {
                var scene = GetScene(sceneID);
                if (scene == null)
                    continue;

                // Full implementation would merge buffer pools properly
            }
        }
    }

    public class MergeOptions
    {
        public bool DeduplicateGeometry { get; set; } = true;
        public bool DeduplicateMaterials { get; set; } = true;
        public bool PreserveHierarchy { get; set; } = true;
        public string TargetSceneName { get; set; }
    }
}
