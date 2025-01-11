using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Simple_Musical_Player.Models
{
    public class PlaylistModel
    {
        /// <summary>
        /// List of songs that are included in to playlist
        /// </summary>
        public List<AudioFileModel> RelatedFilesList = new List<AudioFileModel>();

        /// <summary>
        /// Playlist name used by frontend
        /// </summary>
        public string PlaylistName = null;

        /// <summary>
        /// Playlist id in ListBox
        /// </summary>
        public int Id = 0;

        public PlaylistModel(int id, string name)
        {
            this.Id = id > 0 ? id : -1;
            this.PlaylistName = name;
        }

        public PlaylistModel(int id, string name, List<AudioFileModel> songList)
        {
            this.Id = id > 0 ? id : -1;
            this.PlaylistName = name;
            this.RelatedFilesList = songList;
        }
    }  
}
