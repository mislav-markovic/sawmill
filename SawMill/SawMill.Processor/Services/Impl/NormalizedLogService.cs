using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class NormalizedLogService : INormalizedLogService
  {
    private readonly INormalizedLogRepository _normalizedLogRepository;
    private readonly ISystemRepository _systemRepository;

    public NormalizedLogService(INormalizedLogRepository normalizedLogRepository, ISystemRepository systemRepository)
    {
      _normalizedLogRepository = normalizedLogRepository;
      _systemRepository = systemRepository;
    }

    public async Task<NormalizedLog> Create(NormalizedLog model)
    {
      var createdId = await _normalizedLogRepository.Create(model);
      return await _normalizedLogRepository.Read(createdId);
    }

    public async Task<NormalizedLog> Edit(NormalizedLog model)
    {
      var result = await _normalizedLogRepository.Update(model);
      if (result)
      {
        return await _normalizedLogRepository.Read(model.Id);
      }

      throw new ArgumentException();
    }

    public async Task<IEnumerable<NormalizedLog>> GetAll()
    {
      return await _normalizedLogRepository.ReadAll();
    }

    public async Task<NormalizedLog> Get(int id)
    {
      return await _normalizedLogRepository.Read(id);
    }

    public async Task Delete(int id)
    {
      var result = await _normalizedLogRepository.Delete(id);

      if (!result)
      {
        throw new ArgumentOutOfRangeException();
      }
    }

    public async Task<IEnumerable<NormalizedLog>> ComponentsLogWithinTimeFrame(int componentId, DateTime start,
      DateTime end)
    {
      return await _normalizedLogRepository.WithinTimeFrame(componentId, start, end);
    }

    public async Task<IEnumerable<NormalizedLog>> ComponentsLog(int componentId)
    {
      return await ComponentsLogWithinTimeFrame(componentId, DateTime.UnixEpoch, DateTime.MaxValue);
    }

    public async Task<IEnumerable<NormalizedLog>> SystemLogs(int systemId, DateTime start, DateTime end)
    {
      var componentIds = (await _systemRepository.Read(systemId)).Components.Select(c => c.Id).ToArray();
      var logs = await _normalizedLogRepository.WithinTimeFrameForComponents(componentIds, start, end, null);

      return logs;
    }

    public async Task<IEnumerable<NormalizedLog>> SystemLogs(int systemId)
    {
      var componentIds = (await _systemRepository.Read(systemId)).Components.Select(c => c.Id).ToArray();
      var logs = await _normalizedLogRepository.WithinTimeFrameForComponents(componentIds, DateTime.UnixEpoch,
        DateTime.MaxValue, null);

      return logs;
    }

    public async Task<IEnumerable<NormalizedLog>> SystemLogs(int systemId, DateTime start, DateTime end, int howMany)
    {
      var componentIds = (await _systemRepository.Read(systemId)).Components.Select(c => c.Id).ToArray();
      var logs = await _normalizedLogRepository.WithinTimeFrameForComponents(componentIds, start, end, howMany);

      return logs;
    }

    public async Task<IEnumerable<NormalizedLog>> SystemLogsPaginated(int systemId, int lastNormalizedLogId,
      int howMany)
    {
      var all = await SystemLogs(systemId);
      return all.SkipWhile(elem => elem.Id != lastNormalizedLogId).Skip(1).Take(howMany);
    }
  }
}