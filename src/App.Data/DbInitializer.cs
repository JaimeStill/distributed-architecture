using App.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Platform.Contracts;

namespace App.Data;
public static class DbInitializer
{
    public static async Task Initialize(this AppDbContext db, IStreamService<IPhoto> picsumSvc)
    {
        Console.WriteLine("Initializing database");
        await db.Database.MigrateAsync();
        Console.WriteLine("Database initialized");

        Console.WriteLine("Seeding photos");
        List<Photo> photos = new();

        var observer = picsumSvc.GetObserver(
            (IPhoto iphoto) => db.SeedPhoto(iphoto),
            (Exception error) => throw new Exception("Error seeding photos"),
            () => db.SaveChanges()
        );

        await picsumSvc.Stream(observer);
    }

    static Photo ToPhoto(this IPhoto photo) => new()
    {
        PicsumId = photo.Id,
        Author = photo.Author,
        DownloadUrl = photo.DownloadUrl,
        Url = photo.Url,
        Height = photo.Height,
        Width = photo.Width
    };

    static void SeedPhoto(this AppDbContext db, IPhoto iphoto)
    {
        Photo photo = iphoto.ToPhoto();

        var check = !db.Photos
            .Any(p =>
                p.Id != photo.Id
                && p.PicsumId == photo.PicsumId
            );

        if (check)
            db.Photos.Add(photo);
    }
}