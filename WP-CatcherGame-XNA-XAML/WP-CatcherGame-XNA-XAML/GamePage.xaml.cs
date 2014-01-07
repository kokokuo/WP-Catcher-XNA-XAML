using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;


using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Devices.Sensors; //重力感測器

using CatcherGame.TextureManager;
using CatcherGame.GameStates;
using CatcherGame.FontManager;
using System.Diagnostics;
using Facebook.Client;
using System.Threading.Tasks;
using Microsoft.Phone.Shell; //客製化Tile用
using CatcherGame.FileStorageHelper;
using CatcherGame.SongManager;
using CatcherGame.SoundManager;
using System.IO.IsolatedStorage;

namespace WP_CatcherGame_XNA_XAML
{
    //處理顯示動態磚的類別
    public class ApplicationTileHandler{
        GameRecordData readData;
        string topSavedPeoepleNumber = "";

        public void GetNewCatcherRecord() {
            // Application Tile is always the first Tile, even if it is not pinned to Start.
            ShellTile AppTile = ShellTile.ActiveTiles.First();
           
            // Application should always be found
            if (AppTile != null)
            {
                //取得資料
                readData = CatcherGame.FileStorageHelper.StorageHelperSingleton.Instance.LoadGameRecordData();
                if (readData != null && readData.HistoryTopSavedNumber != 0)
                { //分數不為零
                    topSavedPeoepleNumber = "You saved\n\""+ readData.HistoryTopSavedNumber.ToString() + "\" people\nTry it better!";
                }
                else { 
                    topSavedPeoepleNumber = "Not Anyone be Saved\n Hurry up!";
                }
                // set the properties to update for the Application Tile
                // Empty strings for the text values and URIs will result in the property being cleared.
                StandardTileData NewTileData = new StandardTileData
                {
                    Title = "CatcherGame",
                    BackgroundImage = new Uri("gamelogo_173.png", UriKind.Relative),
                    BackTitle = "CatcherGame",
                    BackBackgroundImage = new Uri("gamelogo_back2_173.png", UriKind.Relative),
                    BackContent = topSavedPeoepleNumber
                };

                // Update the Application Tile
                AppTile.Update(NewTileData);
            }

        
        }
    }



    public partial class GamePage : PhoneApplicationPage
    {
        private FacebookSession session;
        bool isGameFirstEnter = true; //用來判斷應用程式是否第一次進入
        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;
        GameSongManager songManager;
        SoundEffectManager soundManager;
        Queue<TouchLocation> touchQueue;

        //遊戲狀態表
        Dictionary<GameStateEnum, GameState> gameStateTable;
        GameState pCurrentScreenState;

        //圖片管理器
        Texture2DManager texture2DManager;

        //文字管理器
        SpriteFontManager fontManager;

        //現在這張frame所擁有的所有觸控點集合
        TouchCollection currtenTouchCollection;

        Accelerometer acc; //三軸加速器
        Vector3 accVector; //紀錄三軸加速器資料
        ApplicationTileHandler catcherTile;

        IsolatedStorageSettings settings;
        /*
         * 從MainPage進入到GamePage 會先進到建構子,再到NavigateTo,如果在遊戲中(選單或是遊戲進行狀態中)點擊Home或BingSearch後再回到遊戲
         * 會直接進到NavigateTo,因為目前在NavigateTo都會呼叫Init,所以要在這邊判斷
         */

        public GamePage()
        {
            InitializeComponent();
            //Tile
            catcherTile = new ApplicationTileHandler();

            this.SupportedOrientations = SupportedPageOrientation.Landscape; //設定可支援方位
            this.Orientation = PageOrientation.LandscapeLeft; //設定方位
            // 從應用程式取得內容管理員
            contentManager = (Application.Current as App).Content;

           

            // 為這個頁面建立計時器
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
            SupportedOrientations = SupportedPageOrientation.Landscape;
            Debug.WriteLine(SharedGraphicsDeviceManager.Current.PreferredBackBufferHeight);
            Debug.WriteLine(SharedGraphicsDeviceManager.Current.PreferredBackBufferWidth);


            //遊戲狀態表
            gameStateTable = new Dictionary<GameStateEnum, GameState>();
            gameStateTable.Add(GameStateEnum.STATE_MENU, new HomeMenuState(this));
            gameStateTable.Add(GameStateEnum.STATE_START_COMIC, new GameStartComicState(this));
            gameStateTable.Add(GameStateEnum.STATE_PLAYGAME, new PlayGameState(this));
            gameStateTable.Add(GameStateEnum.STATE_GAME_OVER, new GameOverState(this));
            //設定水平橫向時的座標
            TouchPanel.DisplayOrientation = Microsoft.Xna.Framework.DisplayOrientation.LandscapeLeft;
            TouchPanel.EnabledGestures = GestureType.Tap | GestureType.FreeDrag | GestureType.None | GestureType.Hold;

            touchQueue = new Queue<TouchLocation>();

            //三軸加速器
            accVector = new Vector3();
            acc = new Accelerometer();
           
          
            acc.CurrentValueChanged += new EventHandler<SensorReadingEventArgs<AccelerometerReading>>(acc_CurrentValueChanged);

            if (Accelerometer.IsSupported)
            {
                Debug.WriteLine("Support");
                try
                {
                    acc.Start();
                }
                catch { }
            }
            else {
                Debug.WriteLine("Not Support");
            }
        }

