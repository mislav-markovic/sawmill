using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class ParsingRules
  {
    public ParsingRules()
    {
      CustomAttributeRuleParsingRules = new HashSet<CustomAttributeRuleParsingRules>();
      Components = new List<Component>();
    }

    public int ParsingRulesId { get; set; }
    public int DateTimeRuleId { get; set; }
    public int MessageRuleId { get; set; }
    public int SeverityLevelRuleId { get; set; }

    public virtual DateTimeRule DateTimeRule { get; set; }
    public virtual MessageRule MessageRule { get; set; }
    public virtual SeverityLevelRule SeverityLevelRule { get; set; }
    public virtual ICollection<Component> Components { get; set; }
    public virtual ICollection<CustomAttributeRuleParsingRules> CustomAttributeRuleParsingRules { get; set; }
  }
}