using Microsoft.Win32;
using Musical_Player.Views;
using Simple_Musical_Player.Properties;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace Simple_Musical_Player.Models
{
    public class PlaylistService
    {
        /// <summary>
        /// Creates a new playlist with the specified user name
        /// </summary>
        public static void CreatePlaylist()
        {
            // Open a dialog to get the playlist name from user
            var nameDialog = new NameDialog();
            bool? dialogResult = nameDialog.ShowDialog();

            // If the user confirms the dialog, proceed to create the playlist
            if (dialogResult == true)
            {

                // Use the user-entered name if available, otherwise use the default name
                var playlistName = nameDialog.InputTextbox.Text.Trim();

                XDocument xDocument = XDocument.Load(Path.Combine(Self.Path, "Playlists.xml"));
                if (xDocument.Root.Elements("playlist").Any(element => element.Attribute("name").Value == playlistName))
                {
                    MessageBox.Show("Playlist with the same name is already exists!");
                    return;
                }
                XElement newPlaylistElement = new XElement("playlist", new XAttribute("name", playlistName));
                xDocument.Root.Add(newPlaylistElement);
                xDocument.Save(Path.Combine(Self.Path, "Playlists.xml"));
            }
        }

        /// <summary>
        /// Adds a new song to the playlist
        /// </summary>
        /// <param name="playlistName">Playlist name</param>
        public static void AddSongToPlaylist(string playlistName)
        {
            // Open a dialog to choose songs
            OpenFileDialog opnFileDlg = new OpenFileDialog();
            opnFileDlg.Filter = "(mp3),(wav),(ogg)|*.mp3;*.wav;*.ogg;";
            opnFileDlg.Multiselect = true;

            // If files are selected, add their paths to the playlist file
            if (opnFileDlg.ShowDialog() == true)
            {
                XDocument xDocument = XDocument.Load(Path.Combine(Self.Path, "Playlists.xml"));

                XElement playlistElement = xDocument.Root.Elements("playlist").FirstOrDefault(element => element.Attribute("name").Value == playlistName);

                foreach (string filePath in opnFileDlg.FileNames)
                {
                    AudioFileModel newSong = new AudioFileModel()
                    {
                        FileName = Path.GetFileName(filePath),
                        FilePath = filePath,
                        Id = playlistElement.Elements().Count()
                    };

                    if (!playlistElement.Elements("song").Any(element => element.Attribute("id").Value == newSong.Id.ToString()))
                    {
                        XElement newSongElement = new XElement("song",
                        new XAttribute("id", newSong.Id),
                        new XAttribute("name", newSong.FileName),
                        new XAttribute("path", newSong.FilePath));

                        playlistElement.Add(newSongElement);
                    }
                }
                xDocument.Save(Path.Combine(Self.Path, "Playlists.xml"));
            }
        }

        /// <summary>
        /// Deletes a song from the playlist
        /// </summary>
        /// <param name="playlistName">Playlist name</param>
        /// <param name="songIndex">Song name</param>
        public static void DeleteSong(string playlistName, int songIndex)
        {
            XDocument xDocument = XDocument.Load(Path.Combine(Self.Path, "Playlists.xml"));

            XElement playlistElement = xDocument.Root.Elements("playlist")
                .FirstOrDefault(element => element.Attribute("name").Value == playlistName);

            if (playlistElement != null)
            {
                XElement songElement = playlistElement.Elements("song")
                    .FirstOrDefault(element => element.Attribute("id").Value == songIndex.ToString());

                songElement?.Remove();

                int newIndex = 0;
                foreach (XElement remainingSong in playlistElement.Elements("song"))
                {
                    remainingSong.Attribute("id").Value = newIndex.ToString();
                    newIndex++;
                }
            }

            xDocument.Save(Path.Combine(Self.Path, "Playlists.xml"));
        }

        /// <summary>
        /// Deletes a playlist
        /// </summary>
        /// <param name="playlistName">Playlist index</param>
        public static void DeletePlaylist(string playlistName)
        {
            XDocument xDocument = XDocument.Load(Path.Combine(Self.Path, "Playlists.xml"));

            XElement playlistElement = xDocument.Root.Elements("playlist")
                .FirstOrDefault(element => element.Attribute("name").Value == playlistName);

            playlistElement.Remove();

            xDocument.Save(Path.Combine(Self.Path, "Playlists.xml"));
        }


        /// <summary>
        /// Moves a song up or down in the playlist
        /// </summary>
        /// <param name="songIndex">Song index in the list</param>
        /// <param name="playlistName">Playlist index</param>
        /// <param name="direction">Move direction (down = 0, up = 1)</param>
        public static void MoveSongInQueue(int songIndex, string playlistName, SongRelocationDirections direction)
        {
            // Calculate the new index based on the specified direction
            int newIndex = direction == SongRelocationDirections.DOWN ? songIndex + 1 : songIndex - 1;
            XDocument xDocument = XDocument.Load(Path.Combine(Self.Path, "Playlists.xml"));

            XElement playlistElement = xDocument.Root.Elements("playlist").FirstOrDefault(element => element.Attribute("name").Value == playlistName);

            // Check if the new index is within valid bounds
            if (newIndex >= 0 && newIndex < playlistElement.Elements("song").Count())
            {
                if (playlistElement != null)
                {
                    XElement songElement = playlistElement.Elements("song").FirstOrDefault(element => element.Attribute("id").Value == songIndex.ToString());

                    if (songElement != null)
                    {

                        XElement nextSongElement = playlistElement.Elements("song").FirstOrDefault(element => element.Attribute("id").Value == newIndex.ToString());

                        if (nextSongElement != null)
                        {
                            songElement.Attribute("id").Value = newIndex.ToString();
                            nextSongElement.Attribute("id").Value = songIndex.ToString();

                            playlistElement.ReplaceNodes(playlistElement.Elements("song").OrderBy(element => int.Parse(element.Attribute("id").Value)));

                            xDocument.Save(Path.Combine(Self.Path, "Playlists.xml"));
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Gets the names of playlists
        /// </summary>
        /// <returns>List of playlists</returns>
        public static List<string> GetPlaylists()
        {
            bool erroredPath = false;
            Dictionary<string, List<AudioFileModel>> playlists = new Dictionary<string, List<AudioFileModel>>();

            XDocument xDocument = XDocument.Load(Path.Combine(Self.Path, "Playlists.xml"));

            foreach (var playlistElement in xDocument.Root.Elements("playlist"))
            {
                string playlistName = playlistElement.Attribute("name").Value;
                List<AudioFileModel> songs = playlistElement.Elements("song")
                    .Select(songElement => new AudioFileModel
                    {
                        Id = int.Parse(songElement.Attribute("id").Value),
                        FilePath = songElement.Attribute("path").Value,
                        FileName = songElement.Attribute("name").Value
                    })
                    .ToList();

                playlists.Add(playlistName, songs);
            }

            foreach (var key in playlists.Keys)
            {
                for (int i = 0; i < playlists[key].Count(); i++)
                {
                    if (!ValidatePath(playlists[key][i].FilePath))
                    {
                        DeleteSong(key, i);
                        erroredPath = true;
                    }
                }
            }

            if (erroredPath)
            {
                return GetPlaylists();
            }

            Self.PlaylistToSongListMap = playlists;
            return playlists.Keys.ToList();
        }

        /// <summary>
        /// Checks if a song exists at the given path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>True if the song exists. False if the song does not exist</returns>
        public static bool ValidatePath(string path)
        {
            return File.Exists(path);
        }

        /// <summary>
        /// Adds a song to the playlist from a file. Used for Drag and Drop.
        /// </summary>
        /// <param name="path">Path to the song</param>
        /// <param name="playlistName">Name of the playlist to which to add the song</param>
        public static void AddSong(string path, string playlistName)
        {
            // Check the path and add the song to the playlist
            if (!ValidatePath(path))
            {
                return;
            }

            XDocument xDocument = XDocument.Load(Path.Combine(Self.Path, "Playlists.xml"));

            XElement playlistElement = xDocument.Root.Elements("playlist").FirstOrDefault(element => element.Attribute("name").Value == playlistName);

            AudioFileModel newSong = new AudioFileModel()
            {
                FileName = Path.GetFileName(path),
                FilePath = path,
                Id = playlistElement.Elements().Count()
            };

            if (!playlistElement.Elements("song").Any(element => element.Attribute("id").Value == newSong.Id.ToString()))
            {
                XElement newSongElement = new XElement("song",
                new XAttribute("id", newSong.Id),
                new XAttribute("name", newSong.FileName),
                new XAttribute("path", newSong.FilePath));

                playlistElement.Add(newSongElement);
            }

            xDocument.Save(Path.Combine(Self.Path, "Playlists.xml"));
        }
    }

    public enum SongRelocationDirections
    {
        DOWN,
        UP
    }
}
