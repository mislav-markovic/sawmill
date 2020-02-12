namespace SawMill.WebApi.ViewModel.ParsingRules
{
  public class ParsingRulesViewModel
  {
    public ParsingRulesViewModel(int severityRuleId, int messageRuleId, int dateTimeRuleId, int id,
      int[] customAttributeRuleIds, int componentId)
    {
      SeverityRuleId = severityRuleId;
      MessageRuleId = messageRuleId;
      DateTimeRuleId = dateTimeRuleId;
      Id = id;
      ComponentId = componentId;
      CustomAttributeRuleIds = customAttributeRuleIds ?? new int[0];
    }

    public int Id { get; }
    public int ComponentId { get; }
    public int DateTimeRuleId { get; }
    public int MessageRuleId { get; }
    public int SeverityRuleId { get; }
    public int[] CustomAttributeRuleIds { get; }
  }
}