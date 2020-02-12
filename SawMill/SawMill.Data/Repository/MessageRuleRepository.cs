using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using MessageRule = SawMill.Processor.Model.MessageRule;

namespace SawMill.Data.Repository
{
  public class MessageRuleRepository : IMessageRuleRepository
  {
    private readonly SawMillDbContext _db;
    private readonly IGeneralRuleRepository _generalRuleRepository;

    public MessageRuleRepository(SawMillDbContext db, IGeneralRuleRepository generalRuleRepository)
    {
      _db = db;
      _generalRuleRepository = generalRuleRepository;
    }

    public async Task<int> Create(MessageRule model)
    {
      var generalRuleId = await _generalRuleRepository.Create(model.GeneralRule);
      model.GeneralRuleId = generalRuleId;

      var entity = await _db.MessageRule.AddAsync(model.ToDomainModel());
      await _db.SaveChangesAsync();

      return entity.Entity.MessageRuleId;
    }

    public async Task<MessageRule> Read(int id)
    {
      var dto = await _db.MessageRule.Include(e => e.GeneralRule).FirstAsync(elem => elem.MessageRuleId == id);
      return dto.ToBusinessModel();
    }

    public async Task<IEnumerable<MessageRule>> ReadAll()
    {
      var dtoList = await _db.MessageRule.Include(e => e.GeneralRule).ToListAsync();
      return dtoList.Select(elem => elem.ToBusinessModel());
    }

    public async Task<bool> Update(MessageRule model)
    {
      var dto = model.ToDomainModel();

      if (model.GeneralRule != null)
      {
        await _generalRuleRepository.Update(model.GeneralRule);
      }

      _db.MessageRule.Update(dto);
      await _db.SaveChangesAsync();

      return true;
    }

    public async Task<bool> Delete(int id)
    {
      try
      {
        var dto = await _db.MessageRule.FindAsync(id);
        _db.MessageRule.Remove(dto);
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