namespace SawMill.WebApi.ViewModel.DateTimeRule
{
  public class ParseRequestDateTimeRuleViewModel
  {
    public ParseRequestDateTimeRuleViewModel(string line, DateTimeRuleViewModel ruleViewModel)
    {
      Line = line;
      RuleViewModel = ruleViewModel;
    }

    public string Line { get; }
    public DateTimeRuleViewModel RuleViewModel { get; }
  }
}