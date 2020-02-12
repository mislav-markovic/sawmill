using System.Collections.Generic;

namespace SawMill.Processor.Model
{
  public class ParsingRules
  {
    public ParsingRules()
    {
      CustomAttributeRules = new HashSet<GeneralRule>();
      Components = new List<Component>();
    }

    public int Id { get; set; }
    public DateTimeRule TimestampParsingRule { get; set; }
    public MessageRule MessageParsingRule { get; set; }
    public SeverityRule SeverityParsingRule { get; set; }
    public IList<Component> Components { get; set; }
    public ISet<GeneralRule> CustomAttributeRules { get; set; }

    public int DateTimeRuleId { get; set; }
    public int MessageRuleId { get; set; }
    public int SeverityRuleId { get; set; }
  }
}