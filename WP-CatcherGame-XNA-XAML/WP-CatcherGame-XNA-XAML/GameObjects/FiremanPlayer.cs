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
    public delegate void CaughtEffectItemsEventHandler(EffectItem item);
    public  delegate void ValueUpdateEventHandler(int newValue);
    
    public class FiremanPlayer : GameObject
    {
        public event ValueUpdateEventHandler SaveNewPerson;

        enum EffectState{
            NORMAL = 0,
            SPEED_UP,
            SLOW
        }


        AnimationSprite walkAnimation;
        //移動步伐
        int LEFT_MOVE_STEP = -7;
        int RIGHT_MOVE_STEP = 7;

        bool isWalking; //是否移動
        Net savedNet; //網子類別(Has)
        float init_x, init_y;
        int savePeopleNumber;
        List<int> willRemoveItemsId;
        //被接到的道具
        LinkedList<EffectItem> caughtEffectItem;
        EffectState state;
        List<DropObjectsKeyEnum> caughtCreaturesKey; 
        public FiremanPlayer(GameState currentGameState, int id, float x, float y)
            : base(currentGameState, id, x, y)
        {
            Init();
            //取得網子

            //數值待解決(改為依裝置吃尺寸去調整)
            savedNet = new Net(currentGameState, id, x + 73, y + 85, this);
            savedNet.AddSavedPerson += savedNet_AddSavedPerson;
            savedNet.CaughtEffectItems += savedNet_CaughtEffectItems;
            savedNet.LoadResource(TexturesKeyEnum.PLAY_NET);

            caughtCreaturesKey = new List<DropObjectsKeyEnum>();
        }
        
        private void savedNet_CaughtEffectItems(EffectItem item)
        {
            bool isEliminated = false;
            float displayX = (((PlayGameState)gameState).GetLifeTextureLayer().X + ((PlayGameState)gameState).GetLifeTextureLayer().Width / 2 ) - item.Width / 2;
            //註冊使用時間到的時候的事件
            item.EffectTimesUp += item_EffectTimesUp;

            //鞋子部分
            if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES && caughtEffectItem.Count > 0)
            {
                foreach (EffectItem slowItem in caughtEffectItem)
                {
                    if (slowItem.GetKeyEnum() == DropObjectsKeyEnum.ITEM_SLOW_SHOES)
                    {
                        //互相抵消 所以兩個道具效果都要消除 ->set dead
                        slowItem.SetEffectElimination(); 
                        item.SetEffectElimination();
                        willRemoveItemsId.Add(slowItem.Id);
                        isEliminated = true;
                        break;
                    }
                }
                
            }
            else if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_SLOW_SHOES && caughtEffectItem.Count >0)
            {
                foreach (EffectItem boostingItem in caughtEffectItem) {
                    if (boostingItem.GetKeyEnum() == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES) {
                        //互相抵消 所以兩個道具效果都要消除 -> set dead
                        boostingItem.SetEffectElimination();
                        item.SetEffectElimination();
                         willRemoveItemsId.Add(boostingItem.Id);
                        isEliminated = true;
                        break;
                    }
                }
               
            }
            if (!isEliminated) //如果沒有被抵銷
            {
                if (caughtEffectItem.Count == 0) //第一個道具 加在最上面 位置對其在Life的下方
                {
                    item.X = displayX;
                    item.Y = ((PlayGameState)gameState).GetLifeTextureLayer().Y + ((PlayGameState)gameState).GetLifeTextureLayer().Height;
                    caughtEffectItem.AddFirst(item);
                }
                else //加在上一個道具顯示的位置下方
                {
                    item.X = displayX;
                    item.Y = (caughtEffectItem.Last.Value.Y + caughtEffectItem.Last.Value.Height);
                    caughtEffectItem.AddLast(item);
                }
                if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES || item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_SLOW_SHOES){
                    SetSpeedEffect(item.GetKeyEnum());
                }
            }
            else { //被削除的話重設為預設速度
                if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES || item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_SLOW_SHOES){
                    resetSpeedEffect();
                }
            }

            //愛心
            if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_HEART)
            {
                ((PlayGameState)gameState).AddCanLostPeopleNumber();
            }
        }

       

        private void SetSpeedEffect(DropObjectsKeyEnum key) {
            if (state == EffectState.NORMAL)
            {
                if (key == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES)
                {
                    state = EffectState.SPEED_UP;
                    LEFT_MOVE_STEP = -12;
                    RIGHT_MOVE_STEP = 12;
                }
                else if (key == DropObjectsKeyEnum.ITEM_SLOW_SHOES)
                {
                    state = EffectState.SLOW;
                    LEFT_MOVE_STEP = -4;
                    RIGHT_MOVE_STEP = 4;
                }
            }
        }
        private void resetSpeedEffect() {
            state = EffectState.NORMAL;
            LEFT_MOVE_STEP = -7;
            RIGHT_MOVE_STEP = 7;
        }

        /// <summary>
        /// 道具效果時間已到移除
        /// </summary>
        /// <param name="effectItem"></param>
        private void item_EffectTimesUp(EffectItem effectItem)
        {
            willRemoveItemsId.Add(effectItem.Id);
            if (effectItem.GetKeyEnum() == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES || effectItem.GetKeyEnum() == DropObjectsKeyEnum.ITEM_SLOW_SHOES) {
                resetSpeedEffect();
            }

        }

        /// <summary>
        /// 累加新的Person
        /// </summary>
        private  void savedNet_AddSavedPerson(DropObjects obj)
        {
            savePeopleNumber++;
            //判斷有無接到此Key有的話則不再放入
            if (obj is Creature) {
                bool isFind = false;
                    foreach(DropObjectsKeyEnum caughtObjKey in caughtCreaturesKey){
                        if(caughtObjKey == obj.GetKeyEnum()){
                            isFind = true;
                            break;
                        }
                    }
                   if(!isFind)
                       caughtCreaturesKey.Add(obj.GetKeyEnum());
            }
                
            
            //觸發事件
            if (SaveNewPerson != null)
            {
                SaveNewPerson.Invoke(savePeopleNumber);
            }
        }
        /// <summary>
        /// 取得接到的角色
        /// </summary>
        /// <returns></returns>
        public List<DropObjectsKeyEnum> GetCaughtKey() {
            return caughtCreaturesKey;
        }

        protected override void Init()
        {
            isWalking = false;
            this.init_x = this.x = x;
            this.init_y = this.y = y;
            walkAnimation = new AnimationSprite(new Vector2(this.x, this.y), 300);
            caughtEffectItem = new LinkedList<EffectItem>();
            savePeopleNumber = 0;
            willRemoveItemsId = new List<int>();
            state = EffectState.NORMAL;
           
        }

        //從List後面尋找同類的效果道具,並把屬性附加上去
        //e.g加速協時間到移除掉後,第二個加速協移動為第一個,並把效果附加上去
        private void UpdateEffectFromBufferItem()
        {
            foreach (EffectItem item in caughtEffectItem)
            {
                //速度的部分
                if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES || item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_SLOW_SHOES)
                {
                    SetSpeedEffect(item.GetKeyEnum());
                }
            }

        }
       
        /// <summary>
        /// 確認掉落的所有元件有無接觸到網子
        /// </summary>
        /// <param name="fallingObjs"></param>
        public void CheckIsCaught(List<DropObjects> fallingObjs) {
            savedNet.CheckCollision(fallingObjs);    
        }

        public override void LoadResource(TextureManager.TexturesKeyEnum key)
        {
            SetTexture2DList(key);
        }

        /// <summary>
        /// 設定載入的圖片組,使用給予Key方式設定載入
        /// </summary>
        /// <param name="key"></param>
        private void SetTexture2DList(TexturesKeyEnum key)
        {
            walkAnimation.SetTexture2DList(base.gameState.GetTexture2DList(key));
            this.Height = walkAnimation.GetCurrentFrameTexture().Height;
            this.Width = walkAnimation.GetCurrentFrameTexture().Width;
        }

      
        

        public override void Update()
        {
            //如果正在移動則更新圖像動畫
            if (isWalking){
                walkAnimation.SetToLeftPos(this.x, this.y);
                walkAnimation.UpdateFrame(base.gameState.GetTimeSpan());
                //設定現在的圖片長寬為遊戲元件的長寬
                this.Height = walkAnimation.GetCurrentFrameTexture().Height;
                this.Width = walkAnimation.GetCurrentFrameTexture().Width;
            }
            if(caughtEffectItem.Count > 0 ){
                foreach (EffectItem item in caughtEffectItem){
                    item.Update();
                }
            }

           

            if (willRemoveItemsId.Count > 0) {
                RemoveEffectItemFromList();
            }
            savedNet.Update();
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spriteBatch)
        {
            walkAnimation.Draw(spriteBatch);
            savedNet.Draw(spriteBatch);
            if (caughtEffectItem.Count >0){
                foreach (EffectItem item in caughtEffectItem)
                {
                    item.Draw(spriteBatch);
                }
            }
        }

        //設定站立
        public void SetStand() {
            isWalking = false;
        }
        //設定消防員往左移動
        public void MoveLeft(float leftGameScreenBorder)
        {
            //檢查如果要移動是否會超處邊界(以網子為基準) 不會才給予下一步的移動座標
            if ((this.savedNet.X + LEFT_MOVE_STEP) >= leftGameScreenBorder)
            {
                //Debug.WriteLine("Can Move Left Way");
                this.x += LEFT_MOVE_STEP;
                this.savedNet.X += LEFT_MOVE_STEP; //網子跟著移動
                isWalking = true;
                
            }
        }

        //重力感測器移動
        public void Move(float leftGameScreenBorder, float rightGameScreenBorder, Vector3 accSpeed) {

            if (accSpeed.Y == 0)
            {
                isWalking = false;
            }
            else
            {
                //檢查如果要移動是否會超處邊界(以網子為基準) 不會才給予下一步的移動座標
                if ((this.savedNet.X + (accSpeed.Y * LEFT_MOVE_STEP)) >= leftGameScreenBorder)
                {
                    //Debug.WriteLine("Can Move Left Way");
                    this.x += (accSpeed.Y * LEFT_MOVE_STEP);
                    this.savedNet.X += (accSpeed.Y * LEFT_MOVE_STEP); //網子跟著移動
                    isWalking = true;

                }
                else if ((this.savedNet.X + this.savedNet.Width) + (accSpeed.Y * RIGHT_MOVE_STEP) <= rightGameScreenBorder)
                {
                    //Debug.WriteLine("Can Move Right Way");
                    this.x += (accSpeed.Y * RIGHT_MOVE_STEP);
                    this.savedNet.X += (accSpeed.Y * RIGHT_MOVE_STEP); //網子跟著移動
                    isWalking = true;

                }
            }
        }

        //設定消防員往右移動
        public void MoveRight(float rightGameScreenBorder)
        {
            //檢查如果要移動是否會超處邊界(以網子為基準) 不會才給予下一步的移動座標
            if ((this.savedNet.X + this.savedNet.Width) + RIGHT_MOVE_STEP <= rightGameScreenBorder)
            {
                //Debug.WriteLine("Can Move Right Way");
                this.x += RIGHT_MOVE_STEP;
                this.savedNet.X += RIGHT_MOVE_STEP; //網子跟著移動
                isWalking = true;
                
            }
        }

        private void MoveTheDisplayItemPosition() {
            //移動道具的位置
            if (caughtEffectItem.Count > 0)
            {
                caughtEffectItem.First.Value.Y = ((PlayGameState)gameState).GetLifeTextureLayer().Y + ((PlayGameState)gameState).GetLifeTextureLayer().Height;
                for (LinkedListNode<EffectItem> itemNode = caughtEffectItem.First.Next; itemNode != null; itemNode = itemNode.Next)
                {
                    itemNode.Value.Y = itemNode.Previous.Value.Y;
                }
            }
        }
        /// <summary>
        /// 將 id 放入準備要被刪除的 list
        /// </summary>
        /// <param name="id"></param>
        public void RemoveDropObject(int id)
        {
            willRemoveItemsId.Add(id);
        }

        /// <summary>
        /// 真正將 DropObjects 刪除
        /// </summary>
        private void RemoveEffectItemFromList()
        {
            foreach (int removeId in willRemoveItemsId)
            {
                foreach (EffectItem obj in caughtEffectItem)
                {
                    if (obj.Id == removeId)
                    {
                        caughtEffectItem.Remove(obj);
                        break;
                    }
                }
            }

            //移動道具顯示的位置
            MoveTheDisplayItemPosition();
            //尋找後面的有無道具可以繼續附加效果
            UpdateEffectFromBufferItem();

            willRemoveItemsId.Clear();
        }

        protected override void Dispose(bool disposing)
        {
            if (!base.disposed)
            {

                if (disposing)
                {
                    if (walkAnimation != null)
                    {
                        walkAnimation.Dispose();
                    }
                    if (savedNet != null) {
                        savedNet.Dispose();
                    }
                    Debug.WriteLine("FirePlayer disposed.");
                }
            }
            disposed = true;
        }
    }
}
