using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace File_Auto_Sync
{
    /// <summary>
    /// Deals with reading and writing the XML file
    /// <example>
    /// Data is stored in XML in the following format:
    ///     <Start>/some/path/name
    ///         <Destination>/first/destination/path/name</Destination>
    ///         <Destination>/second/destination/path/name</Destination>
    ///         ...
    ///         <Destination>/nth/destination/path/name</Destination>
    ///     </Start>
    /// </example>
    /// Currently only one Start and Dest node are read at a time
    /// </summary>
    class FileSyncXMLParser
    {
        /* Attempts to open the file at filepath and read the XML */
        public WatchedPath ReadFile(string filepath)
        {
            XmlTextReader textReader = new XmlTextReader(filepath);
            WatchedPath path = new WatchedPath();

            // Read in the data
            while (textReader.Read())
            {
                if (textReader.NodeType == XmlNodeType.Element && textReader.Name == "Start")
                {
                    path.Path = textReader.GetAttribute("path");

                    while (textReader.NodeType != XmlNodeType.EndElement)
                    {
                        textReader.Read();
                        if (textReader.Name == "Destination")
                        {
                            path.Destinations.Add(textReader.GetAttribute("path"));
                        }
                    }
                }
            }
            // Close the reader
            textReader.Close();

            return path;
        }

        public bool WriteFile(string filepath, WatchedPath path)
        {
            XmlTextWriter textWriter = new XmlTextWriter(filepath, null);
            // Write the start of the document
            textWriter.WriteStartDocument();

            // Write the starting name
            textWriter.WriteStartElement("Start");
            textWriter.WriteAttributeString("path", path.Path);
            foreach (string dest in path.Destinations) 
            {
                textWriter.WriteStartElement("Destination");
                textWriter.WriteAttributeString("path", dest);
                textWriter.WriteEndElement();
            }
            textWriter.WriteEndElement();

            // End the document and close
            textWriter.WriteEndDocument();
            textWriter.Close();

            return true;
        }
    }
}