        private void acc_CurrentValueChanged(object sender, SensorReadingEventArgs<AccelerometerReading> arg)
        {
            accVector.X = (float)arg.SensorReading.Acceleration.X;
            accVector.Y = (float)arg.SensorReading.Acceleration.Y;
            accVector.Z = (float)arg.SensorReading.Acceleration.Z;
        }


        private void InitResourceManager() {
            fontManager = new SpriteFontManager(this);
            texture2DManager = new Texture2DManager(this);
            songManager = new GameSongManager(this);
            soundManager = new SoundEffectManager(this);
        }

        private void InitState()
        {
            pCurrentScreenState = gameStateTable[GameStateEnum.STATE_MENU];
            pCurrentScreenState.BeginInit();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

            // 設定圖形裝置的共用模式，以開啟 XNA 呈現
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // 建立可用來繪製紋理的新 SpriteBatch。
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            if (isGameFirstEnter)
            {
                //初始化各個資源管理器(需要設定SetSharingMode後才能使用)
                InitResourceManager();
                // TODO: 在此處使用 this.content 載入遊戲內容
                InitState();
              

                isGameFirstEnter = false;
                pCurrentScreenState.LoadResource();
            }
            else {
                //gameStateTable.Clear();
                //gameStateTable.Add(GameStateEnum.STATE_MENU, ((Dictionary<GameStateEnum, GameState>)settings["gameStateTable"])[GameStateEnum.STATE_MENU]);
                //gameStateTable.Add(GameStateEnum.STATE_START_COMIC, ((Dictionary<GameStateEnum, GameState>)settings["gameStateTable"])[GameStateEnum.STATE_START_COMIC]);
                //gameStateTable.Add(GameStateEnum.STATE_PLAYGAME, ((Dictionary<GameStateEnum, GameState>)settings["gameStateTable"])[GameStateEnum.STATE_PLAYGAME]);
                //gameStateTable.Add(GameStateEnum.STATE_GAME_OVER, ((Dictionary<GameStateEnum, GameState>)settings["gameStateTable"])[GameStateEnum.STATE_GAME_OVER]);
            }
            

            // 啟動計時器
            timer.Start();

            base.OnNavigatedTo(e);
        }

        //
        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // 停止計時器
            timer.Stop();

            // 設定圖形裝置的共用模式，以關閉 XNA 呈現 
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);
            
            
            // txtInput is a TextBox defined in XAML.
            //if (!settings.Contains("GameTable"))
            //{
                
            //    settings.Add("GameTable", gameStateTable);
            //}
            //else
            //{
            //    settings["GameTable"] = gameStateTable;
            //}
            //settings.Save();
            

