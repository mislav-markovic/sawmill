using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using AlertGroupValue = SawMill.Processor.Model.AlertGroupValue;

namespace SawMill.Data.Repository
{
  public class AlertGroupValueRepository : IAlertGroupValueRepository
  {
    private readonly SawMillDbContext _db;

    public AlertGroupValueRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(AlertGroupValue model)
    {
      var dto = model.ToDomainModel();
      _db.AlertGroupValue.Add(dto);
      await _db.SaveChangesAsync();

      return dto.AlertGroupValueId;
    }

    public async Task<AlertGroupValue> Read(int id)
    {
      return (await _db.AlertGroupValue.FirstAsync(elem => elem.AlertGroupValueId == id))
        .ToBusinessModel();
    }

    public async Task<IEnumerable<AlertGroupValue>> ReadAll()
    {
      return await _db.AlertGroupValue.Include(agv => agv.AlertValues).Select(elem => elem.ToBusinessModel()).ToListAsync();
    }

    public async Task<bool> Update(AlertGroupValue model)
    {
      var dto = model.ToDomainModel();
      _db.AlertGroupValue.Update(dto);
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var dto = await _db.AlertGroupValue.FindAsync(id);

      if (dto != null)
      {
        _db.AlertGroupValue.Remove(dto);
        await _db.SaveChangesAsync();
        return true;
      }

      throw new ArgumentOutOfRangeException();
    }

    public async Task<IEnumerable<AlertGroupValue>> ValuesForAlertGroup(int alertGroupId)
    {
      return await _db.AlertGroupValue.Where(e => e.AlertGroupId == alertGroupId).Select(elem => elem.ToBusinessModel()).ToListAsync();
    }
  }
}