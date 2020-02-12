using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using AlertGroup = SawMill.Processor.Model.AlertGroup;

namespace SawMill.Data.Repository
{
  public class AlertGroupRepository : IAlertGroupRepository
  {
    private readonly SawMillDbContext _db;

    public AlertGroupRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(AlertGroup model)
    {
      var dto = model.ToDomainModel();
      var alerts = dto.AlertGroupAlert;
      dto.AlertGroupAlert = null;
      var alertGroupEntity = _db.AlertGroup.Add(dto);
      await _db.SaveChangesAsync();

      var createdId = alertGroupEntity.Entity.AlertGroupId;

      _db.AddRange(alerts.Select(e =>
      {
        e.AlertGroupId = createdId;
        return e;
      }));
      await _db.SaveChangesAsync();

      return createdId;
    }

    public async Task<AlertGroup> Read(int id)
    {
      return (await _db.AlertGroup
          .Include(a => a.AlertGroupValues)
          .Include(a => a.AlertGroupAlert)
          .ThenInclude(aga => aga.Alert)
          .FirstAsync(elem => elem.AlertGroupId == id))
        .ToBusinessModel();
    }

    public async Task<IEnumerable<AlertGroup>> ReadAll()
    {
      return await _db.AlertGroup
        .Include(a => a.AlertGroupValues)
        .Include(a => a.AlertGroupAlert)
        .ThenInclude(aga => aga.Alert)
        .Select(elem => elem.ToBusinessModel())
        .ToListAsync();
    }

    public async Task<bool> Update(AlertGroup model)
    {
      var dto = model.ToDomainModel();
      _db.AlertGroup.Update(dto);
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var dto = await _db.AlertGroup.FindAsync(id);

      if (dto != null)
      {
        _db.AlertGroup.Remove(dto);
        await _db.SaveChangesAsync();
        return true;
      }

      throw new ArgumentOutOfRangeException();
    }
  }
}