using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class RawLog
  {
    public RawLog()
    {
      NormalizedLog = new HashSet<NormalizedLog>();
    }

    public int RawLogId { get; set; }
    public string Message { get; set; }
    public int ComponentId { get; set; }

    public virtual Component Component { get; set; }
    public virtual ICollection<NormalizedLog> NormalizedLog { get; set; }
  }
}