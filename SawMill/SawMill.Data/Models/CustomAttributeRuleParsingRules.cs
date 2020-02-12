namespace SawMill.Data.Models
{
  public class CustomAttributeRuleParsingRules
  {
    public int ParsingRulesId { get; set; }
    public int GeneralRuleId { get; set; }

    public virtual ParsingRules ParsingRules { get; set; }
    public virtual GeneralRule GeneralRule { get; set; }
  }
}