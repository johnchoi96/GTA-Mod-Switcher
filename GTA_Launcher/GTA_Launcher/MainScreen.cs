using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace GTA_Launcher
{
    public partial class MainScreen : Form
    {
        private readonly Service Service;

        public MainScreen()
        {
            InitializeComponent();
            MaximizeBox = false;

            Service = new(ModToggleBtn, RunGameBtn, OpenPathBtn, ModStatusLabel);
            // Set up buttons based on current setup state.
            Service.SetButtonState();
        }

        #region Button Controllers
        private void SetPathClicked(object sender, EventArgs e)
        {
            FolderBrowserDialog dialog = new();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                string title = "Directory not set!";
                string message = "You must set the path of your GTA Game";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            // if not set, let the user choose
            string folderPath = dialog.SelectedPath;
            Constants.GamePath = folderPath;

            // update button states
            Service.SetButtonState();
            if (!Constants.GTAExists)
            {
                string title = "Invalid Directory!";
                string message = "GTA not found";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (!Constants.ModExists)
            {
                string title = "Mod Not Found!";
                string message = "GTA Mod Initial set up is missing. Contact the developer for assistance.";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
        }

        private void OpenPathClicked(object sender, EventArgs e)
        {
            Process.Start(Constants.GamePath + $@"\{Constants.GameName}");
        }

        private void ToggleClicked(object sender, EventArgs e)
        {
            StatusLabel.Text = "Running";
            Service.EnableMod(!Constants.ModEnabled);
            StatusLabel.Text = "Idle";
        }

        /**
         * Runs the game.
         * Runs RPH if mod is enabled, steam version if not.
         */
        private void RunGameClicked(object sender, EventArgs e)
        {
            if (Constants.ModEnabled)
            {
                try
                {
                    // set the current working directory
                    Directory.SetCurrentDirectory(Constants.GamePath + $@"{Constants.GameName}");
                    // Start the process with the info we specified.
                    Process.Start(Constants.RPHProcessName);
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
                Process.Start(Constants.GTASteamAppIdProcessName);
            }
        }

        /**
         * Reclones the game folder to update the game for the modded version.
         * Make sure Mod is disabled before clicking on this button.
         * Saves the modded contents to the temp directory and deletes the modded version of the game.
         * Create a new folder called "Grand Theft Auto V - mods" and copy the contents from the clean version.
         * Paste the modded contents to the new folder.
         */
        private void UpdateGameClicked(object sender, EventArgs e)
        {
            // TODO: implement
            // make sure mod is disabled
            if (Constants.ModEnabled)
            {
                string msgtitle = "Invalid Operation!";
                string msgmessage = "Mod must be disabled to update the game!";
                MessageBoxIcon msgicon = MessageBoxIcon.Error;
                MessageBoxButtons msgbuttons = MessageBoxButtons.OK;
                MessageBox.Show(msgmessage, msgtitle, msgbuttons, msgicon);
                return;
            }

            Directory.SetCurrentDirectory(Constants.GamePath);
            Directory.CreateDirectory(Constants.TempGameFolderName);



            Directory.Delete(Constants.TempGameFolderName, true);
        }
        #endregion
    }
}
