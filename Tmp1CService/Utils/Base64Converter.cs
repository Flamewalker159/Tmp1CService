using System.Text;

namespace Tmp1CService.Utils;

public static class Base64Converter
{
    public static string ConvertToBase64(string login, string password)
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes($"{login}:{password}"));
    }
}