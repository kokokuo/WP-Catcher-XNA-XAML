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
using Microsoft.Xna.Framework.Audio;
using CatcherGame.TextureManager;
using Microsoft.Xna.Framework.Media;

namespace CatcherGame.GameObjects
{
    /// <summary>
    /// 有生命的掉落物體:人 動物
    /// </summary>
    public class Creature : DropObjects
    {
        List<AnimationSprite> animationList;
        AnimationSprite pCurrentAnimation;
        bool isSaved;
        SoundEffect caughtSound; //目前音效皆一致,所以寫死在LoadResource
       
        float savedWalkSpeed; //被接住後離開畫面移動的速度
        const int FALLING_KEY = 0, CAUGHT_KEY = 1, WALK_KEY = 2,DIE_KEY = 3;
        const float DEAD_FLY_UP_SPEED = 10; //死掉飛上去的速度
        int walkOrienation;
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="currentGameState">目前的遊戲狀態</param>
        /// <param name="id">遊戲物件的ID</param>
        /// <param name="x">X左上角座標</param>
        /// <param name="y">Y左上角座標</param>
        /// <param name="fallingSpeed">掉落的速度(數值應為正)</param>
        /// <param name="fallingWave">擺動的位移量(數值應為正)</param>
        /// <param name="walkSpeed"></param>
        /// <param name="orienation">移動的方向(0為左邊,1為右邊)</param>
        public Creature(GameState currentGameState, DropObjectsKeyEnum key, int id, float x, float y, float fallingSpeed, float fallingWave, float walkSpeed, int orienation)
            : base(currentGameState,key, id, x, y, fallingSpeed, fallingWave) 
        { 
            Init();
            //設定掉下來的速度與擺動的位移量
            
            this.walkOrienation = orienation;
            //沒有辦法預設參數值,只好這樣做
            if (walkSpeed <= 0)
                this.savedWalkSpeed = 5;
            else
                this.savedWalkSpeed = walkSpeed;

           
        }
        protected override void Init()
        {
            base.fallingNextXPos = this.x ;
            base.fallingNextYPos = this.y ;
            
            animationList = new List<AnimationSprite>();
            isDead = false;
           
        }

        
        public bool GetIsSaved() {
            return this.isSaved;
        }

        

        /// <summary>
        /// 設定載入的圖片組,使用給予Key方式設定載入
        /// </summary>
        /// <param name="key"></param>
        private void SetTexture2DList(TexturesKeyEnum key)
        {
            AnimationSprite animation = new AnimationSprite(new Vector2(this.x, this.y), 300);
            animation.SetTexture2DList(base.gameState.GetTexture2DList(key));
            animationList.Add(animation);
        }
        /// <summary>
        /// 呼叫此方法設定為接到
        /// </summary>
        public override void SetCaught() {
            base.isCaught = true;
            base.isFalling = false;
            //切換圖片組
            pCurrentAnimation = animationList[CAUGHT_KEY];

            //判斷是否背景音樂為靜音(暫時)
            if(MediaPlayer.Volume!=0.0f)
            //播放接到音效
            caughtSound.Play();
        }
        public override void LoadResource(TexturesKeyEnum key)
        {
            SetTexture2DList(key);
           
            if (animationList.Count >= 3)
            {
                //自動載入死亡的圖片
                SetTexture2DList(TexturesKeyEnum.PLAY_DIE);
                //設定目前的圖片組是"掉下來"
                pCurrentAnimation = animationList[FALLING_KEY];

                //載入音效
                caughtSound = base.gameState.GetSoundEffectManagerByMainGame(SoundManager.SoundEffectKeyEnum.CAUGHT_SOUND);
            }

            
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            pCurrentAnimation.Draw(spriteBatch);
        }

        
        //墜落搖晃時是否碰到邊界
        private bool IsFallingWaveCollideBorder()
        {
            //超出左邊邊框或超出右邊邊框 
            if ( ((base.x + base.width) >= base.gameState.GetRightGameScreenBorder() )
                || (base.x < base.gameState.GetLeftGameScreenBorder())) 
            {
                return true;
            }
            else
                return false;
        }

        //拯救到或死亡後,人物是否離開遊戲畫面
        private bool IsOuterGameScreenBorder()
        {
            //超出左邊邊框或超出右邊邊框
            if ((base.x  >= base.gameState.GetBackgroundTexture().Width)
                || ( (base.x + base.width) < 0)  || ((base.y + base.height) < 0 ) )
            {
                return true;
            }
            else
                return false;
        }


        public override void Update()
        {

            //判斷人物目前的狀態
            if (isFalling) {
                fallingNextYPos = this.y + fallingSpeed;
                //判斷有無落到地板 ,沒有就繼續往下掉
                if (! base.IsFallingCollideFloor())
                {
                    this.y = fallingNextYPos;
                }
                else {
                    isDead = true;
                    //告知遊戲可以漏接的人數減少
                    ((PlayGameState)this.gameState).SubtractCanLostPeopleNumber();
                    isFalling = false;
                    pCurrentAnimation = animationList[DIE_KEY];
                }
            }
            else if (isCaught) { 
                //如果播完一輪,代表被接住也站起來=>切換成走路
                if (pCurrentAnimation.GetIsRoundAnimation()){
                    isCaught = false;
                    isSaved = true;
                    //切換到走路的圖片組
                    pCurrentAnimation = animationList[WALK_KEY];
                    
                }
            }
            else if (isSaved) { 
               
                //向左或向右走
                if (walkOrienation == 0) { 
                    this.x -=savedWalkSpeed;
                }
                else if (walkOrienation == 1) {
                    this.x += savedWalkSpeed;
                }
                //走出遊戲畫面
                if (IsOuterGameScreenBorder()) { 
                    //移除自己
                    ((PlayGameState)this.gameState).RemoveGameObject(this.id);
                }
                
            }
            else if (isDead) { 
                //往上飄
                this.y -= DEAD_FLY_UP_SPEED;
                if (IsOuterGameScreenBorder()) {
                    //從DropObjectList中移除
                    ((PlayGameState)gameState).RemoveDropObjs(this);
                    //從GameObjects移除自己
                    ((PlayGameState)this.gameState).RemoveGameObject(this.id);
                    
                }
            }

            //設定座標
            pCurrentAnimation.SetToLeftPos(base.x, base.y);
            //更新frame
            pCurrentAnimation.UpdateFrame(base.gameState.GetTimeSpan());
            //設定現在的圖片長寬為遊戲元件的長寬
            this.Height = pCurrentAnimation.GetCurrentFrameTexture().Height;
            this.Width = pCurrentAnimation.GetCurrentFrameTexture().Width;

            //如果是活著的,這邊調整圖片站在地面上的位置
            if(isSaved)
                //Y軸 = 遊戲的畫面高度 - 目前播放的圖片高度
                this.y = (this.gameState.SetGetHeight - this.height);
        }


        protected override void Dispose(bool disposing)
        {
            if (!base.disposed)
            {

                if (disposing)
                {
                    if (animationList.Count > 0)
                    {
                        foreach (AnimationSprite animation in animationList) {
                            animation.Dispose();
                        }
                        animationList.Clear();
                        pCurrentAnimation = null;
                        caughtSound = null; 
                    }
                    
                    Debug.WriteLine("FirePlayer disposed.");
                }
            }
            disposed = true;
        }
    }
}
