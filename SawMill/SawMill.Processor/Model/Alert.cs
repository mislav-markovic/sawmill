using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace SawMill.Processor.Model
{
  public class Alert
  {
    public int Id { get; set; }
    public TimeSpan Timespan { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int Threshold { get; set; }
    public int? Position { get; set; }
    public bool HasConstantValue { get; set; }
    public Regex Value { get; set; }
    public int GeneralRuleId { get; set; }
    public GeneralRule GeneralRule { get; set; }
    public int ComponentId { get; set; }
    public bool Not { get; set; }
    public int? AlertGroupId { get; set; }

    public ICollection<AlertValue> AlertValues { get; set; }
  }
}