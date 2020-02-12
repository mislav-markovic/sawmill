using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.WebApi.ViewModel.Alert;

namespace SawMill.WebApi.Presenter
{
  public class AlertPresenter : IPresenter<AlertViewModel, Alert>
  {
    public Task<AlertViewModel> Present(Alert model)
    {
      var position = model.Position ?? -1;
      var alertGroupId = model.AlertGroupId ?? -1;
      var time = (long) model.Timespan.TotalSeconds;
      var viewModel = new AlertViewModel(model.Id, model.Threshold, model.Value.ToString(), model.ComponentId,
        model.Name, model.Description, time, model.GeneralRuleId, model.HasConstantValue, position, alertGroupId);

      return Task.FromResult(viewModel);
    }

    public Task<Alert> Request(AlertViewModel viewModel)
    {

      var model = new Alert
      {
        Id = viewModel.Id, ComponentId = viewModel.ComponentId, GeneralRuleId = viewModel.GeneralRuleId,
        Name = viewModel.Name, Description = viewModel.Description,
        Timespan = TimeSpan.FromSeconds(viewModel.Timespan), Threshold = viewModel.Threshold,
        Value = new Regex(viewModel.Value),
        HasConstantValue = viewModel.HasConstantValue,
        Position = null,
        AlertGroupId = null,
      };

      if (viewModel.Position > -1)
      {
        model.Position = viewModel.Position;
      }

      if (viewModel.AlertGroupId > -1)
      {
        model.AlertGroupId = viewModel.AlertGroupId;
      }

      return Task.FromResult(model);
    }
  }
}