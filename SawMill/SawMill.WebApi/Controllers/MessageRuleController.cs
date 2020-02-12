using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.MessageRule;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class MessageRuleController : ControllerBase
  {
    private readonly IMessageRuleService _messageRuleService;
    private readonly IPresenter<MessageRuleViewModel, MessageRule> _presenter;

    public MessageRuleController(IPresenter<MessageRuleViewModel, MessageRule> presenter,
      IMessageRuleService messageRuleService)
    {
      _presenter = presenter;
      _messageRuleService = messageRuleService;
    }

    // GET: api/MessageRule
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MessageRuleViewModel>>> Index()
    {
      try
      {
        var all = await _messageRuleService.GetAll();

        var transformed = new List<MessageRuleViewModel>();
        foreach (var component in all)
        {
          var transform = await _presenter.Present(component);
          transformed.Add(transform);
        }

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        return StatusCode((int) HttpStatusCode.InternalServerError);
      }
    }

    // GET: api/MessageRule/5
    [HttpGet("{id}")]
    public async Task<ActionResult<MessageRuleViewModel>> Get(int id)
    {
      try
      {
        var result = await _messageRuleService.Get(id);
        return Ok(await _presenter.Present(result));
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound();
      }
      catch (Exception)
      {
        return BadRequest($"Couldn't fetch message rule with id {id}");
      }
    }

    // POST: api/MessageRule
    [HttpPost]
    public async Task<ActionResult<MessageRuleViewModel>> Create([FromBody] MessageRuleViewModel model)
    {
      if (model == null)
      {
        return BadRequest();
      }

      var messageRuleRequest = await _presenter.Request(model);
      try
      {
        var created = await _messageRuleService.Create(messageRuleRequest);
        return CreatedAtAction(nameof(Create), await _presenter.Present(created));
      }
      catch (Exception debug)
      {
        return BadRequest("Failed to create message rule");
      }
    }

    // PUT: api/MessageRule/5
    [HttpPut("{id}")]
    public async Task<ActionResult<MessageRuleViewModel>> Edit(int id, [FromBody] MessageRuleViewModel model)
    {
      if (model == null || model.Id != id)
      {
        return BadRequest();
      }

      try
      {
        var dateTimeRule = await _presenter.Request(model);

        var updatedDateTimeRule = await _messageRuleService.Edit(dateTimeRule);

        return Ok(await _presenter.Present(updatedDateTimeRule));
      }
      catch
      {
        return BadRequest("Failed to edit message rule");
      }
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
      try
      {
        await _messageRuleService.Delete(id);
        return Ok(true);
      }
      catch (ArgumentOutOfRangeException)
      {
        return BadRequest($"Message rule with id {id} does not exists");
      }
      catch (Exception)
      {
        return BadRequest($"Failed to delete message rule with  {id}");
      }
    }
  }
}