using GalaSoft.MvvmLight;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinkoffBanks.ViewModel
{
    public class TwitItem:ViewModelBase
    {
        public TwitItem()
        {
        }

        private string _content = "";
        public string Content
        {
            get
            {
                return _content;
            }
            set
            {
                _content = value;
                RaisePropertyChanged("Content");
            }
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
                RaisePropertyChanged("CreatedAt");
            }
        }

        public object Author { get; set; }

        public LinqToTwitter.Coordinate Coordinates { get; set; }

        public string Contributor { get; set; }
    }
}
