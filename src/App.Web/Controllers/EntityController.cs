using Microsoft.AspNetCore.Mvc;
using Platform.Contracts;
using Platform.Core.Query;

namespace App.Web.Controllers;

public class EntityController<T> : ControllerBase where T : EntityBase
{
    protected readonly IService<T> svc;

    public EntityController(IService<T> svc)
    {
        this.svc = svc;
    }

    [HttpGet("[action]")]
    public virtual async Task<IActionResult> Query([FromQuery]QueryParams query) =>
        Ok(await svc.QueryAll(query));

    [HttpGet("[action]")]
    public virtual async Task<IActionResult> Find([FromRoute]int id) =>
        Ok(await svc.Find(id));

    [HttpPost("[action]")]
    public virtual async Task<IActionResult> Save([FromBody]T entity) =>
        Ok(await svc.Save(entity));

    [HttpPost("[action]")]
    public virtual async Task<IActionResult> Remove([FromBody]T entity) =>
        Ok(await svc.Remove(entity));
}