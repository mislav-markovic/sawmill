using System;
using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class NormalizedLog
  {
    public NormalizedLog()
    {
      CustomAttributeValues = new HashSet<CustomAttributeValue>();
    }

    public int NormalizedLogId { get; set; }
    public int RawLogId { get; set; }
    public int ComponentId { get; set; }
    public string Message { get; set; }
    public DateTime Timestamp { get; set; }
    public int SeverityLevelId { get; set; }

    public virtual Component Component { get; set; }
    public virtual RawLog RawLog { get; set; }
    public virtual SeverityLevel SeverityLevel { get; set; }
    public virtual ICollection<CustomAttributeValue> CustomAttributeValues { get; set; }
  }
}