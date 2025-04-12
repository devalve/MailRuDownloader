namespace MailRuDownloader.Helpers;

public static class CommonHelper
{
    public static string SanitizeFileName(string title)
    {
        foreach (var c in Path.GetInvalidFileNameChars())
        {
            title = title.Replace(c, '_');
        }
        return title;
    }

}