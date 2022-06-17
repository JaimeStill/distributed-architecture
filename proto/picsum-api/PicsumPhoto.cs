using System.Text.Json.Serialization;

namespace PicsumApi;

public class PicsumPhoto
{
    public string Id { get; set; }
    public string Author { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
    public string Url { get; set; }

    [JsonPropertyName("download_url")]
    public string DownloadUrl { get; set; }
}