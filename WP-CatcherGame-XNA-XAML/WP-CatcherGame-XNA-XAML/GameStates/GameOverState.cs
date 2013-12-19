using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CatcherGame.GameObjects;
using CatcherGame.TextureManager;
using CatcherGame.FileStorageHelper;
using System.Diagnostics;
using WP_CatcherGame_XNA_XAML;
namespace CatcherGame.GameStates
{
    public class GameOverState : GameState
    {
        Button menuButton;
        Button againButton;

        GameRecordData readData;
        string currentSavedPeoepleNumber;
        SpriteFont currentSavedPeopleNumberFont;
        TextureLayer characterForeground; //前景角色圖(依照分數高低會放不同的圖片)
        public GameOverState(GamePage gMainGame)
            : base(gMainGame)
        {
         

        }
        public override void LoadResource()
        {
            base.background = base.GetTexture2DList(TexturesKeyEnum.GAMEOVER_BACKGROUND)[0];
            characterForeground.LoadResource(TexturesKeyEnum.GAMEOVER_CHARACTER_SHOW_FOREGROUND);
            currentSavedPeopleNumberFont = base.GetSpriteFontFromKeyByGameState(FontManager.SpriteFontKeyEnum.GAME_VOER_CURRENT_SAVED_PEOPLE_FONT);
            menuButton.LoadResource(TexturesKeyEnum.GAMEOVER_MENU_BUTTON);
            againButton.LoadResource(TexturesKeyEnum.GAMEOVER_AGAIN_BUTTON);

        }

        public override void BeginInit()
        {
            base.objIdCount = 0;
            menuButton = new Button(this, objIdCount++, 0, 0);
            againButton = new Button(this, objIdCount++, 0, 0);
            characterForeground = new TextureLayer(this, objIdCount++, 0, 0);
            currentSavedPeoepleNumber = "";

            AddGameObject(menuButton);
            AddGameObject(againButton);
            AddGameObject(characterForeground);
        }
        public override void Update()
        {
          
            TouchCollection tc = base.GetCurrentFrameTouchCollection();
            bool isClickMenu, isClickAgain;
            isClickMenu = isClickAgain = false;

            if (tc.Count > 0)
            {
                //取出點此frame下同時點擊的所有座標,並先對所有座標去做按鈕上的點擊判斷
                foreach (TouchLocation touchLocation in tc)
                {
                    if (!isClickMenu)
                        isClickMenu = menuButton.IsPixelPressed((int)touchLocation.Position.X, (int)touchLocation.Position.Y);
                    if (!isClickAgain)
                        isClickAgain = againButton.IsPixelPressed((int)touchLocation.Position.X, (int)touchLocation.Position.Y);
                }

                //遊戲邏輯判斷
                if (!( isClickMenu && isClickAgain))
                {
                     if (isClickAgain && !isClickMenu)
                    {
                        Debug.WriteLine("CLICK!! STATE_COMIC");
                        SetNextGameSateByMain(GameStateEnum.STATE_START_COMIC);
                    }
                }
                 //使用觸控單次點擊方式
                TouchLocation tL = base.GetTouchLocation();
                if (tL.State == TouchLocationState.Released)
                {

                    //關閉按鈕
                    if ( menuButton.IsPixelPressed(tL.Position.X, tL.Position.Y))
                    {
                        
                        base.LoginFacebook();
                        //Debug.WriteLine("CLICK!! STATE_MENU");
                        //SetNextGameSateByMain(GameStateEnum.STATE_MENU);
                    }
                }

                //清除TouchQueue裡的觸控點，因為避免Dequeue時候並不在Dialog中，因此要清除TouchQueue。
                base.ClearTouchQueue();
               
            
            }


            base.Update(); //會把　AddGameObject方法中加入的物件作更新
        }
        public override void Draw()
        {
            gameSateSpriteBatch.Draw(base.background, base.backgroundPos, Color.White);
            gameSateSpriteBatch.DrawString(currentSavedPeopleNumberFont, currentSavedPeoepleNumber.ToString(), new Vector2(723,515), Color.Black);
            base.Draw(); //會把　AddGameObject方法中加入的物件作繪製
        }


     
    }
}
