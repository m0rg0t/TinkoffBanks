using GalaSoft.MvvmLight;
using LinqToTwitter;
using System;
using System.Windows.Navigation;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;

namespace TinkoffBanks.ViewModel
{
    /// <summary>
    /// This class contains properties that the main View can data bind to.
    /// <para>
    /// Use the <strong>mvvminpc</strong> snippet to add bindable properties to this ViewModel.
    /// </para>
    /// <para>
    /// You can also use Blend to data bind with the tool's support.
    /// </para>
    /// <para>
    /// See http://www.galasoft.ch/mvvm
    /// </para>
    /// </summary>
    public class MainViewModel : ViewModelBase
    {
        /// <summary>
        /// Initializes a new instance of the MainViewModel class.
        /// </summary>
        public MainViewModel()
        {
            ////if (IsInDesignMode)
            ////{
            ////    // Code runs in Blend --> create design time data.
            ////}
            ////else
            ////{
            ////    // Code runs "for real"
            ////}
        }

        private NewsListViewModel _news = new NewsListViewModel();
        public NewsListViewModel News
        {
            get
            {
                return _news;
            }
            set
            {
                _news = value;
                RaisePropertyChanged("News");
            }
        }

        private bool _loading = true;
        public bool Loading
        {
            get
            {
                return _loading;
            }
            set
            {
                _loading = value;
                RaisePropertyChanged("Loading");
            }
        }

        private ObservableCollection<TwitItem> _items = new ObservableCollection<TwitItem>();
        public ObservableCollection<TwitItem> Items {
            get
            {
                return _items;
            }
            set
            {
                _items = value;
                RaisePropertyChanged("Items");
            }
        }

        public void LoadRecords()
        {
            ITwitterAuthorizer auth = SharedState.Authorizer;
            if (auth == null || !auth.IsAuthorized)
            {
                //NavigationService.Navigate(new Uri("/OAuth.xaml", UriKind.Relative));
            }
            else
            {
                var twitterCtx = new TwitterContext(auth);
                    (from search in twitterCtx.Search
                     where search.Type == SearchType.Search && search.Query == "#ihatetowait" &&
                           search.Count == 30
                     select search).MaterializedAsyncCallback(resp =>
            {
                ObservableCollection<TwitItem> nowitems = new ObservableCollection<TwitItem>();
                Search srch = resp.State.First();
                Console.WriteLine("\nQuery: {0}\n", srch.SearchMetaData.Query);
                srch.Statuses.ForEach(entry =>
                {
                    Console.WriteLine(
                        "ID: {0, -15}, Source: {1}\nContent: {2}\n",
                        entry.StatusID, entry.Source, entry.Text);
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                            TwitItem twit = new TwitItem();
                            twit.Content = entry.Text;
                            twit.CreatedAt = entry.CreatedAt;
                            twit.Coordinates = entry.Coordinates;
                            try
                            {
                                twit.Contributor = entry.Contributors.First().ScreenName;
                            }
                            catch {
                                twit.Contributor = "Скрыто";
                            };
                            nowitems.Add(twit);   
                    });
                });
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    ViewModelLocator.MainStatic.Items = nowitems;
                });                
            });


                
            };
        }
    }
}