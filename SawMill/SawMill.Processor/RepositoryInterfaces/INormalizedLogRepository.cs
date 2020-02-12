using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.RepositoryInterfaces
{
  public interface INormalizedLogRepository : IRepository<NormalizedLog>
  {
    Task<IEnumerable<NormalizedLog>> WithinTimeFrame(int componentId, DateTime start, DateTime end);

    Task<IEnumerable<NormalizedLog>> WithinTimeFrameForComponents(int[] componentIds, DateTime start, DateTime end,
      int? howMany);

    Task<IEnumerable<NormalizedLog>> PrecedingLogs(NormalizedLog log, int[] componentIds, int? howMany);
  }
}