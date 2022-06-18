using App.Models.Entities;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Platform.Core.Query;

namespace App.Web.Controllers;

[Route("api/[controller]")]
public class PhotoController : EntityController<Photo>
{
    readonly PhotoService photoSvc;

    public PhotoController(PhotoService svc) : base(svc)
    {
        photoSvc = svc;
    }

    [HttpGet("[action]/{author}")]
    [ProducesResponseType(typeof(QueryResult<Photo>), 200)]
    public async Task<IActionResult> QueryByAuthor(
        [FromRoute]string author,
        [FromQuery]QueryParams query
    ) => Ok(await photoSvc.QueryByAuthor(author, query));

    [HttpPost("[action]")]
    [ProducesResponseType(typeof(bool), 200)]
    public async Task<IActionResult> Validate([FromBody]Photo photo) =>
        Ok(await photoSvc.Validate(photo));

    [HttpPost("[action]")]
    [ProducesResponseType(typeof(Photo), 200)]
    [ProducesResponseType(typeof(InvalidDataException), 400)]
    [ProducesResponseType(typeof(Exception), 500)]
    public override async Task<IActionResult> Save([FromBody]Photo photo)
    {
        try
        {
            return Ok(await svc.Save(photo));
        }
        catch (InvalidDataException ex)
        {
            return BadRequest(ex);
        }
        catch (Exception ex)
        {
            return StatusCode(500, ex);
        }
    }
}