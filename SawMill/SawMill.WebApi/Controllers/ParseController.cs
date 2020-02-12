using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.CustomAttributeRule;
using SawMill.WebApi.ViewModel.DateTimeRule;
using SawMill.WebApi.ViewModel.MessageRule;
using SawMill.WebApi.ViewModel.SeverityRule;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ParseController : ControllerBase
  {
    private readonly IPresenter<CustomAttributeRuleViewModel, GeneralRule> _customAttributeRulePresenter;
    private readonly IPresenter<DateTimeRuleViewModel, DateTimeRule> _dateTimeRulePresenter;
    private readonly IPresenter<MessageRuleViewModel, MessageRule> _messageRulePresenter;
    private readonly IParseService _parser;
    private readonly IPresenter<SeverityRuleViewModel, SeverityRule> _severityRulePresenter;


    public ParseController(IParseService parser, IPresenter<DateTimeRuleViewModel, DateTimeRule> dateTimeRulePresenter,
      IPresenter<MessageRuleViewModel, MessageRule> messageRulePresenter,
      IPresenter<SeverityRuleViewModel, SeverityRule> severityRulePresenter,
      IPresenter<CustomAttributeRuleViewModel, GeneralRule> customAttributeRulePresenter)
    {
      _parser = parser;
      _dateTimeRulePresenter = dateTimeRulePresenter;
      _messageRulePresenter = messageRulePresenter;
      _severityRulePresenter = severityRulePresenter;
      _customAttributeRulePresenter = customAttributeRulePresenter;
    }

    [HttpPost("datetime")]
    public async Task<ActionResult<string>> TryParseDateTime([FromBody] ParseRequestDateTimeRuleViewModel viewModel)
    {
      try
      {
        var rule = await _dateTimeRulePresenter.Request(viewModel.RuleViewModel);
        var parsed = _parser.ParseDateTime(viewModel.Line, rule);

        return Ok(parsed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }

    [HttpPost("severity")]
    public async Task<ActionResult<SeverityLevelViewModel>> TryParseSeverity(
      [FromBody] ParseRequestSeverityViewModel viewModel)
    {
      try
      {
        var rule = await _severityRulePresenter.Request(viewModel.RuleViewModel);
        var parsed = _parser.ParseSeverityLevel(viewModel.Line, rule);
        var presented = new SeverityLevelViewModel(parsed, SeverityLevelDisplay(viewModel.RuleViewModel, parsed));

        return Ok(presented);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }

    private static string SeverityLevelDisplay(SeverityRuleViewModel rule, SeverityLevel level)
    {
      switch (level)
      {
        case SeverityLevel.Trace: return rule.Trace;
        case SeverityLevel.Debug: return rule.Debug;
        case SeverityLevel.Info: return rule.Info;
        case SeverityLevel.Warning: return rule.Warning;
        case SeverityLevel.Error: return rule.Error;
        case SeverityLevel.Fatal: return rule.Fatal;
        default:
          throw new ArgumentOutOfRangeException(nameof(level), level, null);
      }
    }

    [HttpPost("message")]
    public async Task<ActionResult<string>> TryParseMessage([FromBody] ParseRequestMessageViewModel viewModel)
    {
      try
      {
        var rule = await _messageRulePresenter.Request(viewModel.RuleViewModel);
        var parsed = _parser.ParseMessage(viewModel.Line, rule);

        return Ok(parsed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }

    [HttpPost("customattribute")]
    public async Task<ActionResult<string>> TryParseCustomAttribute(
      [FromBody] ParseRequestCustomAttributeRuleViewModel viewModel)
    {
      try
      {
        var rule = await _customAttributeRulePresenter.Request(viewModel.RuleViewModel);
        var parsed = _parser.ParseCustomAttribute(viewModel.Line, rule);

        return Ok(parsed);
      }
      catch (Exception debug)
      {
        Console.WriteLine(debug);
        throw;
      }
    }
  }
}