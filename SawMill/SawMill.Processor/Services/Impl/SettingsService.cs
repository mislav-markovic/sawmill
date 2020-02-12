using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class SettingsService : ISettingsService
  {
    private readonly ISettingsRepository _settingsRepository;

    public SettingsService(ISettingsRepository settingsRepository)
    {
      _settingsRepository = settingsRepository;
    }

    public async Task<Settings> GetSettings()
    {
      return await _settingsRepository.GetAnalyzerFrequencySettings();
    }

    public async Task UpdateSettings(Settings model)
    {
      await _settingsRepository.UpdateAnalyzerFrequencySettings(model);
    }
  }
}