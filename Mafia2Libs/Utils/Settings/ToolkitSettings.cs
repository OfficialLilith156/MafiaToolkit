using System;
using Utils.Logging;
using Utils.Discord;
using System.Windows.Forms;
using System.Diagnostics;
using System.Reflection;
using System.Globalization;

namespace Utils.Settings
{
    public class ToolkitSettings
    {
        private static IniFile ini;

        //Discord keys;
        public static bool DiscordEnabled;
        public static bool DiscordElapsedTimeEnabled;
        public static bool DiscordStateEnabled;
        public static bool DiscordDetailsEnabled;
        public static string CustomStateText;

        //ModelViewer keys;
        public static bool VSync;
        public static float ScreenDepth;
        public static float ScreenNear;
        public static float CameraSpeed;
        public static string TexturePath1;
        public static string TexturePath2;
        public static string TexturePath3;
        public static string TexturePath4;
        public static string TexturePath5;
        public static bool Experimental;
        public static bool bNavigation;
        public static bool bTranslokatorTint;
        public static bool UseMIPS;
        public static float FieldOfView;

        public static bool LoadFrameResource = true;
        public static bool LoadCollisions = true;   
        public static bool LoadActors = true;       
        public static bool LoadTranslokator = true; 
        public static bool LoadAIWorld = true;      
        public static bool LoadOBJData = true;      
        public static bool LoadRoads = true;        
        public static bool LoadATP = true;              
        public static bool LoadPrefabs = true;          
        public static bool LoadItemDescs = true;        
        public static bool LoadHPD = true;              
        public static bool LoadSoundSectors = true;

        //Model Exporting keys;
        public static int Format;
        public static string ExportPath;

        //Misc vars;
        private static long ElapsedTime;
        private static DiscordController controller;
        public static bool LoggingEnabled;
        public static int Language;
        public static int SerializeSDSOption;
        public static bool bUseOodleCompression;
        public static float CompressionRatio;
        public static bool DecompileLUA;
        public static bool EnableLuaHelper;
        public static bool bBackupEnabled;
        public static bool AddTimeDataBackup;
        public static bool UseSDSToolFormat;
        public static int IndexMemorySizePerBuffer;
        public static int VertexMemorySizePerBuffer;
        public static bool CookCollisions;
        public static bool CheckForUpdates;
        public static bool SkipGameSelector;
        public static int DefaultGame;

        // Update vars
        public static float CurrentVersion = 1.0f;
        public static readonly float Version = 2.48f;

