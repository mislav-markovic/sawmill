using System;
using System.Collections.Generic;

namespace SawMill.Processor.Model
{
  public class AlertGroupValue
  {
    public int Id { get; set; }
    public int AlertGroupId { get; set; }
    public virtual AlertGroup AlertGroup { get; set; }
    public DateTime TimespanStart { get; set; }
    public DateTime TimespanEnd { get; set; }
    public ICollection<AlertValue> AlertValues { get; set; }
  }
}