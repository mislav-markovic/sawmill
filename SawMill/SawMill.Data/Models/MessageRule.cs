using System.Collections.Generic;

namespace SawMill.Data.Models
{
  public class MessageRule
  {
    public MessageRule()
    {
      ParsingRules = new HashSet<ParsingRules>();
    }

    public int MessageRuleId { get; set; }
    public int? MaxLength { get; set; }
    public int GeneralRuleId { get; set; }

    public virtual GeneralRule GeneralRule { get; set; }
    public virtual ICollection<ParsingRules> ParsingRules { get; set; }
  }
}