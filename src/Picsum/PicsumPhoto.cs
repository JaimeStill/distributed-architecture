using Platform.Contracts;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json.Serialization;

namespace Picsum;

public class PicsumPhoto : IPhoto
{
    public int Id { get; set; }
    public string Author { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Url { get; set; }

    [JsonPropertyName("download_url")]
    public string DownloadUrl { get; set; }

    public void Print()
    {
        Console.WriteLine($"Id: {Id}");
        Console.WriteLine($"Author: {Author}");
        Console.WriteLine($"Url: {Url}");
        Console.WriteLine($"Download URL: {DownloadUrl}");
        Console.WriteLine($"Width: {Width}");
        Console.WriteLine($"Height: {Height}");
        Console.WriteLine();
    }
    public static string PicsumUrl(int page) =>
        $"https://picsum.photos/v2/list?limit=100&sort=id&page={page}";

    public static async IAsyncEnumerable<PicsumPhoto> Stream()
    {
        using HttpClient http = new();

        await foreach(PicsumPhoto photo in StreamPicsumPhotos(http, PicsumUrl(1)))
            yield return photo;
    }

    static async IAsyncEnumerable<PicsumPhoto> StreamPicsumPhotos(HttpClient http, string url)
    {
        HttpResponseMessage response = await http.GetAsync(
            url
        ).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var photos = await response
            .Content
            .ReadFromJsonAsync<IAsyncEnumerable<PicsumPhoto>>()
            .ConfigureAwait(false);

        var next = GetNextLink(response.Headers);

        await foreach (PicsumPhoto photo in photos)
            yield return photo;

        if (!string.IsNullOrEmpty(next))
            await foreach(PicsumPhoto photo in StreamPicsumPhotos(http, next))
                yield return photo;
    }

    static string GetNextLink(HttpResponseHeaders headers)
    {
        if (headers.TryGetValues("link", out IEnumerable<string> links))
        {
            var link = links.First();

            if (link.Contains("rel=\"next\""))
            {
                return link.Contains(',')
                    ? ParseNextLink(link.Split(',').Last())
                    : ParseNextLink(link);
            }
            else
                return string.Empty;
        }
        else
            return string.Empty;
    }

    static string ParseNextLink(string link) =>
        link.Split(';')
            .First()
            .Trim()
            .TrimStart('<')
            .TrimEnd('>');
}