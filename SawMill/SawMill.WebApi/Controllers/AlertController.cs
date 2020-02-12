using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.Alert;
using SawMill.WebApi.ViewModel.AlertValue;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AlertController : ControllerBase
  {
    private readonly IPresenter<AlertViewModel, Alert> _alertPresenter;
    private readonly IAlertService _alertService;
    private readonly IPresenter<AlertValueViewModel, AlertValue> _alertValuePresenter;
    private readonly IAlertValueService _alertValueService;

    public AlertController(IAlertService alertService, IPresenter<AlertViewModel, Alert> alertPresenter,
      IAlertValueService alertValueService, IPresenter<AlertValueViewModel, AlertValue> alertValuePresenter)
    {
      _alertService = alertService;
      _alertPresenter = alertPresenter;
      _alertValueService = alertValueService;
      _alertValuePresenter = alertValuePresenter;
    }

    // GET: api/Alert
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlertViewModel>>> Index()
    {
      try
      {
        var all = await _alertService.GetAll();
        var transformed = new List<AlertViewModel>();

        foreach (var alert in all) transformed.Add(await _alertPresenter.Present(alert));

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }

    // GET: api/Alert/value/5
    [HttpGet("value/{alertId}")]
    public async Task<ActionResult<IEnumerable<AlertValueViewModel>>> GetAlertValue(int alertId)
    {
      try
      {
        var alertValues = await _alertValueService.AlertValuesForAlert(alertId);
        var transformed = new List<AlertValueViewModel>();

        foreach (var alertValue in alertValues) transformed.Add(await _alertValuePresenter.Present(alertValue));

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }

    // GET: api/Alert/5
    [HttpGet("{id}")]
    public async Task<ActionResult<AlertViewModel>> Get(int id)
    {
      try
      {
        var alert = await _alertService.Get(id);
        if (alert == null)
        {
          return BadRequest($"Alerts with id {id} does not exist");
        }

        return Ok(await _alertPresenter.Present(alert));
      }
      catch
      {
        return BadRequest($"Alerts with id {id} does not exist");
      }
    }

    // POST: api/Alert
    [HttpPost]
    public async Task<ActionResult<AlertViewModel>> Create([FromBody] AlertViewModel model)
    {
      if (model == null)
      {
        return BadRequest();
      }

      try
      {
        var alert = await _alertPresenter.Request(model);
        var result = await _alertService.Create(alert);
        return CreatedAtAction(nameof(Create), await _alertPresenter.Present(result));
      }
      catch (Exception debug)
      {
        return BadRequest("Couldn't create alert");
      }
    }

    // PUT: api/Alert/5
    [HttpPut("{id}")]
    public async Task<ActionResult<AlertViewModel>> Edit(int id, [FromBody] AlertViewModel model)
    {
      if (model == null || model.Id != id)
      {
        return BadRequest($"Could not edit alert with id {id}");
      }

      try
      {
        var alert = await _alertService.Edit(await _alertPresenter.Request(model));
        return Ok(await _alertPresenter.Present(alert));
      }
      catch (Exception debug)
      {
        return BadRequest($"Could not edit alert with id {id}");
      }
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
      try
      {
        await _alertService.Delete(id);
        return true;
      }
      catch
      {
        return BadRequest($"Could not delete alert with id {id}");
      }
    }
  }
}