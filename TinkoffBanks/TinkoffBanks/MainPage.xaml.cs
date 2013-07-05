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
using TweetSharp;
using TinkoffBanks.ViewModel;

namespace TinkoffBanks
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();

            // Set the data context of the listbox control to the sample data
            DataContext = App.ViewModel;
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);

            try
            {
                ViewModelLocator.MainStatic.LoadRecords();
            }
            catch { };
        }

        // Load data for the ViewModel Items
        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (NavigationService.CanGoBack)
                {
                    while (NavigationService.RemoveBackEntry() != null)
                    {
                        NavigationService.RemoveBackEntry();
                    };
                };
            }
            catch { };
        }

        private void authButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //NavigationService.Navigate(new Uri("/Pages/TwitterLogin.xaml", UriKind.Relative));
            }
            catch { };
        }


        private void NewsList_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/Pages/News/NewsListPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void NewsListRad_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.News.CurrentNews = (NewsViewModel)NewsListRad.SelectedItem;
                NavigationService.Navigate(new Uri("/Pages/News/NewsPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            ViewModelLocator.MainStatic.News.LoadNews();
        }

        private void AddReview_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/Pages/Review/CreateReview.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void BanksMap_Tap(object sender, GestureEventArgs e)
        {
            try
            {
                NavigationService.Navigate(new Uri("/Pages/Map/MapPage.xaml", UriKind.Relative));
            }
            catch { };
        }

        private void TwitsListRad_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentReview = (TwitItem)TwitsListRad.SelectedItem;
                NavigationService.Navigate(new Uri("/Pages/Review/ViewReview.xaml", UriKind.Relative));
            }
            catch { };
        }

    }

}