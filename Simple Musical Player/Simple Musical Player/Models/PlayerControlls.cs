using System.Collections.Generic;

namespace Simple_Musical_Player.Models
{
    public static class PlayerControlls
    {
        /// <summary>
        /// File name filter. Cleans the file name from unnecessary elements
        /// </summary>
        /// <param name="name">File name</param>
        /// <returns>Cleaned name</returns>
        public static string NameFilter(string name)
        {
            // List of file formats to be removed from the name
            List<string> DeletingFormats = new List<string>()
            {
                ".mp3",
                ".wav",
                ".ogg",
                ".playlist",
            };

            // Iterating through each format and removing it from the name
            foreach (string to_replace in DeletingFormats)
            {
                name = name.Replace(to_replace, "");
            }
            return name; // Returning the filtered name
        }
    }
}
