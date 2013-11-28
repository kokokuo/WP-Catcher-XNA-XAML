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
    /// <summary>
    /// 掉落物類別,記錄了跟掉落物相關的方法或變數
    /// </summary>
    public abstract class DropObjects : GameObject 
    {
        protected float fallingSpeed, fallingWave;
        protected float fallingNextYPos; //接下來會掉落的Y座標
        protected float fallingNextXPos; //接下來會擺動的x座標
        protected bool isFalling, isCaught,isTouch;
        protected DropObjectsKeyEnum objectKey;
        protected bool isDead;
        public DropObjects(GameState currentGameState, DropObjectsKeyEnum key,int id, float x, float y, float fallingSpeed, float fallingWave)
            : base(currentGameState, id, x, y)
        {
            this.objectKey = key;
            this.fallingSpeed = fallingSpeed;
            this.fallingWave = fallingWave;
            this.isFalling = true;
            this.isCaught = this.isTouch = false;
            this.isDead = false;

        }

        //是否墜落到地板
        protected virtual bool IsFallingCollideFloor()
        {
            if ((base.y + base.height) >= base.gameState.GetBackgroundTexture().Height)
            {
                return true;
            }
            else
                return false;
        }

        public abstract void SetCaught();

        public void SetTouched() {
            this.isTouch = true;
        }

        public bool GetIsDead()
        {
            return this.isDead;
        }

        public bool GetIsCaught() {
            return this.isCaught;
        }

        public bool GetIsTouch()
        {
            return this.isTouch;
        }
        public bool GetIsFalling(){
            return this.isFalling;
        }

        public DropObjectsKeyEnum GetKeyEnum() {
            return this.objectKey;
        }

        
        public float GetNextFallingY() {
            return y + fallingSpeed;
        }
    }
}
