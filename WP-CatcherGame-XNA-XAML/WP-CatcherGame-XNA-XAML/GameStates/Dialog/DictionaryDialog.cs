using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CatcherGame.GameStates;
using CatcherGame.GameObjects;
using CatcherGame.TextureManager;
using System.Diagnostics;
using CatcherGame.FileStorageHelper;
using CatcherGame.FontManager;
namespace CatcherGame.GameStates.Dialog
{
    public class DictionaryDialog : GameDialog
    {


        //按鈕
        Button leftButton;
        Button rightButton;
        //角色
        TextureLayer littlegirlTexture;
        TextureLayer littlegirlIntroTexture;
        TextureLayer fatdancerTexture;
        TextureLayer fatdancerIntroTexture;
        TextureLayer flyoldladyTexture;
        TextureLayer flyoldladyIntroTexture;
        TextureLayer manstubbleTexture;
        TextureLayer manstubbleIntroTexture;
        TextureLayer naughtyboyTexture;
        TextureLayer naughtyboyIntroTexture;
        TextureLayer oldmanTexture;
        TextureLayer oldmanIntroTexture;
        TextureLayer roxanneTexture;
        TextureLayer roxanneIntroTexture;
        TextureLayer nicoleTexture;
        TextureLayer nicoleIntroTexture;
        TextureLayer noTexture;

        //人物表參考DialogGameObjectEnum
        int roleStart;
        int roleEnd;

        //記錄檔
        GameRecordData readData;
        List<DropObjectsKeyEnum> caughtObjects;


        public DictionaryDialog(GameState pCurrentState)
            : base(pCurrentState)
        {

        }
        public override void BeginInit()
        {

            caughtObjects = new List<DropObjectsKeyEnum>();

            //設定人物起始直參考DialogGameObjectEnum數值
            roleStart = 1;
            roleEnd = 8;

            //初始化按鈕、圖片位置
            backgroundPos = new Vector2(0, 0);
            closeButton = new Button(base.currentState, base.countId++, 0, 0);
            leftButton = new Button(base.currentState, base.countId++, 0, 0);
            rightButton = new Button(base.currentState, base.countId++, 0, 0);

            fatdancerTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            fatdancerIntroTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            littlegirlTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            littlegirlIntroTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            flyoldladyTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            flyoldladyIntroTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            manstubbleTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            manstubbleIntroTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            naughtyboyTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            naughtyboyIntroTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            oldmanTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            oldmanIntroTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            roxanneTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            roxanneIntroTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            nicoleTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            nicoleIntroTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);
            noTexture = new TextureLayer(base.currentState, base.countId++, 0, 0);

            //設定目前Dialog狀態
            stCurrent = DialogStateEnum.STATE_DICTIONARY;

            //設定每次點近來都是以LittltGirl開頭
            gtCurrent = DialogGameObjectEnum.DICTIONARY_FATDANCER;

            //把遊戲中物件加入gameObject，讓切換可以分開顯示
            AddgameObject(DialogGameObjectEnum.DICTIONARY_FATDANCER, new GameObject[] { fatdancerTexture, fatdancerIntroTexture, rightButton });
            AddgameObject(DialogGameObjectEnum.DICTIONARY_FLYOLDLADY, new GameObject[] { flyoldladyTexture, flyoldladyIntroTexture, leftButton, rightButton });
            AddgameObject(DialogGameObjectEnum.DICTIONARY_LITTLEGIRL, new GameObject[] { littlegirlTexture, littlegirlIntroTexture,leftButton, rightButton });
            AddgameObject(DialogGameObjectEnum.DICTIONARY_MANSTUBBLE, new GameObject[] { manstubbleTexture, manstubbleIntroTexture, leftButton, rightButton });
            AddgameObject(DialogGameObjectEnum.DICTIONARY_NAUGHTYBOY, new GameObject[] { naughtyboyTexture, naughtyboyIntroTexture, leftButton, rightButton });
            AddgameObject(DialogGameObjectEnum.DICTIONARY_OLDMAN, new GameObject[] { oldmanTexture, oldmanIntroTexture, leftButton, rightButton });
            AddgameObject(DialogGameObjectEnum.DICTIONARY_ROXANNE, new GameObject[] { roxanneTexture, roxanneIntroTexture, leftButton,rightButton });
            AddgameObject(DialogGameObjectEnum.DICTIONARY_NICOLE, new GameObject[] { nicoleTexture, nicoleIntroTexture, leftButton });

            //把gameObject放到ObjectTable集合裡面
            AddObjectTable(DialogStateEnum.STATE_DICTIONARY, GetDialogGameObject);

