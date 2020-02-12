using System;
using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class AlertGroupValue
  {
    public AlertGroupValue()
    {
      AlertValues = new List<AlertValue>();
    }

    public int AlertGroupValueId { get; set; }
    public int AlertGroupId { get; set; }
    public virtual AlertGroup AlertGroup { get; set; }
    public DateTime TimespanStart { get; set; }
    public DateTime TimespanEnd { get; set; }
    public virtual ICollection<AlertValue> AlertValues { get; set; }
  }
}