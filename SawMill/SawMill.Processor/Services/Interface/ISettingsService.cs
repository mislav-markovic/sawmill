using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface ISettingsService
  {
    Task<Settings> GetSettings();
    Task UpdateSettings(Settings model);
  }
}