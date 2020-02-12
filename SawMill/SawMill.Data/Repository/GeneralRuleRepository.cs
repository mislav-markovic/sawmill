using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using GeneralRule = SawMill.Processor.Model.GeneralRule;

namespace SawMill.Data.Repository
{
  public class GeneralRuleRepository : IGeneralRuleRepository
  {
    private readonly SawMillDbContext _db;

    public GeneralRuleRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(GeneralRule model)
    {
      var dto = model.ToDomainModel();

      var entity = _db.GeneralRule.Add(dto);

      await _db.SaveChangesAsync();

      entity.State = EntityState.Detached;
      return entity.Entity.GeneralRuleId;
    }

    public async Task<GeneralRule> Read(int id)
    {
      return (await _db.GeneralRule.FindAsync(id)).ToBusinessModel();
    }

    public async Task<IEnumerable<GeneralRule>> ReadAll()
    {
      return await _db.GeneralRule.Select(elem => elem.ToBusinessModel()).ToListAsync();
    }

    public async Task<bool> Update(GeneralRule model)
    {
      try
      {
        var dto = model.ToDomainModel();

        _db.GeneralRule.Update(dto);
        await _db.SaveChangesAsync();
        return true;
      }
      catch
      {
        return false;
      }
    }

    public async Task<bool> Delete(int id)
    {
      try
      {
        var dto = await _db.GeneralRule.FindAsync(id);

        if (dto == null)
        {
          return false;
        }

        _db.GeneralRule.Remove(dto);
        await _db.SaveChangesAsync();

        return true;
      }
      catch
      {
        return false;
      }
    }
  }
}