using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Musical_Player.Models
{
    public class AudioFileModel
    {
        /// <summary>
        /// Name of audio file
        /// </summary>
        public string FileName = string.Empty;

        /// <summary>
        /// Id of song in playlist
        /// </summary>
        public int Id = 0;

        /// <summary>
        /// Path to audio file
        /// </summary>
        public string FilePath = string.Empty;

    }
}
