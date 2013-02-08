using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace File_Auto_Sync
{
    /// <summary>
    /// A wrapper for a filepath being watched
    /// </summary>
    public class WatchedPath
    {
        public List<string> Destinations { get; set; }
        public string Path { get; set; }

        public WatchedPath()
        {
            Destinations = new List<string>();
            Path = "";
        }
    }
}
