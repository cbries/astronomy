using System;

namespace Astronomie
{
    // see https://dotnet-snippets.de/snippet/astronomie-bibliothek-sonnenaufgang-mondphasen/1223
    public class Moon
    {
        /// <summary>
        /// The moonphase of the given date.
        /// </summary>
        public MoonPhases Phase;

        /// <summary>
        /// Initializes a new instance of the <see cref="Moon"/> class.
        /// </summary>
        /// <param name="year">The year.</param>
        /// <param name="day">The day.</param>
        /// <param name="hour">The hour.</param>
        public Moon(int year, int month, int day)
        {
            var P2 = 3.14159 * 2;
            var YY = year - (Int32)((12 - month) / 10);
            var MM = month + 9;
            if (MM >= 12) { MM = MM - 12; }
            var K1 = (Int32)(365.25 * (YY + 4712));
            var K2 = (Int32)(30.6 * MM + .5);
            var K3 = (Int32)((Int32)((YY / 100) + 49) * .75) - 38;
            var J = K1 + K2 + day + 59;
            if (J > 2299160) { J = J - K3; }
            var V = (J - 2451550.1) / 29.530588853;
            V = V - (Int32)(V);
            if (V < 0) { V = V + 1; }
            var AG = V * 29.53;
            if ((AG > 27.6849270496875) || (AG <= 1.8456618033125))
            {
                Phase = MoonPhases.newmoon;
            }
            else if ((AG > 1.8456618033125) && (AG <= 5.5369854099375))
            {
                Phase = MoonPhases.waxingcrescent;
            }
            else if ((AG > 5.5369854099375) && (AG <= 9.2283090165625))
            {
                Phase = MoonPhases.firstquarter;
            }
            else if ((AG > 9.2283090165625) && (AG <= 12.9196326231875))
            {
                Phase = MoonPhases.waxinggibbous;
            }
            else if ((AG > 12.9196326231875) && (AG <= 16.6109562298125))
            {
                Phase = MoonPhases.fullmoon;
            }
            else if ((AG > 16.6109562298125) && (AG <= 20.3022798364375))
            {
                Phase = MoonPhases.waninggibbous;
            }
            else if ((AG > 20.3022798364375) && (AG <= 23.9936034430625))
            {
                Phase = MoonPhases.lastquarter;
            }
            else if ((AG > 23.9936034430625) && (AG <= 27.6849270496875))
            {
                Phase = MoonPhases.waningcrescent;
            }
            else
            {
                Phase = MoonPhases.fullmoon;
            }
        }
    }
}
