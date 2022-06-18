namespace Platform.Contracts;
public interface IPhoto
{
    int Id { get; set; }
    string Author { get; set; }
    string Url { get; set; }
    string DownloadUrl { get; set; }
    int Width { get; set; }
    int Height { get; set; }
}