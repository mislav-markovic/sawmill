using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.AlertGroup;
using SawMill.WebApi.ViewModel.AlertGroupValue;
using SawMill.WebApi.ViewModel.AlertValue;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AlertGroupController : ControllerBase
  {
    private readonly IAlertGroupService _alertGroupService;
    private readonly IAlertGroupValueService _alertGroupValueService;
    private readonly IPresenter<AlertGroupViewModel, AlertGroup> _presenter;

    public AlertGroupController(IAlertGroupService alertGroupService,
      IPresenter<AlertGroupViewModel, AlertGroup> presenter, IAlertGroupValueService alertGroupValueService)
    {
      _alertGroupService = alertGroupService;
      _presenter = presenter;
      _alertGroupValueService = alertGroupValueService;
    }

    // GET: api/AlertGroup
    [HttpGet]
    public async Task<ActionResult<IEnumerable<AlertGroupViewModel>>> Index()
    {
      try
      {
        var all = await _alertGroupService.GetAll();
        var result = new List<AlertGroupViewModel>();
        foreach (var alertGroup in all)
        {
          var transformed = await _presenter.Present(alertGroup);
          result.Add(transformed);
        }

        return Ok(result);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }


    // GET: api/AlertGroup/value/5
    [HttpGet("value/{alertGroupId}")]
    public async Task<ActionResult<IEnumerable<AlertGroupValueViewModel>>> GetAlertGroupValue(int alertGroupId)
    {
      try
      {
        var alertGroupValues = await _alertGroupValueService.AlertGroupValuesForAlertGroup(alertGroupId);

        var transformed = alertGroupValues.Select(alertGroupValue => new AlertGroupValueViewModel(alertGroupValue.Id,
          alertGroupValue.AlertGroupId, alertGroupValue.TimespanStart, alertGroupValue.TimespanEnd,
          alertGroupValue.AlertValues
            .Select(e => new AlertValueViewModel(e.Id, e.AlertId, e.TimespanStart, e.TimespanEnd)).ToArray())).ToList();

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }

    // POST: api/AlertGroup
    [HttpPost]
    public async Task<ActionResult<AlertGroupViewModel>> Create([FromBody] AlertGroupViewModel model)
    {
      try
      {
        var alertGroup = await _presenter.Request(model);
        var created = await _alertGroupService.Create(alertGroup);
        return Ok(created);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }
  }
}