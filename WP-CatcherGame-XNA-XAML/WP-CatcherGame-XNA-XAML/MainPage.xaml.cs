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
        }

        // 簡單按鈕，按一下事件處理常式，把我們帶到第二頁
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Uri("/GamePage.xaml", UriKind.Relative));
        }
    }
}