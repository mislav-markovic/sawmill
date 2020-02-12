namespace SawMill.WebApi.ViewModel.SeverityRule
{
  public class SeverityRuleViewModel
  {
    public SeverityRuleViewModel(int id, string name, string description, string matcher, string startAnchor,
      string endAnchor, string trace, string debug, string info, string warning, string error, string fatal)
    {
      Id = id;
      Name = name;
      Description = description;
      Matcher = matcher;
      StartAnchor = startAnchor;
      EndAnchor = endAnchor;
      Trace = trace;
      Debug = debug;
      Info = info;
      Warning = warning;
      Error = error;
      Fatal = fatal;
    }

    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public string Matcher { get; }
    public string StartAnchor { get; }
    public string EndAnchor { get; }
    public string Trace { get; }
    public string Debug { get; }
    public string Info { get; }
    public string Warning { get; }
    public string Error { get; }
    public string Fatal { get; }
  }
}