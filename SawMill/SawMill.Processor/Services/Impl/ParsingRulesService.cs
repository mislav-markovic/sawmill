using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class ParsingRulesService : IParsingRulesService
  {
    private readonly IParsingRulesRepository _parsingRulesRepository;

    public ParsingRulesService(IParsingRulesRepository parsingRulesRepository)
    {
      _parsingRulesRepository = parsingRulesRepository;
    }

    public async Task<ParsingRules> Create(ParsingRules model)
    {
      var createdId = await _parsingRulesRepository.Create(model);
      return await _parsingRulesRepository.Read(createdId);
    }

    public async Task<ParsingRules> Edit(ParsingRules model)
    {
      var result = await _parsingRulesRepository.Update(model);
      if (result)
      {
        return await _parsingRulesRepository.Read(model.Id);
      }

      throw new ArgumentException();
    }

    public async Task<IEnumerable<ParsingRules>> GetAll()
    {
      return await _parsingRulesRepository.ReadAll();
    }

    public async Task<ParsingRules> Get(int id)
    {
      var result = await _parsingRulesRepository.Read(id);
      if (result == null)
      {
        throw new ArgumentException($"Cannot read parsing rules with id {id}", nameof(id));
      }

      return result;
    }

    public async Task Delete(int id)
    {
      var result = await _parsingRulesRepository.Delete(id);

      if (!result)
      {
        throw new ArgumentOutOfRangeException();
      }
    }

    public async Task<ParsingRules> GetComponentsParsingRules(int componentId)
    {
      var all = await _parsingRulesRepository.ReadAll();
      return all.First(elem => elem.Components.Select(c => c.Id).Contains(componentId));
    }
  }
}