            //
            AddGameObject(closeButton);
            base.isInit = true;
        }
        public override void LoadResource()
        {
            
            

            //載入字典遊戲物件資源檔
            background = currentState.GetTexture2DList(TextureManager.TexturesKeyEnum.DICTIONARY_BACKGROUND)[0];
            leftButton.LoadResource(TexturesKeyEnum.DICTIONARY_LEFT_BUTTON);
            rightButton.LoadResource(TexturesKeyEnum.DICTIONARY_RIGHT_BUTTON);
            littlegirlTexture.LoadResource(TexturesKeyEnum.DICTIONARY_LITTLEGIRL_TEXTURE);
            littlegirlIntroTexture.LoadResource(TexturesKeyEnum.DICTIONARY_LITTLEGIRL_INTRO_TEXTURE);
            fatdancerTexture.LoadResource(TexturesKeyEnum.DICTIONARY_FATDANCER_TEXTURE);
            fatdancerIntroTexture.LoadResource(TexturesKeyEnum.DICTIONARY_FATDANCER_INTRO_TEXTURE);
            flyoldladyTexture.LoadResource(TexturesKeyEnum.DICTIONARY_FLYOLDLADY_TEXTURE);
            flyoldladyIntroTexture.LoadResource(TexturesKeyEnum.DICTIONARY_FLYOLDLADY_INTRO_TEXTURE);
            manstubbleTexture.LoadResource(TexturesKeyEnum.DICTIONARY_MANSTUBBLE_TEXTURE);
            manstubbleIntroTexture.LoadResource(TexturesKeyEnum.DICTIONARY_MANSTUBBLE_INTRO_TEXTURE);
            naughtyboyTexture.LoadResource(TexturesKeyEnum.DICTIONARY_NAUGHTYBOY_TEXTURE);
            naughtyboyIntroTexture.LoadResource(TexturesKeyEnum.DICTIONARY_NAUGHTYBOY_INTRO_TEXTURE);
            oldmanTexture.LoadResource(TexturesKeyEnum.DICTIONARY_OLDMAN_TEXTURE);
            oldmanIntroTexture.LoadResource(TexturesKeyEnum.DICTIONARY_OLDMAN_INTRO_TEXTURE);
            roxanneTexture.LoadResource(TexturesKeyEnum.DICTIONARY_ROXANNE_TEXTURE);
            roxanneIntroTexture.LoadResource(TexturesKeyEnum.DICTIONARY_ROXANNE_INTRO_TEXTURE);
            nicoleTexture.LoadResource(TexturesKeyEnum.DICTIONARY_NICOLE_TEXTURE);
            nicoleIntroTexture.LoadResource(TexturesKeyEnum.DICTIONARY_NICOLE_INTRO_TEXTURE);
            closeButton.LoadResource(TexturesKeyEnum.DIALOG_CLOSE_BUTTON);
            noTexture.LoadResource(TexturesKeyEnum.DICTIONARY_NO);

            base.LoadResource();
        }
        public override void Update()
        {
            bool isTouchReleased = false;
            bool isClickCloseButton, isClickLeftButton, isClickRightButton;
            isClickCloseButton = isClickRightButton = isClickLeftButton = false;
            
            ReleaseGameObject();
            //讀取紀錄檔
            readData = FileStorageHelper.StorageHelperSingleton.Instance.LoadGameRecordData();
            if (readData != null && readData.CaughtDropObjects.ToList() != null)
            {
                caughtObjects = readData.CaughtDropObjects.ToList();
            }
            
            //指定當前頁面是DictionaryDialog頁面
            stCurrent = DialogStateEnum.STATE_DICTIONARY;

            //片段當前頁面是空值，就初始化給第一個角色
            if (gtCurrent == DialogGameObjectEnum.EMPTY)
                gtCurrent = DialogGameObjectEnum.DICTIONARY_FATDANCER;

            TouchCollection tc = base.currentState.GetCurrentFrameTouchCollection();

            if (tc.Count > 0)
            {
                foreach (TouchLocation touchLocation in tc)
                {
                    if (touchLocation.State == TouchLocationState.Released)
                    {
                        isTouchReleased = true;
                            
                    }
                    else
                    {
                        isTouchReleased = false;
                    }
                    isClickCloseButton = closeButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, isTouchReleased);
                    isClickLeftButton = leftButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, isTouchReleased);
                    isClickRightButton = rightButton.IsPixelClicked(touchLocation.Position.X, touchLocation.Position.Y, isTouchReleased);
                }

