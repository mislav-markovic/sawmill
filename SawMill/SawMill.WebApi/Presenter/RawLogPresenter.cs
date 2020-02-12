using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.WebApi.ViewModel.Log;

namespace SawMill.WebApi.Presenter
{
  public class RawLogPresenter : IPresenter<RawLogViewModel, RawLog>
  {
    public Task<RawLogViewModel> Present(RawLog model)
    {
      return Task.FromResult(new RawLogViewModel(model.Id, model.Message, model.ParentComponentId));
    }

    public Task<RawLog> Request(RawLogViewModel viewModel)
    {
      return Task.FromResult(new RawLog
        {Id = viewModel.Id, Message = viewModel.Message, ParentComponentId = viewModel.ComponentId});
    }
  }
}