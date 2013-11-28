using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using CatcherGame;
using Microsoft.Xna.Framework;
using WP_CatcherGame_XNA_XAML;
namespace CatcherGame.FontManager
{
    public class SpriteFontManager
    {
        private GamePage mainGame;
        private Dictionary<SpriteFontKeyEnum, SpriteFont> _dictionary;

        public SpriteFontManager(GamePage mainGame)
        {
            Debug.WriteLine("SpriteFontManager construct ...");
            this.mainGame = mainGame;
            _dictionary = new Dictionary<SpriteFontKeyEnum, SpriteFont>();
            //載入文字資源
            LoadTopScoreFont();
            LoadPlaySavedPeopleNumberFont();
            LoadPlayLostPeopleNumberFont();
            LoadGameOverCurrentSavedPeopleNumberFont();
        }

        /// <summary>
        /// 取得對應的Key的文字資源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SpriteFont GetSpriteFontFromKey(SpriteFontKeyEnum key) {

            return _dictionary[key];
        }

        private void LoadTopScoreFont(){
            SpriteFontKeyEnum key = SpriteFontKeyEnum.TOP_SCORE_FONT;
             if (!_dictionary.ContainsKey(key)){
                 _dictionary.Add(key, mainGame.GetContentManager.Load<SpriteFont>("TopScoreFont"));
             }
        }


        private void LoadPlaySavedPeopleNumberFont()
        {
            SpriteFontKeyEnum key = SpriteFontKeyEnum.PLAY_SAVED_PEOPLE_FONT;
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, mainGame.GetContentManager.Load<SpriteFont>("SavedPeopleFont"));
            }
        }
        private void LoadGameOverCurrentSavedPeopleNumberFont()
        {
            SpriteFontKeyEnum key = SpriteFontKeyEnum.GAME_VOER_CURRENT_SAVED_PEOPLE_FONT;
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, mainGame.GetContentManager.Load<SpriteFont>("CurrentScoreFont"));
            }
        }

        private void LoadPlayLostPeopleNumberFont()
        {
            SpriteFontKeyEnum key = SpriteFontKeyEnum.PLAT_LOST_PEOPLE_FONT;
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, mainGame.GetContentManager.Load<SpriteFont>("LostPeopleFont"));
            }
        }
    }
}
