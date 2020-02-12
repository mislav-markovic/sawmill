using System.Threading;
using System.Threading.Tasks;

namespace SawMill.WebApi.BackgroundService
{
  public interface IAnalyzerJobService
  {
    Task Analyze(CancellationToken cancellationToken);
  }
}