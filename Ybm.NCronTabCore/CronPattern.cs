/*
┌───────────── minute (0 - 59)
│ ┌───────────── hour (0 - 23)
│ │ ┌───────────── day of month (1 - 31)
│ │ │ ┌───────────── month (1 - 12)
│ │ │ │ ┌───────────── day of week (0 - 6) (Sunday to Saturday;
│ │ │ │ │                                       7 is also Sunday)
│ │ │ │ │
│ │ │ │ │
* * * * *  command to execute
* */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Ybm.NCronTabCore
{
    internal class CronPattern
    {


        public string patternSecond { get; set; }
        public string patternMinute { get; set; }
        public string patternHour { get; set; }
        public string patternDayOfMonth { get; set; }
        public string patternMonth { get; set; }
        public string patternDayOfWeek { get; set; }


        public CronPattern()
        {
            Minutes = new HashSet<int>();
            Hours = new HashSet<int>();
            Days = new HashSet<int>();
            Months = new HashSet<int>();
            Years = new HashSet<int>();
            WeekDays = new HashSet<int>();

        }
        public HashSet<int> Minutes { get; set; }
        public HashSet<int> Hours { get; set; }
        public HashSet<int> Days { get; set; }
        public HashSet<int> Months { get; set; }
        public HashSet<int> Years { get; set; }
        public HashSet<int> WeekDays { get; set; }



        public Pattern Parse2(string cron)
        {
            var parts = cron.Split(' ');

            patternSecond = parts[0];
            patternMinute = parts[1];
            patternHour = parts[2];
            patternDayOfMonth = parts[3];
            patternMonth = parts[4];
            patternDayOfWeek = parts[5];

            // secondly
            if (Regex.IsMatch(cron, @"((\*\/\d+|\d+) \* \* \* \* \*)+"))
            {
                var pattern = new Pattern();
                pattern.UnitType = EnumUnitType.Secondly;
                ParsePattern(patternSecond, pattern);

                return pattern;
            }

            // minutely
            if (Regex.IsMatch(cron, @"(\* (\*\/\d+|\d+) \* \* \* \*)+"))
            {
                var pattern = new Pattern();
                pattern.UnitType = EnumUnitType.Minutely;
                ParsePattern(patternMinute, pattern);
                return pattern;
            }

            // hourly
            if (Regex.IsMatch(cron, @"(\* \* (\*\/\d+|\d+) \* \* \*)+"))
            {
                var pattern = new Pattern();
                pattern.UnitType = EnumUnitType.Hourly;
                ParsePattern(patternHour, pattern);
                return pattern;
            }

            // Daily
            if (Regex.IsMatch(cron, @"(\* \d+ \d+ \*/\d+ \* \*)+"))
            {
                var pattern = new Pattern();
                pattern.UnitType = EnumUnitType.Daily;

                var everNDays = patternDayOfMonth.Split('/')[1].Split(',');
                pattern.Days.AddRange(Array.ConvertAll(everNDays, int.Parse));

                pattern.Hour = int.Parse(patternHour);
                pattern.Minute = int.Parse(patternMinute);
                return pattern;
            }



            // Days of months
            if (Regex.IsMatch(cron, @"(^\* \d+ \d+ \d+ \*\/(\d+|\,)+ \*)"))
            {
                var pattern = new Pattern();
                pattern.UnitType = EnumUnitType.Monthly;
                var months = patternMonth.Split('/')[1].Split(',');
                pattern.Months.AddRange(Array.ConvertAll(months, int.Parse));

                var daysOfmonth = patternDayOfMonth.Split(',');
                pattern.Days.AddRange(Array.ConvertAll(daysOfmonth, int.Parse));

                pattern.Hour = int.Parse(patternHour);
                pattern.Minute = int.Parse(patternMinute);

                return pattern;
            }

            // Days of week
            if (Regex.IsMatch(cron, @"(^\* \d+ \d+ \* \* \*\/(\d+|\,)+)"))
            {
                var pattern = new Pattern();
                pattern.UnitType = EnumUnitType.Weekly;
                
                var daysOfWeek= patternDayOfWeek.Split('/')[1].Split(',');
                pattern.Days.AddRange(Array.ConvertAll(daysOfWeek, int.Parse));

                pattern.Hour = int.Parse(patternHour);
                pattern.Minute = int.Parse(patternMinute);

                return pattern;
            }


            //(\* \d \d (\d|L) \* \*)  first or last days of every months
            //* 0 9 1 * *
            //* 0 9 1 * *
            //* 0 9 L * *
            if (Regex.IsMatch(cron,@"(\* \d+ \d+ (\d+|L) \* \*)"))
            {
                var pattern = new Pattern();
                pattern.UnitType = EnumUnitType.Monthly;

                int day = 0;
                if (int.TryParse(patternDayOfMonth, out day))
                {
                    pattern.Days.Add(day);
                }
                else
                {
                    if (patternDayOfMonth.ToLower() == "l")
                    {
                        pattern.Days.Add(-1);
                    }
                }
                pattern.Hour = int.Parse(patternHour);
                pattern.Minute= int.Parse(patternMinute);
                return pattern;
            }

            


                return null;
        }

        private void ParsePattern(string patternSecond, Pattern pattern)
        {
            if (patternSecond.Contains("/"))
            {
                var parts = patternSecond.Split('/');
                if (parts[0] == "*")
                {
                    int unit;
                    if (int.TryParse(parts[1], out unit))
                    {
                        pattern.EveryNUnit = unit;
                    }
                    else
                    {
                        pattern.Units = Array.ConvertAll(parts[1].Split(','), int.Parse).ToList();

                    }
                }
            }
            else
                pattern.Unit = int.Parse(patternSecond);

        }

    }
}
