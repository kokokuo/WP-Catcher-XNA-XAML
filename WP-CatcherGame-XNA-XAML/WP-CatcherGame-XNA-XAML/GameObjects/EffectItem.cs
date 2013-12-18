using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Diagnostics;

using CatcherGame;
using CatcherGame.Sprite;
using CatcherGame.GameStates;

using CatcherGame.TextureManager;

namespace CatcherGame.GameObjects
{
    public delegate void EffectTimesUpEventHandler(EffectItem effectItem);
    public class EffectItem : DropObjects
    {
        public event EffectTimesUpEventHandler EffectTimesUp;
        AnimationSprite itemAnimation;
        float totalEapsed;
        float effectTime;
        public EffectItem(GameState currentGameState, DropObjectsKeyEnum key, int id, float x, float y, float fallingSpeed, float fallingWave,float effectTime)
            : base(currentGameState, key, id, x, y, fallingSpeed, fallingWave)
        {
            Init();
            this.effectTime = effectTime;
        }
        public void SetDisplayPostion(Vector2 pos) {
            base.x = pos.X;
            base.y = pos.Y;
        }
        public override void SetCaught()
        {
            if (!isDead) //如果沒有死掉的話
            {
                base.isCaught = true;
                base.isFalling = false;
            }
        }

        protected override void Init()
        {
            itemAnimation = new AnimationSprite(new Vector2(this.x, this.y), 300f);
            totalEapsed = 0;
        }

        /// <summary>
        /// 設定載入的圖片組,使用給予Key方式設定載入
        /// </summary>
        /// <param name="key"></param>
        private void SetTexture2DList(TexturesKeyEnum key)
        {
            itemAnimation.SetTexture2DList(base.gameState.GetTexture2DList(key));
            
            this.Height = itemAnimation.GetCurrentFrameTexture().Height;
            this.Width = itemAnimation.GetCurrentFrameTexture().Width;
        }

        public override void LoadResource(TexturesKeyEnum key)
        {
            SetTexture2DList(key);
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            itemAnimation.Draw(spriteBatch);
        }
        public void SetEffectElimination() {
            isDead = true;
            isCaught = false;
        }

        public override void Update()
        {
            if (isFalling) {
                fallingNextYPos = this.y + fallingSpeed;
                //判斷有無落到地板 ,沒有就繼續往下掉
                if (!base.IsFallingCollideFloor())
                {
                    this.y = fallingNextYPos;
                }
                else
                {
                    isDead = true;
                    isFalling = false;
                }    
            }
            else if (isCaught)
            {
                totalEapsed += gameState.GetTimeSpan().Milliseconds;
                //消失
                if (totalEapsed >= (effectTime*1000) ) {
                    isDead = true;
                    isCaught = false;
                    if (EffectTimesUp != null) {
                        EffectTimesUp.Invoke(this);
                    }
                }
               
            }
            else if (isDead) {
                //從DropObjectList中移除
                ((PlayGameState)gameState).RemoveDropObjs(this);
                //移除自己
                ((PlayGameState)this.gameState).RemoveGameObject(this.id);
            }


            //設定座標
            itemAnimation.SetToLeftPos(base.x, base.y);

            if (totalEapsed >= (effectTime * 1000) / 2)
            {
                itemAnimation.UpdateFrame(gameState.GetTimeSpan());
            }
            else
            {
                //更新frame
                itemAnimation.SetNextWantFrameIndex(0);

            }
           
            //設定現在的圖片長寬為遊戲元件的長寬
            this.Height = itemAnimation.GetCurrentFrameTexture().Height;
            this.Width = itemAnimation.GetCurrentFrameTexture().Width;
        }
        protected override void Dispose(bool disposing)
        {
            if (!base.disposed)
            {

                if (disposing)
                {
                    if (itemAnimation != null)
                    {
                        itemAnimation.Dispose();
                    }
                    totalEapsed = 0;
                    Debug.WriteLine("FirePlayer disposed.");
                }
            }
            disposed = true;
        }
    }
}
