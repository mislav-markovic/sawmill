using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface INormalizedLogService : ICrudServiceAsync<NormalizedLog>
  {
    Task<IEnumerable<NormalizedLog>> ComponentsLogWithinTimeFrame(int componentId, DateTime start, DateTime end);
    Task<IEnumerable<NormalizedLog>> ComponentsLog(int componentId);
    Task<IEnumerable<NormalizedLog>> SystemLogs(int systemId, DateTime start, DateTime end);
    Task<IEnumerable<NormalizedLog>> SystemLogs(int systemId);
    Task<IEnumerable<NormalizedLog>> SystemLogs(int systemId, DateTime start, DateTime end, int howMany);
    Task<IEnumerable<NormalizedLog>> SystemLogsPaginated(int systemId, int lastNormalizedLogId, int howMany);
  }
}