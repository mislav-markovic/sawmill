using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class SeverityRuleService : ISeverityRuleService
  {
    private readonly ISeverityRuleRepository _severityRuleRepository;

    public SeverityRuleService(ISeverityRuleRepository severityRuleRepository)
    {
      _severityRuleRepository = severityRuleRepository;
    }

    public async Task<SeverityRule> Create(SeverityRule model)
    {
      var createdId = await _severityRuleRepository.Create(model);
      return await _severityRuleRepository.Read(createdId);
    }

    public async Task<SeverityRule> Edit(SeverityRule model)
    {
      var isUpdated = await _severityRuleRepository.Update(model);
      if (isUpdated)
      {
        return await _severityRuleRepository.Read(model.Id);
      }

      throw new Exception($"Failed to update message rule {model.Id}");
    }

    public async Task<IEnumerable<SeverityRule>> GetAll()
    {
      return await _severityRuleRepository.ReadAll();
    }

    public async Task<SeverityRule> Get(int id)
    {
      var result = await _severityRuleRepository.Read(id);
      if (result == null)
      {
        throw new ArgumentException($"Cannot read message rule {id}", nameof(id));
      }

      return result;
    }

    public async Task Delete(int id)
    {
      var result = await _severityRuleRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete message rule {id}", nameof(id));
      }
    }

    public async Task<int> GetGeneralRuleId(int id)
    {
      var model = await _severityRuleRepository.Read(id);
      return model.GeneralRuleId;
    }
  }
}