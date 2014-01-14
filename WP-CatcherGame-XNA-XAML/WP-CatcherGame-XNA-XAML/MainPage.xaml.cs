using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.ComponentModel;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
//處理6.5.1
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Media;

namespace WP_CatcherGame_XNA_XAML
{
    public partial class MainPage : PhoneApplicationPage
    {
        // 建構函式
        public MainPage()
        {
            InitializeComponent();
           
            // Can only access the NavigationService when the page has been loaded 
            this.Loaded += new RoutedEventHandler(NavigationPage_Loaded);
            
        }
        void NavigationPage_Loaded(object sender, RoutedEventArgs e)
        {
            FrameworkDispatcher.Update();
            //處理6.5.1
            //使用者有在放音樂 GameHasControl = false,沒有放音樂是true
            if (!MediaPlayer.GameHasControl)
            {

                string msg = String.Format("Music + Videos Hub is currently playing music. Catcher Game needs to stop music.\nPlease click Ok to continue or Cancel to close the Catcher Game.");
                MessageBoxResult res = MessageBox.Show(msg, "Music + Videos Hub", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.Cancel)
                {
                    Exit();
                }
                else
                {
                    //有在撥放音樂,Stop (PodCast播客不包含,Podcast是走AudioPlayer,不過進入遊戲後Mediaplayer優先權高,所以會關掉Podcast)
                    if (MediaPlayer.State == MediaState.Playing) {
                        MediaPlayer.Stop();
                    }
                    logo.Begin();
                }
            }
            else
            {
                logo.Begin();          
            }

        }

        private void Exit() {
            //在WP7中沒有 像WP8有Application.Current.Termiated()這樣的函式
            //所以唯一非正常方法是透過XNA的Game.Exit()離開
            new Microsoft.Xna.Framework.Game().Exit();
            
        }
        //當動畫播完後會進入此事件，導向遊戲選單
        private void DoubleAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
    }
}