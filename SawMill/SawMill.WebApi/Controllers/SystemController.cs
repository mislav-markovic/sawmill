using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.System;

namespace SawMill.WebApi.Controllers
{
  // TODO implement endpoints with actual repositories
  [Route("api/[controller]")]
  [ApiController]
  public class SystemController : ControllerBase
  {
    private readonly IPresenter<SystemViewModel, Processor.Model.System> _presenter;
    private readonly ISystemService _systemService;

    public SystemController(ISystemService systemService, IPresenter<SystemViewModel, Processor.Model.System> presenter)
    {
      _systemService = systemService;
      _presenter = presenter;
    }

    // create new
    [HttpPost]
    public async Task<ActionResult<SystemViewModel>> Create(SystemViewModel model)
    {
      if (model == null)
      {
        return BadRequest();
      }

      var systemToCreate = await _presenter.Request(model);
      try
      {
        var created = await _systemService.Create(systemToCreate);
        return CreatedAtAction(nameof(Create), await _presenter.Present(created));
      }
      catch (Exception debug)
      {
        return BadRequest("Failed to create system");
      }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<SystemViewModel>> Edit([FromBody] SystemViewModel model, int id)
    {
      if (model == null || model.Id != id)
      {
        return BadRequest();
      }

      try
      {
        var system = await _presenter.Request(model);

        var updatedSystem = await _systemService.Edit(system);

        return Ok(await _presenter.Present(updatedSystem));
      }
      catch
      {
        return BadRequest("Failed to edit system");
      }
    }

    // delete existing
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
      try
      {
        await _systemService.Delete(id);
        return Ok(true);
      }
      catch (ArgumentOutOfRangeException)
      {
        return BadRequest($"System with id {id} does not exists");
      }
      catch (Exception)
      {
        return BadRequest($"Failed to delete {id}");
      }
    }

    // get one
    [HttpGet("{id}")]
    public async Task<ActionResult<SystemViewModel>> Get(int id)
    {
      try
      {
        var result = await _systemService.Get(id);
        return Ok(await _presenter.Present(result));
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound();
      }
      catch (Exception)
      {
        return BadRequest($"Couldn't fetch system with id {id}");
      }
    }

    // get all
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SystemViewModel>>> Index()
    {
      try
      {
        var results = await _systemService.GetAll();
        var transformed = new List<SystemViewModel>();

        foreach (var component in results)
        {
          var transform = await _presenter.Present(component);
          transformed.Add(transform);
        }

        return Ok(transformed);
      }
      catch (Exception)
      {
        return StatusCode((int) HttpStatusCode.InternalServerError);
      }
    }
  }
}