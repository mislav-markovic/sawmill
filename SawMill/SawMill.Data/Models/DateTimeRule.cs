using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class DateTimeRule
  {
    public DateTimeRule()
    {
      ParsingRules = new HashSet<ParsingRules>();
    }

    public int DateTimeRuleId { get; set; }
    public int GeneralRuleId { get; set; }
    public string DateTimeFormat { get; set; }

    public virtual GeneralRule GeneralRule { get; set; }
    public virtual ICollection<ParsingRules> ParsingRules { get; set; }
  }
}