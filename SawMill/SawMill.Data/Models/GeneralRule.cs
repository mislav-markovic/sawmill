using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class GeneralRule
  {
    public GeneralRule()
    {
      DateTimeRule = new HashSet<DateTimeRule>();
      MessageRule = new HashSet<MessageRule>();
      SeverityLevelRule = new HashSet<SeverityLevelRule>();
      Alerts = new List<Alert>();
      CustomAttributeValues = new List<CustomAttributeValue>();
      CustomAttributeRuleParsingRules = new List<CustomAttributeRuleParsingRules>();
    }

    public int GeneralRuleId { get; set; }
    public string GeneralRuleName { get; set; }
    public string GeneralRuleDescription { get; set; }
    public string GeneralRuleMatcher { get; set; }
    public string GeneralRuleStartAnchor { get; set; }
    public string GeneralRuleEndAnchor { get; set; }

    public virtual ICollection<DateTimeRule> DateTimeRule { get; set; }
    public virtual ICollection<MessageRule> MessageRule { get; set; }
    public virtual ICollection<SeverityLevelRule> SeverityLevelRule { get; set; }
    public virtual ICollection<Alert> Alerts { get; set; }
    public virtual ICollection<CustomAttributeValue> CustomAttributeValues { get; set; }
    public virtual ICollection<CustomAttributeRuleParsingRules> CustomAttributeRuleParsingRules { get; set; }
  }
}