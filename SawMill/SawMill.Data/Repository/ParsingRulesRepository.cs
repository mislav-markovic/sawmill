using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using ParsingRules = SawMill.Processor.Model.ParsingRules;

namespace SawMill.Data.Repository
{
  public class ParsingRulesRepository : IParsingRulesRepository
  {
    private readonly SawMillDbContext _db;

    public ParsingRulesRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<int> Create(ParsingRules model)
    {
      var dto = model.ToDomainModel();
      var component = dto.Components.First();
      dto.Components = null;

      var entity = _db.ParsingRules.Add(dto);
      await _db.SaveChangesAsync();

      entity.State = EntityState.Detached;

      component.ParsingRulesId = entity.Entity.ParsingRulesId;

      var componentEntity = _db.Component.Update(component);
      await _db.SaveChangesAsync();
      componentEntity.State = EntityState.Detached;

      return entity.Entity.ParsingRulesId;
    }

    public async Task<ParsingRules> Read(int id)
    {
      try
      {
        return (await ReadDtoParsingRules(_db, id)).ToBusinessModel();
      }
      catch (Exception debug)
      {
        throw new ArgumentOutOfRangeException();
      }
    }

    public async Task<IEnumerable<ParsingRules>> ReadAll()
    {
      try
      {
        return await _db.ParsingRules
          .Include(pr => pr.Components)
          .Include(pr => pr.MessageRule).ThenInclude(r => r.GeneralRule)
          .Include(pr => pr.SeverityLevelRule).ThenInclude(r => r.GeneralRule)
          .Include(pr => pr.DateTimeRule).ThenInclude(r => r.GeneralRule)
          .Include(pr => pr.CustomAttributeRuleParsingRules).ThenInclude(carpr => carpr.GeneralRule)
          .Select(rule => rule.ToBusinessModel())
          .ToListAsync();
      }
      catch (Exception e)
      {
        Console.WriteLine(e);
        throw;
      }
    }

    public async Task<bool> Update(ParsingRules model)
    {
      var dto = model.ToDomainModel();
      var original = await ReadDtoParsingRules(_db, model.Id);

      var originalCustomAttributeIds =
        original.CustomAttributeRuleParsingRules.Select(c => c.GeneralRuleId).ToList();
      var currentCustomAttributeIds = dto.CustomAttributeRuleParsingRules.Select(c => c.GeneralRuleId).ToList();
      // all in original except those that were present in original and are present in current
      var toDelete = originalCustomAttributeIds.Except(originalCustomAttributeIds.Intersect(currentCustomAttributeIds));
      // all that are in current but were not in original
      var toCreate = currentCustomAttributeIds.Except(originalCustomAttributeIds);

      foreach (var customAttributeRuleId in toDelete)
      {
        var delete = new CustomAttributeRuleParsingRules
          {ParsingRulesId = model.Id, GeneralRuleId = customAttributeRuleId};
        _db.CustomAttributeRuleParsingRules.Remove(delete);
      }

      foreach (var customAttributeCreateId in toCreate)
      {
        var create = new CustomAttributeRuleParsingRules
          {ParsingRulesId = model.Id, GeneralRuleId = customAttributeCreateId};
        _db.CustomAttributeRuleParsingRules.Add(create);
      }

      await _db.SaveChangesAsync();
      var entity = _db.ParsingRules.Update(dto);
      await _db.SaveChangesAsync();
      entity.State = EntityState.Detached;
      return true;
    }


    public async Task<bool> Delete(int id)
    {
      var dto = await ReadDtoParsingRules(_db, id);
      var entity = _db.ParsingRules.Remove(dto);
      await _db.SaveChangesAsync();
      entity.State = EntityState.Detached;
      return true;
    }

    private static async Task<Models.ParsingRules> ReadDtoParsingRules(SawMillDbContext db, int id)
    {
      return await db.ParsingRules
        .Include(pr => pr.Components)
        .Include(pr => pr.CustomAttributeRuleParsingRules).ThenInclude(carpr => carpr.GeneralRule)
        .FirstAsync(elem => elem.ParsingRulesId == id);
    }
  }
}