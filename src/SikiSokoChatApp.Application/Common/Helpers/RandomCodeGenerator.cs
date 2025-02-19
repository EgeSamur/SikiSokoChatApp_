namespace SikiSokoChatApp.Application.Common.Helpers;

public class RandomCodeGenerator
{
    private static Random _random = new Random();

    public static string GenerateContactCode(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789>£#$½{[]}|"; // Harfler ve rakamlar
        char[] stringChars = new char[length];

        for (int i = 0; i < length; i++)
        {
            stringChars[i] = chars[_random.Next(chars.Length)];
        }

        return new string(stringChars);
    }
}
