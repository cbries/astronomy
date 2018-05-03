using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Astronomie
{
    class Program
    {
        private static int StartYear = 2018;
        private static int EndYear = 2050;

        internal class AstronomyEntry
        {
            public Sun Sun { get; set; }
            public Moon Moon { get; set; }
        }

        internal class DateOfAYear
        {
            private readonly int _year;
            private readonly double _latitude;
            private readonly double _longitude;

            private readonly Dictionary<DateTime, AstronomyEntry> _entries = new Dictionary<DateTime, AstronomyEntry>();

            public Dictionary<DateTime, AstronomyEntry> Entries { get { return _entries; } }

            public DateOfAYear(int year, double latitude, double longitude)
            {
                _year = year;
                _latitude = latitude;
                _longitude = longitude;
            }

            public void Generate()
            {
                var startDate = new DateTime(_year, 1, 1, 0, 0, 0);
                var endDate = new DateTime(_year, 12, 31, 23, 59, 59);

                for (DateTime date = startDate; date.Date <= endDate.Date; date = date.AddDays(1))
                {
                    var data = new AstronomyEntry
                    {
                        Sun = new Sun(_latitude, _longitude, date.Year, date.Month, date.Day),
                        Moon = new Moon(date.Year, date.Month, date.Day)
                    };
                    if (_entries.ContainsKey(date))
                        _entries[date] = data;
                    else
                        _entries.Add(date, data);
                }
            }

            public JArray ToJson()
            {
                if (_entries.Count <= 0)
                    return null;

                JArray a = new JArray();
                foreach (var e in _entries)
                {
                    JObject o = new JObject();
                    o["sunrise"] = e.Value.Sun.Sunrise.ToLocalTime().ToString("T");
                    o["sunset"] = e.Value.Sun.Sunset.ToLocalTime().ToString("T");
                    o["moonphase"] = e.Value.Moon.Phase.ToString();

                    JObject oo = new JObject();
                    oo["date"] = e.Key.ToLocalTime().ToString("yy-MM-dd");
                    oo["data"] = o;

                    a.Add(oo);
                }

                return a;
            }
        }

        static void Main(string[] args)
        {
            // Bielefeld
            const double latitude = 52.0121;
            const double longitude = 8.3158;

            for (int year = StartYear; year <= EndYear; ++year)
            {
                DateOfAYear data = new DateOfAYear(year, latitude, longitude);
                data.Generate();
                var jsonArray = data.ToJson();
                try
                {
                    string targetName = string.Format("..\\..\\Data\\DateOfAYear-{0}.json", year);
                    File.WriteAllText(targetName, jsonArray.ToString(Formatting.Indented), Encoding.UTF8);
                }
                catch
                {
                    // ignore
                }
            }

            
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }
    }
}
