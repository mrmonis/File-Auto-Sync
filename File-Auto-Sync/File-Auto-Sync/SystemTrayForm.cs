using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace File_Auto_Sync
{
    /// <summary>
    /// The form appears in the system tray, with all program options
    /// </summary>
    public partial class SystemTrayForm : Form
    {
        private NotifyIcon m_tray_icon;
        private ContextMenu m_tray_menu;
        private FileSystemWatcher m_file_watcher;

        public SystemTrayForm()
        {
            // Set up the system tray menu
            m_tray_menu = new ContextMenu();
            m_tray_menu.MenuItems.Add("Folders", OnFoldersSelect);
            m_tray_menu.MenuItems.Add("-");
            m_tray_menu.MenuItems.Add("Exit", OnExit);

            m_tray_icon = new NotifyIcon();
            m_tray_icon.Text = "AutoSync";
            m_tray_icon.Icon = new Icon(SystemIcons.Application, 40, 40);

            m_tray_icon.ContextMenu = m_tray_menu;
            m_tray_icon.Visible = true;

            // Initialize the file watcher
            m_file_watcher = new FileSystemWatcher();


            try
            {
                string path = GetFilePath();
                if (path != null)
                {
                    m_file_watcher.Path = path;
                }
            }
            catch (Exception e)
            {
                // Do something?
            }

            /* Watch for changes in LastAccess and LastWrite times, and
                the renaming of files or directories. */
            m_file_watcher.NotifyFilter = NotifyFilters.LastWrite | NotifyFilters.FileName | NotifyFilters.DirectoryName;

            // Add event handlers.
            m_file_watcher.Changed += new FileSystemEventHandler(OnChanged);
            m_file_watcher.Created += new FileSystemEventHandler(OnChanged);
            m_file_watcher.Deleted += new FileSystemEventHandler(OnChanged);
            m_file_watcher.Renamed += new RenamedEventHandler(OnRenamed);

            // Begin watching.
            if (!m_file_watcher.Path.Equals(""))
            {
                m_file_watcher.EnableRaisingEvents = true;
            }
        }

        protected override void OnLoad(EventArgs e)
        {
            Visible = false;
            ShowInTaskbar = false;

            base.OnLoad(e);
        }

        protected void OnExit(object sender, EventArgs e) 
        {
            Application.Exit();
        }

        /* When a folder is selected with the context menu */
        protected void OnFoldersSelect(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                ChangeFilePath(folderDlg.SelectedPath);
            }
        }

        /* Retrieve the path stored in the paths.sav file */
        protected string GetFilePath()
        {
            FileStream fs = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "\\paths.sav", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs);
            string listenPath = sr.ReadLine();
            sr.Close();
            fs.Close();
            return listenPath;
        }

        /* Changes the path to the folder being watched by the file watcher */
        protected void ChangeFilePath(String filePath)
        {
            m_file_watcher.EnableRaisingEvents = false;
            FileStream fs = new FileStream(Path.GetDirectoryName(Application.ExecutablePath) + "\\paths.sav", FileMode.Create, FileAccess.Write);
            StreamWriter sr = new StreamWriter(fs);
            sr.WriteLine(filePath);
            sr.Close();
            fs.Close();
            m_file_watcher.Path = filePath;
            m_file_watcher.EnableRaisingEvents = true;
        }

        /* When a file has changed */
        protected void OnChanged(object sender, FileSystemEventArgs e)
        {
            
        }

        /* When a file is renamed */
        protected void OnRenamed(object sender, EventArgs e)
        {
            
        }
    }
}
