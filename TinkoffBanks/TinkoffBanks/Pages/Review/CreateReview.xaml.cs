using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using LinqToTwitter;
using TinkoffBanks.ViewModel;

namespace TinkoffBanks.Pages.Review
{
    public partial class CreateReview : PhoneApplicationPage
    {
        public CreateReview()
        {
            InitializeComponent();
        }

        private void CancelButton_Click(object sender, EventArgs e)
        {
            try
            {
                NavigationService.GoBack();
            }
            catch { };
        }

        private void BankName_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Comment.Focus();
            };
        }

        private void Comment_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Inline.Focus();
            };
        }

        private void Inline_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.ToWait.Focus();
            };
        }

        private void ToWait_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Avail.Focus();
            };
        }

        private void Avail_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                this.Closed.Focus();
            };
        }

        private void CreateButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (CheckDataAndCreateTweet())
                {
                    string outstr = "";
                    //#ihatetowait @eurobank #5inline #15mtowait #5avail #robberyinprogress стульчик бы хоть поставили
                    outstr = "#ihatetowait "+this.BankName.Text+" ";

                    if (this.Inline.Text!="") {
                        outstr += "#" + this.Inline.Text+"inline ";
                    };
                    if (this.ToWait.Text != "")
                    {
                        outstr += "#" + this.ToWait.Text + "mtowait ";
                    };
                    if (this.Avail.Text != "")
                    {
                        outstr += "#" + this.Avail.Text + "avail ";
                    };
                    if (this.Closed.Text != "")
                    {
                        outstr += "#" + this.Closed.Text + "closed ";
                    };

                    if (this.AngryOldLady.IsChecked==true)
                    {
                        outstr += "#angryoldlady ";
                    };
                    if (this.NoChairs.IsChecked == true)
                    {
                        outstr += "#nochairs ";
                    };
                    if (this.RetardedTeller.IsChecked == true)
                    {
                        outstr += "#retardedteller ";
                    };
                    if (this.UssrStyle.IsChecked == true)
                    {
                        outstr += "#ussrstyle ";
                    };
                    if (this.RobberyInProgress.IsChecked == true)
                    {
                        outstr += "#robberyinprogress ";
                    };

                    outstr += this.Comment.Text;

                    ITwitterAuthorizer auth = SharedState.Authorizer;
                    if (auth == null || !auth.IsAuthorized)
                    {
                        //NavigationService.Navigate(new Uri("/OAuth.xaml", UriKind.Relative));
                    }
                    else
                    {
                        
                        var twitterCtx = new TwitterContext(auth);
                        //Action<TwitterAsyncResponse<Status>> callback = new Action<TwitterAsyncResponse<Status>>(var target);
                        twitterCtx.UpdateStatus(outstr, (decimal)ViewModelLocator.MainStatic.Latitued, (decimal)ViewModelLocator.MainStatic.Longitude, true, callback);
                    };

                    NavigationService.GoBack();
                }
                else
                {
                };
            }
            catch { };
        }

        private void callback(TwitterAsyncResponse<Status> obj)
        {
            //throw new NotImplementedException();
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            if (obj.Status == TwitterErrorStatus.Success)
                            {
                                ViewModelLocator.MainStatic.LoadRecords();
                            };
                            //MessageBox.Show(obj.Status.ToString());
                        }
                        catch { };
                    });
        }

        private bool CheckDataAndCreateTweet()
        {
            if (this.BankName.Text == "")
            {
                MessageBox.Show("Пожалуста введите название банка.");
                this.BankName.Focus();
                return false;
            };
            if (this.Comment.Text == "")
            {
                MessageBox.Show("Пожалуста введите комментарий.");
                this.Comment.Focus();
                return false;
            };
            return true;
        }

    }
}