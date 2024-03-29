﻿using GalaSoft.MvvmLight;
using GART.Data;
using System;
using System.Collections.Generic;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace TinkoffBanks.ViewModel
{
    public class TwitItem : ARItem //ViewModelBase
    {
        public TwitItem()
        {
        }

        private DateTime _createdAt = DateTime.Now;
        public DateTime CreatedAt
        {
            get
            {
                return _createdAt;
            }
            set
            {
                _createdAt = value;
                //RaisePropertyChanged("CreatedAt");
            }
        }

        public string CreatedAtShortDate
        {
            get
            {
                return CreatedAt.ToShortDateString();
            }
            private set
            {
            }
        }

        private string _rawContent = "";
        public string RawContent
        {
            get
            {
                return _rawContent;
            }
            set
            {
                _rawContent = value;
                ParseRawContent();
            }
        }

        private string _profileImage = "";
        public string ProfileImage
        {
            get
            {
                return _profileImage;
            }
            set
            {
                _profileImage = value;                
            }
        }

        private bool _angryOldLady = false;
        public bool AngryOldLady
        {
            get
            {
                return _angryOldLady;
            }
            set
            {
                _angryOldLady = value;
            }
        }

        public string AngryOldLadyStr
        {
            private set
            {
            }
            get
            {
                if (AngryOldLady)
                {
                    return "Злые бабушки.";
                }
                else
                {
                    return "";
                };

            }
        }

        private bool _noChairs = false;
        public bool NoChairs
        {
            get
            {
                return _noChairs;
            }
            set
            {
                _noChairs = value;
            }
        }

        public string NoChairsStr
        {
            private set
            {
            }
            get
            {
                if (NoChairs)
                {
                    return "Нет стульев.";
                }
                else
                {
                    return "";
                };

            }
        }
        
        private bool _retardedTeller = false;
        public bool RetardedTeller
        {
            get
            {
                return _retardedTeller;
            }
            set
            {
                _retardedTeller = value;
            }
        }

        public string RetardedTellerStr
        {
            private set
            {
            }
            get
            {
                if (RetardedTeller)
                {
                    return "Упоротый персонал.";
                }
                else
                {
                    return "";
                };

            }
        }

        private bool _ussrStyle = false;
        public bool UssrStyle
        {
            get
            {
                return _ussrStyle;
            }
            set
            {
                _ussrStyle = value;
            }
        }

        public string UssrStyleStr
        {
            private set
            {
            }
            get
            {
                if (UssrStyle)
                {
                    return "В стиле СССР.";
                }
                else
                {
                    return "";
                };

            }
        }

        private bool _robberyInProgress = false;
        public bool RobberyInProgress
        {
            get
            {
                return _robberyInProgress;
            }
            set
            {
                _robberyInProgress = value;
            }
        }

        public string RobberyInProgressStr
        {
            private set
            {
            }
            get
            {
                if (RobberyInProgress)
                {
                    return "Грабеж в процессе.";
                }
                else
                {
                    return "";
                };
                
            }
        }

        //кол-во людей в очереди
        private int _inline = 0;
        public int Inline
        {
            get
            {
                return _inline;
            }
            set
            {
                _inline = value;
            }
        }

        public string InlineStr
        {
            private set
            {
            }
            get
            {
                return "В очереди: "+Inline.ToString();
            }
        }

        private int _avail = 0;
        public int Avail
        {
            get
            {
                return _avail;
            }
            set
            {
                _avail = value;
            }
        }

        public string AvailStr
        {
            private set
            {
            }
            get
            {
                return "Открыто касс: " + Avail.ToString();
            }
        }

        private int _closed = 0;
        public int Closed
        {
            get
            {
                return _closed;
            }
            set
            {
                _closed = value;
            }
        }

        public string ClosedStr
        {
            private set
            {
            }
            get
            {
                return "Закрыто касс: " + Avail.ToString();
            }
        }

        private int _mtowait = 0;
        public int MToWait
        {
            get
            {
                return _mtowait;
            }
            set
            {
                _mtowait = value;
            }
        }

        public string MToWaitStr
        {
            private set
            {
            }
            get
            {
                return "Ждать в очреди: " + MToWait.ToString() + " мин.";
            }
        }

        public void ParseRawContent() {
            string data = RawContent;
            //data = "#ihatetowait @eurobank #5inline #15mtowait #5avail #robberyinprogress стульчик бы хоть поставили";
            data = data.Replace("#ihatetowait","");

            if (data.IndexOf("#angryoldlady") != -1)
            {
                AngryOldLady = true;
                data = data.Replace(" #angryoldlady", "");
                data = data.Replace("#angryoldlady", "");
            };

            if (data.IndexOf("#nochairs") != -1)
            {
                NoChairs = true;
                data = data.Replace(" #nochairs", "");
                data = data.Replace("#nochairs", "");
            };

            if (data.IndexOf("#retardedteller") != -1)
            {
                RetardedTeller = true;
                data = data.Replace(" #retardedteller", "");
                data = data.Replace("#retardedteller", "");
            };

            if (data.IndexOf("#ussrstyle") != -1)
            {
                UssrStyle = true;
                data = data.Replace(" #ussrstyle", "");
                data = data.Replace("#ussrstyle", "");
            };

            if (data.IndexOf("#robberyinprogress") != -1)
            {
                RobberyInProgress = true;
                data = data.Replace(" #robberyinprogress", "");
                data = data.Replace("#robberyinprogress", "");
            };

            try
            {
                string pattern = @"#(\d+)inline";
                Regex newReg = new Regex(pattern);
                MatchCollection matches = newReg.Matches(data);
                foreach (Match mat in matches)
                {
                    data = data.Replace(" "+mat.Value.ToString(), "");
                    data = data.Replace(mat.Value.ToString(), "");
                    string inline_data = mat.Value.ToString().Replace("inline", "").Replace("#", "");
                    Inline = Int32.Parse(inline_data);
                }
            }
            catch { };

            try
            {
                string pattern = @"#(\d+)mtowait";
                Regex newReg = new Regex(pattern);
                MatchCollection matches = newReg.Matches(data);
                foreach (Match mat in matches)
                {
                    data = data.Replace(" " + mat.Value.ToString(), "");
                    data = data.Replace(mat.Value.ToString(), "");
                    string mtowait_data = mat.Value.ToString().Replace("mtowait", "").Replace("#", "");
                    MToWait = Int32.Parse(mtowait_data);
                }
            }
            catch { };

            try
            {
                string pattern = @"#(\d+)htowait";
                Regex newReg = new Regex(pattern);
                MatchCollection matches = newReg.Matches(data);
                foreach (Match mat in matches)
                {
                    data = data.Replace(" " + mat.Value.ToString(), "");
                    data = data.Replace(mat.Value.ToString(), "");
                    string htowait_data = mat.Value.ToString().Replace("htowait", "").Replace("#", "");
                    MToWait = Int32.Parse(htowait_data)*60;
                }
            }
            catch { };

            try
            {
                string pattern = @"#(\d+)closed";
                Regex newReg = new Regex(pattern);
                MatchCollection matches = newReg.Matches(data);
                foreach (Match mat in matches)
                {
                    data = data.Replace(" " + mat.Value.ToString(), "");
                    data = data.Replace(mat.Value.ToString(), "");
                    string closed_data = mat.Value.ToString().Replace("closed", "").Replace("#", "");
                    Closed = Int32.Parse(closed_data);
                }
            }
            catch { };

            try
            {
                string pattern = @"#(\d+)avail";
                Regex newReg = new Regex(pattern);
                MatchCollection matches = newReg.Matches(data);
                foreach (Match mat in matches)
                {
                    data = data.Replace(" " + mat.Value.ToString(), "");
                    data = data.Replace(mat.Value.ToString(), "");
                    string avail_data = mat.Value.ToString().Replace("avail", "").Replace("#", "");
                    Avail = Int32.Parse(avail_data);
                }
            }
            catch { };

            this.Content = data;
        }


        public string DistanceInKm
        {
            get
            {
                double km = Distance / 1000;
                km = Math.Round(km);
                return km.ToString() + " км";
            }
            private set
            {
            }
        }
        public double Distance
        {
            get
            {
                double distanceInMeter = 0.0;

                double curLat = 0.0;
                double curLon = 0.0;

                try
                {
                    curLat = Convert.ToDouble(ViewModelLocator.MainStatic.Latitued.ToString());
                }
                catch { };
                try
                {
                    curLon = Convert.ToDouble(ViewModelLocator.MainStatic.Longitude.ToString());
                }
                catch { };

                try
                {
                    GeoCoordinate currentLocation = new GeoCoordinate(curLat, curLon);
                    GeoCoordinate clientLocation = new GeoCoordinate(Lat, Lon);
                    distanceInMeter = currentLocation.GetDistanceTo(clientLocation);
                }
                catch { };
                if (distanceInMeter == 0)
                {
                    distanceInMeter = 100500;
                };
                return distanceInMeter;
            }
            private set { }
        }

        //public object Author { get; set; }
        //public LinqToTwitter.Coordinate Coordinates { get; set; }

        public string UserName { get; set; }

        public double Lat { get; set; }

        public double Lon { get; set; }
    }
}
