using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using CatcherGame.GameObjects;
using CatcherGame.GameStates;

namespace CatcherGame.GameStates.Dialog
{
    //基礎類別,如果有新的Dialog皆要繼承此 GameDialog
    public abstract class GameDialog  
    {
        protected GameState currentState;
        protected Texture2D background;
        protected Vector2 backgroundPos;
        protected Button closeButton;
        protected int countId;
        protected DialogGameObjectEnum gtCurrent;
        protected DialogStateEnum stCurrent;
        protected List<GameObject> gameObjects;
        protected Dictionary<DialogStateEnum, Dictionary<DialogGameObjectEnum, GameObject[]>> objectTable;
        protected Dictionary<DialogGameObjectEnum, GameObject[]> dgameObject;
        protected bool isInit;
        protected bool isLoadContent;
        protected SoundEffect clickSound; //播放click音效的元件
        public GameDialog(GameState pCurrentState)
        {
            gameObjects = new List<GameObject>();
            //objectTable存放該Dialog裡要有哪些遊戲物件的集合
            objectTable = new Dictionary<DialogStateEnum, Dictionary<DialogGameObjectEnum, GameObject[]>>();
            //dgameObject存放每個Dialog裡有哪些遊戲物件，之後再放入到objectTable裡
            dgameObject = new Dictionary<DialogGameObjectEnum, GameObject[]>();
            stCurrent = DialogStateEnum.EMPTY;//設定該Dialog的頁面狀態
            gtCurrent = DialogGameObjectEnum.EMPTY;//設定該Dialog是否需額外要載入遊戲物件，預設EMPTY
            this.currentState = pCurrentState;
            countId = 0;
            isInit = false;
            isLoadContent = false;
        }

        public abstract void BeginInit();

        //座標由子類別(真正要用的Dialog)設定
        public virtual void LoadResource()
        {
            //隨然closeButton可以由子類別決定，但觸控時候的點還是原位置的點。
            //暫時先這樣解決，等郭董起床再問他!!!!><''
            //因此暫時註解掉這行，
            //closeButton.LoadResource(TextureManager.TexturesKeyEnum.DIALOG_CLOSE_BUTTON);

        }
        /// <summary>
        /// 關閉Dialog,子類別Dialgo可以透過此來關閉
        /// </summary>
        protected void CloseDialog() {
            currentState.SetPopGameDialog(DialogStateEnum.EMPTY);
            stCurrent = DialogStateEnum.EMPTY;
            gtCurrent = DialogGameObjectEnum.EMPTY;

        }

        /// <summary>
        /// 當返回鍵按下時,處理返回鍵,預設是關閉Dialog
        /// </summary>
        public virtual void HandleBackButtonPressed() {
            CloseDialog();
        }

        //更新在Dialog中的所有遊戲物件
        public virtual void Update()
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Update();
            }

            if (stCurrent != DialogStateEnum.EMPTY)
            {//如果stCurrent不等於EMPTY,代表有需要更新遊戲物件在畫面上   
                Dictionary<DialogGameObjectEnum, GameObject[]> _dgameObject = new Dictionary<DialogGameObjectEnum, GameObject[]>();
                _dgameObject = objectTable[stCurrent];//取出該Dialog的遊戲物件

                foreach (GameObject gameObject in _dgameObject[gtCurrent])
                {
                    gameObject.Update();
                }
            }
        }

        public virtual void Draw(SpriteBatch gSpriteBatch)
        {
            foreach (GameObject gameObject in gameObjects)
            {
                gameObject.Draw(gSpriteBatch);
            }

            if (stCurrent != DialogStateEnum.EMPTY)
            {//如果stCurrent不等於EMPTY,代表有需要繪製遊戲物件在畫面上   
                Dictionary<DialogGameObjectEnum, GameObject[]> _dgameObject = new Dictionary<DialogGameObjectEnum, GameObject[]>();
                _dgameObject = objectTable[stCurrent];//取出該Dialog的遊戲物件

                foreach (GameObject gameObject in _dgameObject[gtCurrent])
                {
                    gameObject.Draw(gSpriteBatch);
                }
               

            }
        }
        /// <summary>
        /// 加入遊戲物件至此State
        /// </summary>
        /// <param name="gameObject"></param>
        public void AddGameObject(GameObject gameObject)
        {
            gameObjects.Add(gameObject);
        }

        /// <summary>
        /// 取得Dialog狀態是否已經初始化(避免再次初始化,或是如果有需要可以把遊戲狀態釋放,重新設定無初始化)
        /// </summary>
        public bool GetGameDialogHasInit
        {
            get { return isInit; }
        }

        /// <summary>
        /// 取得Dialog狀態是否已經載入資源(避免再次初始化,或是如果有需要可以把遊戲狀態釋放,重新設定無初始化)
        /// </summary>
        public bool GetGameDialogHasLoadContent
        {
            get { return isLoadContent; }
        }

        /// <summary>
        /// 將該Dialog的遊戲集合放置到objetTable裡
        /// </summary>
        /// <param name="stateEnum"></param>
        /// <param name="dgameObject"></param>
        public void AddObjectTable(DialogStateEnum stateEnum, Dictionary<DialogGameObjectEnum, GameObject[]> dgameObject)
        {
            objectTable.Add(stateEnum, dgameObject);
        }

        /// <summary>
        /// 將遊戲物件放入到Dictionary集合裡
        /// </summary>
        /// <param name="objectEnum"></param>
        /// <param name="gameObject"></param>
        public void AddgameObject(DialogGameObjectEnum objectEnum, GameObject[] gameObject)
        {
            dgameObject.Add(objectEnum, gameObject);
        }

        /// <summary>
        /// 取出該Dialog裡的集合
        /// </summary>
        public Dictionary<DialogGameObjectEnum, GameObject[]> GetDialogGameObject
        {
            get { return dgameObject; }
        }


       
    }
}
