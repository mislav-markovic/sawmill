using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace SawMill.Processor.Model
{
  public class GeneralRule
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public Regex Matcher { get; set; }
    public string StartAnchor { get; set; }
    public string EndAnchor { get; set; }
    public MatchType MatchType { get; set; }

    public virtual string Parse(string line)
    {
      var workingString = new string(line);
      if (!string.IsNullOrEmpty(StartAnchor))
      {
        var startingAnchorIndex = line.IndexOf(StartAnchor, StringComparison.Ordinal);
        workingString = workingString.Substring(startingAnchorIndex);
      }

      if (!string.IsNullOrEmpty(EndAnchor))
      {
        var endAnchorIndex = line.LastIndexOf(EndAnchor, StringComparison.Ordinal);
        workingString = workingString.Substring(0, endAnchorIndex);
      }

      var match = Matcher.Matches(workingString);
      if (match.Count == 0)
      {
        return string.Empty;
      }

      switch (MatchType)
      {
        case MatchType.First:
          return match.First().Value;
        case MatchType.Last:
          return match.Last().Value;
        case MatchType.All:
          return string.Join(' ', match.Select(m => m.Value));
        default:
          throw new ArgumentOutOfRangeException();
      }
    }
  }

  public enum MatchType
  {
    First,
    Last,
    All
  }
}