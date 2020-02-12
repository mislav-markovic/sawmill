using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class RawLogService : IRawLogService
  {
    private readonly IRawLogRepository _rawLogRepository;

    public RawLogService(IRawLogRepository rawLogRepository)
    {
      _rawLogRepository = rawLogRepository;
    }

    public async Task<RawLog> Create(RawLog model)
    {
      var createdId = await _rawLogRepository.Create(model);
      return await _rawLogRepository.Read(createdId);
    }

    public async Task<RawLog> Edit(RawLog model)
    {
      var isUpdated = await _rawLogRepository.Update(model);
      if (isUpdated)
      {
        return await _rawLogRepository.Read(model.Id);
      }

      throw new Exception($"Failed to update raw log {model.Id}");
    }

    public async Task<IEnumerable<RawLog>> GetAll()
    {
      return await _rawLogRepository.ReadAll();
    }

    public async Task<RawLog> Get(int id)
    {
      var result = await _rawLogRepository.Read(id);
      if (result == null)
      {
        throw new ArgumentException($"Cannot read raw log {id}", nameof(id));
      }

      return result;
    }

    public async Task Delete(int id)
    {
      var result = await _rawLogRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete raw log {id}", nameof(id));
      }
    }

  }
}