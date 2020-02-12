using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface ICustomAttributeRuleService : ICrudServiceAsync<GeneralRule>
  {
    Task<int> GetGeneralRuleId(int id);
  }
}