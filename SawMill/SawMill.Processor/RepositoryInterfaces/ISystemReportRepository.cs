using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.RepositoryInterfaces
{
  public interface ISystemReportRepository
  {
    Task<SystemReport> GenerateReportForSystem(int systemId);
    Task<IEnumerable<SystemReport>> ReportsForSystem(int systemId);
    Task<SystemReport> GetReport(int reportId);
  }
}
