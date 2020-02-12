using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;

namespace SawMill.Data.Repository
{
  public class SystemRepository : ISystemRepository
  {
    private readonly SawMillDbContext _db;

    public SystemRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(Processor.Model.System model)
    {
      var dto = model.ToDomainModel();
      dto.Component = null;
      var entity = _db.System.Add(dto);
      await _db.SaveChangesAsync();

      foreach (var modelComponent in model.Components)
      {
        var dtoComponent = modelComponent.ToDomainModel();
        var exists = await _db.Component.AnyAsync();
        dtoComponent.SystemId = entity.Entity.SystemId;

        if (exists)
        {
          _db.Component.Update(dtoComponent);
        }
        else
        {
          await _db.Component.AddAsync(dtoComponent);
        }

        await _db.SaveChangesAsync();
      }

      entity.State = EntityState.Detached;
      return entity.Entity.SystemId;
    }

    public async Task<Processor.Model.System> Read(int id)
    {
      try
      {
        return (await ReadDtoSystem(_db, id)).ToBusinessModel();
      }
      catch (InvalidOperationException)
      {
        throw new ArgumentOutOfRangeException();
      }
    }

    public async Task<IEnumerable<Processor.Model.System>> ReadAll()
    {
      try
      {
        return await _db.System
          .Include(s => s.Component)
          .AsNoTracking()
          .Select(elem => elem.ToBusinessModel())
          .ToListAsync();
      }
      catch
      {
        return new List<Processor.Model.System>();
      }
    }

    public async Task<bool> Update(Processor.Model.System model)
    {
      var dto = model.ToDomainModel();
      var original = await ReadDtoSystem(_db, model.Id);
      var componentsToUnlink =
        original.Component.Where(c => !dto.Component.Select(comp => comp.ComponentId).Contains(c.ComponentId));
      var componentEntities = new List<EntityEntry<Component>>();

      foreach (var component in componentsToUnlink)
      {
        component.SystemId = null;
        component.System = null;
        componentEntities.Add(_db.Component.Update(component));
      }

      var entity = _db.System.Update(dto);
      await _db.SaveChangesAsync();
      entity.State = EntityState.Detached;
      foreach (var componentEntity in componentEntities) componentEntity.State = EntityState.Detached;
      return true;
    }

    public async Task<bool> Delete(int id)
    {
      var dto = await ReadDtoSystem(_db, id);
      var entity = _db.System.Remove(dto);
      await _db.SaveChangesAsync();
      entity.State = EntityState.Detached;
      return true;
    }

    // Reads dto system with no tracking
    private static async Task<Models.System> ReadDtoSystem(SawMillDbContext db, int id)
    {
      return await db.System.Include(s => s.Component).AsNoTracking().FirstAsync(elem => elem.SystemId == id);
    }
  }
}