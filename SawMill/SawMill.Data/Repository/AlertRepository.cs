using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using Alert = SawMill.Processor.Model.Alert;

namespace SawMill.Data.Repository
{
  public class AlertRepository : IAlertRepository
  {
    private readonly SawMillDbContext _db;

    public AlertRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(Alert model)
    {
      var dto = model.ToDomainModel();
      _db.Alert.Add(dto);
      await _db.SaveChangesAsync();

      return dto.AlertId;
    }

    public async Task<Alert> Read(int id)
    {
      return (await _db.Alert.Include(a => a.GeneralRule).FirstAsync(elem => elem.AlertId == id))
        .ToBusinessModel();
    }

    public async Task<IEnumerable<Alert>> ReadAll()
    {
      return await _db.Alert.Include(a => a.GeneralRule).Select(elem => elem.ToBusinessModel()).ToListAsync();
    }

    public async Task<bool> Update(Alert model)
    {
      var dto = model.ToDomainModel();
      _db.Alert.Update(dto);
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var dto = await _db.Alert.FindAsync(id);

      if (dto != null)
      {
        _db.Alert.Remove(dto);
        await _db.SaveChangesAsync();
        return true;
      }

      throw new ArgumentOutOfRangeException();
    }
  }
}