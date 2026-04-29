using Rendering.Graphics;
using System.Numerics;
using Utils.Settings;

namespace Rendering.Core
{
    public class WorldSettings
    {
        public LightClass Lighting { get; set; }
        
        public int RenderMode { get; set; }
        public bool RenderSky { get; set; }
        public bool RenderClouds { get; set; }

        public WorldSettings()
        {
            Lighting = new LightClass();
            RenderMode = 2;
            RenderSky = true;
            RenderClouds = false;
        }

        public void SetupLighting()
        {
            Lighting.SetAmbientColor(ToolkitSettings.AmbientR, ToolkitSettings.AmbientG, ToolkitSettings.AmbientB, ToolkitSettings.AmbientA);
            Lighting.SetDiffuseColour(ToolkitSettings.DiffuseR, ToolkitSettings.DiffuseG, ToolkitSettings.DiffuseB, ToolkitSettings.DiffuseA);
            Lighting.Direction = new Vector3(ToolkitSettings.LightDirX, ToolkitSettings.LightDirY, ToolkitSettings.LightDirZ);
            Lighting.SetSpecularColor(ToolkitSettings.SpecularR, ToolkitSettings.SpecularG, ToolkitSettings.SpecularB, ToolkitSettings.SpecularA);
            Lighting.SetSpecularPower(ToolkitSettings.SpecularPower);
        }

        public void Shutdown()
        {
            Lighting = null;
        }
    }
}
