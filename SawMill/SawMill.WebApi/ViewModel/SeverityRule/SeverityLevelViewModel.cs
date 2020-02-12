using SawMill.Processor.Model;

namespace SawMill.WebApi.ViewModel.SeverityRule
{
  public class SeverityLevelViewModel
  {
    public SeverityLevelViewModel(SeverityLevel level, string display)
    {
      Level = level;
      Display = display;
    }

    public SeverityLevel Level { get; }
    public string Display { get; }
  }
}