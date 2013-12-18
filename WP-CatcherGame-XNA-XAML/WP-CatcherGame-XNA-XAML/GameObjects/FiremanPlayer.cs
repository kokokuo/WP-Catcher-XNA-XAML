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


        AnimationSprite leftFireManWalkAnimation; //左邊的消防員
        AnimationSprite rightFireManWalkAnimation; //右邊的消防員
        //移動步伐
        int LEFT_MOVE_STEP = -7;
        int RIGHT_MOVE_STEP = 7;
       
        float rightFiremanXPos; //又邊消防員的位置
        float rightFiremanWidth;
        float leftFiremanWidth;
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
            //數值待解決(改為依裝置吃尺寸去調整)
            float leftFireManWidth = this.gameState.GetTexture2DList(TexturesKeyEnum.PLAY_FIREMAN_LEFT)[0].Width;
            savedNet = new Net(currentGameState, id, x + leftFireManWidth, y + 85, this);
            savedNet.AddSavedPerson += savedNet_AddSavedPerson;
            savedNet.CaughtEffectItems += savedNet_CaughtEffectItems;
            //網子資源
            savedNet.LoadResource(TexturesKeyEnum.PLAY_NET_NORMAL);
            savedNet.LoadResource(TexturesKeyEnum.PLAY_NET_SMALL);
            savedNet.LoadResource(TexturesKeyEnum.PLAY_NET_LARGE);

            Init();
            

            
        }
        protected override void Init()
        {
            isWalking = false;
            this.init_x = this.x = x;
            this.init_y = this.y = y;
            leftFireManWalkAnimation = new AnimationSprite(new Vector2(this.x, this.y), 300);
            //位置是網子座標 + 網子寬
            rightFiremanXPos = savedNet.X + savedNet.Width;
            rightFireManWalkAnimation = new AnimationSprite(new Vector2(rightFiremanXPos, this.y), 300); 
            caughtEffectItem = new LinkedList<EffectItem>();
            savePeopleNumber = 0;
            willRemoveItemsId = new List<int>();
            state = EffectState.NORMAL;

            caughtCreaturesKey = new List<DropObjectsKeyEnum>();

        }

        private void savedNet_CaughtEffectItems(EffectItem item)
        {
            bool isShoesEliminated = false;
            bool isNetEliminated = false;
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
                        isShoesEliminated = true;
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
                        isShoesEliminated = true;
                        break;
                    }
                }
               
            }
            

            //愛心
            if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_HEART)
            {
                ((PlayGameState)gameState).AddCanLostPeopleNumber();
            }


            //網子
            if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_EXPANDER && caughtEffectItem.Count > 0)
            {
                foreach (EffectItem slowItem in caughtEffectItem)
                {
                    if (slowItem.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_SHRINKER)
                    {
                        //互相抵消 所以兩個道具效果都要消除 ->set dead
                        slowItem.SetEffectElimination();
                        item.SetEffectElimination();
                        willRemoveItemsId.Add(slowItem.Id);
                        isNetEliminated = true;
                        break;
                    }
                }

            }
            else if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_SHRINKER && caughtEffectItem.Count > 0)
            {
                foreach (EffectItem boostingItem in caughtEffectItem)
                {
                    if (boostingItem.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_EXPANDER)
                    {
                        //互相抵消 所以兩個道具效果都要消除 -> set dead
                        boostingItem.SetEffectElimination();
                        item.SetEffectElimination();
                        willRemoveItemsId.Add(boostingItem.Id);
                        isNetEliminated = true;
                        break;
                    }
                }

            }

            if (!isShoesEliminated || !isNetEliminated) //如果沒有被抵銷
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
                if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES || item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_SLOW_SHOES)
                {
                    SetSpeedEffect(item.GetKeyEnum());
                }
                if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_SHRINKER || item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_EXPANDER)
                {
                    if(item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_SHRINKER){
                        savedNet.SetNetState(1);
                    }
                    else{
                        savedNet.SetNetState(2);
                    }
                    
                }
            }
            else
            { //被削除的話重設為預設速度
                if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES || item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_SLOW_SHOES)
                {
                    resetSpeedEffect();
                }
                if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_SHRINKER || item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_EXPANDER) {
                    savedNet.SetNetState(0);
                }
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
            //變回原來的網子
            if (effectItem.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_EXPANDER || effectItem.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_SHRINKER)
            {
                savedNet.SetNetState(0);
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

       
        //從List後面尋找同類的效果道具,並把屬性附加上去
        //e.g加速鞋時間到移除掉後,第二個加速鞋移動為第一個,並把效果附加上去
        private void UpdateEffectFromBufferItem()
        {
            foreach (EffectItem item in caughtEffectItem)
            {
                //可能同時附加(if)
                //速度的部分
                if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_BOOSTING_SHOES || item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_SLOW_SHOES)
                {
                    SetSpeedEffect(item.GetKeyEnum());
                }
                //網子
                if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_EXPANDER || item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_SHRINKER)
                {
                    if (item.GetKeyEnum() == DropObjectsKeyEnum.ITEM_NET_SHRINKER)
                    {
                        savedNet.SetNetState(1);
                    }
                    else
                    {
                        savedNet.SetNetState(2);
                    }
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
            if (key == TexturesKeyEnum.PLAY_FIREMAN) {
                leftFireManWalkAnimation.SetTexture2DList(base.gameState.GetTexture2DList(TexturesKeyEnum.PLAY_FIREMAN_LEFT));
                rightFireManWalkAnimation.SetTexture2DList(base.gameState.GetTexture2DList(TexturesKeyEnum.PLAY_FIREMAN_RIGHT));
                //左邊 + 右邊的高除2(平均高)
                this.Height = (leftFireManWalkAnimation.GetCurrentFrameTexture().Height + rightFireManWalkAnimation.GetCurrentFrameTexture().Height)/2;
                //寬 = 左消防員寬 + 網子寬 + 右邊消防員寬
                this.Width = leftFireManWalkAnimation.GetCurrentFrameTexture().Width + savedNet.Width + rightFireManWalkAnimation.GetCurrentFrameTexture().Width;

                this.rightFiremanWidth = rightFireManWalkAnimation.GetCurrentFrameTexture().Width;
                this.leftFiremanWidth = leftFireManWalkAnimation.GetCurrentFrameTexture().Width;
            }
            
        }

      
        

        public override void Update()
        {
            
            //更新左邊右邊
            //判斷往右後放大後會不會超過邊界
            if (savedNet.X + savedNet.Width + rightFiremanWidth >= this.gameState.GetRightGameScreenBorder())
            {
                //左邊新的消防員位置 = 右邊消防員的位置座標 - 網子(可能是新的狀態) - 左邊消防員的寬度
                this.x = this.rightFiremanXPos - savedNet.Width - leftFiremanWidth;
                //更新網子座標
                savedNet.X = this.rightFiremanXPos - savedNet.Width;
                //更新左邊消防員位置座標
                leftFireManWalkAnimation.SetToLeftPos(this.x, this.y);
                rightFireManWalkAnimation.SetToLeftPos(this.rightFiremanXPos, this.y); //位置不變
            }
            else
            {
                this.rightFiremanXPos = savedNet.X + savedNet.Width; //新的位置

                leftFireManWalkAnimation.SetToLeftPos(this.x, this.y);
                rightFireManWalkAnimation.SetToLeftPos(this.rightFiremanXPos, this.y);
            }
            //如果正在移動則更新圖像動畫
            if (isWalking){
                //左邊
                leftFireManWalkAnimation.UpdateFrame(base.gameState.GetTimeSpan());
                //右邊
                rightFireManWalkAnimation.UpdateFrame(base.gameState.GetTimeSpan());
                //設定現在的圖片長寬為遊戲元件的長寬
            }
            //左邊 + 右邊的高除2(平均高)
            this.Height = (leftFireManWalkAnimation.GetCurrentFrameTexture().Height + rightFireManWalkAnimation.GetCurrentFrameTexture().Height) / 2;
            //寬 = 左消防員寬 + 網子寬 + 右邊消防員寬
            this.Width = leftFireManWalkAnimation.GetCurrentFrameTexture().Width + savedNet.Width + rightFireManWalkAnimation.GetCurrentFrameTexture().Width;

            this.rightFiremanWidth = rightFireManWalkAnimation.GetCurrentFrameTexture().Width;
            this.leftFiremanWidth = leftFireManWalkAnimation.GetCurrentFrameTexture().Width;

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
            leftFireManWalkAnimation.Draw(spriteBatch);
            rightFireManWalkAnimation.Draw(spriteBatch);
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

        public void MoveByTouch(float vecX) {

            //檢查如果要移動是否會超處邊界(以網子為基準) 不會才給予下一步的移動座標
            if ((rightFiremanXPos + vecX + rightFiremanWidth) <= this.gameState.GetRightGameScreenBorder() && vecX > 0) //右邊
            {
               
                this.x += vecX;
                this.savedNet.X += vecX ; //網子跟著移動
                isWalking = true;

            }
            else if ((this.x + vecX) >= this.gameState.GetLeftGameScreenBorder() && vecX < 0) //左邊
            {
                this.x += vecX;
                this.savedNet.X += vecX ; //網子跟著移動
                isWalking = true;

            }
        }
        //重力感測器移動
        public void MoveByGSensor(float leftGameScreenBorder, float rightGameScreenBorder, Vector3 accSpeed) {

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

        //
        public bool IsBoundBoxClick(float x, float y) {
            if (x >= this.X &&
                x <= this.X + this.Width &&
                y >= this.Y &&
                y <= this.Y + this.Height)
            {
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
                    if (leftFireManWalkAnimation != null)
                    {
                        leftFireManWalkAnimation.Dispose();
                    }
                    if (rightFireManWalkAnimation != null)
                    {
                        rightFireManWalkAnimation.Dispose();
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
