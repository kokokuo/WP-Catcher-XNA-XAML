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
namespace CatcherGame.TextureManager
{
    /// <summary>
    /// 圖片管理器
    /// </summary>
    class Texture2DManager
    {
        private GamePage mainGame;
        private Dictionary<TexturesKeyEnum, List<Texture2D>> _dictionary;

        public Texture2DManager(GamePage mainGame)
        {
            Debug.WriteLine("Texture2DManager construct ...");
            this.mainGame = mainGame;
            _dictionary = new Dictionary<TexturesKeyEnum, List<Texture2D>>();

            Debug.WriteLine("Load Texture2Ds ...");
            
            ///Load圖片
            
            //載入選單相關圖像資源
            LoadMenuBackground();
            LoadMenuSide();
            LoadPlayButton();
            LoadHowToPlayButton();
            LoadTopScoreButton();
            LoadDictionaryButton();

            //載入對話框關閉用的按鈕
            LoadCloseDialog();

            //載入最高分對話框所用的圖像資源
            LoadTopScoreDialogBackground();
            LoadTopScoreDialogCloseButton();


            //載入開頭動畫
            LoadGameStartComic();

            //載入遊戲中的資源
            LoadPlayBackground();
            LoadPlayFloor();
            LoadPlayGamePauseButton();
            LoadPlayGameLeftMoveButton();
            LoadPlayGameRightMoveButton();
            //消防員
            LoadPlayGameFiremanLeft();
            LoadPlayGameFiremanRight();

            LoadPlayGameSmokePicture();
            LoadPlayGameScorePicture();
            LoadPlayGameLifePicture();
            //網子
            LoadPlayNetNormal();
            LoadPlayNetSmall();
            LoadPlayNetLarge();
            
            LoadPlayDie();
            LoadPlayFlyOldLady();
            LoadPlayFatDance();
            LoadPlayLittleGirl();
            LoadPlayManStubble();
            LoadPlayNaughtyBoy();
            LoadPlayOldMan();
            LoadPlayRoxanne();
            //道具
            LoadPlayDropItemBoostingShoes();
            LoadPlayDropItemSlowShoes();
            LoadPlayDropItemLifeHeart();
            LoadPlayDropItemNetExpander();
            LoadPlayDropItemNetShrinker();
            LoadPlayDropItemStrongSmoke();

            //載入遊戲中的暫停對話框
            LoadPauseDialogBackground();
            LoadPauseDialogExitButton();
            LoadPauseDialogContinueButton();

            //載入GameOver元件
            LoadGameOverBackground();
            LoadGameOverMenuButton();
            LoadGameOverAgainButton();
            LoadGameOverCharacterShow();

            //載入Dialog中共用元件
            LoadDialogLeftButton();
            LoadDialogRightButton();

            //載入字典對話框所用的圖像資源
            LoadDictionaryDialogNoTexture();
            LoadDictionaryDialogBackground();
            LoadDictionaryDialogCloseButton();
            LoadDictionaryDialogLeftButton();
            LoadDictionaryDialogRightButton();
            LoadDictionaryDialogLittleGirlTexture();
            LoadDictionaryDialogLittleGirlIntroTexture();
            LoadDictionaryDialogFatDancerTexture();
            LoadDictionaryDialogFatDancerIntroTexture();
            LoadDictionaryDialogFlyOldLadyTexture();
            LoadDictionaryDialogFlyOldLadyIntroTexture();
            LoadDictionaryDialogManStubbleTexture();
            LoadDictionaryDialogManStubbleIntroTexture();
            LoadDictionaryDialogNaughtyBoyTexture();
            LoadDictionaryDialogNaughtyBoyIntroTexture();
            LoadDictionaryDialogOldManTexture();
            LoadDictionaryDialogOldManIntroTexture();
            LoadDictionaryDialogRoxanneTexture();
            LoadDictionaryDialogRoxanneIntroTexture();

            //載入HOWTOPLAY所用的圖像資源
            LoadHowtoplayDialogBackground();
            LoadHowtoplayDialogCloseButton();
            LoadHowtoplayDialogPage1();
            LoadHowtoplayDialogPage2();
            LoadHowtoplayDialogPage3();

            Debug.WriteLine("Load Texture2Ds Done ");
        }
        /// <summary>
        /// 取得對應Key值的圖片組
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<Texture2D> GetTexture2DList(TexturesKeyEnum key)
        {
            return _dictionary[key];
        }

