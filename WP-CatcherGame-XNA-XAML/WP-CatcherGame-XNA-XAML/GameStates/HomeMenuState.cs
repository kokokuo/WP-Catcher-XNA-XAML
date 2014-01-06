using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CatcherGame.GameObjects;
using CatcherGame.TextureManager;
using CatcherGame.GameStates.Dialog;
using WP_CatcherGame_XNA_XAML;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
namespace CatcherGame.GameStates
{
    public class HomeMenuState : GameState
    {
        Button playButton;
        Button topScoreButton;
        Button collectionDictionaryButton;
        Button howToPlayButtion;
        TextureLayer menuSide;

        Song backgroundSong;
        
        public HomeMenuState(GamePage gMainGame)
            : base(gMainGame) {
                //建立好要在Menu的Dialog
                dialogTable = new Dictionary<DialogStateEnum, GameDialog>();
                dialogTable.Add(DialogStateEnum.STATE_DICTIONARY, new DictionaryDialog(this));
                dialogTable.Add(DialogStateEnum.STATE_TOPSCORE, new TopScoreDialog(this));
                dialogTable.Add(DialogStateEnum.STATE_HOW_TO_PLAY, new HowToPlayDialog(this));

                base.x = 0; base.y = 0;
                base.backgroundPos = new Vector2(base.x, base.y);
        }

        public override void LoadResource(){
            playButton.LoadResource(TexturesKeyEnum.MENU_PLAY_BUTTON);
            howToPlayButtion.LoadResource(TexturesKeyEnum.MENU_HOW_TO_PLAY_BUTTON);
            collectionDictionaryButton.LoadResource(TexturesKeyEnum.MENU_DICTIONARY_BUTTON);
            topScoreButton.LoadResource(TexturesKeyEnum.MENU_TOP_SCORE_BUTTON);

            //因為背景只有一張圖,所以這邊我們直接用index抽出圖片
            base.background = GetTexture2DList(TexturesKeyEnum.MENU_BACKGROUND)[0];

            menuSide.LoadResource(TexturesKeyEnum.MENU_SIDE);

            //設定邊界
            base.leftGameScreenBorder = 0;
            base.rightGameScreenBorder = base.background.Width;

            //載入對話框的圖片資源
            foreach (KeyValuePair<DialogStateEnum, GameDialog> dialog in dialogTable)
            {
                if (!dialog.Value.GetGameDialogHasLoadContent)
                {
                    //把繪製的元件 gameSateSpriteBatch 傳入進去,讓對話框可以透過此 gameSateSpriteBatch 來繪製
                    dialog.Value.SetSpriteBatch(this.gameSateSpriteBatch);
                    dialog.Value.LoadResource();
                }
            }

            //載入音效
            base.clickSound = base.mainGame.GetSoundEffectManagerByKey(SoundManager.SoundEffectKeyEnum.CLICK_SOUND);
            //音樂
            backgroundSong = base.mainGame.GetGameSongManagerByKey(SongManager.GameSongKeyEnum.MENU_BACKGOUND_SONG);
            MediaPlayer.IsRepeating = true; //設定要重複撥放
        }

