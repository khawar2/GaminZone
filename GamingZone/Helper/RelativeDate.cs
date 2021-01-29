using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;

namespace GamingZone.Helper
{
    public static class RelativeDate
    {
        public static string ToRelativeDate(this DateTime? dateTime)
        {
            var timeSpan = DateTime.Now - dateTime;

            if (timeSpan <= TimeSpan.FromSeconds(60))
                return string.Format("{0} seconds ago", timeSpan.Value.Seconds);

            if (timeSpan <= TimeSpan.FromMinutes(60))
                return timeSpan.Value.Minutes > 1 ? String.Format("about {0} minutes ago", timeSpan.Value.Minutes) : "about a minute ago";

            if (timeSpan <= TimeSpan.FromHours(24))
                return timeSpan.Value.Hours > 1 ? String.Format("about {0} hours ago", timeSpan.Value.Hours) : "about an hour ago";

            if (timeSpan <= TimeSpan.FromDays(30))
                return timeSpan.Value.Days > 1 ? String.Format("about {0} days ago", timeSpan.Value.Days) : "yesterday";

            if (timeSpan <= TimeSpan.FromDays(365))
                return timeSpan.Value.Days > 30 ? String.Format("about {0} months ago", timeSpan.Value.Days / 30) : "about a month ago";

            return timeSpan.Value.Days > 365 ? String.Format("about {0} years ago", timeSpan.Value.Days / 365) : "about a year ago";
        }

        public static string ToDate(this DateTime? dateTime)
        {
            if (dateTime != null)
            {

                string ss = dateTime.ToString();
                DateTime dt = DateTime.ParseExact(ss, "M/d/yyyy h:mm:ss tt", CultureInfo.InvariantCulture);
                string s = dt.ToString("dd/M/yyyy", CultureInfo.InvariantCulture);

                return s;
            }
            return null;
        }
        
    }
}