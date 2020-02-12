namespace SawMill.WebApi.ViewModel.CustomAttributeRule
{
  public class CustomAttributeRuleViewModel
  {
    public CustomAttributeRuleViewModel(int id, string name, string description, string matcher, string startAnchor,
      string endAnchor)
    {
      Id = id;
      Name = name;
      Description = description;
      Matcher = matcher;
      StartAnchor = startAnchor;
      EndAnchor = endAnchor;
    }

    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string Matcher { get; }
    public string StartAnchor { get; }
    public string EndAnchor { get; }
  }
}