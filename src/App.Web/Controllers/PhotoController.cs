using App.Models.Entities;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.Contracts;
using Platform.Core.Query;

namespace App.Web.Controllers;

[Route("api/[controller]")]
public class PhotoController : EntityController<Photo>
{
    readonly PhotoService photoSvc;
    readonly IStreamService<IPhoto> picsumSvc;

    public PhotoController(PhotoService svc, IStreamService<IPhoto> picsumSvc)
        : base(svc)
    {
        photoSvc = svc;
        this.picsumSvc = picsumSvc;
    }

    [HttpGet("[action]/{author}")]
    [ProducesResponseType(typeof(QueryResult<Photo>), 200)]
    public async Task<IActionResult> QueryByAuthor(
        [FromRoute]string author,
        [FromQuery]QueryParams query
    ) => Ok(await photoSvc.QueryByAuthor(author, query));

    [HttpGet("[action]")]
    public async Task SeedByObservable() => await photoSvc.SeedByObservable(picsumSvc);

    [HttpGet("[action]")]
    public async Task SeedByStream() => await photoSvc.SeedByStream(picsumSvc);

    [HttpPost("[action]")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> Validate([FromBody]Photo photo) =>
        Ok(await photoSvc.Validate(photo));
}