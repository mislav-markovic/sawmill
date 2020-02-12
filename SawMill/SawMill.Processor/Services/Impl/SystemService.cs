using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class SystemService : ISystemService
  {
    private readonly ISystemRepository _systemRepository;

    public SystemService(ISystemRepository systemRepository)
    {
      _systemRepository = systemRepository;
    }

    public async Task<Model.System> Create(Model.System model)
    {
      var createdId = await _systemRepository.Create(model);
      return await _systemRepository.Read(createdId);
    }

    public async Task<Model.System> Edit(Model.System model)
    {
      var result = await _systemRepository.Update(model);
      if (result)
      {
        return await _systemRepository.Read(model.Id);
      }

      throw new ArgumentException();
    }

    public async Task<IEnumerable<Model.System>> GetAll()
    {
      return await _systemRepository.ReadAll();
    }

    public async Task<Model.System> Get(int id)
    {
      return await _systemRepository.Read(id);
    }

    public async Task Delete(int id)
    {
      var result = await _systemRepository.Delete(id);

      if (!result)
      {
        throw new ArgumentOutOfRangeException();
      }
    }
  }
}