                //判斷有無點擊到按鈕並切換(沒有同時都點到)
                if (!(isClickCloseButton && isClickLeftButton && isClickRightButton))
                {
                    //關閉按鈕
                    if (isClickCloseButton && !isClickLeftButton && !isClickRightButton)
                    {
                        base.CloseDialog();//透過父類別來關閉
                    }

                    //左邊按鈕
                    if (!isClickCloseButton && isClickLeftButton && !isClickRightButton)
                    {
                        if ((int)gtCurrent > roleStart)
                            gtCurrent--;//gtCurrent-1來切換目前的遊戲顯示物件
                    }

                    //右邊按鈕
                    if (!isClickCloseButton && !isClickLeftButton && isClickRightButton)
                    {
                        //判斷
                        if ((int)gtCurrent < roleEnd)
                            gtCurrent++;//gtCurrent+1來切換目前的遊戲顯示物件
                    }
                }
                    
            }

            base.Update(); //更新遊戲元件
        }
        public override void Draw()
        {

            gameSateSpriteBatch.Draw(background, backgroundPos, Color.White);
            base.Draw(); //繪製遊戲元件
        }

        /// <summary>
        /// 把遊戲中物件加入gameObject，讓切換可以分開顯示
        /// </summary>
        public void ReleaseGameObject()
        {
            objectTable[DialogStateEnum.STATE_DICTIONARY].Clear();

            

            //FatDancer
            if (caughtObjects.Contains(DropObjectsKeyEnum.PERSON_FAT_DANCE))
                AddgameObject(DialogGameObjectEnum.DICTIONARY_FATDANCER, new GameObject[] { fatdancerTexture, fatdancerIntroTexture, rightButton });
            else
                AddgameObject(DialogGameObjectEnum.DICTIONARY_FATDANCER, new GameObject[] { noTexture, rightButton });

            //LittleGirl
            if (caughtObjects.Contains(DropObjectsKeyEnum.PERSON_LITTLE_GIRL))
                AddgameObject(DialogGameObjectEnum.DICTIONARY_LITTLEGIRL, new GameObject[] { littlegirlTexture, littlegirlIntroTexture, leftButton, rightButton });
            else
                AddgameObject(DialogGameObjectEnum.DICTIONARY_LITTLEGIRL, new GameObject[] { noTexture, leftButton, rightButton });

            //FlyOldlady
            if (caughtObjects.Contains(DropObjectsKeyEnum.PERSON_FLY_OLDLADY))
                AddgameObject(DialogGameObjectEnum.DICTIONARY_FLYOLDLADY, new GameObject[] { flyoldladyTexture, flyoldladyIntroTexture, leftButton, rightButton });
            else
                AddgameObject(DialogGameObjectEnum.DICTIONARY_FLYOLDLADY, new GameObject[] { noTexture, leftButton, rightButton });

            //Manstubble
            if (caughtObjects.Contains(DropObjectsKeyEnum.PERSON_MAN_STUBBLE))
                AddgameObject(DialogGameObjectEnum.DICTIONARY_MANSTUBBLE, new GameObject[] { manstubbleTexture, manstubbleIntroTexture, leftButton, rightButton });
            else
                AddgameObject(DialogGameObjectEnum.DICTIONARY_MANSTUBBLE, new GameObject[] { noTexture, leftButton, rightButton });

            //NaughtyBoy
            if (caughtObjects.Contains(DropObjectsKeyEnum.PERSON_NAUGHTY_BOY))
                AddgameObject(DialogGameObjectEnum.DICTIONARY_NAUGHTYBOY, new GameObject[] { naughtyboyTexture, naughtyboyIntroTexture, leftButton, rightButton });
            else
                AddgameObject(DialogGameObjectEnum.DICTIONARY_NAUGHTYBOY, new GameObject[] { noTexture, leftButton, rightButton });

            //OldMan
            if (caughtObjects.Contains(DropObjectsKeyEnum.PERSON_OLD_MAN))
                AddgameObject(DialogGameObjectEnum.DICTIONARY_OLDMAN, new GameObject[] { oldmanTexture, oldmanIntroTexture, leftButton, rightButton });
            else
                AddgameObject(DialogGameObjectEnum.DICTIONARY_OLDMAN, new GameObject[] { noTexture, leftButton, rightButton });

            //Roxanne
            if (caughtObjects.Contains(DropObjectsKeyEnum.PERSON_ROXANNE))
                AddgameObject(DialogGameObjectEnum.DICTIONARY_ROXANNE, new GameObject[] { roxanneTexture, roxanneIntroTexture, leftButton,rightButton });
            else
                AddgameObject(DialogGameObjectEnum.DICTIONARY_ROXANNE, new GameObject[] { noTexture,rightButton, leftButton });

            //Nicole
            if (caughtObjects.Contains(DropObjectsKeyEnum.PERSON_NICOLE))
                AddgameObject(DialogGameObjectEnum.DICTIONARY_NICOLE, new GameObject[] { nicoleTexture, nicoleIntroTexture, leftButton });
            else
                AddgameObject(DialogGameObjectEnum.DICTIONARY_NICOLE, new GameObject[] { noTexture, leftButton });

        }
    }
}
