using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
//XNA Tool
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;

using CatcherGame.GameStates;

using CatcherGame.TextureManager;
namespace CatcherGame.GameObjects
{
    /// <summary>
    /// 遊戲物件的基礎類別
    /// </summary>
    public abstract class GameObject :IDisposable
    {
        protected GameState gameState; //知道物件目前所屬的遊戲狀態
        
        protected int id; //物件編號
        protected float x, y; //物件座標
        protected float width, height; //物件的寬高
        protected bool disposed;
        public GameObject() { 
        
        }

        public GameObject(GameState gameState, int objId, float x, float y)
        {
            this.gameState = gameState;
            this.id = objId;
            this.x = x;
            this.y = y;
            disposed = false;
        }
        
        protected abstract void Init();
        public abstract void LoadResource(TexturesKeyEnum key);
        /// <summary>
        /// 繪製物件圖片
        /// </summary>
        public abstract void Draw(SpriteBatch spriteBatch);

        /// <summary>
        /// 更新物件的邏輯
        /// </summary>
        public abstract void Update();

        #region 屬性

        public float X
        {
            get { return this.x; }
            set { this.x = value; }
        }

        public float Y
        {
            get { return this.y; }
            set { this.y = value; }
        }

        public float Width
        {
            get { return this.width; }
            set { this.width = value; }
        }

        public float Height
        {
            get { return this.height; }
            set { this.height = value; }
        }

        public int Id
        {
            get { return this.id; }
            set { this.id = value; }
        }
        
        #endregion

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);     
        }
        protected virtual void Dispose(bool disposing) { }
    }
}
