using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CatcherGame.GameStates;
using CatcherGame.GameObjects;
using CatcherGame.FontManager;
using CatcherGame.FileStorageHelper;
using CatcherGame.TextureManager;
namespace CatcherGame.GameStates.Dialog
{
    public class TopScoreDialog : GameDialog
    {
        SpriteFont topSavedPeopleNumberFont;
        GameRecordData readData;
        string topSavedPeoepleNumber;
       
        public TopScoreDialog(GameState pCurrentState)
            : base(pCurrentState) 
        { 
        }

        public override void BeginInit()
        {
            backgroundPos = new Vector2(0,0);
            closeButton = new Button(base.currentState, base.countId++, 0, 0);
            AddGameObject(closeButton);
            topSavedPeoepleNumber = "Not Saved";

            //讀取紀錄檔
            readData = FileStorageHelper.StorageHelperSingleton.Instance.LoadGameRecordData();
            if (readData !=null && readData.HistoryTopSavedNumber != 0)
            { //分數不為零
                topSavedPeoepleNumber = readData.HistoryTopSavedNumber.ToString();
            }
            base.isInit = true;
        }
        public override void LoadResource()
        {
            background = currentState.GetTexture2DList(TextureManager.TexturesKeyEnum.TOP_SCORE_DIALOG_BACK)[0];
            topSavedPeopleNumberFont = currentState.GetSpriteFontFromKeyByMainGame(SpriteFontKeyEnum.TOP_SCORE_FONT);
            closeButton.LoadResource(TexturesKeyEnum.DIALOG_CLOSE_BUTTON);
            //音效
            clickSound = currentState.GetSoundEffectManagerByMainGame(SoundManager.SoundEffectKeyEnum.CLICK_SOUND);
            base.isLoadContent = true;
        }
        public override void Update()
        {
            //base.stCurrent = DialogStateEnum.STATE_TOPSCORE;
            //讀取紀錄檔
            readData = FileStorageHelper.StorageHelperSingleton.Instance.LoadGameRecordData();
            if (readData != null && readData.HistoryTopSavedNumber != 0)
            { //分數不為零
                topSavedPeoepleNumber = readData.HistoryTopSavedNumber.ToString();
            }
               

            TouchCollection tc = base.currentState.GetCurrentFrameTouchCollection();
            bool isClickClose = false;
            if (tc.Count > 0){
                //所有當下的觸控點去判斷有無點到按鈕
                foreach (TouchLocation touchLocation in tc) {
                    if (touchLocation.State == TouchLocationState.Released)
                        isClickClose = closeButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y,true);
                    else
                        isClickClose = closeButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, false);
                }
                //關閉按鈕
                if (isClickClose)
                {
                    SoundEffectPlay();//base.clickSound.Play();
                    base.CloseDialog();//透過父類別來關閉
                }
            }

            base.Update(); //更新遊戲元件
        }
        public override void Draw(SpriteBatch gSpriteBatch)
        {
            gSpriteBatch.Draw(background, backgroundPos, Color.White);
            gSpriteBatch.DrawString(topSavedPeopleNumberFont, topSavedPeoepleNumber.ToString(), new Vector2(background.Width / 2, background.Height / 2 - topSavedPeopleNumberFont.MeasureString(topSavedPeoepleNumber.ToString()).Y / 2), Color.Black);
            base.Draw(gSpriteBatch); //繪製遊戲元件
        }

      

    }
}
