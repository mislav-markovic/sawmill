using System;
using System.Linq;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.WebApi.ViewModel.Reports;

namespace SawMill.WebApi.Presenter
{
  public class SystemReportPresenter : IPresenter<SystemReportViewModel, SystemReport>
  {
    public Task<SystemReportViewModel> Present(SystemReport model)
    {
      var componentReports = model.ComponentReports
        .Select(elem => new ComponentReportViewModel(elem.Id,
          elem.ComponentId, model.Id, elem.ComponentReportAlert.ToDictionary(alert => alert.AlertId, alert => alert.Count)));

      var systemReport = new SystemReportViewModel(model.Id, model.SystemId,
        model.SystemReportAlertGroup.ToDictionary(group => group.AlertGroupId, group => group.Count), componentReports.ToArray(),
        model.Timestamp);

      return Task.FromResult(systemReport);
    }

    public Task<SystemReport> Request(SystemReportViewModel viewModel)
    {
      throw new NotImplementedException();
    }
  }
}