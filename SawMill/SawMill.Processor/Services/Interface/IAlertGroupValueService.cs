using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface IAlertGroupValueService : ICrudServiceAsync<AlertGroupValue>
  {
    Task<IEnumerable<AlertGroupValue>> CorrelateAlertsForSystem(int systemId);
    Task<IEnumerable<AlertGroupValue>> AlertGroupValuesForAlertGroup(int alertGroupId);
  }
}