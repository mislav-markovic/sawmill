namespace SawMill.WebApi.ViewModel.DateTimeRule
{
  public class DateTimeRuleViewModel
  {
    public DateTimeRuleViewModel(int id, string name, string description, string matcher, string startAnchor,
      string endAnchor, string dateFormat)
    {
      Id = id;
      Name = name;
      Description = description;
      Matcher = matcher;
      StartAnchor = startAnchor;
      EndAnchor = endAnchor;
      DateFormat = dateFormat;
    }

    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string Matcher { get; }
    public string StartAnchor { get; }
    public string EndAnchor { get; }
    public string DateFormat { get; }
  }
}