using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.ViewModel.MessageRule;

namespace SawMill.WebApi.Presenter
{
  public class MessageRulePresenter : IPresenter<MessageRuleViewModel, MessageRule>
  {
    private readonly IMessageRuleService _service;

    public MessageRulePresenter(IMessageRuleService service)
    {
      _service = service;
    }

    public async Task<MessageRuleViewModel> Present(MessageRule model)
    {
      if (model.GeneralRule == null)
      {
        model = await _service.Get(model.Id);
      }

      return new MessageRuleViewModel(model.Id, model.GeneralRule.Name, model.GeneralRule.Description,
        model.GeneralRule.Matcher.ToString(), model.GeneralRule.StartAnchor, model.GeneralRule.EndAnchor,
        model.MaxLength, model.GeneralRuleId);
    }

    public async Task<MessageRule> Request(MessageRuleViewModel viewModel)
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

      return new MessageRule
        {Id = viewModel.Id, GeneralRule = generalRule, MaxLength = viewModel.MaxLength, GeneralRuleId = generalRuleId};
    }
  }
}