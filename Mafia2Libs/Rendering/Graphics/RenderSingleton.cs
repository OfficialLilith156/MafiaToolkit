using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using Vortice.Direct3D11;

namespace Rendering.Graphics
{
    public sealed class RenderStorageSingleton
    {
        public List<RenderLine> SplineStorage;

        // Multi-scene support: dictionaries keyed by sceneID
        public Dictionary<string, Dictionary<ulong, RenderStaticCollision>> SceneCollisions;
        public Dictionary<string, Dictionary<ulong, ID3D11ShaderResourceView>> SceneTextureCache;

        // Legacy single-scene dictionaries for backward compatibility
        public Dictionary<ulong, RenderStaticCollision> StaticCollisions;
        public Dictionary<ulong, ID3D11ShaderResourceView> TextureCache;
        public Dictionary<ulong, Image> TextureThumbnails;

        public ShaderManager ShaderManager;
        public RenderPrefabs Prefabs;
        private bool isInit;
        private string currentSceneContext = "";

        RenderStorageSingleton()
        {
            SplineStorage = new List<RenderLine>();
            SceneCollisions = new Dictionary<string, Dictionary<ulong, RenderStaticCollision>>();
            SceneTextureCache = new Dictionary<string, Dictionary<ulong, ID3D11ShaderResourceView>>();
            StaticCollisions = new Dictionary<ulong, RenderStaticCollision>();
            TextureCache = new Dictionary<ulong, ID3D11ShaderResourceView>();
            TextureThumbnails = new Dictionary<ulong, Image>();
            ShaderManager = new ShaderManager();
            Prefabs = new RenderPrefabs();
        }

        public void SetSceneContext(string sceneID)
        {
            currentSceneContext = sceneID;

            if (!SceneCollisions.ContainsKey(sceneID))
                SceneCollisions[sceneID] = new Dictionary<ulong, RenderStaticCollision>();

            if (!SceneTextureCache.ContainsKey(sceneID))
            {
                SceneTextureCache[sceneID] = new Dictionary<ulong, ID3D11ShaderResourceView>();

                // Copy default textures (0 and 1) to new scene context if they exist
                // These are the default diffuse and normal map textures used by shaders
                if (TextureCache != null)
                {
                    if (TextureCache.ContainsKey(0))
                        SceneTextureCache[sceneID][0] = TextureCache[0];
                    if (TextureCache.ContainsKey(1))
                        SceneTextureCache[sceneID][1] = TextureCache[1];
                }
            }

            // Update legacy references to point to current scene
            StaticCollisions = SceneCollisions[sceneID];
            TextureCache = SceneTextureCache[sceneID];
        }

        public void CleanupScene(string sceneID)
        {
            if (SceneCollisions.ContainsKey(sceneID))
            {
                foreach (var col in SceneCollisions[sceneID].Values)
                    col.Shutdown();
                SceneCollisions.Remove(sceneID);
            }

            if (SceneTextureCache.ContainsKey(sceneID))
            {
                foreach (var tex in SceneTextureCache[sceneID].Values)
                    tex.Dispose();
                SceneTextureCache.Remove(sceneID);
            }
        }

        public bool IsInitialised()
        {
            return isInit;
        }

        public bool Initialise(DirectX11Class D3D)
        {
            if (!ShaderManager.Init(D3D.Device))
            {
                MessageBox.Show("Failed to initialize Shader Manager!");
                return false;
            }

            // Precache textures and thumbnails which will be reused pretty often.
            Instance.TextureCache.Add(0, TextureLoader.LoadTexture(D3D.Device, D3D.DeviceContext, "texture.dds"));
            Instance.TextureCache.Add(1, TextureLoader.LoadTexture(D3D.Device, D3D.DeviceContext, "default_n.dds"));

            Instance.TextureThumbnails.Add(0, TextureLoader.LoadThumbnail("Resources/Texture.dds"));
            Instance.TextureThumbnails.Add(1, TextureLoader.LoadThumbnail("Resource/MissingMaterial.dds"));

            isInit = true;
            return true;
        }

        public void Shutdown()
        {
            foreach (KeyValuePair<ulong, ID3D11ShaderResourceView> texture in TextureCache)
            {
                texture.Value.Dispose();
            }

            foreach (RenderLine line in SplineStorage)
            {
                line.Shutdown();
            }

            foreach (KeyValuePair<ulong, RenderStaticCollision> col in StaticCollisions)
            {
                col.Value.Shutdown();
            }

            SplineStorage.Clear();
            StaticCollisions.Clear();
            TextureCache.Clear();
            TextureThumbnails.Clear();
            ShaderManager.Shutdown();
            isInit = false;
        }

        public static RenderStorageSingleton Instance {
            get {
                return Nested.instance;
            }
        }

        class Nested
        {
            static Nested()
            {
            }

            internal static readonly RenderStorageSingleton instance = new RenderStorageSingleton();
        }
    }
}
