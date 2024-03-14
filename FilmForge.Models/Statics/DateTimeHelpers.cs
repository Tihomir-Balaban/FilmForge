namespace FilmForge.Models.Statics;

public static class DateTimeHelpers
{
    public static bool IsDateBetween(this DateTime dateToCheck, DateTime startDate, DateTime endDate)
    {
        return dateToCheck >= startDate && dateToCheck <= endDate;
    }
}