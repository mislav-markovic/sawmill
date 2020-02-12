using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface IAlertValueService : ICrudServiceAsync<AlertValue>
  {
    Task<IEnumerable<AlertValue>> GenerateAlerts(IList<NormalizedLog> logs);

    Task<IEnumerable<AlertValue>> AlertValuesForAlert(int alertId);
  }
}