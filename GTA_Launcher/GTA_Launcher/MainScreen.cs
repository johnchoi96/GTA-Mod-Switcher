using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GTA_Launcher
{
    public partial class MainScreen : Form
    {

        /**
         * Returns the directory path that contains the Grand Theft Auto V game.
         * When this computed variable is set to a new value, <code>Properties.Settings.Default</code> is
         * updated and saved.
         */
        private string GamePath
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

        private bool GTAExists
        {
            get
            {
                string path = GamePath + "Grand Theft Auto V";
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
        private bool ModEnabled
        {
            get
            {
                string modPath = GamePath + "Grand Theft Auto V - clean";
                modPath = Environment.ExpandEnvironmentVariables(modPath);

                return GTAExists && ModExists && Directory.Exists(modPath);
            }
        }

        /**
         * Returns true if either clean or mods folder exists (not both).
         */
        private bool ModExists
        {
            get
            {
                string cleanPath = GamePath + "Grand Theft Auto V - clean";
                cleanPath = Environment.ExpandEnvironmentVariables(cleanPath);
                string modPath = GamePath + "Grand Theft Auto V - mods";
                modPath = Environment.ExpandEnvironmentVariables(modPath);
                return Directory.Exists(cleanPath) ^ Directory.Exists(modPath);
            }
        }

        public MainScreen()
        {
            InitializeComponent();
            MaximizeBox = false;
            // check if game directory has been set
            if (GamePath == "")
            {
                SetButtonState();
            }

            // set up toggle button label
            ModToggleBtn.Text = ModEnabled ? "Disable Mod" : "Enable Mod";
            // set up mod status label
            ModStatusLabel.Text = ModEnabled ? "Enabled" : "Disabled";
        }

        private void SetPathClicked(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                string title = "Directory not set!";
                string message = "You must set the path of your GTA Game";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // if not set, let the user choose
            string folderPath = dialog.SelectedPath;
            GamePath = folderPath;

            if (!GTAExists)
            {
                string title = "Invalid Directory!";
                string message = "GTA not found";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!ModExists)
            {
                string title = "Mod Not Found!";
                string message = "GTA Mod Initial set up is missing. Contact the developer for assistance.";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            SetButtonState();
        }

        private void ToggleClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = "Running";
            EnableMod(!ModEnabled);
            StatusLabel.Text = "Idle";
            ModToggleBtn.Text = ModEnabled ? "Disable Mod" : "Enable Mod";
            ModStatusLabel.Text = ModEnabled ? "Enabled" : "Disabled";
        }

        /**
         * Enables or disables (toggles) the mod status for GTA V.
         * If <code>enable</code> is true, mod gets enabled. If false, mod gets disabled.
         */
        private void EnableMod(bool enable)
        {
            string gamePath = GamePath + "Grand Theft Auto V";
            gamePath = Environment.ExpandEnvironmentVariables(gamePath);
            string cleanPath = GamePath + "Grand Theft Auto V - clean";
            cleanPath = Environment.ExpandEnvironmentVariables(cleanPath);
            string modPath = GamePath + "Grand Theft Auto V - mods";
            modPath = Environment.ExpandEnvironmentVariables(modPath);
            if (enable)
            {
                // if already enabled, do nothing
                if (ModEnabled)
                {
                    return;
                }

                // enable it
                // rename game folder to clean
                Directory.Move(gamePath, cleanPath);

                // rename mods folder to actual game
                Directory.Move(modPath, gamePath);
            }
            else
            {
                // if already disabled, do nothing
                if (!ModEnabled)
                {
                    return;
                }

                // disable it
                // rename game folder to mod
                Directory.Move(gamePath, modPath);
                // rename clean folder to actual game
                Directory.Move(cleanPath, gamePath);
            }
        }

        /**
         * Runs the game.
         * Runs RPH if mod is enabled, steam version if not.
         */
        private void RunGameClicked(object sender, EventArgs e)
        {
            if (ModEnabled)
            {
                try
                {
                    // set the current working directory
                    Directory.SetCurrentDirectory(GamePath + @"Grand Theft Auto V");
                    // Start the process with the info we specified.
                    Process.Start("RAGEPluginHook.exe");
                }
                catch (Exception exception)
                {
                    // Log error.
                    string msgtitle = "Error";
                    string msgmessage = exception.Message;
                    MessageBoxIcon msgicon = MessageBoxIcon.Error;
                    MessageBoxButtons msgbuttons = MessageBoxButtons.OK;
                    MessageBox.Show(msgmessage, msgtitle, msgbuttons, msgicon);
                    return;
                }
            }
            else
            {
                // start GTA using Steam appid
                Process.Start(@"steam://rungameid/271590");
            }
        }

        /**
         * Sets up the button state.
         */
        private void SetButtonState()
        {
            ModToggleBtn.Enabled = GamePath != "";
            RunGameBtn.Enabled = GamePath != "";
            
            if (GamePath == "")
            {
                ModStatusLabel.Text = "Game Path not Set";
            } 
            else
            {
                if (ModEnabled)
                {
                    ModStatusLabel.Text = "Enabled";
                    ModToggleBtn.Text = "Disable Mod";
                }
                else
                {
                    ModStatusLabel.Text = "Disabled";
                    ModToggleBtn.Text = "Enable Mod";
                }
            }
        }
    }
}
