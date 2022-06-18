using System.Text.RegularExpressions;

namespace Platform.Core;
public static class CoreExtensions
{
    private static readonly string jsDateTime = "yyyy-MM-ddTHH:mm:ss.fffZ";

    public static string ToJsDateString(this DateTime date) =>
        date.ToString(jsDateTime);

    private static readonly string urlPattern = "[^a-zA-Z0-9-.]";
    public static string UrlEncode(this string url, string pattern, string replace = "")
    {
        var friendlyUrl = Regex
            .Replace(url, @"\s", "-")
            .ToLower();

        friendlyUrl = Regex
            .Replace(friendlyUrl, pattern, replace);

        return friendlyUrl;
    }

    public static string UrlEncode(this string url) =>
        url.UrlEncode(urlPattern, "-");

    public static IQueryable<T> SetupSearch<T>(
        this IQueryable<T> values,
        string search,
        Func<IQueryable<T>, string, IQueryable<T>> action,
        char split = '|'
    )
    {
        if (search.Contains(split))
        {
            var searches = search.Split(split);

            foreach (var s in searches)
                values = action(values, s.Trim());

            return values;
        }
        else
            return action(values, search);
    }
}