        public static Texture2D Flip(Texture2D source, bool vertical, bool horizontal)
        {
            Texture2D flipped = new Texture2D(source.GraphicsDevice, source.Width, source.Height);
            Color[] data = new Color[source.Width * source.Height];
            Color[] flippedData = new Color[data.Length];

            source.GetData<Color>(data);

            for (int x = 0; x < source.Width; x++)
                for (int y = 0; y < source.Height; y++)
                {
                    int idx = (horizontal ? source.Width - 1 - x : x) + ((vertical ? source.Height - 1 - y : y) * source.Width);
                    flippedData[x + y * source.Width] = data[idx];
                }

            flipped.SetData<Color>(flippedData);

            return flipped;
        }

      
        //選單------------------------------------------------------------------------------

        //開始遊戲按鈕
        private void LoadPlayButton() { 
             TexturesKeyEnum key = TexturesKeyEnum.MENU_PLAY_BUTTON;
             if (!_dictionary.ContainsKey(key)){
                 List<Texture2D> texture2Ds = new List<Texture2D>();
                 texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Menu/start"));
                 _dictionary.Add(key, texture2Ds);
             }
        }

        private void LoadHowToPlayButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.MENU_HOW_TO_PLAY_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Menu/how"));
                _dictionary.Add(key, texture2Ds);
            }
        }


        private void LoadTopScoreButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.MENU_TOP_SCORE_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Menu/top"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        private void LoadDictionaryButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.MENU_DICTIONARY_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Menu/dictionary"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //選單
        private void LoadMenuBackground()
        {
            TexturesKeyEnum key = TexturesKeyEnum.MENU_BACKGROUND;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Menu/menu_back"));
                _dictionary.Add(key, texture2Ds);
            }
        }
        //選單
        private void LoadMenuSide()
        {
            TexturesKeyEnum key = TexturesKeyEnum.MENU_SIDE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();

                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Menu/side"));
                _dictionary.Add(key, texture2Ds);
            }
        }
        //對話框的關閉按鈕
        private void LoadCloseDialog() {
            TexturesKeyEnum key = TexturesKeyEnum.DIALOG_CLOSE_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("close"));
                _dictionary.Add(key, texture2Ds);
            }
        }
        //最高分的對話框背景
        private void LoadTopScoreDialogBackground()
        {
            TexturesKeyEnum key = TexturesKeyEnum.TOP_SCORE_DIALOG_BACK;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("TopScore/top_score_back"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //最高分的關閉按鈕
        private void LoadTopScoreDialogCloseButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.TOP_SCORE_CLOSE_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("close"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //遊戲開場動畫----------------------------------------------------------------
        private void LoadGameStartComic()
        {
            TexturesKeyEnum key = TexturesKeyEnum.GAME_START_COMIC_BACK;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("StartComic/comic800"));
                _dictionary.Add(key, texture2Ds);
            }

        }

        //遊戲中元件--------------------------------------------------------

        //遊戲中的背景
        private void LoadPlayBackground() {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_BACKGROUND;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/play_back"));
                _dictionary.Add(key, texture2Ds);
            }
        
        }
        //遊戲中的背景
        private void LoadPlayFloor()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_FLOOR;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/floor"));
                _dictionary.Add(key, texture2Ds);
            }

        }
        //遊戲中的煙霧
        private void LoadPlayGameSmokePicture() {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_SMOKE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/smoke"));
                _dictionary.Add(key, texture2Ds);
            }

        }
        //遊戲中的濃煙霧
        private void LoadPlayGameStrongSmokePicture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_STRONG_SMOKE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/smoke_strong"));
                _dictionary.Add(key, texture2Ds);
            }

        }


        //遊戲中的救援失敗剩餘次數圖示
        private void LoadPlayGameLifePicture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_LIFE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/life_fix"));
                _dictionary.Add(key, texture2Ds);
            }

        }
        
        //遊戲中的分數圖示
        private void LoadPlayGameScorePicture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_SCORE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/score_fix"));
                _dictionary.Add(key, texture2Ds);
            }

        }
       
        //遊戲中的暫停鈕
        private void LoadPlayGamePauseButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_PAUSE_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/pause_bigger"));
                _dictionary.Add(key, texture2Ds);
            }

        }

        //遊戲中的左鍵
        private void LoadPlayGameLeftMoveButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_LEFT_MOVE_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/left_button"));
                _dictionary.Add(key, texture2Ds);
            }

        }

        //遊戲中的右鍵
        private void LoadPlayGameRightMoveButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_RIGHT_MOVE_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/right_button"));
                _dictionary.Add(key, texture2Ds);
            }

        }

        //遊戲中的消防員(左邊)
        private void LoadPlayGameFiremanLeft()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_FIREMAN_LEFT;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fireman_left1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fireman_left2"));
                _dictionary.Add(key, texture2Ds);
            }

        }
        //遊戲中的消防員(右邊)
        private void LoadPlayGameFiremanRight()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_FIREMAN_RIGHT;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fireman_right1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fireman_right2"));
                _dictionary.Add(key, texture2Ds);
            }

        }

        //遊戲中的救人網子(正常)
        private void LoadPlayNetNormal()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_NET_NORMAL;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/net_mid"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/net_mid_caught"));
                _dictionary.Add(key, texture2Ds);
            }

        }

        //遊戲中的救人網子(縮小)
        private void LoadPlayNetSmall()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_NET_SMALL;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/net_small"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/net_small_caught"));
                _dictionary.Add(key, texture2Ds);
            }

        }

        //遊戲中的救人網子(放大)
        private void LoadPlayNetLarge()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_NET_LARGE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/net_big"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/net_big_caught"));
                _dictionary.Add(key, texture2Ds);
            }

        }


        //遊戲中掉落的放大網子道具
        private void LoadPlayDropItemNetExpander()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_ITEM_NET_EXPANDER;

            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/net_expander"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/disappear"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //遊戲中掉落的縮小網子道具
        private void LoadPlayDropItemNetShrinker()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_ITEM_NET_SHRINKER;

            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/net_shrinker"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/disappear"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //遊戲中加速鞋
        private void LoadPlayDropItemBoostingShoes()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_ITEM_BOOSTING_SHOES;

            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/boosting_shoes"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/disappear"));
                _dictionary.Add(key, texture2Ds);
            }
        }


        //遊戲中的愛心
        private void LoadPlayDropItemLifeHeart()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_ITEM_HEART;

            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/heart"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/disappear"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //遊戲中的濃霧
        private void LoadPlayDropItemStrongSmoke()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_ITEM_STRONG_SMOKE;

            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/smoke"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/disappear"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //遊戲中降速
        private void LoadPlayDropItemSlowShoes()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_ITEM_SLOW_SHOES;

            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/slow"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/Item/disappear"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //遊戲中死亡的符號
        private void LoadPlayDie() {
            TexturesKeyEnum key = TexturesKeyEnum.PLAY_DIE;

            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/die"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/die_2"));
                _dictionary.Add(key, texture2Ds);
            }
        }
        //遊戲中的老婦人
        private void LoadPlayFlyOldLady()
        {
            TexturesKeyEnum fallkey = TexturesKeyEnum.PLAY_FLYOLDELADY_FALL;
            if (!_dictionary.ContainsKey(fallkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/flyoldlady/flyoldlady_fall1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/flyoldlady/flyoldlady_fall2"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/flyoldlady/flyoldlady_fall3"));
                _dictionary.Add(fallkey, texture2Ds);
            }

            TexturesKeyEnum catchkey = TexturesKeyEnum.PLAY_FLYOLDELADY_CAUGHT;
            if (!_dictionary.ContainsKey(catchkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/flyoldlady/flyoldlady_caught"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/flyoldlady/flyoldlady_floor"));
                _dictionary.Add(catchkey, texture2Ds);
            }


            TexturesKeyEnum walkkey = TexturesKeyEnum.PLAY_FLYOLDELADY_WALK;
            if (!_dictionary.ContainsKey(walkkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/flyoldlady/flyoldlady_run1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/flyoldlady/flyoldlady_run2"));
                _dictionary.Add(walkkey, texture2Ds);
            }
        }
        

        //遊戲中的胖胖舞者
        private void LoadPlayFatDance()
        {
            TexturesKeyEnum fallkey = TexturesKeyEnum.PLAY_FATDANCE_FALL;
            if (!_dictionary.ContainsKey(fallkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fatdance/fatdancerfall1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fatdance/fatdancerfall2"));
                _dictionary.Add(fallkey, texture2Ds);
            }

            TexturesKeyEnum catchkey = TexturesKeyEnum.PLAY_FATDANCE_CAUGHT;
            if (!_dictionary.ContainsKey(catchkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fatdance/fatdancercaught1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fatdance/fatdancercaught2"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fatdance/fatdancerfloor"));
                _dictionary.Add(catchkey, texture2Ds);
            }


            TexturesKeyEnum walkkey = TexturesKeyEnum.PLAY_FATDANCE_WALK;
            if (!_dictionary.ContainsKey(walkkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fatdance/fatdancerrun1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/fatdance/fatdancerrun2"));
                _dictionary.Add(walkkey, texture2Ds);
            }
        }

        //遊戲中的小女孩
        private void LoadPlayLittleGirl()
        {
            TexturesKeyEnum fallkey = TexturesKeyEnum.PLAY_LITTLEGIRL_FALL;
            if (!_dictionary.ContainsKey(fallkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/littlegirl/littlegirlfall1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/littlegirl/littlegirlfall2"));
                _dictionary.Add(fallkey, texture2Ds);
            }

            TexturesKeyEnum catchkey = TexturesKeyEnum.PLAY_LITTLEGIRL_CAUGHT;
            if (!_dictionary.ContainsKey(catchkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/littlegirl/littlegirlhavecaught"));
                _dictionary.Add(catchkey, texture2Ds);
            }


            TexturesKeyEnum walkkey = TexturesKeyEnum.PLAY_LITTLEGIRL_WALK;
            if (!_dictionary.ContainsKey(walkkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/littlegirl/littlegirlrun1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/littlegirl/littlegirlrun2"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/littlegirl/littlegirlrun3"));
                _dictionary.Add(walkkey, texture2Ds);
            }
        }

        //遊戲中的男人
        private void LoadPlayManStubble()
        {
            TexturesKeyEnum fallkey = TexturesKeyEnum.PLAY_MANSTUBBLE_FALL;
            if (!_dictionary.ContainsKey(fallkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/manstubble/manstubblefall1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/manstubble/manstubblefall2"));
                _dictionary.Add(fallkey, texture2Ds);
            }

            TexturesKeyEnum catchkey = TexturesKeyEnum.PLAY_MANSTUBBLE_CAUGHT;
            if (!_dictionary.ContainsKey(catchkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/manstubble/manstubblehavecaught"));
                _dictionary.Add(catchkey, texture2Ds);
            }


            TexturesKeyEnum walkkey = TexturesKeyEnum.PLAY_MANSTUBBLE_WALK;
            if (!_dictionary.ContainsKey(walkkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/manstubble/manstubblerun1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/manstubble/manstubblerun2"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/manstubble/manstubblerun3"));
                _dictionary.Add(walkkey, texture2Ds);
            }
        }

        //遊戲中的小男孩
        private void LoadPlayNaughtyBoy()
        {
            TexturesKeyEnum fallkey = TexturesKeyEnum.PLAY_NAUGHTYBOY_FALL;
            if (!_dictionary.ContainsKey(fallkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/naughtyboy/naughtyboy_fall1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/naughtyboy/naughtyboy_fall2"));
                _dictionary.Add(fallkey, texture2Ds);
            }

            TexturesKeyEnum catchkey = TexturesKeyEnum.PLAY_NAUGHTYBOY_CAUGHT;
            if (!_dictionary.ContainsKey(catchkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/naughtyboy/naughtyboy_caught"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/naughtyboy/naughtyboy_floor"));
                _dictionary.Add(catchkey, texture2Ds);
            }


            TexturesKeyEnum walkkey = TexturesKeyEnum.PLAY_NAUGHTYBOY_WALK;
            if (!_dictionary.ContainsKey(walkkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/naughtyboy/naughtyboy_run1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/naughtyboy/naughtyboy_run2"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/naughtyboy/naughtyboy_run3"));
                _dictionary.Add(walkkey, texture2Ds);
            }
        }

        //遊戲中的老人
        private void LoadPlayOldMan()
        {
            TexturesKeyEnum fallkey = TexturesKeyEnum.PLAY_OLDMAN_FALL;
            if (!_dictionary.ContainsKey(fallkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/oldman/oldmanfall1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/oldman/oldmanfall2"));
                _dictionary.Add(fallkey, texture2Ds);
            }

            TexturesKeyEnum catchkey = TexturesKeyEnum.PLAY_OLDMAN_CAUGHT;
            if (!_dictionary.ContainsKey(catchkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/oldman/oldmancaught"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/oldman/oldmanfloor"));
                _dictionary.Add(catchkey, texture2Ds);
            }


            TexturesKeyEnum walkkey = TexturesKeyEnum.PLAY_OLDMAN_WALK;
            if (!_dictionary.ContainsKey(walkkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/oldman/oldmanrun1"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/oldman/oldmanrun2"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/oldman/oldmanrun3"));
                _dictionary.Add(walkkey, texture2Ds);
            }
        }

        //遊戲中的老人
        private void LoadPlayRoxanne()
        {
            TexturesKeyEnum fallkey = TexturesKeyEnum.PLAY_ROXANNE_FALL;
            if (!_dictionary.ContainsKey(fallkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/roxanne/roxannefall"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/roxanne/roxannefall2"));
                _dictionary.Add(fallkey, texture2Ds);
            }

            TexturesKeyEnum catchkey = TexturesKeyEnum.PLAY_ROXANNE_CAUGHT;
            if (!_dictionary.ContainsKey(catchkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/roxanne/roxannehave_caught"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/roxanne/roxanne_floor"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/roxanne/roxanne_floor2"));
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/roxanne/roxanne_floor3"));
                _dictionary.Add(catchkey, texture2Ds);
            }


            TexturesKeyEnum walkkey = TexturesKeyEnum.PLAY_ROXANNE_WALK;
            if (!_dictionary.ContainsKey(walkkey))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Play/roxanne/roxanne_run"));
                
                _dictionary.Add(walkkey, texture2Ds);
            }
        }


        //暫停對話框---------------------------------------------------------------------------

        //暫停對話框的背景
        private void LoadPauseDialogBackground()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PAUSE_DIALOG_BACK;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Pause/pause_back"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //暫停對話框的離開按鈕
        private void LoadPauseDialogExitButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PAUSE_EXIT;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Pause/exit"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //暫停對話框的繼續遊戲按鈕
        private void LoadPauseDialogContinueButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.PAUSE_CONTINUE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Pause/continue"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //GAME OVER遊戲結束元件------------------------------------------------------
        private void LoadGameOverBackground()
        {
            TexturesKeyEnum key = TexturesKeyEnum.GAMEOVER_BACKGROUND;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("GameOver/gameover_back"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        private void LoadGameOverMenuButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.GAMEOVER_MENU_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("GameOver/gameover_menu"));
                _dictionary.Add(key, texture2Ds);
            }
        }
        private void LoadGameOverAgainButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.GAMEOVER_AGAIN_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("GameOver/gameover_again"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        private void LoadGameOverCharacterShow()
        {
            TexturesKeyEnum key = TexturesKeyEnum.GAMEOVER_CHARACTER_SHOW_FOREGROUND;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("GameOver/gameover_character_1"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //Dialog中元件---------------------------------------------------------------------------------

        //左按鈕
        private void LoadDialogLeftButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DIALOG_LEFT_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/left_button"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //右按鈕
        private void LoadDialogRightButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DIALOG_RIGHT_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/right_button"));
                _dictionary.Add(key, texture2Ds);

            }
        }


        //字典中元件------------------------------------------------------

        //人物問號
        private void LoadDictionaryDialogNoTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_NO;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_no"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //字典背景
        private void LoadDictionaryDialogBackground()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_BACKGROUND;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_back"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        private void LoadDictionaryDialogLeftButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_LEFT_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_leftButton"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        private void LoadDictionaryDialogRightButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_RIGHT_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_rightButton"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        private void LoadDictionaryDialogCloseButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_CLOSE_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("close"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        //字典內容  
        private void LoadDictionaryDialogLittleGirlIntroTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_LITTLEGIRL_INTRO_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_littlegirl_intro"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogFatDancerIntroTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_FATDANCER_INTRO_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_fatdancer_intro"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogFlyOldLadyIntroTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_FLYOLDLADY_INTRO_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_flyoldlady_intro"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogManStubbleIntroTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_MANSTUBBLE_INTRO_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_manstubble_intro"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogNaughtyBoyIntroTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_NAUGHTYBOY_INTRO_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_naughtyboy_intro"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogOldManIntroTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_OLDMAN_INTRO_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_oldman_intro"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogRoxanneIntroTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_ROXANNE_INTRO_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_roxanne_intro"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        //字典人物
        private void LoadDictionaryDialogLittleGirlTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_LITTLEGIRL_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_littlegirl"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogFatDancerTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_FATDANCER_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_fatdancer"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogFlyOldLadyTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_FLYOLDLADY_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_flyoldlady"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogManStubbleTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_MANSTUBBLE_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_manstubble"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogNaughtyBoyTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_NAUGHTYBOY_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_naughtyboy"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogOldManTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_OLDMAN_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_oldman"));
                _dictionary.Add(key, texture2Ds);

            }
        }
        private void LoadDictionaryDialogRoxanneTexture()
        {
            TexturesKeyEnum key = TexturesKeyEnum.DICTIONARY_ROXANNE_TEXTURE;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Dictionary/dictionary_roxanne"));
                _dictionary.Add(key, texture2Ds);

            }
        }

        //HOWTOPLAY元件--------------------------------------
        private void LoadHowtoplayDialogBackground()
        {
            TexturesKeyEnum key = TexturesKeyEnum.HOWTOPLAY_BACKGROUND;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Howtoplay/howtoplay_back"));
                _dictionary.Add(key, texture2Ds);
            }
        }
        private void LoadHowtoplayDialogCloseButton()
        {
            TexturesKeyEnum key = TexturesKeyEnum.HOWTOPLAY_CLOSE_BUTTON;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("close"));
                _dictionary.Add(key, texture2Ds);
            }
        }

        private void LoadHowtoplayDialogPage1()
        {
            TexturesKeyEnum key = TexturesKeyEnum.HOWTOPLAY_PAGE1;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Howtoplay/howtoplay_page1"));
                _dictionary.Add(key, texture2Ds);
            }
        }
        private void LoadHowtoplayDialogPage2()
        {
            TexturesKeyEnum key = TexturesKeyEnum.HOWTOPLAY_PAGE2;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Howtoplay/howtoplay_page2"));
                _dictionary.Add(key, texture2Ds);
            }
        }
        private void LoadHowtoplayDialogPage3()
        {
            TexturesKeyEnum key = TexturesKeyEnum.HOWTOPLAY_PAGE3;
            if (!_dictionary.ContainsKey(key))
            {
                List<Texture2D> texture2Ds = new List<Texture2D>();
                texture2Ds.Add(mainGame.GetContentManager.Load<Texture2D>("Dialog/Howtoplay/howtoplay_page3"));
                _dictionary.Add(key, texture2Ds);
            }
        }
    }
}
