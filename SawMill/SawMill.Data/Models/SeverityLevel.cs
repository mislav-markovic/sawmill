using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class SeverityLevel
  {
    public SeverityLevel()
    {
      NormalizedLogs = new List<NormalizedLog>();
    }

    public int SeverityLevelId { get; set; }
    public string SeverityLevelValue { get; set; }
    public virtual ICollection<NormalizedLog> NormalizedLogs { get; set; }
  }
}