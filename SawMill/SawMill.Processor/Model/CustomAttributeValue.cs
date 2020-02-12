namespace SawMill.Processor.Model
{
  public class CustomAttributeValue
  {
    public int Id { get; set; }
    public string Value { get; set; }
    public int CustomAttributeRuleId { get; set; }
    public GeneralRule GeneralRule { get; set; }
    public int NormalizedLogId { get; set; }
    public NormalizedLog NormalizedLog { get; set; }
  }
}