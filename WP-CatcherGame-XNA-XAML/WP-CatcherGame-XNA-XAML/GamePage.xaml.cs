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

using CatcherGame.TextureManager;
using CatcherGame.GameStates;
using CatcherGame.FontManager;
using System.Diagnostics;
using Facebook.Client;
using System.Threading.Tasks;
namespace WP_CatcherGame_XNA_XAML
{
    public partial class GamePage : PhoneApplicationPage
    {
        private FacebookSession session;

        ContentManager contentManager;
        GameTimer timer;
        SpriteBatch spriteBatch;

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

        public GamePage()
        {
            InitializeComponent();
            this.SupportedOrientations = SupportedPageOrientation.Landscape; //設定方位
            
            // 從應用程式取得內容管理員
            contentManager = (Application.Current as App).Content;

            // 為這個頁面建立計時器
            timer = new GameTimer();
            timer.UpdateInterval = TimeSpan.FromTicks(333333);
            timer.Update += OnUpdate;
            timer.Draw += OnDraw;
          
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

           
        }

        private void Init() {
            fontManager = new SpriteFontManager(this);
            texture2DManager = new Texture2DManager(this);
            pCurrentScreenState = gameStateTable[GameStateEnum.STATE_MENU];
            pCurrentScreenState.BeginInit();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // 設定圖形裝置的共用模式，以開啟 XNA 呈現
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(true);

            // 建立可用來繪製紋理的新 SpriteBatch。
            spriteBatch = new SpriteBatch(SharedGraphicsDeviceManager.Current.GraphicsDevice);

            // TODO: 在此處使用 this.content 載入遊戲內容
            Init();

            pCurrentScreenState.SetSpriteBatch(spriteBatch);
            pCurrentScreenState.LoadResource();
            // 啟動計時器
            timer.Start();

            base.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            // 停止計時器
            timer.Stop();

            // 設定圖形裝置的共用模式，以關閉 XNA 呈現
            SharedGraphicsDeviceManager.Current.GraphicsDevice.SetSharingMode(false);

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
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                
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
            pCurrentScreenState.Draw();
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
        public SpriteFont GetSpriteFontFromKeyByMainGame(SpriteFontKeyEnum key)
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
                pCurrentScreenState.SetSpriteBatch(spriteBatch);
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

    }
}