using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.Log;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class LogController : ControllerBase
  {
    private readonly IAlertValueService _alertValueService;
    private readonly IComponentService _componentService;

    private readonly IPresenter<NormalizedLogViewModel, NormalizedLog> _normalizedLogPresenter;
    private readonly INormalizedLogService _normalizedLogService;
    private readonly IParseService _parseService;
    private readonly IParsingRulesService _parsingRulesService;
    private readonly IPresenter<RawLogViewModel, RawLog> _rawLogPresenter;
    private readonly IRawLogService _rawLogService;


    public LogController(
      INormalizedLogService normalizedLogService,
      IRawLogService rawLogService,
      IParsingRulesService parsingRulesService,
      IParseService parseService,
      IPresenter<NormalizedLogViewModel, NormalizedLog> normalizedLogPresenter,
      IPresenter<RawLogViewModel, RawLog> rawLogPresenter, IAlertValueService alertValueService, IComponentService componentService)
    {
      _normalizedLogService = normalizedLogService;
      _rawLogService = rawLogService;
      _parsingRulesService = parsingRulesService;
      _parseService = parseService;
      _normalizedLogPresenter = normalizedLogPresenter;
      _rawLogPresenter = rawLogPresenter;
      _alertValueService = alertValueService;
      _componentService = componentService;
    }

    // logs for component by component name, delegates to existing implementation
    [HttpPost("raw")]
    public async Task<ActionResult<bool>> LogForComponentByName([FromQuery] string componentName,
      [FromBody] string[] lines)
    {
      var comp = await _componentService.ComponentByName(componentName);
      return await LogForComponent(comp.Id, lines);
    }

    [HttpPost("raw/{componentId}")]
    public async Task<ActionResult<bool>> LogForComponent(int componentId, [FromBody] string[] lines)
    {
      try
      {
        var parsingRules = await _parsingRulesService.GetComponentsParsingRules(componentId);
        var logs = new List<NormalizedLog>();
        foreach (var line in lines)
        {
          var workingLine = line.Trim();
          if (string.IsNullOrWhiteSpace(workingLine))
          {
            continue;
          }

          try
          {
            var raw = new RawLog { Id = 0, ParentComponentId = componentId, Message = workingLine };
            var rawSaved = await _rawLogService.Create(raw);
            var normalized = _parseService.ParseAll(rawSaved, parsingRules);
            logs.Add(await _normalizedLogService.Create(normalized));
          }
          catch (Exception e)
          {
            continue;
          }

        }

        var alerts = await _alertValueService.GenerateAlerts(logs);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }

      return Ok(true);
    }

    [HttpPost("raw/file/{componentId}")]
    public async Task<ActionResult<bool>> LogFileForComponent(int componentId, [FromForm] IFormFile logFile)
    {
      if (logFile == null || logFile.Length == 0)
      {
        return Content("file not selected");
      }

      var lines = new List<string>();
      using (var stream = new StreamReader(logFile.OpenReadStream()))
      {
        while (stream.Peek() > 0) lines.Add(await stream.ReadLineAsync());
      }

      return await LogForComponent(componentId, lines.ToArray());
    }

    [HttpGet("normalized/component/{componentId}")]
    public async Task<ActionResult<IEnumerable<NormalizedLogViewModel>>> Normalized(
      int componentId,
      [FromQuery] DateTime? start = null,
      [FromQuery] DateTime? end = null)
    {
      IEnumerable<NormalizedLog> all;
      if (start == null && end == null)
      {
        all = await _normalizedLogService.ComponentsLog(componentId);
      }
      else
      {
        if (start == null)
        {
          start = DateTime.MinValue;
        }

        if (end == null)
        {
          end = DateTime.MaxValue;
        }

        all = await _normalizedLogService.ComponentsLogWithinTimeFrame(componentId, start.Value, end.Value);
      }

      var transformed = new List<NormalizedLogViewModel>();
      foreach (var normalizedLog in all)
      {
        var presented = await _normalizedLogPresenter.Present(normalizedLog);
        transformed.Add(presented);
      }

      return Ok(transformed);
    }

    [HttpGet("normalized/system/paginated/{systemId}")]
    public async Task<ActionResult<IEnumerable<NormalizedLogViewModel>>> NormalizedLogsForSystemPaginated(int systemId,
      [FromQuery] int? paginated, [FromQuery] int? lastLogId)
    {
      try
      {
        var all = await _normalizedLogService.SystemLogsPaginated(systemId, lastLogId.Value, paginated.Value);
        var transformed = new List<NormalizedLogViewModel>();

        foreach (var normalizedLog in all)
        {
          var transform = await _normalizedLogPresenter.Present(normalizedLog);
          transformed.Add(transform);
        }

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }

    [HttpGet("normalized/system/{systemId}")]
    public async Task<ActionResult<IEnumerable<NormalizedLogViewModel>>> NormalizedLogsForSystem(int systemId,
      [FromQuery] DateTime? start = null, [FromQuery] DateTime? end = null, [FromQuery] int? paginated = null)
    {
      try
      {
        IEnumerable<NormalizedLog> all;

        if (start == null)
        {
          start = DateTime.UnixEpoch;
        }

        if (end == null)
        {
          end = DateTime.MaxValue;
        }

        if (paginated.HasValue)
        {
          all = await _normalizedLogService.SystemLogs(systemId, start.Value, end.Value, paginated.Value);
        }
        else
        {
          all = await _normalizedLogService.SystemLogs(systemId, start.Value, end.Value);
        }

        var transformed = new List<NormalizedLogViewModel>();
        foreach (var normalizedLog in all)
        {
          var presented = await _normalizedLogPresenter.Present(normalizedLog);
          transformed.Add(presented);
        }

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        return BadRequest(debug.Message);
      }
    }
  }
}