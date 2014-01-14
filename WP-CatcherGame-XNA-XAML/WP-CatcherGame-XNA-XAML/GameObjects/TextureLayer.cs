using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using CatcherGame.GameStates;
using CatcherGame.TextureManager;
using CatcherGame.Sprite;
using System.Diagnostics;

namespace CatcherGame.GameObjects
{
    //顯示單純的圖層圖片,如果有某些圖片會在最上層,且和遊戲中的邏輯無關的話,可以用此類別 e.g 遊戲中的煙霧 或 選單的紅色邊
    public class TextureLayer : GameObject
    {
        AnimationSprite layer;
        Rectangle bounds;
        bool isAutoUpdate;
        public TextureLayer(GameState currentGameState, int objId, float x, float y)
            : base(currentGameState, objId, x, y)
        {
            Init();
        }

        protected override void Init()
        {
            layer = new AnimationSprite(new Vector2(base.x,base.y), 300);
            isAutoUpdate = true;
        }

        public override void LoadResource(TexturesKeyEnum key)
        {
            layer.SetTexture2DList(base.gameState.GetTexture2DList(key));
            height = layer.GetCurrentFrameTexture().Height;
            width = layer.GetCurrentFrameTexture().Width;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            layer.Draw(spriteBatch);
        }
        /// <summary>
        /// 設定是否要自動撥放圖片組為動畫
        /// </summary>
        /// <param name="decision"></param>
        public void SetAnimationAutoUpdate(bool decision) {
            isAutoUpdate = decision;
        }
        public override void Update() {
            if (isAutoUpdate)
            {
                layer.UpdateFrame(base.gameState.GetTimeSpan());
            }

            this.Height = layer.GetCurrentFrameTexture().Height;
            this.Width = layer.GetCurrentFrameTexture().Width;
        }
        /// <summary>
        /// 手動設定下一個要撥放的圖片
        /// </summary>
        /// <param name="index"></param>
        public void SetNextPlayFrameIndex(int index) {
            layer.SetNextWantFrameIndex(index);
            this.Height = layer.GetCurrentFrameTexture().Height;
            this.Width = layer.GetCurrentFrameTexture().Width;
        
        }
        protected override void Dispose(bool disposing)
        {
            if (!base.disposed)
            {

                if (disposing)
                {
                    if (layer != null)
                    {
                        layer.Dispose();
                    }

                    Debug.WriteLine("Object disposed.");
                }
            }
            disposed = true;   
        }
    }
}
