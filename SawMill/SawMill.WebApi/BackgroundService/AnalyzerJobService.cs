using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using SawMill.Processor.Services.Interface;

namespace SawMill.WebApi.BackgroundService
{
  public class AnalyzerJobService : IAnalyzerJobService
  {
    private readonly IAlertGroupValueService _alertGroupValueService;
    private readonly ISystemService _systemService;


    public AnalyzerJobService(IAlertGroupValueService alertGroupValueService,
      ISystemService systemService)
    {
      _alertGroupValueService = alertGroupValueService;
      _systemService = systemService;
    }

    public async Task Analyze(CancellationToken cancellationToken)
    {
      var systems = await _systemService.GetAll();
      foreach (var system in systems) await _alertGroupValueService.CorrelateAlertsForSystem(system.Id);

      await Task.CompletedTask;
    }
  }
}