using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.SeverityRule;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SeverityRuleController : ControllerBase
  {
    private readonly IPresenter<SeverityRuleViewModel, SeverityRule> _presenter;
    private readonly ISeverityRuleService _severityRuleService;

    public SeverityRuleController(IPresenter<SeverityRuleViewModel, SeverityRule> presenter,
      ISeverityRuleService severityRuleService)
    {
      _presenter = presenter;
      _severityRuleService = severityRuleService;
    }

    // GET: api/SeverityRule
    [HttpGet]
    public async Task<ActionResult<IEnumerable<SeverityRuleViewModel>>> Index()
    {
      try
      {
        var all = await _severityRuleService.GetAll();

        var transformed = new List<SeverityRuleViewModel>();
        foreach (var component in all)
        {
          var transform = await _presenter.Present(component);
          transformed.Add(transform);
        }

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        return StatusCode((int) HttpStatusCode.InternalServerError);
      }
    }

    // GET: api/SeverityRule/5
    [HttpGet("{id}")]
    public async Task<ActionResult<SeverityRuleViewModel>> Get(int id)
    {
      try
      {
        var result = await _severityRuleService.Get(id);
        return Ok(await _presenter.Present(result));
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound();
      }
      catch (Exception)
      {
        return BadRequest($"Couldn't fetch severity rule with id {id}");
      }
    }

    // POST: api/SeverityRule
    [HttpPost]
    public async Task<ActionResult<SeverityRuleViewModel>> Create([FromBody] SeverityRuleViewModel model)
    {
      if (model == null)
      {
        return BadRequest();
      }

      var severityRuleToCreate = await _presenter.Request(model);
      try
      {
        var created = await _severityRuleService.Create(severityRuleToCreate);
        return CreatedAtAction(nameof(Create), await _presenter.Present(created));
      }
      catch (Exception debug)
      {
        return BadRequest("Failed to create severity rule");
      }
    }

    // PUT: api/SeverityRule/5
    [HttpPut("{id}")]
    public async Task<ActionResult<SeverityRuleViewModel>> Edit(int id, [FromBody] SeverityRuleViewModel model)
    {
      if (model == null || model.Id != id)
      {
        return BadRequest();
      }

      try
      {
        var dateTimeRule = await _presenter.Request(model);

        var updatedDateTimeRule = await _severityRuleService.Edit(dateTimeRule);

        return Ok(await _presenter.Present(updatedDateTimeRule));
      }
      catch
      {
        return BadRequest("Failed to edit severity rule");
      }
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
      try
      {
        await _severityRuleService.Delete(id);
        return Ok(true);
      }
      catch (ArgumentOutOfRangeException)
      {
        return BadRequest($"Severity rule with id {id} does not exists");
      }
      catch (Exception)
      {
        return BadRequest($"Failed to delete severity rule with  {id}");
      }
    }
  }
}