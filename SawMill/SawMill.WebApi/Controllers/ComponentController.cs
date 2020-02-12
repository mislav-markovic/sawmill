using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.Component;

namespace SawMill.WebApi.Controllers
{
  // TODO implement with actual repositories
  [Route("api/[controller]")]
  [ApiController]
  public class ComponentController : ControllerBase
  {
    private readonly IComponentService _componentService;
    private readonly IPresenter<ComponentViewModel, Component> _presenter;

    public ComponentController(IPresenter<ComponentViewModel, Component> presenter, IComponentService componentService)
    {
      _presenter = presenter;
      _componentService = componentService;
    }

    // create new
    [HttpPost]
    public async Task<ActionResult<ComponentViewModel>> Create(ComponentViewModel model)
    {
      if (model == null)
      {
        return BadRequest();
      }

      try
      {
        var component = await _presenter.Request(model);
        var result = await _componentService.Create(component);
        return CreatedAtAction(nameof(Create), await _presenter.Present(result));
      }
      catch (Exception debug)
      {
        return BadRequest("Couldn't create component");
      }
    }

    // delete existing
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
      try
      {
        await _componentService.Delete(id);
        return true;
      }
      catch
      {
        return BadRequest($"Could not delete component with id {id}");
      }
    }

    // get one
    [HttpGet("{id}")]
    public async Task<ActionResult<ComponentViewModel>> Get(int id)
    {
      try
      {
        var component = await _componentService.Get(id);
        if (component == null)
        {
          return BadRequest($"Component with id {id} does not exist");
        }

        return Ok(await _presenter.Present(component));
      }
      catch
      {
        return BadRequest($"Component with id {id} does not exist");
      }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ComponentViewModel>> Edit([FromBody] ComponentViewModel model, int id)
    {
      if (model == null || model.Id != id)
      {
        return BadRequest($"Could not edit component with id {id}");
      }

      try
      {
        var component = await _componentService.Edit(await _presenter.Request(model));
        return Ok(await _presenter.Present(component));
      }
      catch (Exception debug)
      {
        return BadRequest($"Could not edit component with id {id}");
      }
    }

    // Get all or get filtered by systemId param
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ComponentViewModel>>> Index([FromQuery] int? systemId)
    {
      if (systemId.HasValue)
      {
        try
        {
          var components = await _componentService.ComponentsForSystem(systemId.Value);
          var transformed = new List<ComponentViewModel>();
          foreach (var component in components)
          {
            var transform = await _presenter.Present(component);
            transformed.Add(transform);
          }

          return Ok(transformed);
        }
        catch
        {
          return BadRequest($"Could not retrieve components for system with id {systemId.Value}");
        }
      }

      try
      {
        var components = await _componentService.GetAll();
        var transformed = new List<ComponentViewModel>();
        foreach (var component in components)
        {
          var transform = await _presenter.Present(component);
          transformed.Add(transform);
        }

        return Ok(transformed);
      }
      catch
      {
        return BadRequest("Could not fetch all components");
      }
    }
  }
}