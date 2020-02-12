using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class SettingsController : ControllerBase
  {
    private readonly ISettingsService _settingsService;

    public SettingsController(ISettingsService settingsService)
    {
      _settingsService = settingsService;
    }

    // GET: api/Settings
    [HttpGet]
    public async Task<ActionResult<Settings>> Get()
    {
      var model = await _settingsService.GetSettings();
      return Ok(model);
    }

    // PUT: api/Settings/5
    [HttpPut]
    public async Task<ActionResult<bool>> Put([FromBody] Settings value)
    {
      await _settingsService.UpdateSettings(value);
      return Ok(true);
    }
  }
}