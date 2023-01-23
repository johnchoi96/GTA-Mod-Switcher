using System;
using System.IO;

namespace GTA_Launcher
{
	public struct Constants
	{
        public const string GameName = "Grand Theft Auto V";

        public const string CleanGameFolderName = $"{GameName} - clean";

        public const string ModdedGameFolderName = $"{GameName} - mods";

        public const string TempGameFolderName = $"{GameName} - temp";

        public const string GTASteamAppIdProcessName = @"steam://rungameid/271590";

        public const string RPHProcessName = "RAGEPluginHook.exe";

        /**
         * Returns the directory path that contains the Grand Theft Auto V game.
         * When this computed variable is set to a new value, <code>Properties.Settings.Default</code> is
         * updated and saved.
         */
        public static string GamePath
        {
            get
            {
                return Properties.Settings.Default.GTAPath;
            }
            set
            {
                string[] delimited = value.Split('\\');
                string directory = "";
                for (int i = 0; i < delimited.Length - 1; i++)
                {
                    directory += delimited[i] + @"\";
                }
                Properties.Settings.Default.GTAPath = directory;
                Properties.Settings.Default.Save();
            }
        }

        public static bool GTAExists
        {
            get
            {
                string path = GamePath + GameName;
                path = Environment.ExpandEnvironmentVariables(path);
                return Directory.Exists(path);
            }
        }

        /**
         * Returns true if all conditions listed below are satisfied:
         * 1. GTA game exists in the currently set directory.
         * 2. Either clean or mod folder (not both) exists.
         * 3. clean folder exists.
         */
        public static bool ModEnabled
        {
            get
            {
                string modPath = GamePath + CleanGameFolderName;
                modPath = Environment.ExpandEnvironmentVariables(modPath);

                return GTAExists && ModExists && Directory.Exists(modPath);
            }
        }

        /**
         * Returns true if either clean or mods folder exists (not both).
         */
        public static bool ModExists
        {
            get
            {
                string cleanPath = GamePath + CleanGameFolderName;
                cleanPath = Environment.ExpandEnvironmentVariables(cleanPath);
                string modPath = GamePath + ModdedGameFolderName;
                modPath = Environment.ExpandEnvironmentVariables(modPath);
                return Directory.Exists(cleanPath) ^ Directory.Exists(modPath);
            }
        }
    }
}

