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
            //處理6.5.1
            if (MediaPlayer.State == MediaState.Playing && !MediaPlayer.GameHasControl)
            {
                string msg = String.Format("Music + Videos Hub is currently playing music. Catcher Game needs to stop music.\nPlease click Ok to continue or Cancel to close the Catcher Game.");
                MessageBoxResult res = MessageBox.Show(msg, "Music + Videos Hub", MessageBoxButton.OKCancel);
                if (res == MessageBoxResult.Cancel)
                {
                    Exit();
                }
                else
                {
                    logo.Begin();
                }
            }
            else
            {
                logo.Begin();
            }

            
        }

        private void Exit() {

            NavigationService.RemoveBackEntry();
            
        }
        //當動畫播完後會進入此事件，導向遊戲選單
        private void DoubleAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
            Exit();
            //NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
    }
}