using Microsoft.AspNetCore.Mvc;

namespace Picsum.Api.Controllers;
[Route("api/[controller]")]
public class PicsumController : Controller
{
    [HttpGet("[action]")]
    public async IAsyncEnumerable<PicsumPhoto> GetPhotos()
    {
        var stream = PicsumPhoto.Stream();

        await foreach (var photo in stream)
            yield return photo;
    }
}