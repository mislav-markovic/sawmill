using System;
using System.Collections.Generic;
using System.Text;

namespace SawMill.Processor.Model
{
  public class ComponentReportAlert
  {
    public int Id { get; set; }
    public int AlertId { get; set; }
    public int Count { get; set; }
    public int ComponentReportId { get; set; }
  }
}
