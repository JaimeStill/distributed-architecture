using Picsum;
using System.Net.Http.Json;

if (args.Length > 0)
{
    using HttpClient http = new();

    HttpResponseMessage response = await http.GetAsync(
        args.First(),
        HttpCompletionOption.ResponseHeadersRead
    ).ConfigureAwait(false);

    response.EnsureSuccessStatusCode();

    var photos = await response
        .Content
        .ReadFromJsonAsync<IAsyncEnumerable<PicsumPhoto>>()
        .ConfigureAwait(false);

    await foreach (PicsumPhoto photo in photos)
        photo.Print();
}
else
{
    await foreach(PicsumPhoto photo in PicsumPhoto.Stream())
        photo.Print();
}