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
using System.Windows.Shapes;
using Microsoft.Phone.Controls;

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
            logo.Begin();
        }
      
        //當動畫播完後會進入此事件，導向遊戲選單
        private void DoubleAnimationUsingKeyFrames_Completed(object sender, EventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
    }
}