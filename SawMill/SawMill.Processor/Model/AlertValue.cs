using System;

namespace SawMill.Processor.Model
{
  public class AlertValue
  {
    public int Id { get; set; }
    public int AlertId { get; set; }
    public int? AlertGroupValueId { get; set; }

    public string ConstantValue { get; set; }

    public DateTime TimespanStart { get; set; }
    public DateTime TimespanEnd { get; set; }
  }
}