        public override void BeginInit()
        {
            base.objIdCount = 0; 
            playButton = new Button(this, objIdCount++,0,0);
            topScoreButton = new Button(this, objIdCount++, 0, 0);
            collectionDictionaryButton = new Button(this, objIdCount++, 0, 0);
            howToPlayButtion = new Button(this, objIdCount++, 0, 0);
            menuSide = new TextureLayer(this, objIdCount++, 0, 0);


            //這邊的加入有順序,越下面的遊戲元件在繪圖時也會被繪製在最上層
            AddGameObject(playButton);
            AddGameObject(topScoreButton);
            AddGameObject(collectionDictionaryButton);
            AddGameObject(howToPlayButtion);
            AddGameObject(menuSide);


            //對 對話框做初始化
            foreach (KeyValuePair<DialogStateEnum, GameDialog> dialog in dialogTable)
            {
                if (!dialog.Value.GetGameDialogHasInit)
                {
                    dialog.Value.BeginInit();
                }
            }

            base.isInit = true; //設定有初始化過了
        }
        public override void Update()
        {
            if (!isPlayedBackgroundSong) {
                MediaPlayer.Play(backgroundSong);
                isPlayedBackgroundSong = true;
            }

            //如果沒有要顯示Dialog的話,則進入選單中的按鈕判斷
            if (!base.hasDialogShow)
            {
                TouchCollection tc = base.GetCurrentFrameTouchCollection();
                bool isTouchReleased = false;
                bool isClickPlay, isClickDictionary, isClickTopScore, isClickHowToPlay;
                isClickHowToPlay = isClickPlay = isClickDictionary = isClickTopScore = false;
                if (tc.Count > 0){
                    //取出點此frame下同時點擊的所有座標,並先對所有座標去做按鈕上的點擊判斷
                    foreach (TouchLocation touchLocation in tc){
                        //有按壓並釋放
                        if (touchLocation.State == TouchLocationState.Released){
                            isTouchReleased = true;
                        }
                        else {
                            isTouchReleased = false;
                        }
                        isClickPlay = playButton.IsPixelClicked((int)touchLocation.Position.X, (int)touchLocation.Position.Y, isTouchReleased);
                        isClickDictionary = collectionDictionaryButton.IsPixelClicked((int)touchLocation.Position.X, (int)touchLocation.Position.Y, isTouchReleased);
                        isClickTopScore = topScoreButton.IsPixelClicked((int)touchLocation.Position.X, (int)touchLocation.Position.Y, isTouchReleased);
                        isClickHowToPlay = howToPlayButtion.IsPixelClicked((int)touchLocation.Position.X, (int)touchLocation.Position.Y, isTouchReleased);
                    }

                   
                }
                
                //遊戲邏輯判斷
                //如果沒有同時四個都選項接點擊
                if (!(isClickPlay && isClickDictionary && isClickTopScore && isClickHowToPlay))
                {
                    //如果isClickPlay有點擊,並且另外三個按鈕中沒有任何一個被點擊
                    if (isClickPlay && !(isClickDictionary || isClickTopScore || isClickHowToPlay))
                    {
                        Debug.WriteLine("CLICK!! STATE_START_COMIC");
                        base.clickSound.Play();
                        
                        //停止音樂
                        isPlayedBackgroundSong = false;
                        MediaPlayer.Stop();

                        SetNextGameSateByMain(GameStateEnum.STATE_START_COMIC);
                    }
                    else if (isClickDictionary && !(isClickPlay || isClickTopScore || isClickHowToPlay))
                    {
                        Debug.WriteLine("CLICK!! STATE_DICTIONARY");
                        base.clickSound.Play();
                        //設定彈出DictionaryDialog
                        base.SetPopGameDialog(DialogStateEnum.STATE_DICTIONARY);
                    }
                    else if (isClickTopScore && !(isClickPlay || isClickDictionary || isClickHowToPlay))
                    {
                        Debug.WriteLine("CLICK!! STATE_TOPSCORE");
                        base.clickSound.Play();
                        //設定彈出GameDialog
                        base.SetPopGameDialog(DialogStateEnum.STATE_TOPSCORE);
                    }
                    else if (isClickHowToPlay && !(isClickPlay || isClickTopScore || isClickDictionary))
                    {
                        base.SetPopGameDialog(DialogStateEnum.STATE_HOW_TO_PLAY);
                        base.clickSound.Play();
                        Debug.WriteLine("CLICK!! STATE_HOW_TO_PLAY");
                    }
                }
                
            }
           
            base.Update();
        }

        public override void Draw()
        {
            // 繪製主頁背景
            gameSateSpriteBatch.Draw(base.background, base.backgroundPos, Color.White);
            //繪製遊戲元件
            base.Draw();
            
        }

       
    }
}
