using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using TinkoffBanks.ViewModel;

namespace TinkoffBanks
{
    public partial class TwitListPage : PhoneApplicationPage
    {
        // Constructor
        public TwitListPage()
        {
            InitializeComponent();

            // Sample code to localize the ApplicationBar
            //BuildLocalizedApplicationBar();
        }

        private void TwitList_ItemTap(object sender, Telerik.Windows.Controls.ListBoxItemTapEventArgs e)
        {
            try
            {
                ViewModelLocator.MainStatic.CurrentReview = (TwitItem)TwitList.SelectedItem;
                NavigationService.Navigate(new Uri("/Pages/Review/ViewReview.xaml", UriKind.Relative));
            }
            catch { };
        }
    }
}