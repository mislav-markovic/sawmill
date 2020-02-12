using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using DateTimeRule = SawMill.Processor.Model.DateTimeRule;

namespace SawMill.Data.Repository
{
  public class DateTimeRuleRepository : IDateTimeRuleRepository
  {
    private readonly SawMillDbContext _db;
    private readonly IGeneralRuleRepository _generalRuleRepository;

    public DateTimeRuleRepository(SawMillDbContext db, IGeneralRuleRepository generalRuleRepository)
    {
      _db = db;
      _generalRuleRepository = generalRuleRepository;
    }

    public async Task<int> Create(DateTimeRule model)
    {
      var generalRuleId = await _generalRuleRepository.Create(model.GeneralRule);
      model.GeneralRuleId = generalRuleId;

      var entity = await _db.DateTimeRule.AddAsync(model.ToDomainModel());
      await _db.SaveChangesAsync();

      return entity.Entity.DateTimeRuleId;
    }

    public async Task<DateTimeRule> Read(int id)
    {
      var dto = await _db.DateTimeRule.Include(e => e.GeneralRule).FirstAsync(elem => elem.DateTimeRuleId == id);
      return dto.ToBusinessModel();
    }

    public async Task<IEnumerable<DateTimeRule>> ReadAll()
    {
      var dtoList = await _db.DateTimeRule.Include(e => e.GeneralRule).ToListAsync();
      return dtoList.Select(elem => elem.ToBusinessModel());
    }

    public async Task<bool> Update(DateTimeRule model)
    {
      var dto = model.ToDomainModel();

      if (model.GeneralRule != null)
      {
        await _generalRuleRepository.Update(model.GeneralRule);
      }

      _db.DateTimeRule.Update(dto);
      await _db.SaveChangesAsync();

      return true;
    }

    public async Task<bool> Delete(int id)
    {
      try
      {
        var dto = await _db.DateTimeRule.FindAsync(id);
        _db.DateTimeRule.Remove(dto);
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