using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CatcherGame.GameObjects;
using CatcherGame.TextureManager;
using CatcherGame.GameStates.Dialog;
using CatcherGame.FontManager;
using CatcherGame.FileStorageHelper;
using WP_CatcherGame_XNA_XAML;
namespace CatcherGame.GameStates
{
    public class PlayGameState  :GameState
    {
        int FIREMAN_INIT_X ;
        int FIREMAN_INIT_Y ;
        int RIGHT_MOVE_BUTTON_X_POS = 700;
        int LEFT_MOVE_BUTTON_X_POS = 0;
        int MOVE_BUTTON_Y_POS = 355;
        int LIFE_X = 710;
        int LIFE_Y = 20;
        int SCORE_X = 15;
        int SCORE_Y = 95;

        float savedPeoplefontX;
        float savedPeoplefontY;
        float lifefontX;
        float lifefontY;
        Button pauseButton;
        Button leftMoveButton;
        Button rightMoveButton;

        List<int> willRemoveObjectId;

        
        FiremanPlayer player;
        int lostPeopleNumber;
        int savedPeopleNumber;

        //文字資源
        SpriteFont savedPeopleNumberFont;
        SpriteFont lostPeopleNumberFont;

        //純粹圖片,無任何邏輯運算
        TextureLayer smokeTexture;
        TextureLayer lifeTexture;
        TextureLayer scoreTexture;
        TextureLayer floorTexture;
        //掉落的物件 (角色與道具)
        List<DropObjects> fallingObjects;
       
        RandGenerateDropObjsSystem randSys;
        bool isOver,isWriteingFile;
        
        public PlayGameState(GamePage gMainGame) 
            :base(gMainGame)
        {
            dialogTable = new Dictionary<DialogStateEnum, GameDialog>();
            dialogTable.Add(DialogStateEnum.STATE_PAUSE, new PauseDialog(this));
            fallingObjects = new List<DropObjects>();
            willRemoveObjectId = new List<int>();
            base.x = 0; base.y = 0;
            base.backgroundPos = new Vector2(base.x, base.y);

           
        }

        

