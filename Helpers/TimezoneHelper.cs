namespace NutritionTrackingBot.Helpers;

public static class TimezoneHelper
{
    public static (DateTime StartUtc, DateTime EndUtc) GetUtcRangeForLocalDate(string timeZoneId, DateTime localDate)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);

        var localStart = DateTime.SpecifyKind(localDate.Date, DateTimeKind.Unspecified);    
        var localEnd = localStart.AddDays(1);

        var startUtc = TimeZoneInfo.ConvertTimeToUtc(localStart,timeZone);
        var endUtc = TimeZoneInfo.ConvertTimeToUtc(localEnd,timeZone);

        return(startUtc, endUtc);
    }

    public static (DateTime StartUtc, DateTime EndUtc) GetUtcRangeForToday(string timeZoneId)
    {
        var timeZone = TimeZoneInfo.FindSystemTimeZoneById(timeZoneId);
        
        var localNow = DateTime.SpecifyKind(DateTime.UtcNow, DateTimeKind.Unspecified);
        var localStart = localNow;
        var localEnd = localStart.AddDays(1);

        var startUtc = TimeZoneInfo.ConvertTimeToUtc(localStart, timeZone);
        var endUtc = TimeZoneInfo.ConvertTimeToUtc(localEnd, timeZone);
        return (startUtc, endUtc);
    }
}
