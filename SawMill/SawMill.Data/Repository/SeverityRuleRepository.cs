using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;

namespace SawMill.Data.Repository
{
  public class SeverityRuleRepository : ISeverityRuleRepository
  {
    private readonly SawMillDbContext _db;
    private readonly IGeneralRuleRepository _generalRuleRepository;


    public SeverityRuleRepository(SawMillDbContext db, IGeneralRuleRepository generalRuleRepository)
    {
      _db = db;
      _generalRuleRepository = generalRuleRepository;
    }

    public async Task<int> Create(SeverityRule model)
    {
      var severityRuleDto = model.ToDomainModel();
      var generalRuleId = severityRuleDto.GeneralRuleId;

      if (!DoesGeneralRuleExists(generalRuleId))
      {
        if (model.GeneralRule == null)
        {
          throw new ArgumentException();
        }

        generalRuleId = await _generalRuleRepository.Create(model.GeneralRule);
      }

      severityRuleDto.GeneralRuleId = generalRuleId;

      var entity = await _db.SeverityLevelRule.AddAsync(severityRuleDto);
      await _db.SaveChangesAsync();
      entity.State = EntityState.Detached;

      return entity.Entity.SeverityLevelRuleId;
    }

    public async Task<SeverityRule> Read(int id)
    {
      return (await _db.SeverityLevelRule
          .Include(rule => rule.GeneralRule)
          .AsNoTracking()
          .FirstAsync(elem => elem.SeverityLevelRuleId == id))
        .ToBusinessModel();
    }

    public async Task<IEnumerable<SeverityRule>> ReadAll()
    {
      var dtoList = await _db.SeverityLevelRule.Include(elem => elem.GeneralRule).ToListAsync();

      return dtoList.Select(elem => elem.ToBusinessModel());
    }

    public async Task<bool> Update(SeverityRule model)
    {
      try
      {
        var dto = model.ToDomainModel();

        if (model.GeneralRule != null)
        {
          await _generalRuleRepository.Update(model.GeneralRule);
        }

        _db.SeverityLevelRule.Update(dto);
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
        var dto = await _db.SeverityLevelRule.FindAsync(id);
        _db.SeverityLevelRule.Remove(dto);
        await _db.SaveChangesAsync();

        return true;
      }
      catch
      {
        return false;
      }
    }

    private bool DoesGeneralRuleExists(int id)
    {
      try
      {
        return _db.GeneralRule.Any(elem => elem.GeneralRuleId == id);
      }
      catch
      {
        return false;
      }
    }
  }
}