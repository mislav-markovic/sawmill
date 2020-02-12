using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class SeverityLevelRule
  {
    public SeverityLevelRule()
    {
      ParsingRules = new HashSet<ParsingRules>();
    }

    public int SeverityLevelRuleId { get; set; }
    public int GeneralRuleId { get; set; }
    public string Trace { get; set; }
    public string Debug { get; set; }
    public string Info { get; set; }
    public string Warning { get; set; }
    public string Error { get; set; }
    public string Fatal { get; set; }

    public virtual GeneralRule GeneralRule { get; set; }
    public virtual ICollection<ParsingRules> ParsingRules { get; set; }
  }
}