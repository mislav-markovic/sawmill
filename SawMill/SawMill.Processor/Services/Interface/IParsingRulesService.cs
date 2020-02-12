using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface IParsingRulesService : ICrudServiceAsync<ParsingRules>
  {
    Task<ParsingRules> GetComponentsParsingRules(int componentId);
  }
}