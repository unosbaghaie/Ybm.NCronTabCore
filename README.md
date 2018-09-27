# Ybm.NCronTabCore
cronTab with Jalali calendar support 


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


Every N seconds

    "*/10 * * * * *"     // every 10 seconds

Every N minutes

    "* */10 * * * *"  // every 10 minutes

Every N hours

    "* * */5 * * *"  // every 5 hours


    "* 0 9 */5 * * *" // every 5 days at 9:00

    "* 0 9 */1 * * *" // everydays at 9:00

    "* 10 9 15 */1,3,5,7,9,11 *"  // DayOfMonth 15, months = 1,3,5,7,9,11 , at 9:10

    "* 10 9 * * */0,5" // every saturday and thursday at 9:10
    "* 10 9 * * */1"  // every sunday at 9:10

    "* 0 9 L * *"  // DayOfMonth last day, months = * , at 9:30

    ("* 0 9 1 * *"  // DayOfMonth 1, months = * , at 9:00

    "* 0 9 10 * *"  // DayOfMonth 10, months = * , at 9:30



Specific day on special days with hour and minute

    "* 10 9 15 */1,3,5,7,9,11 *" 
   
    [Theory]
    [InlineData("* 10 9 15 */1,3,5,7,9,11 *")]
    public void SpecificDays_in_SpecificMonths(string cronPattern)
    {
         var startDate = new DateTime(2018, 6, 1, 10, 50, 25);
         var endDate = new DateTime(2018, 12, 1, 10, 50, 25);
         var res = new CronTabScheduler().NextMainOccurances(cronPattern, startDate, endDate, true);

         Assert.Equal("1397/3/15 9:10:00 AM", res[0]);
         Assert.Equal("1397/5/15 9:10:00 AM", res[1]);
         Assert.Equal("1397/7/15 9:10:00 AM", res[2]);
    }
