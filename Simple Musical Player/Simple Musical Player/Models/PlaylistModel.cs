using System.Collections.Generic;

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
        public string PlaylistName;

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
