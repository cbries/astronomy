using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Astronomie
{
    class Program
    {
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
            
            DateOfAYear data = new DateOfAYear(2016, latitude, longitude);
            data.Generate();
            var jsonArray = data.ToJson();
            try
            {
                File.WriteAllText("DateOfAYear.json", jsonArray.ToString(Formatting.Indented));
            }
            catch
            {
                // ignore
            }
            Console.WriteLine("Press any key...");
            Console.ReadLine();
        }
    }
}
