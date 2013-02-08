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
        // Name of save file. Saves info to xml.
        private string SAVE_FILE_NAME = Path.GetDirectoryName(Application.ExecutablePath) + "\\paths.xml";

        private NotifyIcon m_tray_icon;
        private ContextMenu m_tray_menu;
        private FileSystemWatcher m_file_watcher;

        private WatchedPath m_watched_path;

        public SystemTrayForm()
        {
            // Initialize private members
            m_tray_menu = new ContextMenu();
            m_tray_icon = new NotifyIcon();
            m_file_watcher = new FileSystemWatcher();
            m_watched_path = new WatchedPath();

            // Set up the system tray menu
            m_tray_menu.MenuItems.Add("Sync folder", OnSyncFolderSelect);
            m_tray_menu.MenuItems.Add("Watched folder", OnWatchedFolderSelect);
            m_tray_menu.MenuItems.Add("-");
            m_tray_menu.MenuItems.Add("Exit", OnExit);

            
            m_tray_icon.Text = "AutoSync";
            m_tray_icon.Icon = new Icon(SystemIcons.Application, 40, 40);

            m_tray_icon.ContextMenu = m_tray_menu;
            m_tray_icon.Visible = true;

            try
            {
                ReadWatchedPath();
                if (m_watched_path != null)
                {
                    m_file_watcher.Path = m_watched_path.Path;
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

            // Begin watching (if possible).
            SwitchFileWatcher(true);
        }

        /* Turns the file watcher on and off. */
        protected void SwitchFileWatcher(bool enabled)
        {
            // If attempting to turn on without specifying a path, return.
            if (enabled && m_file_watcher.Path.Equals(""))
            {
                return;
            }
            // Switch event raising.
            m_file_watcher.EnableRaisingEvents = enabled;
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

        /* Change which folder is synced with */
        protected void OnSyncFolderSelect(object args, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                ChangeDestinationPath(folderDlg.SelectedPath);
            }
        }

        /* Change which folder is being watched */
        protected void OnWatchedFolderSelect(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            DialogResult result = folderDlg.ShowDialog();
            if (result == DialogResult.OK)
            {
                ChangeWatchedPath(folderDlg.SelectedPath);
            }
        }

        /* Retrieve the path stored in the paths.sav file */
        protected void ReadWatchedPath()
        {
            FileSyncXMLParser parser = new FileSyncXMLParser();
            m_watched_path = parser.ReadFile(SAVE_FILE_NAME);
        }

        protected void WriteWatchedPath()
        {
            FileSyncXMLParser parser = new FileSyncXMLParser();
            parser.WriteFile(SAVE_FILE_NAME, m_watched_path);
        }

        /* Changes the path to the folder being watched by the file watcher */
        protected void ChangeWatchedPath(String filePath)
        {
            // Stop watching for file changes
            SwitchFileWatcher(false);
            FileSyncXMLParser parser = new FileSyncXMLParser();
            
            // For now, just overwrite the path and save
            m_watched_path.Path = m_file_watcher.Path = filePath;
            parser.WriteFile(SAVE_FILE_NAME, m_watched_path);

            SwitchFileWatcher(true);
        }

        /* Changes the destination path */
        protected void ChangeDestinationPath(string filePath)
        {
            // Stop watching for file changes
            SwitchFileWatcher(false);
            FileSyncXMLParser parser = new FileSyncXMLParser();

            // For now, just overwrite the destination and save
            m_watched_path.Destinations.Clear();
            m_watched_path.Destinations.Add(filePath);
            parser.WriteFile(SAVE_FILE_NAME, m_watched_path);

            SwitchFileWatcher(true);
        }

        /* When a file has changed */
        protected void OnChanged(object sender, FileSystemEventArgs e)
        {
            // Sync with the destination folder
        }

        /* When a file is renamed */
        protected void OnRenamed(object sender, EventArgs e)
        {
            // Rename the file in the destination folder            
        }
    }
}
