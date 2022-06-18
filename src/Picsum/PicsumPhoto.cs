using Platform.Contracts;
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

    public static async IAsyncEnumerable<PicsumPhoto> Stream()
    {
        var streams = Enumerable.Range(1, 10)
            .Select(page => StreamPicsumPhotos(page))
            .Merge();

        await foreach (PicsumPhoto photo in streams)
            yield return photo;
    }

    static async IAsyncEnumerable<PicsumPhoto> StreamPicsumPhotos(int page)
    {
        static string PicsumUrl(int page) =>
            $"https://picsum.photos/v2/list?limit=100&sort=id&page={page}";

        using HttpClient http = new();

        HttpResponseMessage response = await http.GetAsync(
            PicsumUrl(page),
            HttpCompletionOption.ResponseHeadersRead
        ).ConfigureAwait(false);

        response.EnsureSuccessStatusCode();

        var photos = await response
            .Content
            .ReadFromJsonAsync<IAsyncEnumerable<PicsumPhoto>>()
            .ConfigureAwait(false);

        await foreach (PicsumPhoto photo in photos)
            yield return photo;
    }
}