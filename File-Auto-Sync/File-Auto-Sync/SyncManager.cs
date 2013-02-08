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
        /* Takes the starting path and destination and copies the file */
        /// <summary>
        /// Copies a file from one location to another provided it exists
        /// </summary>
        /// <param name="startPath">The absolute start path</param>
        /// <param name="destPath">The absolute end path</param>
        /// <param name="filename">The relative name of the file</param>
        public static void CopyFile(string startPath, string destPath, string fileName)
        {
            string destFile = System.IO.Path.Combine(destPath, fileName);

            // Create the destination directory if it doesn't exist
            if (!System.IO.Directory.Exists(destPath))
            {
                System.IO.Directory.CreateDirectory(destPath);
            }
            System.IO.File.Copy(startPath, destFile, true);
           
        }

        /// <summary>
        /// Deletes the file at destPath provided it exists
        /// </summary>
        /// <param name="destPath">The absolute path of removal</param>
        /// <param name="filename">The relative filename</param>
        public static void DeleteFile(string destPath, string filename)
        {
            string file = System.IO.Path.Combine(destPath, filename);
            if (System.IO.File.Exists(file))
            {
                try
                {
                    System.IO.File.Delete(file);
                }
                catch (Exception exc)
                {
                    Console.WriteLine(exc.Message);
                }
            }
        }

        /// <summary>
        /// Renames the item at destPath
        /// </summary>
        /// <param name="destPath">absolute path to the file</param>
        /// <param name="oldName">relative old name of the file</param>
        /// <param name="name">what to rename the item to</param>
        public static void Rename(string destPath, string oldName, string name)
        {
            string oldFile = System.IO.Path.Combine(destPath, oldName);
            string file = System.IO.Path.Combine(destPath, name);
            if (File.Exists(oldFile))
            {
                File.Copy(oldFile, file);
                File.Delete(oldFile);
            }
        }
    }
}
