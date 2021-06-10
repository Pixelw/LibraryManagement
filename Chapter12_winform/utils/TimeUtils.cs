using System;
using System.Data;

namespace Chapter12_winform.utils {
    public class TimeUtils {
        private static readonly DateTime Jan1st1970 = new DateTime
            (1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        
        public static long CurrentTimeMillis()
        {
            return ToMillis(DateTime.Now);
        }

        public static long ToMillis(DateTime datetime) {
             return (long) (datetime - Jan1st1970).TotalMilliseconds;
        }

        public static DateTime ToDateTime(long unixEpoch) {
            return Jan1st1970.AddMilliseconds(unixEpoch);
        }

        public static string ToDateTimeString(long unixEpoch) {
            return ToDateTime(unixEpoch).ToString();
        }
    }
}