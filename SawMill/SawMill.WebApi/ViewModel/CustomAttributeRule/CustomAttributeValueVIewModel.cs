namespace SawMill.WebApi.ViewModel.CustomAttributeRule
{
  public class CustomAttributeValueViewModel
  {
    public CustomAttributeValueViewModel(int id, string value, int customAttributeRuleId,
      string customAttributeRuleName)
    {
      Id = id;
      Value = value;
      CustomAttributeRuleId = customAttributeRuleId;
      CustomAttributeRuleName = customAttributeRuleName;
    }

    public int Id { get; }
    public string Value { get; }
    public string CustomAttributeRuleName { get; }
    public int CustomAttributeRuleId { get; }
  }
}