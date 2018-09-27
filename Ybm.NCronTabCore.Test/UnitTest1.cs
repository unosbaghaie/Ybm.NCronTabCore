/*
┌───────────── Second (0 - 59)
| ┌───────────── minute (0 - 59)
| │ ┌───────────── hour (0 - 23)
| │ │ ┌───────────── day of month (1 - 31)
| │ │ │ ┌───────────── month (1 - 12)
| │ │ │ │ ┌───────────── day of week (0 - 6) (Sunday to Saturday;
| │ │ │ │ │                                       7 is also Sunday)
| │ │ │ │ │
| │ │ │ │ │
* * * * * *  command to execute
* 
*/

//  *'/10 * * * * *                 = every 10 seconds
//  * *'/10 * * * *                 = every 10 minutes
//  * * *'/10 * * *                 = every 10 hours

//  * 50 9 *'/10 * *                = every 10 days at9:50

//  * 0 9 1 *'/1 *                  = first day of every month at 9:00
//  * 0 9 L*'/1 *                  = last day of every month at 9:00 

//  * 0 12 15 1 *                  = 15th of farvardin at 12:0
//  * 30 9 15 *'/1,3,5,7,9,11 *       = DayOfMonth 15, months = 1,3,5,7,9,11 , at 9:30 



using System;
using System.Linq;
using Xunit;

namespace Ybm.NCronTabCore.Test
{
    public class UnitTest1
    {
       

        [Theory]
        [InlineData("*/10 * * * * *")]  
        public void Secondly(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 7, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.True(res.Any());
        }


        [Theory]
        [InlineData("* */10 * * * *")]  // every 10 minutes
        public void Minutely(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 10, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.True(res.Any());
        }


        [Theory]
        [InlineData("* * */5 * * *")]  // every 5 hours
        public void Hourly(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 10, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.True(res.Any());
        }


        [Theory]
        [InlineData("* 0 9 */5 * * *")] // every 5 days at 9:00
        public void Every_5_Days_At_9(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 10, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.True(res.Any());
        }


        [Theory]
        [InlineData("* 0 9 */1 * * *")] // everydays at 9:00
        public void EveryDayAt9(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 10, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.True(res.Any());
        }

        [Theory]
        [InlineData("* 10 9 15 */1,3,5,7,9,11 *")]  // DayOfMonth 15, months = 1,3,5,7,9,11 , at 9:10
        public void Montly(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 10, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.True(res.Any());
        }

        [Theory]
        [InlineData("* 10 9 * * */0,5")] // every saturday and thursday at 9:10
        [InlineData("* 10 9 * * */1")]  // every sunday at 9:10
        public void Every_Saturday(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 10, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.True(res.Any());
        }

        [Theory]
        [InlineData("* 0 9 L * *")]  // DayOfMonth last day, months = * , at 9:30
        public void LastDay_SpecificDays_Montly(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 12, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.Equal("1397/3/31 9:00:00 AM", res[0]);
            Assert.Equal("1397/4/31 9:00:00 AM", res[1]);
            Assert.Equal("1397/5/31 9:00:00 AM", res[2]);
            Assert.Equal("1397/6/31 9:00:00 AM", res[3]);
            Assert.Equal("1397/7/30 9:00:00 AM", res[4]);
            Assert.Equal("1397/8/30 9:00:00 AM", res[5]);
        }


        [Theory]
        [InlineData("* 0 9 1 * *")]  // DayOfMonth 1, months = * , at 9:00
        public void FirstDay_SpecificDays_Montly(string cronPattern)
        {
            var startDate = new DateTime(2018, 1, 1, 10, 50, 25);
            var endDate = new DateTime(2019, 12, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.Equal("1396/11/1 9:00:00 AM", res[0]);
            Assert.Equal("1397/9/1 9:00:00 AM", res[10]);
            Assert.Equal("1398/7/1 9:00:00 AM", res[20]);
        }


        [Theory]
        [InlineData("* 0 9 10 * *")]  // DayOfMonth 10, months = * , at 9:30
        public void Tenth_SpecificDays_Montly(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 12, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.Equal("1397/4/10 9:00:00 AM", res[0]);
            Assert.Equal("1397/5/10 9:00:00 AM", res[1]);
            Assert.Equal("1397/6/10 9:00:00 AM", res[2]);
            Assert.Equal("1397/7/10 9:00:00 AM", res[3]);
            Assert.Equal("1397/8/10 9:00:00 AM", res[4]);
        }


        //(\* \d+ \d+ \d+ \*\/(\d+|\p{P}+)* \*)
        //* 10 9 15 */1,3,5,7,9,11 *
        //* 10 9 15 */1 *
        [Theory]
        [InlineData("* 10 9 15 */1,3,5,7,9,11 *")]

        public void SpecificDays_in_SpecificMonths(string cronPattern)
        {
            var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
            var endDate = new DateTime(2018, 12, 1, 10, 50, 25);

            var res = new CronTabScheduler().Occurances(cronPattern, startDate, endDate, true);

            Assert.Equal("1397/3/15 9:10:00 AM", res[0]);
            Assert.Equal("1397/5/15 9:10:00 AM", res[1]);
            Assert.Equal("1397/7/15 9:10:00 AM", res[2]);
        }
    }
}
