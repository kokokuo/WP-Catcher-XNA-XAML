using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Facebook.Client;

namespace WP_CatcherGame_XNA_XAML
{
    public partial class App : Application
    {
        /// <summary>
        /// 提供簡單的方法，來存取電話應用程式的根畫面。
        /// </summary>
        /// <returns>電話應用程式的根畫面。</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// 提供存取應用程式的 ContentManager。
        /// </summary>
        public ContentManager Content { get; private set; }

        /// <summary>
        /// 提供存取為提取 FrameworkDispatcher 而設定的 GameTimer。
        /// </summary>
        public GameTimer FrameworkDispatcherTimer { get; private set; }

        /// <summary>
        /// 提供存取應用程式的 AppServiceProvider。
        /// </summary>
        public AppServiceProvider Services { get; private set; }

        /// <summary>
        /// Application 物件的建構函式。
        /// </summary>
        public App()
        {
            // 無法攔截之例外狀況的全域處理常式。 
            UnhandledException += Application_UnhandledException;

            // 標準 Silverlight 初始化
            InitializeComponent();

            // 電話特有初始化
            InitializePhoneApplication();

            // XNA 初始化
            InitializeXnaApplication();

            // 偵錯時顯示圖形分析資訊。
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 顯示目前的畫面播放速率計數器。
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // 顯示每個畫面中重新繪製的應用程式區域。
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // 啟用非實際執行分析視覺化模式， 
                // 用彩色重疊在頁面上顯示交給 GPU 的區域。
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // 停用應用程式閒置偵測: 將應用程式的 PhoneApplicationService 物件的
                // UserIdleDetectionMode 屬性設為 Disabled。
                // 注意: 這項功能僅限偵錯模式使用。當使用者不使用電話時，停用使用者閒置偵測
                // 的應用程式仍會繼續執行，並消耗電池的電力。
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }
        }

        // 啟動應用程式 (例如，從 [開始]) 時要執行的程式碼
        // 重新啟動應用程式時不會執行這段程式碼
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
        }

        // 啟動應用程式 (帶到前景) 時要執行的程式碼
        // 第一次啟動應用程式時不會執行這段程式碼
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
        }

        // 停用應用程式 (移到背景) 時要執行的程式碼
        // 關閉應用程式時不會執行這段程式碼
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
        }

        // 關閉應用程式 (例如，使用者按 [上一頁]) 時要執行的程式碼
        // 停用應用程式時不會執行這段程式碼
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
        }

        // 巡覽失敗時要執行的程式碼
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 巡覽失敗; 切換到偵錯工具
                System.Diagnostics.Debugger.Break();
            }
        }

        // 發生未處理的例外狀況時要執行的程式碼
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // 發生未處理的例外狀況; 切換到偵錯工具
                System.Diagnostics.Debugger.Break();
            }
        }

        #region 電話應用程式初始化

        // 避免重複初始化
        private bool phoneApplicationInitialized = false;

        // 請勿在這個方法中加入任何其他程式碼
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // 建立畫面，但還不將它設為 RootVisual; 這樣可以讓啟動顯示
            // 畫面保持作用中狀態，直到應用程式準備好呈現為止。
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // 處理巡覽失敗
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // 確定不會重新初始化
            phoneApplicationInitialized = true;
        }

        // 請勿在這個方法中加入任何其他程式碼
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // 設定根 Visual，使應用程式能夠呈現
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // 移除這個處理常式，因為已不再需要
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion

        #region XNA 應用程式初始化

        // 執行應用程式所需 XNA 型別的初始化。
        private void InitializeXnaApplication()
        {
            // 建立服務提供者
            Services = new AppServiceProvider();

            // 將 SharedGraphicsDeviceManager 加入服務，做為應用程式的 IGraphicsDeviceService
            foreach (object obj in ApplicationLifetimeObjects)
            {
                if (obj is IGraphicsDeviceService)
                    Services.AddService(typeof(IGraphicsDeviceService), obj);
            }

            // 建立 ContentManager，使應用程式能夠載入先行編譯的資產
            Content = new ContentManager(Services, "Content");

            // 建立 GameTimer 以提取 XNA FrameworkDispatcher
            FrameworkDispatcherTimer = new GameTimer();
            FrameworkDispatcherTimer.FrameAction += FrameworkDispatcherFrameAction;
            FrameworkDispatcherTimer.Start();
        }

        // 為 FrameworkDispatcher 提取每個畫面的事件處理常式。
        // FrameworkDispatcher 是許多 XNA 事件和
        // 某些功能 (例如 SoundEffect 播放) 的必要項目。
        private void FrameworkDispatcherFrameAction(object sender, EventArgs e)
        {
            FrameworkDispatcher.Update();
        }

        #endregion


        /// <summary>
        /// Facebook參數宣告
        /// </summary>
        internal static string AccessToken = String.Empty;
        internal static string FacebookId = String.Empty;
        public static bool isAuthenticated = false;
        public static FacebookSessionClient FacebookSessionClient = new FacebookSessionClient(Constants.Constants.FacebookAppId);
    }
}