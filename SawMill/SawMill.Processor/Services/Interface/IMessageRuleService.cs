using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface IMessageRuleService : ICrudServiceAsync<MessageRule>
  {
    Task<int> GetGeneralRuleId(int id);
  }
}