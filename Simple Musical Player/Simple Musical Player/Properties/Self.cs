using Simple_Musical_Player.Models;
using System.Collections.Generic;
using System.Reflection;

namespace Simple_Musical_Player.Properties
{
    /// <summary>
    /// Class that contains necessery player parts and variables
    /// </summary>
    public static class Self
    {
        /// <summary>
        /// Conatains current programm location on the PC
        /// </summary>
        public static string Path = System.IO.Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Dictionary mapping playlist(name) to List of the songs(models).
        /// </summary>
        public static Dictionary<string, List<AudioFileModel>> PlaylistToSongListMap { get; set; } = new Dictionary<string, List<AudioFileModel>>();

        /// <summary>
        /// 
        /// </summary>
        public static LanguageModel LanguageModelInstance = new LanguageModel();

    }
}
