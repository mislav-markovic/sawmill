using System;
using System.Collections.Generic;

namespace SawMill.Processor.Model
{
  public class AlertGroup
  {
    public AlertGroup()
    {
      Alerts = new List<Alert>();
      AlertGroupValues = new List<AlertGroupValue>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }

    public TimeSpan? CorrelationWindow { get; set; }
    public int SystemId { get; set; }
    public ICollection<Alert> Alerts { get; set; }
    public ICollection<AlertGroupValue> AlertGroupValues { get; set; }
  }
}