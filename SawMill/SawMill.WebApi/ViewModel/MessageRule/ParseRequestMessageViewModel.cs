namespace SawMill.WebApi.ViewModel.MessageRule
{
  public class ParseRequestMessageViewModel
  {
    public ParseRequestMessageViewModel(MessageRuleViewModel ruleViewModel, string line)
    {
      RuleViewModel = ruleViewModel;
      Line = line;
    }

    public string Line { get; }
    public MessageRuleViewModel RuleViewModel { get; }
  }
}