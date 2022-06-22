using Picsum;
using System.Net.Http.Headers;
using System.Net.Http.Json;

int page = args.Length > 0
    ? int.Parse(args.First())
    : 2;

using HttpClient http = new();

HttpResponseMessage response = await http.GetAsync(
    PicsumPhoto.PicsumUrl(page)
).ConfigureAwait(false);

response.EnsureSuccessStatusCode();

if (response.Headers.TryGetValues("link", out IEnumerable<string> links))
{
    var link = links.First();

    Console.WriteLine($"Link header: {link}");

    if (link.Contains("rel=\"next\""))
    {
        if (link.Contains(','))
            Console.WriteLine($"rel=\"next\": {ParseNextLink(link.Split(',').Last())}");
        else
            Console.WriteLine($"rel=\"next\": {ParseNextLink(link)}");
    }
}

static string ParseNextLink(string link) =>
    link.Split(';')
        .First()
        .Trim()
        .TrimStart('<')
        .TrimEnd('>');