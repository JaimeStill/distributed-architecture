using Microsoft.AspNetCore.Mvc;

namespace Picsum.Api.Controllers;
[Route("api/[controller]")]
public class PicsumController : Controller
{
    [HttpGet("[action]")]
    public IAsyncEnumerable<PicsumPhoto> GetPhotos() =>
        PicsumPhoto.Stream();
}