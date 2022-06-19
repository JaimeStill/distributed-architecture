using Picsum;
using System.Net.Http.Json;

using HttpClient http = new();

HttpResponseMessage response = await http.GetAsync(
    "http://localhost:5001/api/picsum/getPhotos",
    HttpCompletionOption.ResponseHeadersRead
).ConfigureAwait(false);

response.EnsureSuccessStatusCode();

var photos = await response
    .Content
    .ReadFromJsonAsync<IAsyncEnumerable<PicsumPhoto>>()
    .ConfigureAwait(false);

await foreach (PicsumPhoto photo in photos)
    photo.Print();