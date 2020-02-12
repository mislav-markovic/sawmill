using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.WebApi.ViewModel.AlertValue;

namespace SawMill.WebApi.Presenter
{
  public class AlertValuePresenter : IPresenter<AlertValueViewModel, AlertValue>
  {
    public Task<AlertValueViewModel> Present(AlertValue model)
    {
      return Task.FromResult(new AlertValueViewModel(model.Id, model.AlertId, model.TimespanStart, model.TimespanEnd));
    }

    public Task<AlertValue> Request(AlertValueViewModel viewModel)
    {
      return Task.FromResult(new AlertValue
      {
        Id = viewModel.Id, AlertId = viewModel.AlertId, TimespanStart = viewModel.TimespanStart,
        TimespanEnd = viewModel.TimespanEnd
      });
    }
  }
}