using System;
using System.Collections.Generic;
using System.Text;

namespace SawMill.Processor.Model
{
  public class ComponentReport
  {
    public int Id { get; set; }
    public int ComponentId { get; set; }
    public int SystemReportId { get; set; }
    public DateTime Timestamp { get; set; }
    public IEnumerable<ComponentReportAlert> ComponentReportAlert { get; set; }
  }
}
