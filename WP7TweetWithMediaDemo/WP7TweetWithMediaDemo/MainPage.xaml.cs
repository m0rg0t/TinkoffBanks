using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Resources;
using System.Windows.Shapes;
using LinqToTwitter;
using Microsoft.Phone.Controls;

namespace WP7TweetWithMediaDemo
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
            TweetTextBox.Text = "Testing #linqtotwitter Tweet with Media - " + DateTime.Now.ToString();
            ImagePathTextBox.Text = "/WP7TweetWithMediaDemo;component/200xColor_2.png";
        }

        private void PostButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(TweetTextBox.Text))
                MessageBox.Show("Please enter text to tweet.");

            ITwitterAuthorizer auth = SharedState.Authorizer;
            if (auth == null || !auth.IsAuthorized)
            {
                NavigationService.Navigate(new Uri("/OAuth.xaml", UriKind.Relative));
            }
            else
            {
                var twitterCtx = new TwitterContext(auth);

                var media = GetMedia();

                twitterCtx.TweetWithMedia(
                    TweetTextBox.Text, false, StatusExtensions.NoCoordinate, StatusExtensions.NoCoordinate, null, false,
                    media,
                    updateResp => Dispatcher.BeginInvoke(() =>
                    {
                        HandleResponse(updateResp);
                    }));
            }
        }

        List<Media> GetMedia()
        {
            Uri uri = new Uri(ImagePathTextBox.Text, UriKind.Relative);

            StreamResourceInfo sri = App.GetResourceStream(uri);

            byte[] byteArr = null;
            using (var binRdr = new BinaryReader(sri.Stream))
            using (var memStr = new MemoryStream())
            {
                const int ReadSize = 8196;
                int chunkSize = 0;
                do
                {
                    byte[] buf = new byte[ReadSize];
                    chunkSize = binRdr.Read(buf, 0, ReadSize);
                    memStr.Write(buf, 0, ReadSize);
                } while (chunkSize > 0);

                byteArr = new byte[memStr.Length];
                memStr.Position = 0;
                memStr.Read(byteArr, 0, (int)memStr.Length);
            }

            var media =
                new List<Media>
                {
                    new Media
                    {
                        ContentType = MediaContentType.Png,
                        Data = byteArr,
                        FileName = System.IO.Path.GetFileName(ImagePathTextBox.Text)
                    }
                };

            return media;
        }
  
        void HandleResponse(TwitterAsyncResponse<Status> updateResp)
        {
            switch (updateResp.Status)
            {
                case TwitterErrorStatus.Success:
                    HandleSuccess(updateResp);
                    break;
                case TwitterErrorStatus.TwitterApiError:
                case TwitterErrorStatus.RequestProcessingException:
                    HandleFailure(updateResp);
                    break;
            }
        }
  
        void HandleSuccess(TwitterAsyncResponse<Status> updateResp)
        {
            Status tweet = updateResp.State;
            User user = tweet.User;
            UserIdentifier id = user.Identifier;

            MessageBox.Show(
                "User: " + id.ScreenName +
                ", Posted Status: " + tweet.Text,
                "Update Successfully Posted.",
                MessageBoxButton.OK);
        }
  
        void HandleFailure(TwitterAsyncResponse<Status> updateResp)
        {
            MessageBox.Show(
                updateResp.Exception.ToString(),
                updateResp.Message,
                MessageBoxButton.OK);
        }
    }
}