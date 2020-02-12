using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class DateTimeRuleService : IDateTimeRuleService
  {
    private readonly IDateTimeRuleRepository _dateTimeRuleRepository;

    public DateTimeRuleService(IDateTimeRuleRepository dateTimeRuleRepository)
    {
      _dateTimeRuleRepository = dateTimeRuleRepository;
    }

    public async Task<DateTimeRule> Create(DateTimeRule model)
    {
      var createdId = await _dateTimeRuleRepository.Create(model);
      return await _dateTimeRuleRepository.Read(createdId);
    }

    public async Task<DateTimeRule> Edit(DateTimeRule model)
    {
      var isUpdated = await _dateTimeRuleRepository.Update(model);
      if (isUpdated)
      {
        return await _dateTimeRuleRepository.Read(model.Id);
      }

      throw new Exception($"Failed to update date time rule {model.Id}");
    }

    public async Task<IEnumerable<DateTimeRule>> GetAll()
    {
      return await _dateTimeRuleRepository.ReadAll();
    }

    public async Task<DateTimeRule> Get(int id)
    {
      var result = await _dateTimeRuleRepository.Read(id);
      if (result == null)
      {
        throw new ArgumentException($"Cannot read date time rule {id}", nameof(id));
      }

      return result;
    }

    public async Task Delete(int id)
    {
      var result = await _dateTimeRuleRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete date time rule {id}", nameof(id));
      }
    }

    public async Task<int> GetGeneralRuleId(int id)
    {
      var model = await _dateTimeRuleRepository.Read(id);
      return model.GeneralRuleId;
    }
  }
}