using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class ComponentService : IComponentService
  {
    private readonly IComponentRepository _componentRepository;

    public ComponentService(IComponentRepository componentRepository)
    {
      _componentRepository = componentRepository;
    }

    public async Task<Component> Create(Component model)
    {
      var createdId = await _componentRepository.Create(model);
      return await _componentRepository.Read(createdId);
    }

    public async Task<Component> Edit(Component model)
    {
      var isUpdated = await _componentRepository.Update(model);
      if (isUpdated)
      {
        return await _componentRepository.Read(model.Id);
      }

      throw new Exception($"Failed to update component {model.Id}");
    }

    public async Task<IEnumerable<Component>> GetAll()
    {
      return await _componentRepository.ReadAll();
    }

    public async Task<Component> Get(int id)
    {
      var result = await _componentRepository.Read(id);
      if (result == null)
      {
        throw new ArgumentException($"Cannot read component {id}", nameof(id));
      }

      return result;
    }

    public async Task Delete(int id)
    {
      var result = await _componentRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete component {id}", nameof(id));
      }
    }

    public async Task<IEnumerable<Component>> ComponentsForSystem(int systemId)
    {
      return (await _componentRepository.ReadAll()).Where(comp =>
        comp.SystemId.HasValue && comp.SystemId.Value.Equals(systemId));
    }

    public async Task<Component> ComponentByName(string componentName)
    {
      var all = await _componentRepository.ReadAll();

      var result = all.FirstOrDefault(elem => elem.Name.Equals(componentName));

      if (result == null)
      {
        throw new ArgumentException($"Component with name {componentName} does not exists", nameof(componentName));
      }

      return result;
    }
  }
}