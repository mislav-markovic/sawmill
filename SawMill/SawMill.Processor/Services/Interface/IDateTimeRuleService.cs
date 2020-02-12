using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface IDateTimeRuleService : ICrudServiceAsync<DateTimeRule>
  {
    Task<int> GetGeneralRuleId(int id);
  }
}