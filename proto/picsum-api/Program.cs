using System.Net.Http.Json;
using PicsumApi;

for (int i = 1; i <= 10; i++)
    await StreamPicsumPhotos(PicsumUrl(i));
    
static string PicsumUrl(int page) =>
    $"https://picsum.photos/v2/list?limit=100&sort=id&page={page}";

static async Task StreamPicsumPhotos(string url)
{
    using HttpClient http = new();

    HttpResponseMessage response = await http.GetAsync(
        url,
        HttpCompletionOption.ResponseHeadersRead
    ).ConfigureAwait(false);

    response.EnsureSuccessStatusCode();

    IAsyncEnumerable<PicsumPhoto> photos = await response.Content
        .ReadFromJsonAsync<IAsyncEnumerable<PicsumPhoto>>()
        .ConfigureAwait(false);

    await foreach (PicsumPhoto photo in photos)
    {
        Console.WriteLine($"Id: {photo.Id}");
        Console.WriteLine($"Author: {photo.Author}");
        Console.WriteLine($"Url: {photo.Url}");
        Console.WriteLine($"Download URL: {photo.DownloadUrl}");
        Console.WriteLine($"Width: {photo.Width}");
        Console.WriteLine($"Height: {photo.Height}");
        Console.WriteLine();
    }
}