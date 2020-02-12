using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SawMill.WebApi.ViewModel.Reports
{
  public class SystemReportViewModel
  {
    public SystemReportViewModel(int id, int systemId, Dictionary<int, int> alertGroupCount, ComponentReportViewModel[] componentReports, DateTime timestamp)
    {
      Id = id;
      SystemId = systemId;
      AlertGroupCount = alertGroupCount;
      ComponentReports = componentReports;
      Timestamp = timestamp;
    }

    public int Id { get; }
    public int SystemId { get; }
    public Dictionary<int, int> AlertGroupCount { get; }
    public ComponentReportViewModel[] ComponentReports { get; }
    public DateTime Timestamp { get; }
  }
}
