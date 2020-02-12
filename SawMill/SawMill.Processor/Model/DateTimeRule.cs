namespace SawMill.Processor.Model
{
  public sealed class DateTimeRule
  {
    public int Id { get; set; }
    public string DateFormat { get; set; }

    public int GeneralRuleId { get; set; }
    public GeneralRule GeneralRule { get; set; }
  }
}