        public override void BeginInit()
        {
            int player_x = base.GetDeviceScreenWidthByMainGame() / 2 - base.GetTexture2DList(TexturesKeyEnum.PLAY_FIREMAN)[0].Width / 2;
            int player_y = base.GetDeviceScreenHeightByMainGame() - base.GetTexture2DList(TexturesKeyEnum.PLAY_FLOOR)[0].Height / 2 - base.GetTexture2DList(TexturesKeyEnum.PLAY_FIREMAN)[0].Height;
            FIREMAN_INIT_X = player_x;
            FIREMAN_INIT_Y = player_y;
            RIGHT_MOVE_BUTTON_X_POS = base.GetDeviceScreenWidthByMainGame() - base.GetTexture2DList(TexturesKeyEnum.PLAY_RIGHT_MOVE_BUTTON)[0].Width; //-50;
            LEFT_MOVE_BUTTON_X_POS = 10;
            MOVE_BUTTON_Y_POS = base.GetDeviceScreenHeightByMainGame() - base.GetTexture2DList(TexturesKeyEnum.PLAY_FLOOR)[0].Height / 2 -base.GetTexture2DList(TexturesKeyEnum.PLAY_RIGHT_MOVE_BUTTON)[0].Height;
            LIFE_X = base.GetDeviceScreenWidthByMainGame() - base.GetTexture2DList(TexturesKeyEnum.PLAY_LIFE)[0].Width -30; //- 70;
            LIFE_Y = 20;
            SCORE_X = 15;
            SCORE_Y = base.GetTexture2DList(TexturesKeyEnum.PLAY_PAUSE_BUTTON)[0].Height;


            //設定消防員的移動邊界(包含角色掉落的邊界也算在內)
            base.rightGameScreenBorder = RIGHT_MOVE_BUTTON_X_POS;
            base.leftGameScreenBorder = base.GetTexture2DList(TexturesKeyEnum.PLAY_LEFT_MOVE_BUTTON)[0].Width;
            isOver = false;
            isWriteingFile = false;
            //初始化隨機角色產生系統
            randSys = new RandGenerateDropObjsSystem(this, 2,3, 3,3,5,2);
            randSys.SetBorder(leftGameScreenBorder, rightGameScreenBorder);

            base.objIdCount = 0;
            lostPeopleNumber = 3;
            savedPeopleNumber = 0;
            pauseButton = new Button(this, objIdCount++, 0, 0);
            leftMoveButton = new Button(this, objIdCount++, LEFT_MOVE_BUTTON_X_POS, MOVE_BUTTON_Y_POS);
            rightMoveButton = new Button(this, objIdCount++, RIGHT_MOVE_BUTTON_X_POS, MOVE_BUTTON_Y_POS);

            player = new FiremanPlayer(this, objIdCount++, FIREMAN_INIT_X, FIREMAN_INIT_Y);
            player.SaveNewPerson +=player_SaveNewPerson;

            smokeTexture = new TextureLayer(this,objIdCount++, 0, 0);
            lifeTexture = new TextureLayer(this, objIdCount++, LIFE_X, LIFE_Y);
            scoreTexture = new TextureLayer(this, objIdCount++, SCORE_X, SCORE_Y);
            
            int floor_y = base.GetDeviceScreenHeightByMainGame() - base.GetTexture2DList(TexturesKeyEnum.PLAY_FLOOR)[0].Height;
            floorTexture = new TextureLayer(this, objIdCount++, 0, floor_y);

            //加入遊戲元件
            AddGameObject(player);
            
           

            //啟動第一次隨機功能取得掉落角色
            List<DropObjects> generateObjs =  randSys.WorkCreatureRandom();
          
            //加入至遊戲中
            foreach (DropObjects obj in generateObjs)
            {
                AddGameObject(obj);
                fallingObjects.Add(obj);
            }

            //啟動第一次隨機功能取得掉落道具
            generateObjs = randSys.WorkEffectItemRandom();

            //加入至遊戲中
            foreach (DropObjects obj in generateObjs)
            {
                AddGameObject(obj);
                fallingObjects.Add(obj);
            }
          
            //訂閱事件
            randSys.GenerateDropObjs += randSys_GenerateDropObjs;

            //加入圖層
            AddGameObject(leftMoveButton);
            AddGameObject(rightMoveButton);
            AddGameObject(pauseButton);

            //對 對話框做初始化
            foreach (KeyValuePair<DialogStateEnum, GameDialog> dialog in dialogTable)
            {
                //如果初始化過就不再初始化
                if (!dialog.Value.GetGameDialogHasInit)
                {
                    dialog.Value.BeginInit();
                }
            }
            

            base.isInit = true;
        }

        private void player_SaveNewPerson(int newValue)
        {
            savedPeopleNumber = newValue;
        }

        //釋放遊戲中的所有資料
        public void Release() {
            fallingObjects.Clear();
            smokeTexture.Dispose();
            lifeTexture.Dispose();
            scoreTexture.Dispose();
            floorTexture.Dispose();
            willRemoveObjectId.Clear();
            //指向NULL
            savedPeopleNumberFont = null;
            lostPeopleNumberFont = null;
            //取消事件訂閱
            player.SaveNewPerson -= player_SaveNewPerson;
            foreach (GameObject obj in gameObjects) {
                obj.Dispose();
            }
            randSys.Dispose();
            randSys.GenerateDropObjs -= randSys_GenerateDropObjs;
            gameObjects.Clear();
            base.isInit  = false;
        }

        //扣掉沒接到
        public void SubtractCanLostPeopleNumber() {
            this.lostPeopleNumber--;
            if (this.lostPeopleNumber <= 0) {
                //Release();
                //遊戲結束
                Debug.WriteLine("Game Over...");
                isOver = true;
            }
        }

