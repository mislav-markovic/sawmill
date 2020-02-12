using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.ViewModel.ParsingRules;

namespace SawMill.WebApi.Presenter
{
  public class ParsingRulesPresenter : IPresenter<ParsingRulesViewModel, ParsingRules>
  {
    private readonly IComponentService _componentService;
    private readonly ICustomAttributeRuleService _customAttributeRuleService;

    public ParsingRulesPresenter(ICustomAttributeRuleService customAttributeRuleService,
      IComponentService componentService)
    {
      _customAttributeRuleService = customAttributeRuleService;
      _componentService = componentService;
    }

    public Task<ParsingRulesViewModel> Present(ParsingRules model)
    {
      var customAttributeRules = new int[0];
      if (model.CustomAttributeRules != null && model.CustomAttributeRules.Any())
      {
        customAttributeRules = model.CustomAttributeRules.Select(elem => elem.Id).ToArray();
      }


      return Task.FromResult(new ParsingRulesViewModel(model.SeverityRuleId, model.MessageRuleId, model.DateTimeRuleId,
        model.Id, customAttributeRules, model.Components.First().Id));
    }

    public async Task<ParsingRules> Request(ParsingRulesViewModel viewModel)
    {
      var customAttributeRules = new HashSet<GeneralRule>();
      foreach (var viewModelCustomAttributeRuleId in viewModel.CustomAttributeRuleIds)
      {
        var customAttributeRule = await _customAttributeRuleService.Get(viewModelCustomAttributeRuleId);
        customAttributeRules.Add(customAttributeRule);
      }

      var component = await _componentService.Get(viewModel.ComponentId);
      var components = new List<Component>();
      components.Add(component);
      return new ParsingRules
      {
        Id = viewModel.Id, DateTimeRuleId = viewModel.DateTimeRuleId, MessageRuleId = viewModel.MessageRuleId,
        SeverityRuleId = viewModel.SeverityRuleId, CustomAttributeRules = customAttributeRules, Components = components
      };
    }
  }
}