using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.ViewModel.SeverityRule;

namespace SawMill.WebApi.Presenter
{
  public class SeverityRulePresenter : IPresenter<SeverityRuleViewModel, SeverityRule>
  {
    private readonly ISeverityRuleService _service;

    public SeverityRulePresenter(ISeverityRuleService service)
    {
      _service = service;
    }

    public async Task<SeverityRuleViewModel> Present(SeverityRule model)
    {
      if (model.GeneralRule == null)
      {
        model = await _service.Get(model.Id);
      }

      return new SeverityRuleViewModel(model.Id, model.GeneralRule.Name, model.GeneralRule.Description,
        model.GeneralRule.Matcher.ToString(), model.GeneralRule.StartAnchor, model.GeneralRule.EndAnchor,
        model.Trace.ToString(),
        model.Debug.ToString(), model.Info.ToString(), model.Warning.ToString(), model.Error.ToString(),
        model.Fatal.ToString());
    }

    public async Task<SeverityRule> Request(SeverityRuleViewModel viewModel)
    {
      var generalRuleId = 0;
      try
      {
        generalRuleId = await _service.GetGeneralRuleId(viewModel.Id);
      }
      catch (Exception debug)
      {
        // ignore, if exception then we are creating new message rule
      }

      var generalRule = new GeneralRule
      {
        Id = generalRuleId,
        Description = viewModel.Description,
        Name = viewModel.Name,
        EndAnchor = viewModel.EndAnchor,
        StartAnchor = viewModel.StartAnchor,
        Matcher = new Regex(viewModel.Matcher)
      };

      return new SeverityRule
      {
        Id = viewModel.Id, GeneralRule = generalRule, GeneralRuleId = generalRuleId, Info = new Regex(viewModel.Info),
        Trace = new Regex(viewModel.Trace), Warning = new Regex(viewModel.Warning), Debug = new Regex(viewModel.Debug),
        Fatal = new Regex(viewModel.Fatal),
        Error = new Regex(viewModel.Error)
      };
    }
  }
}