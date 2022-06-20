using Picsum;
using System.Net.Http.Headers;
using System.Net.Http.Json;

using HttpClient http = new();

HttpResponseMessage response = await http.GetAsync(
    PicsumPhoto.PicsumUrl(1)
).ConfigureAwait(false);

response.EnsureSuccessStatusCode();

if (response.Headers.TryGetValues("link", out IEnumerable<string> links))
    Console.WriteLine(links.First().Split(';').First().TrimStart('<').TrimEnd('>'));

static void CheckHeaders(HttpResponseHeaders headers)
{
    if (headers.TryGetValues("link", out IEnumerable<string> links))
    {
        var link = links.First().Split(';').First().TrimStart('<').TrimEnd('>');
    }
}

static async IAsyncEnumerable<PicsumPhoto> Stream(string url)
{
    using HttpClient http = new();

    HttpResponseMessage response = await http.GetAsync(
        url
    ).ConfigureAwait(false);

    response.EnsureSuccessStatusCode();

    var photos = await response
        .Content
        .ReadFromJsonAsync<IAsyncEnumerable<PicsumPhoto>>()
        .ConfigureAwait(false);

    await foreach (PicsumPhoto photo in photos)
        yield return photo;
}