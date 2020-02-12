using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.ViewModel.AlertGroup;

namespace SawMill.WebApi.Presenter
{
  public class AlertGroupPresenter : IPresenter<AlertGroupViewModel, AlertGroup>
  {
    private readonly IAlertService _alertService;

    public AlertGroupPresenter(IAlertService alertService)
    {
      _alertService = alertService;
    }

    public Task<AlertGroupViewModel> Present(AlertGroup model)
    {
      var timespan = model.CorrelationWindow?.TotalSeconds ?? 0;
      var alerts = model.Alerts.OrderBy(a => a.Position).Select(a => new AlertGroupAlertVIewModel(a.Id, a.Position.Value, a.Not, a.Name)).ToArray();
      var viewModel = new AlertGroupViewModel(model.Id, model.Name, model.Description, (long)timespan, model.SystemId, alerts);

      return Task.FromResult(viewModel);
    }

    public async Task<AlertGroup> Request(AlertGroupViewModel viewModel)
    {
      var alerts = new List<Alert>();

      foreach (var alert in viewModel.Alerts)
      {
        var fetched = await _alertService.Get(alert.AlertId);

        fetched.AlertGroupId = viewModel.Id;
        fetched.Position = alert.Position;
        fetched.Not = alert.Not;
        alerts.Add(fetched);
      }


      var model = new AlertGroup
      {
        Id = viewModel.Id,
        SystemId = viewModel.SystemId,
        Name = viewModel.Name,
        Description = viewModel.Description,
        Alerts = alerts,
        CorrelationWindow = null
      };

      if (viewModel.Timespan > 0)
      {
        model.CorrelationWindow = TimeSpan.FromSeconds(viewModel.Timespan);
      }

      return model;

    }
  }
}
