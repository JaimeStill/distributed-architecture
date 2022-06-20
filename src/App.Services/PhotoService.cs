using App.Data;
using App.Models.Entities;
using Microsoft.EntityFrameworkCore;
using Platform.Contracts;
using Platform.Core.Query;

namespace App.Services;

public static class PhotoExtensions
{
    public static IQueryable<Photo> Search(this IQueryable<Photo> photos, string search) =>
        photos.Where(x =>
            x.Author.ToLower().Contains(search.ToLower())
            || x.DownloadUrl.ToLower().Contains(search.ToLower())
            || x.Url.ToLower().Contains(search.ToLower())
        );
}

public class PhotoService : ServiceBase<Photo>
{
    IQueryable<Photo> Search(IQueryable<Photo> photos, string search) =>
        photos.Search(search);

    public PhotoService(AppDbContext db) : base(db) { }

    public override async Task<QueryResult<Photo>> QueryAll(QueryParams query) =>
        await Query(set, query, Search);

    public async Task<QueryResult<Photo>> QueryByAuthor(string author, QueryParams query) =>
        await Query(
            set.Where(x => x.Author.ToLower() == author.ToLower()),
            query,
            Search
        );

    public async Task<bool> Validate(Photo photo) =>
        !await set.AnyAsync(p =>
            p.Id != photo.Id
            && p.PicsumId == photo.PicsumId
        );

    public override async Task<Photo> Save(Photo photo)
    {
        if (await Validate(photo))
        {
            return photo.Id > 0
                ? await Update(photo)
                : await Add(photo);
        }
        else
            throw new InvalidDataException("The provided photo already exists");
    }

    public async Task Seed(IStreamService<IPhoto> picsumSvc)
    {
        var observer = picsumSvc.GetObserver(
            (IPhoto iphoto) =>
            {
                var photo = ToPhoto(iphoto);
                if (ValidateSync(photo))
                    set.Add(photo);
            },
            (Exception ex) => throw new Exception("Error seeding photos"),
            () => db.SaveChanges()
        );

        await picsumSvc.Stream(observer);
    }

    public async Task SeedAsync(IStreamService<IPhoto> picsumSvc)
    {
        await foreach (IPhoto iphoto in picsumSvc.StreamAsync())
        {
            var photo = ToPhoto(iphoto);

            if (await Validate(photo))
                set.Add(photo);
        }

        await db.SaveChangesAsync();
    }

    bool ValidateSync(Photo photo) =>
        !set.Any(p =>
            p.Id != photo.Id
            && p.PicsumId == photo.PicsumId
        );

    static Photo ToPhoto(IPhoto photo) => new()
    {
        PicsumId = photo.Id,
        Author = photo.Author,
        DownloadUrl = photo.DownloadUrl,
        Url = photo.Url,
        Height = photo.Height,
        Width = photo.Width
    };
}