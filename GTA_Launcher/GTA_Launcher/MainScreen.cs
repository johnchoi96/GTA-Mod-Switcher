using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace GTA_Launcher
{
    public partial class MainScreen : Form
    {

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
                string path = @"D:Steam\steamapps\common\Grand Theft Auto V";
                path = Environment.ExpandEnvironmentVariables(path);
                return Directory.Exists(path);
            }
        }

        private bool ModEnabled
        {
            get
            {
                return true;
            }
        }

        private bool ModExists
        {
            get
            {
                return true;
            }
        }

        public MainScreen()
        {
            InitializeComponent();
            // check if game directory has been set
            if (GamePath == "")
            {
                SetButtonState();
            }
        }

        private void SetPathClicked(object sender, EventArgs e)
        {
            // if not set, let the user choose
            string folderPath = "";
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            var result = dialog.ShowDialog();
            if (result != DialogResult.OK)
            {
                string title = "Directory not set!";
                string message = "You must set the path of your GTA Game";
                MessageBox.Show(message, title, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            folderPath = dialog.SelectedPath;
            GamePath = folderPath;

            SetButtonState();
        }

        private void ToggleClicked(object sender, EventArgs e)
        {
            Console.WriteLine(Properties.Settings.Default.GTAPath == "");
        }

        private void RunGameClicked(object sender, EventArgs e)
        {
            Console.WriteLine(ModExists);
        }

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
