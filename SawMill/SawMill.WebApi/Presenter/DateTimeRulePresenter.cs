using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.ViewModel.DateTimeRule;

namespace SawMill.WebApi.Presenter
{
  public class DateTimeRulePresenter : IPresenter<DateTimeRuleViewModel, DateTimeRule>
  {
    private readonly IDateTimeRuleService _service;

    public DateTimeRulePresenter(IDateTimeRuleService service)
    {
      _service = service;
    }

    public async Task<DateTimeRuleViewModel> Present(DateTimeRule model)
    {
      if (model.GeneralRule == null)
      {
        model = await _service.Get(model.Id);
      }

      return new DateTimeRuleViewModel(model.Id, model.GeneralRule.Name, model.GeneralRule.Description,
        model.GeneralRule.Matcher.ToString(), model.GeneralRule.StartAnchor, model.GeneralRule.EndAnchor,
        model.DateFormat);
    }

    public async Task<DateTimeRule> Request(DateTimeRuleViewModel viewModel)
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

      return new DateTimeRule
      {
        Id = viewModel.Id, GeneralRule = generalRule, DateFormat = viewModel.DateFormat, GeneralRuleId = generalRuleId
      };
    }
  }
}