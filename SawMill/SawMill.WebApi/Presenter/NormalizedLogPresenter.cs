using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SawMill.Data.Converters;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.ViewModel.CustomAttributeRule;
using SawMill.WebApi.ViewModel.Log;
using SawMill.WebApi.ViewModel.SeverityRule;

namespace SawMill.WebApi.Presenter
{
  public class NormalizedLogPresenter : IPresenter<NormalizedLogViewModel, NormalizedLog>
  {
    private readonly ICustomAttributeRuleService _customAttributeRuleService;
    private readonly IParsingRulesService _parsingRulesService;

    public NormalizedLogPresenter(IParsingRulesService parsingRulesService,
      ICustomAttributeRuleService customAttributeRuleService)
    {
      _parsingRulesService = parsingRulesService;
      _customAttributeRuleService = customAttributeRuleService;
    }

    public async Task<NormalizedLogViewModel> Present(NormalizedLog model)
    {
      var severityRule = (await _parsingRulesService.GetComponentsParsingRules(model.ComponentId)).SeverityParsingRule
        .ToDomainModel();
      var customValues = new List<CustomAttributeValueViewModel>();
      if (model.CustomAttributeValues != null && model.CustomAttributeValues.Any())
      {
        var ruleNames = await GetNameOfCustomAttributeRulesByTheirIds(model.CustomAttributeValues);
        customValues = model.CustomAttributeValues
          .Select(elem => new CustomAttributeValueViewModel(elem.Id, elem.Value, elem.CustomAttributeRuleId,
            ruleNames[elem.CustomAttributeRuleId])).ToList();
      }

      return new NormalizedLogViewModel(model.Id,
        model.ComponentId,
        model.DateTime,
        model.Message,
        new SeverityLevelViewModel(model.Severity, SeverityToString(model.Severity)),
        customValues,
        model.RawLogId
      );
    }

    public Task<NormalizedLog> Request(NormalizedLogViewModel viewModel)
    {
      var customAttributes = new List<CustomAttributeValue>();
      if (viewModel.CustomValues != null && viewModel.CustomValues.Any())
      {
        customAttributes = viewModel.CustomValues
          .Select(elem => new CustomAttributeValue
          {
            Id = elem.Id,
            CustomAttributeRuleId = elem.CustomAttributeRuleId,
            Value = elem.Value,
            NormalizedLogId = viewModel.Id
          })
          .ToList();
      }


      var log = new NormalizedLog(viewModel.DateTime, viewModel.SeverityLevel.Level, viewModel.Message,
        viewModel.ComponentId, viewModel.RawLogId, viewModel.Id, customAttributes.ToArray());

      return Task.FromResult(log);
    }

    private static string SeverityToString(SeverityLevel severityLevel)
    {
      switch (severityLevel)
      {
        case SeverityLevel.Debug: return "Debug";
        case SeverityLevel.Trace: return "Trace";
        case SeverityLevel.Info: return "Info";
        case SeverityLevel.Warning: return "Warning";
        case SeverityLevel.Error: return "Error";
        case SeverityLevel.Fatal: return "Fatal";
        default:
          throw new ArgumentOutOfRangeException(nameof(severityLevel), severityLevel, null);
      }
    }

    private async Task<Dictionary<int, string>> GetNameOfCustomAttributeRulesByTheirIds(
      IEnumerable<CustomAttributeValue> values)
    {
      var ids = values.Select(elem => elem.CustomAttributeRuleId).ToHashSet();
      var dict = new Dictionary<int, string>();

      foreach (var id in ids)
      {
        var rule = await _customAttributeRuleService.Get(id);
        dict.Add(id, rule.Name);
      }

      return dict;
    }
  }
}