using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.DateTimeRule;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class DateTimeRuleController : ControllerBase
  {
    private readonly IDateTimeRuleService _dateTimeRuleService;
    private readonly IPresenter<DateTimeRuleViewModel, DateTimeRule> _presenter;

    public DateTimeRuleController(IPresenter<DateTimeRuleViewModel, DateTimeRule> presenter,
      IDateTimeRuleService dateTimeRuleService)
    {
      _presenter = presenter;
      _dateTimeRuleService = dateTimeRuleService;
    }

    // GET: api/DateTimeRule
    [HttpGet]
    public async Task<ActionResult<IEnumerable<DateTimeRuleViewModel>>> Index()
    {
      try
      {
        var all = await _dateTimeRuleService.GetAll();

        var transformed = new List<DateTimeRuleViewModel>();
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

    // GET: api/DateTimeRule/5
    [HttpGet("{id}")]
    public async Task<ActionResult<DateTimeRuleViewModel>> Get(int id)
    {
      try
      {
        var result = await _dateTimeRuleService.Get(id);
        return Ok(await _presenter.Present(result));
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound();
      }
      catch (Exception)
      {
        return BadRequest($"Couldn't fetch date time rule with id {id}");
      }
    }

    // POST: api/DateTimeRule
    [HttpPost]
    public async Task<ActionResult<DateTimeRuleViewModel>> Create([FromBody] DateTimeRuleViewModel model)
    {
      if (model == null)
      {
        return BadRequest();
      }

      var dateTimeRuleToCreate = await _presenter.Request(model);
      try
      {
        var created = await _dateTimeRuleService.Create(dateTimeRuleToCreate);
        return CreatedAtAction(nameof(Create), await _presenter.Present(created));
      }
      catch (Exception debug)
      {
        return BadRequest("Failed to create date time rule");
      }
    }

    // PUT: api/DateTimeRule/5
    [HttpPut("{id}")]
    public async Task<ActionResult<DateTimeRuleViewModel>> Edit(int id, [FromBody] DateTimeRuleViewModel model)
    {
      if (model == null || model.Id != id)
      {
        return BadRequest();
      }

      try
      {
        var dateTimeRule = await _presenter.Request(model);

        var updatedDateTimeRule = await _dateTimeRuleService.Edit(dateTimeRule);

        return Ok(await _presenter.Present(updatedDateTimeRule));
      }
      catch
      {
        return BadRequest("Failed to edit dateTimeRule");
      }
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
      try
      {
        await _dateTimeRuleService.Delete(id);
        return Ok(true);
      }
      catch (ArgumentOutOfRangeException)
      {
        return BadRequest($"Date time rule with id {id} does not exists");
      }
      catch (Exception)
      {
        return BadRequest($"Failed to delete {id}");
      }
    }
  }
}