            base.OnNavigatedFrom(e);
        }
        /// <summary>
        /// 允許頁面執行邏輯，例如更新環境、
        /// 檢查衝突、收集輸入和播放音訊。
        /// </summary>
        private void OnUpdate(object sender, GameTimerEventArgs e)
        {
            // TODO: 在此處加入更新邏輯
            // 允許遊戲結束 預設方法

            //移除放在堆疊區的前面頁面，用來直接跳回手機畫面
            if (NavigationService.CanGoBack)
            {
                while (NavigationService.RemoveBackEntry() != null)
                {
                    NavigationService.RemoveBackEntry();
                }
            }
            catcherTile.GetNewCatcherRecord();     //取得新的紀錄到Tile中
                
            TouchCollection tc = TouchPanel.GetState();
            currtenTouchCollection = tc;
            foreach (TouchLocation location in tc)
            {
                touchQueue.Enqueue(location);
            }
            pCurrentScreenState.Update();
        }

        /// <summary>
        /// 允許頁面繪製自身。
        /// </summary>
        private void OnDraw(object sender, GameTimerEventArgs e)
        {
            SharedGraphicsDeviceManager.Current.GraphicsDevice.Clear(Color.CornflowerBlue);

            // TODO: 在此處加入繪圖程式碼
           
            spriteBatch.Begin();
            //繪製現在的狀態
            pCurrentScreenState.Draw(this.spriteBatch);
            spriteBatch.End();
        }

        protected override void OnBackKeyPress(System.ComponentModel.CancelEventArgs e)
        {
            if (! (pCurrentScreenState is HomeMenuState) ){
                e.Cancel = true;
            }
            
            base.OnBackKeyPress(e);
        }
        /// <summary>
        /// 清除TouchQueue裡面的所有狀態
        /// </summary>
        public void ClearTouchQueue()
        {
            touchQueue.Clear();
        }

        public bool IsEmptyQueue()
        {
            if (touchQueue.Count != 0) return false;
            else return true;
        }
        public TouchLocation GetTouchLocation()
        {
            return touchQueue.Dequeue();
        }

        /// <summary>
        /// 取得當下觸碰畫面的所有觸控點
        /// </summary>
        /// <returns></returns>
        public TouchCollection GetCurrentFrameTouchCollection()
        {
            return currtenTouchCollection;
        }

        /// <summary>
        /// 取得對應Key的圖片集
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public List<Texture2D> GetTexture2DList(TexturesKeyEnum key)
        {
            return texture2DManager.GetTexture2DList(key);
        }

        /// <summary>
        /// 取得文字資源
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public SpriteFont GetSpriteFontFromKey(SpriteFontKeyEnum key)
        {

            return fontManager.GetSpriteFontFromKey(key);
        }




        public void SetNextGameState(GameStateEnum nextStateKey)
        {
            //切換遊戲狀態
            pCurrentScreenState = gameStateTable[nextStateKey];
            if (!pCurrentScreenState.GetGameStateHasInit)
            {
                //進入新狀態所做的初始化
                pCurrentScreenState.BeginInit();
                //載入資源
                pCurrentScreenState.LoadResource();
            }

        }

        public TimeSpan GetTimeSpan {
            get { return this.timer.UpdateInterval; }
        }
        //因為是使用XAML,所以遊戲畫面的寬高會被鎖定在應用程式手機的直式拿法
        public int GetDeviceScreenWidth() {
            return SharedGraphicsDeviceManager.Current.PreferredBackBufferHeight;
        }
        public int GetDeviceScreenHeight()
        {
            return SharedGraphicsDeviceManager.Current.PreferredBackBufferWidth;
        }
        
        public ContentManager GetContentManager{
            get { return this.contentManager; }
        }
        /// <summary>
        /// 取得遊戲音樂管理器
        /// </summary>
        /// <returns></returns>
        public Song GetGameSongManagerByKey(GameSongKeyEnum key) {
            return this.songManager.GetGameBackgrounSongFromKey(key);
        }
        /// <summary>
        /// 取得遊戲音效管理器
        /// </summary>
        public SoundEffect GetSoundEffectManagerByKey(SoundEffectKeyEnum key) {
            return this.soundManager.GetSoundEffectFromKey(key);
        }

        public async void LoginFacebook()
        {
            if (!App.isAuthenticated)
            {
                App.isAuthenticated = true;
                await Authenticate();
            }
        }
        
        private async Task Authenticate()
        {
            string message = String.Empty;
            try
            {
                session = await App.FacebookSessionClient.LoginAsync("user_about_me,read_stream");
                App.AccessToken = session.AccessToken;
                App.FacebookId = session.FacebookId;

                //Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative)));
            }
            catch (InvalidOperationException e)
            {
                pCurrentScreenState = gameStateTable[GameStateEnum.STATE_MENU];
                //message = "Login failed! Exception details: " + e.Message;
                //MessageBox.Show(message);
                //Authenticate();
                //Dispatcher.BeginInvoke(() => NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative)));
            }
        }

        /// <summary>
        /// 取得三軸加速器的資料
        /// </summary>
        /// <returns></returns>
        public Vector3 GetAccVector() {
            return this.accVector;
        }
    }
}