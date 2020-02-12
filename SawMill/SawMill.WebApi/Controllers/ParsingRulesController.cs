using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.Presenter;
using SawMill.WebApi.ViewModel.ParsingRules;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class ParsingRulesController : ControllerBase
  {
    private readonly IParsingRulesService _parsingRulesService;
    private readonly IPresenter<ParsingRulesViewModel, ParsingRules> _presenter;

    public ParsingRulesController(IPresenter<ParsingRulesViewModel, ParsingRules> presenter,
      IParsingRulesService parsingRulesService)
    {
      _parsingRulesService = parsingRulesService;
      _presenter = presenter;
    }

    // GET: api/ParsingRules
    [HttpGet]
    public async Task<ActionResult<IEnumerable<ParsingRulesViewModel>>> Index()
    {
      try
      {
        var results = await _parsingRulesService.GetAll();
        var transformed = new List<ParsingRulesViewModel>();

        foreach (var parsingRule in results)
        {
          var transform = await _presenter.Present(parsingRule);
          transformed.Add(transform);
        }

        return Ok(transformed);
      }
      catch (Exception debug)
      {
        return StatusCode((int) HttpStatusCode.InternalServerError);
      }
    }

    // GET: api/ParsingRules/5
    [HttpGet("{id}")]
    public async Task<ActionResult<ParsingRulesViewModel>> Get(int id)
    {
      try
      {
        var result = await _parsingRulesService.Get(id);
        return Ok(await _presenter.Present(result));
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound();
      }
      catch (Exception debug)
      {
        return BadRequest($"Couldn't fetch parsing rules with id {id}");
      }
    }

    // GET: api/ParsingRules/forcomponent/5
    [HttpGet("forcomponent/{componentId}")]
    public async Task<ActionResult<ParsingRulesViewModel>> GetForComponent(int componentId)
    {
      try
      {
        var result = await _parsingRulesService.GetComponentsParsingRules(componentId);
        return Ok(await _presenter.Present(result));
      }
      catch (ArgumentOutOfRangeException)
      {
        return NotFound();
      }
      catch (Exception debug)
      {
        return BadRequest($"Couldn't fetch parsing rules for component with id {componentId}");
      }
    }

    // POST: api/ParsingRules
    [HttpPost]
    public async Task<ActionResult<ParsingRulesViewModel>> Create([FromBody] ParsingRulesViewModel model)
    {
      if (model == null)
      {
        return BadRequest();
      }

      var parsingRulesToCreate = await _presenter.Request(model);
      try
      {
        var created = await _parsingRulesService.Create(parsingRulesToCreate);
        return CreatedAtAction(nameof(Create), await _presenter.Present(created));
      }
      catch (Exception debug)
      {
        return BadRequest("Failed to create parsing rules");
      }
    }

    // PUT: api/ParsingRules/5
    [HttpPut("{id}")]
    public async Task<ActionResult<ParsingRulesViewModel>> Edit(int id, [FromBody] ParsingRulesViewModel model)
    {
      if (model == null || model.Id != id)
      {
        return BadRequest();
      }

      try
      {
        var parsingRules = await _presenter.Request(model);

        var updatedParsingRules = await _parsingRulesService.Edit(parsingRules);

        return Ok(await _presenter.Present(updatedParsingRules));
      }
      catch
      {
        return BadRequest("Failed to edit parsing rules");
      }
    }

    // DELETE: api/ApiWithActions/5
    [HttpDelete("{id}")]
    public async Task<ActionResult<bool>> Delete(int id)
    {
      try
      {
        await _parsingRulesService.Delete(id);
        return Ok(true);
      }
      catch (ArgumentOutOfRangeException)
      {
        return BadRequest($"Parsing rules with id {id} does not exists");
      }
      catch (Exception)
      {
        return BadRequest($"Failed to delete {id}");
      }
    }
  }
}