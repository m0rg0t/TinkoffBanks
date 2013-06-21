using System;
using LinqToTwitter;

namespace WP7TweetWithMediaDemo
{
    public static class SharedState
    {
        public static ITwitterAuthorizer Authorizer { get; set; }
    }
}
