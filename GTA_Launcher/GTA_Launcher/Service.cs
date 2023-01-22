using System;
using System.IO;
using System.Windows.Forms;

namespace GTA_Launcher
{
	public class Service
	{
        private readonly Button ModToggleBtn;
        private readonly Button RunGameBtn;
        private readonly ToolStripButton OpenPathBtn;
        private readonly Label ModStatusLabel;

        public Service(Button ModToggleBtn, Button RunGameBtn, ToolStripButton OpenPathBtn, Label ModStatusLabel)
        {
            this.ModToggleBtn = ModToggleBtn;
            this.RunGameBtn = RunGameBtn;
            this.OpenPathBtn = OpenPathBtn;
            this.ModStatusLabel = ModStatusLabel;
        }

        /**
         * Enables or disables (toggles) the mod status for GTA V.
         * Sets the button states after task.
         * If <code>enable</code> is true, mod gets enabled. If false, mod gets disabled.
         */
        public void EnableMod(bool enable)
        {
            string gamePath = Constants.GamePath + Constants.GameName;
            gamePath = Environment.ExpandEnvironmentVariables(gamePath);
            string cleanPath = Constants.GamePath + Constants.CleanGameFolderName;
            cleanPath = Environment.ExpandEnvironmentVariables(cleanPath);
            string modPath = Constants.GamePath + Constants.ModdedGameFolderName;
            modPath = Environment.ExpandEnvironmentVariables(modPath);
            try
            {
                if (enable)
                {
                    // if already enabled, do nothing
                    if (Constants.ModEnabled)
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
                    if (!Constants.ModEnabled)
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
            catch (IOException) // handle cases where other process might be working on GTA directory, both game, mod, and clean
            {
                string msgTitle = "Unable to process request";
                string msgMessage = "Other process is using this directory. Terminate the other process and try again.";
                MessageBoxIcon msgIcon = MessageBoxIcon.Error;
                MessageBoxButtons msgButtons = MessageBoxButtons.OK;
                MessageBox.Show(msgMessage, msgTitle, msgButtons, msgIcon);
            }

            SetButtonState();
        }

        /**
         * Sets up the button state.
         */
        public void SetButtonState()
        {
            ModToggleBtn.Enabled = Constants.GamePath != "";
            RunGameBtn.Enabled = Constants.GamePath != "";
            OpenPathBtn.Enabled = Constants.GTAExists;

            if (Constants.GamePath == "")
            {
                ModStatusLabel.Text = "Game Path not Set";
            }
            else if (!Constants.GTAExists)
            {
                ModStatusLabel.Text = "Invalid Game Path";
                ModToggleBtn.Enabled = false;
                RunGameBtn.Enabled = false;
            }
            else
            {
                if (Constants.ModEnabled)
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

