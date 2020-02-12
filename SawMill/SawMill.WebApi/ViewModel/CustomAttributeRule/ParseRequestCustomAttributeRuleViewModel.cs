namespace SawMill.WebApi.ViewModel.CustomAttributeRule
{
  public class ParseRequestCustomAttributeRuleViewModel
  {
    public ParseRequestCustomAttributeRuleViewModel(string line, CustomAttributeRuleViewModel ruleViewModel)
    {
      Line = line;
      RuleViewModel = ruleViewModel;
    }

    public string Line { get; }
    public CustomAttributeRuleViewModel RuleViewModel { get; }
  }
}