        public override void LoadResource()
        {
            //載入文字
            savedPeopleNumberFont = base.GetSpriteFontFromKeyByGameState(SpriteFontKeyEnum.PLAY_SAVED_PEOPLE_FONT);
            lostPeopleNumberFont = base.GetSpriteFontFromKeyByGameState(SpriteFontKeyEnum.PLAT_LOST_PEOPLE_FONT);

            //載入圖片
            base.background = base.GetTexture2DList(TexturesKeyEnum.PLAY_BACKGROUND)[0];
            pauseButton.LoadResource(TexturesKeyEnum.PLAY_PAUSE_BUTTON);
            leftMoveButton.LoadResource(TexturesKeyEnum.PLAY_LEFT_MOVE_BUTTON);
            rightMoveButton.LoadResource(TexturesKeyEnum.PLAY_RIGHT_MOVE_BUTTON);
            player.LoadResource(TexturesKeyEnum.PLAY_FIREMAN);
            

            smokeTexture.LoadResource(TexturesKeyEnum.PLAY_SMOKE);
            lifeTexture.LoadResource(TexturesKeyEnum.PLAY_LIFE);
            scoreTexture.LoadResource(TexturesKeyEnum.PLAY_SCORE);
            floorTexture.LoadResource(TexturesKeyEnum.PLAY_FLOOR);

            
            //載入對話框的圖片資源
            foreach (KeyValuePair<DialogStateEnum, GameDialog> dialog in dialogTable)
            {
                if (!dialog.Value.GetGameDialogHasLoadContent)
                {
                    //把繪製的元件 gameSateSpriteBatch 傳入進去,讓對話框可以透過此 gameSateSpriteBatch 來繪製
                    dialog.Value.SetSpriteBatch(this.gameSateSpriteBatch);
                    dialog.Value.LoadResource();
                }
            }
        }

        void randSys_GenerateDropObjs(List<DropObjects> objs)
        {
            foreach (DropObjects obj in objs) {
                AddGameObject(obj);
                fallingObjects.Add(obj);
            }
        }

        private void SaveData() {
            GameRecordData readData = null;
            //紀錄檔案
            GameRecordData saveData = new GameRecordData();
            //先載入舊資料
            readData = FileStorageHelper.StorageHelperSingleton.Instance.LoadGameRecordData();
           
            //與舊資料作判斷
            if (readData != null){
                if (readData.HistoryTopSavedNumber < savedPeopleNumber){
                    saveData.HistoryTopSavedNumber = savedPeopleNumber;
                }
                else {
                    saveData.HistoryTopSavedNumber = readData.HistoryTopSavedNumber;
                }
                //check檔案中使否影經擁有角色
                foreach(DropObjectsKeyEnum getkey in   player.GetCaughtKey()){
                    bool isGot = false; 
                    foreach(DropObjectsKeyEnum key in  readData.CaughtDropObjects){
                        if(getkey == key){
                            isGot = true; 
                            break;
                        }
                    }
                    if(!isGot){ //如果沒有拿到過 先放到舊資料中
                         readData.CaughtDropObjects.Add(getkey);
                    }
                }
                //一次把舊資料放到存檔區
                saveData.CaughtDropObjects = readData.CaughtDropObjects;
            }
            else{ //如果沒有資料
                saveData.HistoryTopSavedNumber = savedPeopleNumber;
                saveData.CaughtDropObjects = player.GetCaughtKey();
            }
            saveData.CurrentSavePeopleNumber = savedPeopleNumber;

            if (!isWriteingFile)
            {
                try
                {
                    FileStorageHelper.StorageHelperSingleton.Instance.SaveGameRecordData(saveData);
                }
                catch { }
                isWriteingFile = true;
            }
        }

