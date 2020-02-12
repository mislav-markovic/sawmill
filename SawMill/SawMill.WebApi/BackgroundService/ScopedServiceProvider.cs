using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using SawMill.Processor.Services.Interface;
using Timer = System.Timers.Timer;

namespace SawMill.WebApi.BackgroundService
{
  public class ScopedServiceProvider : IHostedService, IDisposable
  {
    private readonly IServiceProvider _serviceProvider;
    private Timer _timer;


    public ScopedServiceProvider( IServiceProvider serviceProvider)
    {
      _serviceProvider = serviceProvider;
    }

    public void Dispose()
    {
      _timer?.Dispose();
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
      await ScheduleJob(cancellationToken);
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
      _timer?.Stop();
      await Task.CompletedTask;
    }

    public async Task ScheduleJob(CancellationToken cancellationToken)
    {
      var frequency = await GetFrequency();
      _timer = new Timer(frequency.TotalMilliseconds);
      _timer.Elapsed += async (sender, args) =>
      {
        _timer.Stop(); // reset timer
        await DoWork(cancellationToken);
        Console.WriteLine("DoWork finished");
        await ScheduleJob(cancellationToken); // reschedule next
      };
      _timer.Start();

      await Task.CompletedTask;
    }

    private async Task DoWork(CancellationToken cancellationToken)
    {
      using (var scope = _serviceProvider.CreateScope())
      {
        var scopedProcessingService =
          scope.ServiceProvider
            .GetRequiredService<IAnalyzerJobService>();

        await scopedProcessingService.Analyze(cancellationToken);
      }
    }

    private async Task<TimeSpan> GetFrequency()
    {
      using (var scope = _serviceProvider.CreateScope())
      {
        var scopedProcessingService =
          scope.ServiceProvider
            .GetRequiredService<ISettingsService>();
        var settings = await scopedProcessingService.GetSettings();

        return TimeSpan.FromSeconds(settings.Frequency);
      }
    }
  }
}