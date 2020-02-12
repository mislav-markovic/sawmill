namespace SawMill.WebApi.ViewModel.MessageRule
{
  public class MessageRuleViewModel
  {
    public MessageRuleViewModel(int id, string name, string description, string matcher, string startAnchor,
      string endAnchor, int? maxLength, int generalRuleId)
    {
      Id = id;
      Name = name;
      Description = description;
      Matcher = matcher;
      StartAnchor = startAnchor;
      EndAnchor = endAnchor;
      MaxLength = maxLength;
      GeneralRuleId = generalRuleId;
    }

    public int Id { get; }
    public int GeneralRuleId { get; }
    public string Name { get; }
    public string Description { get; }
    public string Matcher { get; }
    public string StartAnchor { get; }
    public string EndAnchor { get; }
    public int? MaxLength { get; }
  }
}