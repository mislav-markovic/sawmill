using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.RepositoryInterfaces
{
  public interface ISettingsRepository
  {
    Task<Settings> GetAnalyzerFrequencySettings();
    Task UpdateAnalyzerFrequencySettings(Settings model);
  }
}