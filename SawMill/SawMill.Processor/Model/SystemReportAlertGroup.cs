using System;
using System.Collections.Generic;
using System.Text;

namespace SawMill.Processor.Model
{
  public class SystemReportAlertGroup
  {
    public int Id { get; set; }
    public int SystemReportId { get; set; }
    public int AlertGroupId { get; set; }
    public int Count { get; set; }
  }
}
