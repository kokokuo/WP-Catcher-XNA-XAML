using CatcherGame.GameObjects;
using CatcherGame.GameStates;
using CatcherGame.GameStates.Dialog;
using CatcherGame.TextureManager;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WP_CatcherGame_XNA_XAML.GameStates.Dialog
{
    public class SettingDialog:GameDialog
    {
        Button minusButton;
        Button plusButton;

        TextureLayer volumeOpenTexture;
        TextureLayer volumeCloseTexture;
        TextureLayer[] volumeTexture = new TextureLayer[5];
        public SettingDialog(GameState pCurrentState)
            : base(pCurrentState) 
        { 
        }

        public override void BeginInit()
        {
            backgroundPos = new Vector2(0,0);
            closeButton = new Button(base.currentState, base.countId++, 0, 0);
            minusButton = new Button(base.currentState, base.countId++, 0, 0);
            plusButton = new Button(base.currentState, base.countId++, 0, 0);
            volumeOpenTexture = new TextureLayer(base.currentState, base.countId++, 270, 320);
            volumeCloseTexture = new TextureLayer(base.currentState, base.countId++, 320, 320);
            
             for (int i = 0; i < volumeTexture.Length; i++)
            {
                 if (i == 0)
                    volumeTexture[i] = new TextureLayer(base.currentState, base.countId++, 260, 320);
                else
                    volumeTexture[i] = new TextureLayer(base.currentState, base.countId++, volumeTexture[i - 1].X + 50, 320);
            }
            
            AddGameObject(closeButton);
            AddGameObject(minusButton);
            AddGameObject(plusButton);
            foreach (TextureLayer t in volumeTexture)
            {
                
                AddGameObject(t);
            }
            

          
            base.isInit = true;
        }
        public override void LoadResource()
        {
            background = currentState.GetTexture2DList(CatcherGame.TextureManager.TexturesKeyEnum.SETTING_BACKGROUND)[0];
            closeButton.LoadResource(TexturesKeyEnum.DIALOG_CLOSE_BUTTON);
            minusButton.LoadResource(TexturesKeyEnum.SETTING_MINUS);
            plusButton.LoadResource(TexturesKeyEnum.SETTING_PLUS);
            volumeOpenTexture.LoadResource(TexturesKeyEnum.SETTING_VOLUME_OPEN);
            volumeCloseTexture.LoadResource(TexturesKeyEnum.SETTING_VOLUME_CLOSE);
            //載入圖片資源黨
            VolumeSetting(MediaPlayer.Volume);
            //音效
            clickSound = currentState.GetSoundEffectManagerByMainGame(CatcherGame.SoundManager.SoundEffectKeyEnum.CLICK_SOUND);
            base.isLoadContent = true;
        }
        public override void Update()
        {
            
            bool isTouchReleased = false;
            bool isClickCloseButton, isClickMinusButton, isClickPlusButton;
            isClickCloseButton = isClickMinusButton = isClickPlusButton = false;

            TouchCollection tc = base.currentState.GetCurrentFrameTouchCollection();
            if (tc.Count > 0)
            {
                foreach (TouchLocation touchLocation in tc)
                {
                    if (touchLocation.State == TouchLocationState.Released)
                    {
                        isTouchReleased = true;

                    }
                    else
                    {
                        isTouchReleased = false;
                    }
                    isClickCloseButton = closeButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, isTouchReleased);
                    isClickMinusButton = minusButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, isTouchReleased);
                    isClickPlusButton = plusButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, isTouchReleased);
                }

                //判斷有無點擊到按鈕並切換(沒有同時都點到)
                if (!(isClickCloseButton && isClickMinusButton && isClickPlusButton))
                {
                    //關閉按鈕
                    if (isClickCloseButton && !isClickMinusButton && !isClickPlusButton)
                    {
                        SoundEffectPlay();//base.clickSound.Play();
                        base.CloseDialog();//透過父類別來關閉
                    }

                    //減少音量按鈕
                    if (!isClickCloseButton && isClickMinusButton && !isClickPlusButton)
                    {
                        SoundEffectPlay();//base.clickSound.Play();
                        VolumeSetting(MediaPlayer.Volume - 0.2f);
                    }

                    //增加音量按鈕
                    if (!isClickCloseButton && !isClickMinusButton && isClickPlusButton)
                    {
                        SoundEffectPlay();//base.clickSound.Play();
                        VolumeSetting(MediaPlayer.Volume + 0.2f);
                    }
                }

            }

            base.Update(); //更新遊戲元件
        }

        /// <summary>
        /// 設定聲音大小載入圖片
        /// </summary>
        /// <param name="volume"></param>
        public void VolumeSetting(float volume)
        {
            MediaPlayer.Volume = volume;
            //只保留小數點1位
            String transVolume = volume.ToString("F1");
            volume = Convert.ToSingle(transVolume);

            //判斷聲音放入圖片
            for (int i = 0; i < volumeTexture.Length; i++)
            {
                if(volume==0.0)
                    volumeTexture[i].LoadResource(TexturesKeyEnum.SETTING_VOLUME_CLOSE);
                else if((float)(i+1)/5<=volume)
                  volumeTexture[i].LoadResource(TexturesKeyEnum.SETTING_VOLUME_OPEN);
                else
                    volumeTexture[i].LoadResource(TexturesKeyEnum.SETTING_VOLUME_CLOSE);
            }
        }
        public override void Draw(SpriteBatch gSpriteBatch)
        {
            gSpriteBatch.Draw(background, backgroundPos, Color.White);
            
            base.Draw(gSpriteBatch); //繪製遊戲元件
        }

    }
}
