using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using AlertValue = SawMill.Processor.Model.AlertValue;

namespace SawMill.Data.Repository
{
  public class AlertValueRepository : IAlertValueRepository
  {
    private readonly SawMillDbContext _db;

    public AlertValueRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(AlertValue model)
    {
      var dto = model.ToDomainModel();
      _db.AlertValue.Add(dto);
      await _db.SaveChangesAsync();

      return dto.AlertValueId;
    }

    public async Task<AlertValue> Read(int id)
    {
      return (await _db.AlertValue.FindAsync(id)).ToBusinessModel();
    }

    public async Task<IEnumerable<AlertValue>> ReadAll()
    {
      return await _db.AlertValue.AsNoTracking().Select(elem => elem.ToBusinessModel()).ToListAsync();
    }

    public async Task<bool> Update(AlertValue model)
    {
      var dto = model.ToDomainModel();
      _db.AlertValue.Update(dto);
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var dto = await _db.AlertValue.FindAsync(id);

      if (dto != null)
      {
        _db.AlertValue.Remove(dto);
        await _db.SaveChangesAsync();
        return true;
      }

      throw new ArgumentOutOfRangeException();
    }
  }
}