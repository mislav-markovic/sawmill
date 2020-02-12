using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.CustomAttributeRule;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class CustomAttributeRuleController : ControllerBase
  {
    private readonly IPresenter<CustomAttributeRuleViewModel, GeneralRule> _presenter;
    private readonly ICustomAttributeRuleService _service;

    public CustomAttributeRuleController(ICustomAttributeRuleService service,
      IPresenter<CustomAttributeRuleViewModel, GeneralRule> presenter)
    {
      _service = service;
      _presenter = presenter;
    }


    // GET: api/GeneralRule
    [HttpGet]
    public async Task<ActionResult<IEnumerable<CustomAttributeRuleViewModel>>> Index()
    {
      try
      {
        var all = await _service.GetAll();

        var transformed = new List<CustomAttributeRuleViewModel>();
        foreach (var customAttributeRule in all)
        {
          var transform = await _presenter.Present(customAttributeRule);
          transformed.Add(transform);
        }

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        return StatusCode((int) HttpStatusCode.InternalServerError);
      }
    }

    // GET: api/GeneralRule/5
    [HttpGet("{id}")]
    public async Task<ActionResult<CustomAttributeRuleViewModel>> Get(int id)
    {
      try
      {
        var result = await _service.Get(id);
        return Ok(await _presenter.Present(result));
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound();
      }
      catch (Exception)
      {
        return BadRequest($"Couldn't fetch custom attribute rule with id {id}");
      }
    }

    // POST: api/GeneralRule
    [HttpPost]
    public async Task<ActionResult<CustomAttributeRuleViewModel>> Create([FromBody] CustomAttributeRuleViewModel model)
    {
      if (model == null)
      {
        return BadRequest();
      }

      var customAttributeRuleRequest = await _presenter.Request(model);
      try
      {
        var created = await _service.Create(customAttributeRuleRequest);
        return CreatedAtAction(nameof(Create), await _presenter.Present(created));
      }
      catch (Exception debug)
      {
        return BadRequest("Failed to create message rule");
      }
    }

    // PUT: api/GeneralRule/5
    [HttpPut("{id}")]
    public async Task<ActionResult<CustomAttributeRuleViewModel>> Edit(int id,
      [FromBody] CustomAttributeRuleViewModel model)
    {
      if (model == null || model.Id != id)
      {
        return BadRequest();
      }

      try
      {
        var customAttributeRule = await _presenter.Request(model);

        var updatedCustomAttributeRule = await _service.Edit(customAttributeRule);

        return Ok(await _presenter.Present(updatedCustomAttributeRule));
      }
      catch
      {
        return BadRequest("Failed to edit custom attribute rule");
      }
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
      try
      {
        await _service.Delete(id);
        return Ok(true);
      }
      catch (ArgumentOutOfRangeException)
      {
        return BadRequest($"Custom attribute rule with id {id} does not exists");
      }
      catch (Exception)
      {
        return BadRequest($"Failed to delete custom attribute rule with  {id}");
      }
    }
  }
}