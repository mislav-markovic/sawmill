using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using RawLog = SawMill.Processor.Model.RawLog;

namespace SawMill.Data.Repository
{
  public class RawLogRepository : IRawLogRepository
  {
    private readonly SawMillDbContext _db;

    public RawLogRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(RawLog model)
    {
      var dto = model.ToDomainModel();
      var entity = _db.RawLog.Add(dto);
      await _db.SaveChangesAsync();

      return entity.Entity.RawLogId;
    }

    public async Task<RawLog> Read(int id)
    {
      return (await _db.RawLog.FindAsync(id)).ToBusinessModel();
    }

    public async Task<IEnumerable<RawLog>> ReadAll()
    {
      return await _db.RawLog.AsNoTracking().Select(elem => elem.ToBusinessModel()).ToListAsync();
    }

    public async Task<bool> Update(RawLog model)
    {
      var dto = model.ToDomainModel();
      _db.RawLog.Update(dto);
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var dto = await _db.RawLog.FindAsync(id);

      if (dto != null)
      {
        _db.RawLog.Remove(dto);
        await _db.SaveChangesAsync();
        return true;
      }

      throw new ArgumentOutOfRangeException();
    }

    public async Task<IEnumerable<RawLog>> WithinTimeFrame(int componentId, DateTime start, DateTime end)
    {
      var filtered = _db.NormalizedLog.Include(log => log.RawLog).Where(log => log.ComponentId == componentId)
        .Where(log => log.Timestamp >= start && log.Timestamp <= end);
      var transformed = filtered.Select(l => l.RawLog.ToBusinessModel());
      return await transformed.ToListAsync();
    }
  }
}