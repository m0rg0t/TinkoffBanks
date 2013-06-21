using LinqToTwitter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TinkoffBanks.ViewModel
{
    public static class SharedState
    {
        public static ITwitterAuthorizer Authorizer { get; set; }
    }
}
