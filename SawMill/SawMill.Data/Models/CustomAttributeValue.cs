namespace SawMill.Data.Models
{
  public class CustomAttributeValue
  {
    public int CustomAttributeValueId { get; set; }
    public string Value { get; set; }
    public int NormalizedLogId { get; set; }
    public int GeneralRuleId { get; set; }

    public virtual GeneralRule GeneralRule { get; set; }
    public virtual NormalizedLog NormalizedLog { get; set; }
  }
}