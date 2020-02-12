using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class CustomAttributeRuleService : ICustomAttributeRuleService
  {
    private readonly IGeneralRuleRepository _generalRuleRepository;

    public CustomAttributeRuleService(IGeneralRuleRepository generalRuleRepository)
    {
      _generalRuleRepository = generalRuleRepository;
    }

    public async Task<GeneralRule> Create(GeneralRule model)
    {
      var createdId = await _generalRuleRepository.Create(model);
      return await _generalRuleRepository.Read(createdId);
    }

    public async Task<GeneralRule> Edit(GeneralRule model)
    {
      var isUpdated = await _generalRuleRepository.Update(model);
      if (isUpdated)
      {
        return await _generalRuleRepository.Read(model.Id);
      }

      throw new Exception($"Failed to update custom attribute rule {model.Id}");
    }

    public async Task<IEnumerable<GeneralRule>> GetAll()
    {
      return await _generalRuleRepository.ReadAll();
    }

    public async Task<GeneralRule> Get(int id)
    {
      var result = await _generalRuleRepository.Read(id);
      if (result == null)
      {
        throw new ArgumentException($"Cannot read custom attribute rule {id}", nameof(id));
      }

      return result;
    }

    public async Task Delete(int id)
    {
      var result = await _generalRuleRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete custom attribute rule {id}", nameof(id));
      }
    }

    public async Task<int> GetGeneralRuleId(int id)
    {
      var model = await _generalRuleRepository.Read(id);
      return model.Id;
    }
  }
}