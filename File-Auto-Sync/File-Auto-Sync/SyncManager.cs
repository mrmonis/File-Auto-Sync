using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace File_Auto_Sync
{
    /// <summary>
    /// Responsible for syncing of all renamed, modified and deleted files
    /// </summary>
    class SyncManager
    {
        public static void CopyFile(string startPath, string destPath)
        {
            // Create the directory if it doesn't exist
            if (!System.IO.Directory.Exists(destPath))
            {
                System.IO.Directory.CreateDirectory(destPath);
            }

            // Copy to the new directory
            if (System.IO.Directory.Exists(startPath))
            {
                System.IO.File.Copy(startPath, destPath, true);
            }
        }
    }
}
