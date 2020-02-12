namespace SawMill.Processor.Model
{
  public sealed class MessageRule
  {
    public int Id { get; set; }
    public int? MaxLength { get; set; }
    public int GeneralRuleId { get; set; }

    public GeneralRule GeneralRule { get; set; }
  }
}