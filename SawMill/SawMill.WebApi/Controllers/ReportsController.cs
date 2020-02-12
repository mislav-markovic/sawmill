using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.Reports;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ReportsController : ControllerBase
  {
    private readonly IPresenter<SystemReportViewModel, SystemReport> _systemReportPresenter;
    private readonly ISystemReportRepository _systemReportRepository;

    public ReportsController(ISystemReportRepository systemReportRepository,
      IPresenter<SystemReportViewModel, SystemReport> systemReportPresenter)
    {
      _systemReportRepository = systemReportRepository;
      _systemReportPresenter = systemReportPresenter;
    }

    // GET: api/Reports/5
    [HttpGet("system/{systemId}")]
    public async Task<ActionResult<IEnumerable<SystemReportViewModel>>> GetForSystem(int systemId)
    {
      try
      {
        var all = await _systemReportRepository.ReportsForSystem(systemId);
        var transformed = new List<SystemReportViewModel>();

        foreach (var systemReport in all)
        {
          var presented = await _systemReportPresenter.Present(systemReport);
          transformed.Add(presented);
        }

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }

    // GET: api/Reports/5
    [HttpGet("{reportId}")]
    public async Task<ActionResult<SystemReportViewModel>> Get(int reportId)
    {
      try
      {
        var report = await _systemReportRepository.GetReport(reportId);
        var transformed = await _systemReportPresenter.Present(report);

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }

    [HttpPost("{systemId}")]
    public async Task<ActionResult<SystemReportViewModel>> Create(int systemId)
    {
      try
      {
        var temp = await _systemReportRepository.GenerateReportForSystem(systemId);
        var transformed = await _systemReportPresenter.Present(temp);
        return Ok(transformed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }
  }
}