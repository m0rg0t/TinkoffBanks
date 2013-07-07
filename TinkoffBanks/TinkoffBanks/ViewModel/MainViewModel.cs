using GalaSoft.MvvmLight;
using LinqToTwitter;
using System;
using System.Windows.Navigation;
using System.Linq;
using System.Collections.ObjectModel;
using System.Windows;
using System.Collections.Generic;
using System.Device.Location;
using TinkoffBanks.Libs;

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
            if (IsInDesignMode)
            {
                // Code runs in Blend --> create design time data.
            }
            else
            {
                try
                {
                    bool geo = true;
                    if (AppSettings.TryGetSetting<bool>("GeolocationStatus", out geo))
                    {
                        GeolocationStatus = geo;
                    };
                }
                catch { };
                // Code runs "for real"
            }

        }

        private bool _geolocationStatus = true;
        public bool GeolocationStatus
        {
            get
            {
                return _geolocationStatus;
            }
            set
            {
                if (_geolocationStatus != value)
                {
                    _geolocationStatus = value;
                    AppSettings.StoreSetting<bool>("GeolocationStatus", value);
                    RaisePropertyChanged("GeolocationStatus");
                };
            }
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


        private TwitItem _currentReview = null;
        public TwitItem CurrentReview
        {
            get
            {
                return _currentReview;
            }
            set
            {
                _currentReview = value;
                RaisePropertyChanged("CurrentReview");
            }
        }


        private double _latitued = 55;
        public double Latitued
        {
            get
            {
                return _latitued;
            }
            set
            {
                _latitued = value;
                RaisePropertyChanged("Latitued");
            }
        }

        private double _longitude = 33;
        public double Longitude
        {
            get
            {
                return _longitude;
            }
            set
            {
                _longitude = value;
                RaisePropertyChanged("Longitude");
            }
        }

        public void UpdateCoordinatesWatcher()
        {
            try
            {
                if (ViewModelLocator.MainStatic.GeolocationStatus)
                {
                    myCoordinateWatcher.Stop();
                    myCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
                    myCoordinateWatcher.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(myCoordinateWatcher_PositionChanged);
                    myCoordinateWatcher.Start();
                };
            }
            catch { };
        }
        public GeoCoordinateWatcher myCoordinateWatcher = new GeoCoordinateWatcher(GeoPositionAccuracy.Default);
        private bool _getCoordinates = false;

        void myCoordinateWatcher_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            if (ViewModelLocator.MainStatic.GeolocationStatus)
            {
                //if (ViewModelLocator.MainStatic.Settings.Location == true)
                //{
                if (((!e.Position.Location.IsUnknown) && (_getCoordinates == false)))
                {
                    Latitued = e.Position.Location.Latitude;
                    Longitude = e.Position.Location.Longitude;

                    _getCoordinates = true;
                };
                /*}
                else
                {
                    Latitued = 55.45;
                    Longitude = 37.36;
                };*/
            };
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
                RaisePropertyChanged("Distance_items");
            }
        }

        private List<TwitItem> _distance_items = new List<TwitItem>();
        public List<TwitItem> Distance_items
        {
            private set
            {
            }
            get
            {
                return (from item in Items
                        where ((item.Lat!=0.0) && (item.Lon!=0.0))
                        orderby item.Distance ascending
                        select item).Take(15).ToList(); 
            }
        }
       

        public void LoadRecords()
        {
            Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        this.Loading = true;
                    });
            UpdateCoordinatesWatcher();
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
                           search.Count == 90
                     select search).MaterializedAsyncCallback(resp =>
            {
                ObservableCollection<TwitItem> nowitems = new ObservableCollection<TwitItem>();
                Search srch = resp.State.First();
                Console.WriteLine("\nQuery: {0}\n", srch.SearchMetaData.Query);
                srch.Statuses.ForEach(entry =>
                {
                    Deployment.Current.Dispatcher.BeginInvoke(() =>
                    {
                        try
                        {
                            TwitItem twit = new TwitItem();
                            twit.RawContent = entry.Text;
                            twit.CreatedAt = entry.CreatedAt;
                            try
                            {
                                //twit.Coordinates = entry.Coordinates;
                                twit.Lat = entry.Coordinates.Latitude;
                                twit.Lon = entry.Coordinates.Longitude;
                                twit.GeoLocation = new GeoCoordinate(twit.Lat, twit.Lon);
                            }
                            catch { };

                            try
                            {
                                twit.UserName = entry.User.Identifier.ScreenName;
                                twit.ProfileImage = entry.User.ProfileImageUrl;
                            }
                            catch
                            {
                                twit.UserName = "Скрыто";
                            };
                            ViewModelLocator.MainStatic.Items.Add(twit);
                        }
                        catch { };
                            //nowitems.Add(twit);   
                    });
                });
                Deployment.Current.Dispatcher.BeginInvoke(() =>
                {
                    this.Loading = false;                    
                    //ViewModelLocator.MainStatic.Items = nowitems;
                });                
            });                
            };
        }
    }
}