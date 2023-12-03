namespace Utils;

using Microsoft.Extensions.FileProviders;

public static class Utils
{
    public static string GetFileContent(string fileName)
    {
        try
        {
            using (var sr = new StreamReader(fileName))
            {
                return sr.ReadToEnd();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"could not read file {fileName}", ex);
        }
        return string.Empty;
    }

    public static IEnumerable<string> GetLines(string fileName)
    {
        return Utils.GetFileContent(fileName).Split('\n').Where(line => !string.IsNullOrEmpty(line));
    }
}