       public override void Update()
        {
            //如果沒有談出對話框->處理遊戲邏輯
            if (!base.hasDialogShow)
            {
                randSys.UpdateTime(this.GetTimeSpan());　//隨機系統更新時間

                TouchCollection tc = base.GetCurrentFrameTouchCollection();
                bool isMoveRight,isMoveLeft,isClickPause;
                isClickPause = isMoveLeft = isMoveRight = false;
                if (tc.Count > 0)  {
                    //取出點此frame下同時點擊的所有座標,並先對所有座標去做按鈕上的點擊判斷
                    foreach (TouchLocation touchLocation in tc) {
                        if (!isMoveRight)
                            isMoveRight = rightMoveButton.IsBoundingBoxClick((int)touchLocation.Position.X, (int)touchLocation.Position.Y);
                        if (!isMoveLeft)
                            isMoveLeft = leftMoveButton.IsBoundingBoxClick((int)touchLocation.Position.X, (int)touchLocation.Position.Y);
                        if (!isClickPause)
                            isClickPause = pauseButton.IsPixelClick((int)touchLocation.Position.X, (int)touchLocation.Position.Y);
                    }
                   
                    //遊戲邏輯判斷
                    if (isMoveLeft && !isMoveRight)
                    {
                        //Debug.WriteLine("Click Left Button");
                        player.MoveLeft(leftGameScreenBorder);
                    }
                    else if (!isMoveLeft && isMoveRight)
                    {
                        //Debug.WriteLine("Click Right Button");
                        player.MoveRight(rightGameScreenBorder);
                    }
                    else if (!isMoveLeft && !isMoveRight)
                    {
                        player.SetStand(); //設定站立
                    }
                    if(isClickPause){
                        this.SetPopGameDialog(DialogStateEnum.STATE_PAUSE);
                    }

                }
                else {
                    player.SetStand(); //設定站立
                }
                player.CheckIsCaught(fallingObjects);

                //如果有要移除的元件,執行移除方法
                if (willRemoveObjectId.Count > 0) {
                    RemoveGameObjectFromList();
                }
                
                //切換到遊戲結束的畫面
                if (isOver) {
                    SaveData();
                    this.Release();
                    //切換狀態
                    base.SetNextGameSateByMain(GameStateEnum.STATE_GAME_OVER);
                    
                }
            }
            base.Update();
        }
        public override void Draw()
        {
            // 繪製主頁背景
            gameSateSpriteBatch.Draw(base.background, base.backgroundPos, Color.White);
            floorTexture.Draw(this.GetSpriteBatch());
            base.Draw();
            smokeTexture.Draw(this.GetSpriteBatch());
            lifeTexture.Draw(this.GetSpriteBatch());
            scoreTexture.Draw(this.GetSpriteBatch());
            //繪製文字資源
            //座標位置以調整為依照圖片之間的位置距離去設定,帶有待調整
            //
            savedPeoplefontX = ((SCORE_X + scoreTexture.Width)/2) - savedPeopleNumberFont.MeasureString(savedPeopleNumber.ToString()).X/2 +5;
            savedPeoplefontY = ((SCORE_Y + scoreTexture.Height)/2) + savedPeopleNumberFont.MeasureString(savedPeopleNumber.ToString()).Y/2 - 10;
            lifefontX = LIFE_X + lifeTexture.Width -5;
            lifefontY = LIFE_Y - 10; //微修正
            gameSateSpriteBatch.DrawString(savedPeopleNumberFont, savedPeopleNumber.ToString(), new Vector2(savedPeoplefontX, savedPeoplefontY), Color.White);
            gameSateSpriteBatch.DrawString(lostPeopleNumberFont, lostPeopleNumber.ToString(), new Vector2(lifefontX, lifefontY), Color.White);

          
        }
        

        /// <summary>
        /// 將 id 放入準備要被刪除的 list
        /// </summary>
        /// <param name="id"></param>
        public void RemoveGameObject(int id)
        {
            willRemoveObjectId.Add(id);
        }

        /// <summary>
        /// 真正將 GameObject 刪除
        /// </summary>
        private void RemoveGameObjectFromList()
        {
            //有空在修改成LINQ語法...
            foreach (int removeId in willRemoveObjectId)
            {
                foreach (GameObject gameObject in gameObjects)
                {
                    if (gameObject.Id == removeId)
                    {
                        gameObject.Dispose(); //釋放資源
                        gameObjects.Remove(gameObject);
                        break;
                    }
                }
            }

            willRemoveObjectId.Clear();
        }

        /// <summary>
        /// 移除掉落的物件
        /// </summary>
        /// <param name="fallingObj"></param>
        public void RemoveDropObjs(DropObjects fallingObj) {
            fallingObjects.Remove(fallingObj);
            //不可Dispose,Dispose應該要由GameObjects來做
        }

        public TextureLayer GetLifeTextureLayer() {
            return lifeTexture;
        }
        public TextureLayer GetScoreTextureLayer()
        {
            return scoreTexture;
        }
    }
}
