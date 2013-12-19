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
    enum CollisionType { PixelCollision = 0, BoundingBoxCollison }

    public class Button : GameObject 
    {
        //isClick用來判斷真的有在按鈕點擊並放開, isPressed用來判斷有點及但是沒有放開(如此狀態不應該進入選項)
        bool isClick,isPressed; 
        private AnimationSprite buttonAnimation;
        private Texture2D currentTexture; //用來取得目前圖片(Pixel碰撞用)
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
            if (isPressed) //如果有按壓,切換到按壓的圖片
            {
                buttonAnimation.SetNextWantFrameIndex(1);
            }
            else if(isClick || !isPressed){ //如果有點擊(按壓並放開)或是沒有按壓,切換到正常按鈕圖片
                buttonAnimation.SetNextWantFrameIndex(0);
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

        /// <summary>
        /// 判斷有無按壓到Button(像素碰撞)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="isTouchReleased">送進來的點擊是否狀態是放開的,對的話請給true</param>
        /// <returns></returns>
        public bool IsPixelClicked(float x, float y,bool isTouchReleased)
        {
            
            Color[] currtentTextureColor = new Color[currentTexture.Width * currentTexture.Height];
            currentTexture.GetData<Color>(currtentTextureColor);
            //偵測按下去的座標換算成圖片圖片的像素位置
            int pixelPos = ((int)x - currentTexture.Bounds.Left) + (((int)y) - currentTexture.Bounds.Top) * currentTexture.Bounds.Width;
            if (currtentTextureColor.Length < pixelPos){
                isPressed = false;
                return false;
            }

            Color clickPoint = currtentTextureColor[pixelPos];
            if (clickPoint.A != 0)
            {
                if (isTouchReleased) //如果有放開並且有放開座標有在按鈕上
                {
                    isClick = true; //設定有按下
                    isPressed = false;
                    return true; //回傳真的有點擊
                }
                else {

                    isPressed = true; //設定只是按壓沒有進入選項
                    return false;
                }

            }
            else
            {
                if (isTouchReleased) //放開點擊,但是不在按鈕中
                    isPressed = false; //按壓變回false表示圖片切回無按壓圖片
                return false; //無真的點擊
            }
            
        }

        /// <summary>
        /// 判斷有無按壓到Button(區塊碰撞)
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool IsBoundingBoxClicked(float x, float y, bool isTouchReleased)
        {
            if (x >= this.X &&
                x <= this.X + this.Width &&
                y >= this.Y &&
                y <= this.Y + this.Height)
            {
                if (isTouchReleased)
                {
                    isClick = true;
                    isPressed = false;
                    return true;
                }
                else
                {
                    
                    isPressed = true;
                    return false;
                }
            }
            else
            {
                
                if (isTouchReleased)
                    isPressed = false;
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
