using Picsum;
using System.Net.Http.Headers;
using System.Net.Http.Json;

var photos = Stream(PicsumPhoto.PicsumUrl(1));

await foreach(PicsumPhoto photo in photos)
    photo.Print();

static async IAsyncEnumerable<PicsumPhoto> Stream(string url)
{
    using HttpClient http = new();

    await foreach (PicsumPhoto photo in StreamPhotos(http, url))
        yield return photo;
}

static async IAsyncEnumerable<PicsumPhoto> StreamPhotos(HttpClient http, string url)
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
    {
        await foreach(PicsumPhoto photo in StreamPhotos(http, next))
            yield return photo;
    }
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