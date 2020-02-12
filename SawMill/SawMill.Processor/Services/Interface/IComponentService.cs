using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface IComponentService : ICrudServiceAsync<Component>
  {
    Task<IEnumerable<Component>> ComponentsForSystem(int systemId);
    Task<Component> ComponentByName(string componentName);
  }
}