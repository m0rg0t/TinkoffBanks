using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.Device.Location;
using TinkoffBanks.ViewModel;
using Microsoft.Phone.Controls.Maps;

namespace TinkoffBanks.Pages.Review
{
    public partial class ViewReview : PhoneApplicationPage
    {
        public ViewReview()
        {
            InitializeComponent();
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GeoCoordinate currentLocation = new GeoCoordinate(ViewModelLocator.MainStatic.Latitued, ViewModelLocator.MainStatic.Longitude);
                Map1.Children.Add(new Pushpin() { Location = currentLocation, Content = "Я" });

                Map1.Children.Add(new Pushpin()
                { 
                    Location = ViewModelLocator.MainStatic.CurrentReview.GeoLocation, Content = ViewModelLocator.MainStatic.CurrentReview.Content.ToString() });

                Map1.ZoomLevel = 14;
                try
                {
                    Map1.Center = ViewModelLocator.MainStatic.CurrentReview.GeoLocation;
                }
                catch {
                    Map1.Center = currentLocation;
                };
                
            }
            catch { };
        }
    }
}