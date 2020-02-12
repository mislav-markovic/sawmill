using System.Threading.Tasks;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using Settings = SawMill.Processor.Model.Settings;

namespace SawMill.Data.Repository
{
  public class SettingsRepository : ISettingsRepository
  {
    private readonly SawMillDbContext _db;

    public SettingsRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<Settings> GetAnalyzerFrequencySettings()
    {
      var dto = await _db.Settings.FindAsync(1);

      return dto.ToBusinessModel();
    }

    public async Task UpdateAnalyzerFrequencySettings(Settings model)
    {
      var dto = model.ToDomainModel();
      _db.Settings.Update(dto);
      await _db.SaveChangesAsync();
    }
  }
}