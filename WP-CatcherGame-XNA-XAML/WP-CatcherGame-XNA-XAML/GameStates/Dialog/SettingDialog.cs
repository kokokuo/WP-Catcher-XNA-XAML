using CatcherGame.GameObjects;
using CatcherGame.GameStates;
using CatcherGame.GameStates.Dialog;
using CatcherGame.TextureManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WP_CatcherGame_XNA_XAML.GameStates.Dialog
{
    public class SettingDialog:GameDialog
    {
        Button minusButton;
        //Button plusButton;
       
        public SettingDialog(GameState pCurrentState)
            : base(pCurrentState) 
        { 
        }

        public override void BeginInit()
        {
            backgroundPos = new Vector2(0,0);
            closeButton = new Button(base.currentState, base.countId++, 0, 0);
            minusButton = new Button(base.currentState, base.countId++, 0, 0);
            //plusButton = new Button(base.currentState, base.countId++, 0, 0);
            AddGameObject(closeButton);
            AddGameObject(minusButton);
            

          
            base.isInit = true;
        }
        public override void LoadResource()
        {
            background = currentState.GetTexture2DList(CatcherGame.TextureManager.TexturesKeyEnum.SETTING_BACKGROUND)[0];
            closeButton.LoadResource(TexturesKeyEnum.DIALOG_CLOSE_BUTTON);
            minusButton.LoadResource(TexturesKeyEnum.SETTING_MINUS);
            //音效
            clickSound = currentState.GetSoundEffectManagerByMainGame(CatcherGame.SoundManager.SoundEffectKeyEnum.CLICK_SOUND);
            base.isLoadContent = true;
        }
        public override void Update()
        {
            

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
                    clickSound.Play();
                    base.CloseDialog();//透過父類別來關閉
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
