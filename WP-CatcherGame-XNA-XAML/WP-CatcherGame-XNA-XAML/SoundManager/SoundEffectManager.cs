using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using System.Diagnostics;
using CatcherGame;
using Microsoft.Xna.Framework;
using WP_CatcherGame_XNA_XAML;
using Microsoft.Xna.Framework.Audio;
namespace CatcherGame.SoundManager
{
    public class SoundEffectManager
    {
          private GamePage mainGame;
        private Dictionary<SoundEffectKeyEnum,SoundEffect> _dictionary;

        public SoundEffectManager(GamePage mainGame)
        {
            Debug.WriteLine("Game Song Manager construct ...");
            this.mainGame = mainGame;
            _dictionary = new Dictionary<SoundEffectKeyEnum, SoundEffect>();

            LoadClickSound();
            LoadCaughtSound();
        }

        /// <summary>
        /// 取得對應的Key的文字資源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SoundEffect GetSoundEffectFromKey(SoundEffectKeyEnum key)
        {

            return _dictionary[key];
        }

        private void LoadClickSound()
        {
            SoundEffectKeyEnum key = SoundEffectKeyEnum.CLICK_SOUND;
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, mainGame.GetContentManager.Load<SoundEffect>("Sound/click-1"));
            }
        }

        private void LoadCaughtSound()
        {
            SoundEffectKeyEnum key = SoundEffectKeyEnum.CAUGHT_SOUND;
            if (!_dictionary.ContainsKey(key))
            {
                _dictionary.Add(key, mainGame.GetContentManager.Load<SoundEffect>("Sound/caught-1"));
            }
        }
    }
}
