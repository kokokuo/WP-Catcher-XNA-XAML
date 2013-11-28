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
        public TextureLayer(GameState currentGameState, int objId, float x, float y)
            : base(currentGameState, objId, x, y)
        {
            Init();
        }
        

        protected override void Init()
        {
            layer = new AnimationSprite(new Vector2(base.x,base.y), 300);
           
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

        public override void Update() {
            layer.UpdateFrame(base.gameState.GetTimeSpan());
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
