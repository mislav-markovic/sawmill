namespace SawMill.WebApi.ViewModel.SeverityRule
{
  public class ParseRequestSeverityViewModel
  {
    public ParseRequestSeverityViewModel(SeverityRuleViewModel ruleViewModel, string line)
    {
      RuleViewModel = ruleViewModel;
      Line = line;
    }

    public string Line { get; }
    public SeverityRuleViewModel RuleViewModel { get; }
  }
}