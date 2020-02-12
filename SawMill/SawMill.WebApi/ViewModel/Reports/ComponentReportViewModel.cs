using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SawMill.WebApi.ViewModel.Reports
{
  public class ComponentReportViewModel
  {
    public ComponentReportViewModel(int id, int componentId, int systemReportId, Dictionary<int, int> alertCount)
    {
      Id = id;
      ComponentId = componentId;
      SystemReportId = systemReportId;
      AlertCount = alertCount;
    }

    public int Id { get; }
    public int ComponentId { get; }
    public int SystemReportId { get; }
    public Dictionary<int, int> AlertCount { get; }
  }
}
