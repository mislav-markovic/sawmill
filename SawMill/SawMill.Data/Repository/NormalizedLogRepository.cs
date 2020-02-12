using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using NormalizedLog = SawMill.Processor.Model.NormalizedLog;

namespace SawMill.Data.Repository
{
  public class NormalizedLogRepository : INormalizedLogRepository
  {
    private readonly SawMillDbContext _db;

    public NormalizedLogRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(NormalizedLog model)
    {
      var dto = model.ToDomainModel();
      dto.CustomAttributeValues = new List<CustomAttributeValue>();

      var entity = await _db.NormalizedLog.AddAsync(dto);
      await _db.SaveChangesAsync();
      entity.State = EntityState.Detached;

      var valueDtos = new List<CustomAttributeValue>();
      foreach (var customAttributeValue in model.CustomAttributeValues)
      {
        var valueDto = customAttributeValue.ToDomainModel();
        valueDto.NormalizedLogId = entity.Entity.NormalizedLogId;
        valueDtos.Add(valueDto);
      }

      await _db.CustomAttributeValue.AddRangeAsync(valueDtos);
      await _db.SaveChangesAsync();

      return entity.Entity.NormalizedLogId;
    }

    public async Task<NormalizedLog> Read(int id)
    {
      var dto = await _db.NormalizedLog.Include(log => log.CustomAttributeValues).Include(log => log.SeverityLevel)
        .FirstAsync(elem => elem.NormalizedLogId == id);

      return dto.ToBusinessModel();
    }

    public async Task<IEnumerable<NormalizedLog>> ReadAll()
    {
      var all = await _db.NormalizedLog.Include(log => log.CustomAttributeValues).Include(log => log.SeverityLevel)
        .OrderBy(log => log.Timestamp).ToListAsync();
      var result = new List<NormalizedLog>();

      foreach (var normalizedLogDto in all) result.Add(normalizedLogDto.ToBusinessModel());

      return result;
    }

    public async Task<bool> Update(NormalizedLog model)
    {
      var dto = model.ToDomainModel();
      _db.NormalizedLog.Update(dto);
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var dto = await _db.NormalizedLog.FindAsync(id);

      if (dto != null)
      {
        _db.NormalizedLog.Remove(dto);
        await _db.SaveChangesAsync();
        return true;
      }

      throw new ArgumentOutOfRangeException();
    }

    public async Task<IEnumerable<NormalizedLog>> WithinTimeFrame(int componentId, DateTime start, DateTime end)
    {
      var filtered = _db.NormalizedLog.Include(log => log.SeverityLevel).Include(log => log.CustomAttributeValues)
        .OrderBy(log => log.Timestamp)
        .Where(log => log.ComponentId == componentId)
        .Where(log => log.Timestamp >= start && log.Timestamp <= end);
      var transformed = filtered.Select(l => l.ToBusinessModel());
      return await transformed.ToListAsync();
    }

    public async Task<IEnumerable<NormalizedLog>> WithinTimeFrameForComponents(int[] componentIds, DateTime start,
      DateTime end, int? howMany)
    {
      var filtered = await _db.NormalizedLog.Include(log => log.SeverityLevel).Include(log => log.CustomAttributeValues)
        .Where(log => componentIds.Contains(log.ComponentId))
        .Where(log => log.Timestamp >= start && log.Timestamp <= end).ToListAsync();


      var ordered = howMany.HasValue
        ? filtered.Take(howMany.Value).OrderBy(log => log.Timestamp)
        : filtered.OrderBy(log => log.Timestamp);

      var transformed = ordered.Select(elem => elem.ToBusinessModel());
      return transformed;
      // return await transformed.ToListAsync();
    }

    public async Task<IEnumerable<NormalizedLog>> PrecedingLogs(NormalizedLog log, int[] componentIds, int? howMany)
    {
      var filtered =
        _db.NormalizedLog
          .Where(elem => componentIds.Contains(elem.ComponentId))
          .Where(elem => elem.Timestamp < log.DateTime)
          .OrderByDescending(elem => elem.Timestamp);

      var taken = howMany.HasValue ? filtered.Take(howMany.Value) : filtered;

      var transformed = await taken.OrderBy(elem => elem.Timestamp)
        .Include(l => l.CustomAttributeValues).ToListAsync();
      var result = transformed.Select(elem => elem.ToBusinessModel());
      return result;
    }
  }
}