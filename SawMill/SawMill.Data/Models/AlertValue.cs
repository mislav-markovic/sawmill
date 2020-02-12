using System;

namespace SawMill.Data.Models
{
  public class AlertValue
  {
    public int AlertValueId { get; set; }
    public int AlertId { get; set; }

    public string ConstantValue { get; set; }

    public DateTime TimespanStart { get; set; }
    public DateTime TimespanEnd { get; set; }

    public virtual Alert Alert { get; set; }
    public int? AlertGroupValueId { get; set; }
    public virtual AlertGroupValue AlertGroupValue { get; set; }
  }
}