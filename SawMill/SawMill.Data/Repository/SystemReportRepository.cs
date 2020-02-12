using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using SawMill.Data.Converters;
using SawMill.Data.Models;
using SawMill.Processor.RepositoryInterfaces;
using SystemReport = SawMill.Processor.Model.SystemReport;

namespace SawMill.Data.Repository
{
  public class SystemReportRepository : ISystemReportRepository
  {
    private readonly SawMillDbContext _db;

    public SystemReportRepository(SawMillDbContext db)
    {
      _db = db;
    }

    public async Task<SystemReport> GenerateReportForSystem(int systemId)
    {
      var timestamp = DateTime.Now;
      var systemAlertGroups = await _db.AlertGroup.Where(ag => ag.SystemId == systemId).ToListAsync();
      var reportAlertGroups = new List<SystemReportAlertGroup>();

      var report = new Models.SystemReport
      {
        SystemReportId = 0,
        SystemId = systemId,
        Timestamp = timestamp
      };

      var systemReportEntity = _db.SystemReport.Add(report);
      await _db.SaveChangesAsync();
      var createdSystemReportId = systemReportEntity.Entity.SystemReportId;
      systemReportEntity.State = EntityState.Detached;

      foreach (var systemAlertGroup in systemAlertGroups)
      {
        var counted = await _db.AlertGroupValue.CountAsync(agv => agv.AlertGroupId == systemAlertGroup.AlertGroupId);
        var temp = new SystemReportAlertGroup
        {
          SystemReportAlertGroupId = 0,
          SystemReportId = createdSystemReportId,
          AlertGroupId = systemAlertGroup.AlertGroupId,
          Count = counted
        };
        reportAlertGroups.Add(temp);
      }

      _db.SystemReportAlertGroup.AddRange(reportAlertGroups);
      await _db.SaveChangesAsync();

      var components = await _db.Component.Where(c => c.SystemId != null && c.SystemId == systemId).ToListAsync();

      foreach (var component in components)
      {
        var componentReportAlerts = new List<ComponentReportAlert>();
        var componentReport = new ComponentReport
        {
          ComponentReportId = 0,
          SystemReportId = createdSystemReportId,
          ComponentId = component.ComponentId,
          Timestamp = timestamp
        };

        var componentReportEntity = _db.ComponentReport.Add(componentReport);
        await _db.SaveChangesAsync();
        var createdComponentReportId = componentReportEntity.Entity.ComponentReportId;
        componentReportEntity.State = EntityState.Detached;

        var componentAlerts = await _db.Alert.Where(a => a.ComponentId == component.ComponentId).ToListAsync();

        foreach (var componentAlert in componentAlerts)
        {
          var alertCount = await _db.AlertValue.CountAsync(av => av.AlertId == componentAlert.AlertId);
          var temp = new ComponentReportAlert
          {
            ComponentReportAlertId = 0,
            AlertId = componentAlert.AlertId,
            Count = alertCount,
            ComponentReportId = createdComponentReportId
          };
          componentReportAlerts.Add(temp);
        }

        _db.ComponentReportAlert.AddRange(componentReportAlerts);
        await _db.SaveChangesAsync();
      }

      return await GetReport(createdSystemReportId);
    }

    public async Task<IEnumerable<SystemReport>> ReportsForSystem(int systemId)
    {
      var all = await _db.SystemReport
        .Include(r => r.ComponentReports).ThenInclude(c => c.ComponentReportAlert)
        .Include(r => r.SystemReportAlertGroup)
        .Where(e => e.SystemId == systemId)
        .Select(e => e.ToBusinessModel())
        .ToListAsync();

      return all;
    }

    public async Task<SystemReport> GetReport(int reportId)
    {
      var report = await _db.SystemReport
        .Include(r => r.ComponentReports).ThenInclude(c => c.ComponentReportAlert)
        .Include(r => r.SystemReportAlertGroup)
        .FirstAsync(e => e.SystemReportId == reportId);

      return report.ToBusinessModel();
    }
  }
}