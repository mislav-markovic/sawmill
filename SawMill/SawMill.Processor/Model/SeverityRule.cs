using System.Text.RegularExpressions;

namespace SawMill.Processor.Model
{
  // Matcher for severity rule should only return value specified in one of defined severity levels
  // Otherwise, if only anchors are provided, match is run between anchors for some value defined in properties, starting at Trace
  public sealed class SeverityRule
  {
    public int Id { get; set; }
    public int GeneralRuleId { get; set; }
    public GeneralRule GeneralRule { get; set; }
    public Regex Trace { get; set; }
    public Regex Debug { get; set; }
    public Regex Info { get; set; }
    public Regex Warning { get; set; }
    public Regex Error { get; set; }
    public Regex Fatal { get; set; }
  }

  // order matters
  public enum SeverityLevel
  {
    Debug = 1,
    Trace = 2,
    Info = 3,
    Warning = 4,
    Error = 5,
    Fatal = 6
  }
}