        public static void ReadINI()
        {
            // Set to invariant culture, some cultures may have issues with ini
            CultureInfo CurrentCulture = CultureInfo.CurrentCulture;
            CultureInfo.CurrentCulture = CultureInfo.InvariantCulture;

            ini = new IniFile();
            ElapsedTime = (int)DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;

            TexturePath1 = ReadKey("TexturePath1", "ModelViewer");
            TexturePath2 = ReadKey("TexturePath2", "ModelViewer");
            TexturePath3 = ReadKey("TexturePath3", "ModelViewer");
            TexturePath4 = ReadKey("TexturePath4", "ModelViewer");
            TexturePath5 = ReadKey("TexturePath5", "ModelViewer");
            CustomStateText = ReadKey("CustomStateText", "Discord", "Developing mods.");
            bool.TryParse(ReadKey("Enabled", "Discord", "True"), out DiscordEnabled);
            bool.TryParse(ReadKey("ElapsedTimeEnabled", "Discord", "True"), out DiscordElapsedTimeEnabled);
            bool.TryParse(ReadKey("StateEmabled", "Discord", "True"), out DiscordStateEnabled);
            bool.TryParse(ReadKey("DetailsEnabled", "Discord", "True"), out DiscordDetailsEnabled);
            int.TryParse(ReadKey("SerializeOption", "SDS", "0"), out SerializeSDSOption);
            bool.TryParse(ReadKey("UseOodleCompression", "SDS", "1"), out bUseOodleCompression);
            float.TryParse(ReadKey("CompressionRatio", "SDS", "0.9"), out CompressionRatio);
            bool.TryParse(ReadKey("VSync", "ModelViewer", "False"), out VSync);
            bool.TryParse(ReadKey("UseMIPS", "ModelViewer", "False"), out UseMIPS);
            float.TryParse(ReadKey("ScreenDepth", "ModelViewer", "10000"), out ScreenDepth);
            float.TryParse(ReadKey("ScreenNear", "ModelViewer", "0.01"), out ScreenNear);
            float.TryParse(ReadKey("CameraSpeed", "ModelViewer", "15"), out CameraSpeed);
            bool.TryParse(ReadKey("EnableExperimental", "ModelViewer", "True"), out Experimental);
            bool.TryParse(ReadKey("EnableNavigation", "ModelViewer", "0"), out bNavigation);
            bool.TryParse(ReadKey("EnableTranslokator", "ModelViewer", "0"), out bTranslokatorTint);
            float.TryParse(ReadKey("FieldOfView", "ModelViewer", "60"), out FieldOfView);
            bool.TryParse(ReadKey("LoadFrameResource", "LoadOptions", "True"), out LoadFrameResource);
            bool.TryParse(ReadKey("LoadCollisions", "LoadOptions", "True"), out LoadCollisions);
            bool.TryParse(ReadKey("LoadActors", "LoadOptions", "True"), out LoadActors);
            bool.TryParse(ReadKey("LoadTranslokator", "LoadOptions", "True"), out LoadTranslokator);
            bool.TryParse(ReadKey("LoadAIWorld", "LoadOptions", "True"), out LoadAIWorld);
            bool.TryParse(ReadKey("LoadOBJData", "LoadOptions", "True"), out LoadOBJData);
            bool.TryParse(ReadKey("LoadRoads", "LoadOptions", "True"), out LoadRoads);
            bool.TryParse(ReadKey("LoadATP", "LoadOptions", "True"), out LoadATP);
            bool.TryParse(ReadKey("LoadPrefabs", "LoadOptions", "True"), out LoadPrefabs);
            bool.TryParse(ReadKey("LoadItemDescs", "LoadOptions", "True"), out LoadItemDescs);
            bool.TryParse(ReadKey("LoadHPD", "LoadOptions", "True"), out LoadHPD);
            bool.TryParse(ReadKey("LoadSoundSectors", "LoadOptions", "True"), out LoadSoundSectors);
            bool.TryParse(ReadKey("Logging", "Misc", "True"), out LoggingEnabled);
            int.TryParse(ReadKey("Language", "Misc", "0"), out Language);
            int.TryParse(ReadKey("Format", "Exporting", "0"), out Format);
            int.TryParse(ReadKey("DefaultGame", "Misc", "0"), out DefaultGame);
            bool.TryParse(ReadKey("BackupEnabled", "SDS", "True"), out bBackupEnabled);
            bool.TryParse(ReadKey("AddTimeDataBackup", "SDS", "True"), out AddTimeDataBackup);
            bool.TryParse(ReadKey("DecompileLUA", "SDS", "False"), out DecompileLUA);
            bool.TryParse(ReadKey("UseSDSToolFormat", "SDS", "False"), out UseSDSToolFormat);
            bool.TryParse(ReadKey("CookCollisions", "SDS", "True"), out CookCollisions);
            bool.TryParse(ReadKey("EnableLUAHelper", "LUA", "False"), out EnableLuaHelper);
            bool.TryParse(ReadKey("CheckForUpdates", "Misc", "True"), out CheckForUpdates);
            bool.TryParse(ReadKey("SkipGameSelector", "Misc", "False"), out SkipGameSelector);
            int.TryParse(ReadKey("IndexMemorySizePerBuffer", "SDS", "945000"), out IndexMemorySizePerBuffer);
            int.TryParse(ReadKey("VertexMemorySizePerBuffer", "SDS", "6000000 "), out VertexMemorySizePerBuffer);
            ExportPath = ReadKey("ModelExportPath", "Directories", Application.StartupPath);
            float.TryParse(ReadKey("CurrentVersion", "Update", Version.ToString()), out CurrentVersion);

            Log.LoggingEnabled = LoggingEnabled;

            if (DiscordEnabled)
            {
                InitRichPresence();
            }

            // Reset culture
            CultureInfo.CurrentCulture = CurrentCulture;
        }

        public static string ReadKey(string key, string section, string defaultValue = null)
        {
            if (!ini.KeyExists(key, section))
            {
                ini.Write(key, defaultValue, section);
            }
            else
            {
                return ini.Read(key, section);
            }

            return defaultValue;
        }

        public static void WriteKey(string key, string section, string defaultValue)
        {
            ini.Write(key, defaultValue, section);
        }

        public static void WriteDirectoryKey(string key, string value)
        {
            WriteKey(key, "Directories", value);
        }

        public static string ReadDirectoryKey(string key)
        {
            return ReadKey(key, "Directories", "");
        }

        private static void InitRichPresence()
        {
            controller = new DiscordController();
            controller.Initialize();
            controller.presence = new DiscordRPC.RichPresence()
            {
                smallImageKey = "",
                smallImageText = "",
                largeImageKey = "main_art",
                largeImageText = "",
                startTimestamp = ElapsedTime
            };
            UpdateRichPresence("Using the Game Explorer.");
        }

        public static void UpdateRichPresence(string details = null)
        {
            if (!DiscordEnabled)
            {
                controller?.Shutdown();
                controller = null;
            }
            else
            {
                if (controller == null)
                {
                    InitRichPresence();
                }

                details = ""; //don't like current imp.
                string detailsLine = string.IsNullOrEmpty(details) ? CustomStateText : details;
                controller.presence.state = DiscordStateEnabled ? detailsLine : null;
                string vString = Debugger.IsAttached ? "DEBUG " : "RELEASE ";
                vString += Version;
                controller.presence.details = DiscordDetailsEnabled ? vString : null;
                controller.presence.startTimestamp = DiscordElapsedTimeEnabled ? ElapsedTime : 0;

                controller.UpdatePresence();
            }
        }
    }
}