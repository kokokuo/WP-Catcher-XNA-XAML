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
    public  delegate void ValueAddedEventHandler(DropObjects obj);
    
   
    //網子類別
    public class Net : GameObject
    {
        public event ValueAddedEventHandler AddSavedPerson;
        public event CaughtEffectItemsEventHandler CaughtEffectItems;

        AnimationSprite netStateAnimation; //網子動畫
        
        List<int> willRemoveObjectId;
        bool isCaught; //用來讓網子在接觸到物體時可以撥放網子往下凹的效果動畫,而做的判斷值
        FiremanPlayer player;
        public Net(GameState currentGameState, int id, float x, float y,FiremanPlayer player)
            : base(currentGameState, id, x, y) 
        {
            Init();
            this.player = player; //知道消防員
        }

        protected override void Init()
        {
            this.x = x;
            this.y = y;
            netStateAnimation = new AnimationSprite(new Vector2(this.x, this.y), 300);
            
            willRemoveObjectId = new List<int>();
            isCaught = false;
           
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
            netStateAnimation.SetTexture2DList(base.gameState.GetTexture2DList(key));
            this.Height = netStateAnimation.GetCurrentFrameTexture().Height;
            this.Width = netStateAnimation.GetCurrentFrameTexture().Width;
        }

        public override void Draw(SpriteBatch spriteBatch)
        {
            netStateAnimation.Draw(spriteBatch);

        }

        public override void Update()
        {
            netStateAnimation.SetToLeftPos(this.x, this.y);

            //如果接住 播完網子往下凹的動畫
            if (isCaught)
            {
                netStateAnimation.SetNextWantFrameIndex(1); //索引是1
            }
            else { //播正常的網子
                netStateAnimation.SetNextWantFrameIndex(0);
            }
        }
        /// <summary>
        /// 傳進遊戲持有的所有掉落物件,計算有無碰撞
        /// </summary>
        /// <param name="dropObjs"></param>
        public void CheckCollision(List<DropObjects> dropObjs) {
            isCaught = false;
            foreach (DropObjects obj in dropObjs){
                
                bool currentYIsTouch = ( (obj.Y + obj.Height) <= (this.y + this.height ) && ( (obj.Y + obj.Height) >= this.y ) );
                bool nextYIsTouch = (obj.GetNextFallingY() + obj.Height > (this.y + this.height) && obj.GetNextFallingY() <this.y) ;
                bool XIsTouch = ((obj.X + (obj.Width / 2) >= this.x) && ((obj.X + (obj.Width / 2)) <= (this.x + this.Width)));
                //角色的Y座標部分 現在由無在網子中,或是下次移動時有無在網子中 並且 X座標有在網子中
                if ((currentYIsTouch || nextYIsTouch) && XIsTouch)
                {
                    //如果掉落的物體沒有接觸到網子
                    if (!obj.GetIsTouch())
                    {
                        obj.SetTouched(); //設定接觸網子
                        this.isCaught = true; //網子有接到
                    }
                    else {
                        obj.SetCaught();  //設定為拯救到
            
                        //如果是People的Type
                        if (obj is Creature)
                        {
                            //累加拯救到的人數
                            if (AddSavedPerson != null) {
                                AddSavedPerson.Invoke(obj);
                            }
                        }
                        else if (obj is EffectItem) {  //是道具的話

                            //累加拯救到的人數
                            if (CaughtEffectItems != null)
                            {
                                CaughtEffectItems.Invoke( (EffectItem)obj );
                            }
                        
                        } 
                        //不使用PlayGameState.RemoveDropObjs(this) ,在迴圈中這樣移除會有問題
                        RemoveDropObject(obj.Id);
                    }
                   
                }
            }
              
            RemoveDropObjectFromList(dropObjs);
        }


        /// <summary>
        /// 將 id 放入準備要被刪除的 list
        /// </summary>
        /// <param name="id"></param>
        public void RemoveDropObject(int id)
        {
            willRemoveObjectId.Add(id);
        }

        /// <summary>
        /// 真正將 DropObjects 刪除
        /// </summary>
        private void RemoveDropObjectFromList(List<DropObjects> dropObjs)
        {
            foreach (int removeId in willRemoveObjectId)
            {
                foreach (DropObjects obj in dropObjs)
                {
                    if (obj.Id == removeId)
                    {
                        dropObjs.Remove(obj);
                        break;
                    }
                }
            }
            willRemoveObjectId.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if (!base.disposed)
            {

                if (disposing)
                {
                    if (netStateAnimation != null)
                    {
                        netStateAnimation.Dispose();
                    }
                    if (willRemoveObjectId.Count > 0) {
                        willRemoveObjectId.Clear();
                    }
                    Debug.WriteLine("FirePlayer disposed.");
                }
            }
            disposed = true;
        }
    }
}
