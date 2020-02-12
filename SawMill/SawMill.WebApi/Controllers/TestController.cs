using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Models;
using Serilog;

namespace SawMill.WebApi.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class TestController : ControllerBase
  {
    private SawMillDbContext _db;

    public TestController(SawMillDbContext db)
    {
      _db = db;
    }

    // GET: api/Test
    [HttpGet]
    public IEnumerable<string> Get()
    {
      Log.Information("You can clap up to 50 times per post!");
      return new[] {"value1", "value2"};
    }

    [HttpGet("login/fail/{userId}")]
    public ActionResult<bool> FailedLogin(int userId)
    {
      Log.Information($"Login failed for user with id {userId}");

      return Ok(false);
    }

    [HttpGet("filter")]
    public ActionResult<IEnumerable<string>> UserNamesByCategory([FromQuery]string category)
    {
      try
      {
        var sql = "SELECT * FROM [sawmill.db].[dbo].[User] WHERE [sawmill.db].[dbo].[User].Category LIKE '%"+category+"%'";

        var users = _db.User.FromSql(sql);
        Thread.Sleep(300);
        Log.Information($"Fetched {users.Count()} when filtered by category");

        return Ok(users.Select(e => e.Name));

      }
      catch (Exception e)
      {
        Thread.Sleep(300);
        Log.Error($"Error occurred when filtering users by category, error type: {e.GetType()}");
        return NotFound();
      }
    }

  }
}