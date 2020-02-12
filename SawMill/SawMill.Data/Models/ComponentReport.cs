using System;
using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class ComponentReport
  {
    public int ComponentReportId { get; set; }
    public int ComponentId { get; set; }
    public int SystemReportId { get; set; }
    public virtual SystemReport SystemReport { get; set; }
    public virtual Component Component { get; set; }
    public DateTime Timestamp { get; set; }
    public virtual ICollection<ComponentReportAlert> ComponentReportAlert { get; set; }
  }
}