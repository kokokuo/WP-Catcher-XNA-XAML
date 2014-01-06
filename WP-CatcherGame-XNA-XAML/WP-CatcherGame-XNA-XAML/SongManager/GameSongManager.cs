using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using CatcherGame;
using Microsoft.Xna.Framework;
using WP_CatcherGame_XNA_XAML;
using Microsoft.Xna.Framework.Media;
namespace CatcherGame.SongManager
{
    //Play background song
    public class GameSongManager
    {
        private GamePage mainGame;
        private Dictionary<GameSongKeyEnum,Song> _dictionary;

        public GameSongManager(GamePage mainGame)
        {
            Debug.WriteLine("Game Song Manager construct ...");
            this.mainGame = mainGame;
            _dictionary = new Dictionary<GameSongKeyEnum, Song>();

            LoadMenuBackSong();
        }

        /// <summary>
        /// 取得對應的Key的文字資源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public Song GetGameBackgrounSongFromKey(GameSongKeyEnum key)
        {

            return _dictionary[key];
        }

        private void LoadMenuBackSong()
        {
            GameSongKeyEnum key = GameSongKeyEnum.MENU_BACKGOUND_SONG;
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, mainGame.GetContentManager.Load<Song>("Song/drum_mp3"));
            }
        }
    }
}
