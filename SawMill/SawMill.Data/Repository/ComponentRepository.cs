using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using Component = SawMill.Processor.Model.Component;

namespace SawMill.Data.Repository
{
  public class ComponentRepository : IComponentRepository
  {
    private readonly SawMillDbContext _db;

    public ComponentRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(Component model)
    {
      var dto = model.ToDomainModel();
      _db.Component.Add(dto);
      await _db.SaveChangesAsync();

      return dto.ComponentId;
    }

    public async Task<Component> Read(int id)
    {
      return (await _db.Component.FindAsync(id)).ToBusinessModel();
    }

    public async Task<IEnumerable<Component>> ReadAll()
    {
      return await _db.Component.AsNoTracking().Select(elem => elem.ToBusinessModel()).ToListAsync();
    }

    public async Task<bool> Update(Component model)
    {
      var dto = model.ToDomainModel();
      _db.Component.Update(dto);
      await _db.SaveChangesAsync();
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var dto = await _db.Component.FindAsync(id);

      if (dto != null)
      {
        _db.Component.Remove(dto);
        await _db.SaveChangesAsync();
        return true;
      }

      throw new ArgumentOutOfRangeException();
    }
  }
}