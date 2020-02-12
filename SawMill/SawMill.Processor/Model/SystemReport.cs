using System;
using System.Collections.Generic;
using System.Text;

namespace SawMill.Processor.Model
{
  public class SystemReport
  {
    public int Id { get; set; }
    public int SystemId { get; set; }
    public DateTime Timestamp { get; set; }
    public IEnumerable<SystemReportAlertGroup> SystemReportAlertGroup { get; set; }
    public IEnumerable<ComponentReport> ComponentReports { get; set; }
  }
}
