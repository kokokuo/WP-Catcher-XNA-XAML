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

namespace CatcherGame.GameStates.Dialog
{
    public class PauseDialog : GameDialog
    {
        Button continueGameButton;
        Button exitGameButton;
        
        public PauseDialog(GameState pCurrentState)
            : base(pCurrentState) 
        { 
        }

        public override void BeginInit()
        {

            backgroundPos = new Vector2(0, 0);
            
            //在Pause Dialog中沒有使用到close按鈕 所以註解掉!
            //closeButton = new Button(base.currentState, base.countId++, 0, 0);
            exitGameButton = new Button(base.currentState, base.countId++, 0, 0);
            continueGameButton = new Button(base.currentState, base.countId++, 0, 0);

            //在Pause Dialog中沒有使用到close按鈕 所以註解掉!
            //AddGameObject(closeButton);

            AddGameObject(continueGameButton);
            AddGameObject(exitGameButton);
            base.isInit = true;
        }
        public override void LoadResource()
        {
            background = currentState.GetTexture2DList(TextureManager.TexturesKeyEnum.PAUSE_DIALOG_BACK)[0];
            continueGameButton.LoadResource(TextureManager.TexturesKeyEnum.PAUSE_CONTINUE);
            exitGameButton.LoadResource(TextureManager.TexturesKeyEnum.PAUSE_EXIT);
            //在Pause Dialog中沒有使用到close按鈕 所以註解掉!
            //base.LoadResource(); //載入CloseButton 圖片資源
            //音效
            clickSound = currentState.GetSoundEffectManagerByMainGame(SoundManager.SoundEffectKeyEnum.CLICK_SOUND);
            base.isLoadContent = true;
        
        }
        public override void Update()
        {
            base.stCurrent = DialogStateEnum.STATE_PAUSE;

            TouchCollection tc = base.currentState.GetCurrentFrameTouchCollection();
            bool isClickClose, isClickContinue;
            isClickClose = isClickContinue = false;
            if (tc.Count > 0)
            {
                //所有當下的觸控點去判斷有無點到按鈕
                foreach (TouchLocation touchLocation in tc)
                {
                    if (touchLocation.State == TouchLocationState.Released)
                    {
                        isClickContinue =  continueGameButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y,true);
                        isClickClose = exitGameButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, true);
                    }
                    else
                    {
                        isClickContinue = continueGameButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, false);
                        isClickClose = exitGameButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, false);
                    }
                        
                }

                //遊戲邏輯判斷
                if ( !(isClickClose && isClickContinue) ) {
                    if (isClickClose && !isClickContinue)
                    {
                        clickSound.Play();
                        base.CloseDialog(); //透過父類別來關閉視窗
                        ((PlayGameState)base.currentState).Release(); //釋放遊戲元件資源
                        base.currentState.SetNextGameSateByMain(GameStateEnum.STATE_MENU); //切換回選單
                    }
                    else if (!isClickClose && isClickContinue)
                    {
                        clickSound.Play();
                        base.CloseDialog(); //透過父類別來關閉
                    }

                }
                    
            }

            base.Update(); //更新遊戲元件
        }
        public override void Draw(SpriteBatch gSpriteBatch)
        {
            gSpriteBatch.Draw(background, backgroundPos, Color.White);
            base.Draw(gSpriteBatch); //繪製遊戲元件
        }
    }
}
