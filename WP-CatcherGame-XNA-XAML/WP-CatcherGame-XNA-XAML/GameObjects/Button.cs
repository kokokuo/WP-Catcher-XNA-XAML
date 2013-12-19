using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

using CatcherGame;
using CatcherGame.Sprite;
using CatcherGame.GameStates;

using CatcherGame.TextureManager;
using System.Diagnostics;
namespace CatcherGame.GameObjects
{
    public class Button : GameObject 
    {
        bool isClick,isPressed;
        private AnimationSprite buttonAnimation;
        private Texture2D currentTexture;
        public Button(GameState currentGameState, int id, float x, float y)
            : base(currentGameState, id, x, y)
        {
            Init();
        }

        protected override void Init()
        {
            this.x = x;
            this.y = y;
            buttonAnimation = new AnimationSprite(new Vector2(this.x, this.y), 300);
            isClick = false;
            isPressed = false;
        }

        public override void LoadResource(TexturesKeyEnum key)
        {
            SetTexture2DList(key);
        }

        /// <summary>
        /// 設定載入的圖片組,使用給予Key方式設定載入
        /// </summary>
        /// <param name="key"></param>
        private void SetTexture2DList(TexturesKeyEnum key)
        {
            buttonAnimation.SetTexture2DList(base.gameState.GetTexture2DList(key));
            //加入圖片組後要馬上取得當前的圖片
            currentTexture = buttonAnimation.GetCurrentFrameTexture(); 
            this.Height = buttonAnimation.GetCurrentFrameTexture().Height;
            this.Width = buttonAnimation.GetCurrentFrameTexture().Width;
        }

        public override void Update()
        {
            if (isPressed)
            {
                buttonAnimation.UpdateFrame(this.gameState.GetTimeSpan());
                if (buttonAnimation.GetIsRoundAnimation()) {
                    isPressed = false;
                    isClick = true;
                }
            }
            //加入圖片組後要馬上取得當前的圖片
            currentTexture = buttonAnimation.GetCurrentFrameTexture();
            //設定現在的圖片長寬為遊戲元件的長寬
            this.Height = buttonAnimation.GetCurrentFrameTexture().Height;
            this.Width = buttonAnimation.GetCurrentFrameTexture().Width;

        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            buttonAnimation.Draw(spriteBatch);
        }

        /// <summary>
        /// 設定載入的圖片組(尚未使用)
        /// </summary>
        /// <param name="texture2DList">直接給予List</param>
        private void SetTexture2DList(List<Texture2D> texture2DList)
        {
            buttonAnimation.SetTexture2DList(texture2DList);
            currentTexture = buttonAnimation.GetCurrentFrameTexture();
            this.Height = buttonAnimation.GetCurrentFrameTexture().Height;
            this.Width = buttonAnimation.GetCurrentFrameTexture().Width;
        }

        public bool CheckIsClick() {
            if (isClick)
            {
                isClick = false;
                return true;
            }
            return false;
        }
       
        /// <summary>
        /// 判斷有無按壓到Button(像素碰撞)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsPixelPressed(float x, float y)
        {
            
            Color[] currtentTextureColor = new Color[currentTexture.Width * currentTexture.Height];
            currentTexture.GetData<Color>(currtentTextureColor);
            //偵測按下去的座標換算成圖片圖片的像素位置
            int pixelPos = ((int)x - currentTexture.Bounds.Left) + (((int)y) - currentTexture.Bounds.Top) * currentTexture.Bounds.Width;
            if(currtentTextureColor.Length < pixelPos)
                return false;

            Color clickPoint = currtentTextureColor[pixelPos];
            if (clickPoint.A != 0)
            {
                isPressed = true;
                return true;

            }
            else
            {
                return false;
            }
            
        }

        /// <summary>
        /// 判斷有無按壓到Button(區塊碰撞)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsBoundingBoxPressed(float x, float y)
        {
            if (x >= this.X &&
                x <= this.X + this.Width &&
                y >= this.Y &&
                y <= this.Y + this.Height)
            {
                isPressed = true;
                return true;
            }
            else
            {
                return false;
            }
        
        }



        protected override void Dispose(bool disposing)
        {
            if (!base.disposed)
            {

                if (disposing)
                {
                    if (buttonAnimation != null )
                    {
                        buttonAnimation.Dispose();
                    }

                    Debug.WriteLine("FirePlayer disposed.");
                }
            }
            disposed = true;
        }
    }
}
