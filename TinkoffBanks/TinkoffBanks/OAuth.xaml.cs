using System;
using System.Linq;
using System.Windows;
using System.Windows.Navigation;
using LinqToTwitter;
using Microsoft.Phone.Controls;
using TinkoffBanks.ViewModel;

namespace TinkoffBanks
{
    public partial class OAuth: PhoneApplicationPage
    {
        PinAuthorizer pinAuth;

        public OAuth()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(Page_Loaded);
            OAuthWebBrowser.LoadCompleted += new System.Windows.Navigation.LoadCompletedEventHandler(OAuthWebBrowser_LoadCompleted);
        }

        void OAuthWebBrowser_LoadCompleted(object sender, NavigationEventArgs e)
        {
            EnterPinTextBlock.Visibility = Visibility.Visible;
            PinTextBox.IsEnabled = true;
            AuthenticateButton.IsEnabled = true;

            ViewModelLocator.MainStatic.News.LoadNews();
        }

        void Page_Loaded(object sender, RoutedEventArgs e)
        {
            this.pinAuth = new PinAuthorizer
            {
                Credentials = new InMemoryCredentials
                {
                    ConsumerKey = "T3aK3GddllYD1FiAfKaeQ",
                    ConsumerSecret = "MkTvlQ0liOKMUOEKo20rrU2Xyg5YrGrqR4eLKzQlA"
                },
                UseCompression = true,
                GoToTwitterAuthorization = pageLink =>
                    Dispatcher.BeginInvoke(() => OAuthWebBrowser.Navigate(new Uri(pageLink, UriKind.Absolute)))
            };

            this.pinAuth.BeginAuthorize(resp =>
                Dispatcher.BeginInvoke(() =>
                {
                    switch (resp.Status)
                    {
                        case TwitterErrorStatus.Success:
                            break;
                        case TwitterErrorStatus.TwitterApiError:
                        case TwitterErrorStatus.RequestProcessingException:
                            MessageBox.Show(
                                resp.Exception.ToString(),
                                resp.Message,
                                MessageBoxButton.OK);
                            break;
                    }
                }));
        }

        private void AuthenticateButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    ViewModelLocator.MainStatic.Loading = true;
                }
                catch { };

                pinAuth.CompleteAuthorize(
                    PinTextBox.Text,
                    completeResp => Dispatcher.BeginInvoke(() =>
                    {
                        switch (completeResp.Status)
                        {
                            case TwitterErrorStatus.Success:
                                SharedState.Authorizer = pinAuth;
                                try
                                {
                                    ViewModelLocator.MainStatic.Loading = false;
                                }
                                catch { };
                                NavigationService.Navigate(new Uri("/MainPage.xaml", UriKind.Relative));
                                break;
                            case TwitterErrorStatus.TwitterApiError:
                            case TwitterErrorStatus.RequestProcessingException:
                                MessageBox.Show(
                                    completeResp.Exception.ToString(),
                                    completeResp.Message,
                                    MessageBoxButton.OK);
                                break;
                        }
                    }));
            }
            catch { };
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
        }
    }
}