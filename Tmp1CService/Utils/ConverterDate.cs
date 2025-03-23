namespace Tmp1CService.Utils;

public static class ConverterDate
{
    public static DateTime ConvertToUtc(DateTime date)
    {
        return DateTime.SpecifyKind(date, DateTimeKind.Utc);
    }
}