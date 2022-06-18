using Platform.Contracts;

namespace App.Models.Entities;
public class Photo : EntityBase, IPhoto
{
    public string Author { get; set; }
    public string Url { get; set; }
    public string DownloadUrl { get; set; }
    public int Width { get; set; }
    public int Height { get; set; }
}