using System;

namespace Astronomie
{
    // see https://dotnet-snippets.de/snippet/astronomie-bibliothek-sonnenaufgang-mondphasen/1223
    public class Sun
    {
        #region ctor

        /// <summary> Initializes a new instance of the <see cref="Sun"/> class. </summary>
        /// <param name="latitude">The latitude.</param>
        /// <param name="longtitude">The longtitude.</param>
        /// <param name="year">The year.</param>
        /// <param name="month">The month.</param>
        /// <param name="day">The day.</param>
        public Sun(double latitude, double longtitude, int year, int month, int day)
        {
            const double PI = Math.PI;
            const double DR = PI / 180;
            const double RD = 1 / DR;
            var B5 = latitude;
            var L5 = longtitude;
            var H = 0;    // timezone UTC
            var Now = DateTime.Now;
            var M = month;
            var D = day;
            B5 = DR * B5;
            var N = (Int32)(275 * M / 9) - 2 * (Int32)((M + 9) / 12) + D - 30;
            var L0 = 4.8771 + .0172 * (N + .5 - L5 / 360);
            var C = .03342 * Math.Sin(L0 + 1.345);
            var C2 = RD * (Math.Atan(Math.Tan(L0 + C)) - Math.Atan(.9175 * Math.Tan(L0 + C)) - C);
            var SD = .3978 * Math.Sin(L0 + C);
            var CD = Math.Sqrt(1 - SD * SD);
            var SC = (SD * Math.Sin(B5) + .0145) / (Math.Cos(B5) * CD);

            if (Math.Abs(SC) <= 1)
            {
                // calculate sunrise 
                var C3 = RD * Math.Atan(SC / Math.Sqrt(1 - SC * SC));
                var R1 = 6 - H - (L5 + C2 + C3) / 15;
                var HR = (Int32)(R1);
                var MR = (Int32)((R1 - HR) * 60);
                Sunrise = new DateTime(year, month, day, HR, MR, 0);
                // calculate sunset
                var S1 = 18 - H - (L5 + C2 - C3) / 15;
                var HS = (Int32)(S1);
                var MS = (Int32)((S1 - HS) * 60);
                Sunset = new DateTime(year, month, day, HS, MS, 0);
            }
            else
            {
                if (SC > 1)
                {
                    // sun is up all day ...
                    // Set Sunset to be in the future ...
                    Sunset = new DateTime(Now.Year + 1, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second);
                    // Set Sunrise to be in the past ...
                    Sunrise = new DateTime(Now.Year - 1, Now.Month, Now.Day, Now.Hour, Now.Minute - 1, Now.Second);
                }
                if (SC < -1)
                {
                    // sun is down all day ...
                    // Set Sunrise and Sunset to be in the future ...
                    Sunrise = new DateTime(Now.Year + 1, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second);
                    Sunset = new DateTime(Now.Year + 1, Now.Month, Now.Day, Now.Hour, Now.Minute, Now.Second);
                }
            }
        }

        #endregion

        #region fields

        /// <summary>
        /// DateTime representation of the sunrise-timestamp of a given day on a given location.
        /// </summary>
        public DateTime Sunrise;

        /// <summary>
        /// DateTime representation of the sunset-timestamp of a given day on a given location.
        /// </summary>
        public DateTime Sunset;


        #endregion
    }
}
