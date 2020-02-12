using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.ViewModel.System;

namespace SawMill.WebApi.Presenter
{
  public class SystemPresenter : IPresenter<SystemViewModel, Processor.Model.System>
  {
    private readonly IComponentService _componentService;

    public SystemPresenter(IComponentService componentService)
    {
      _componentService = componentService;
    }

    public Task<SystemViewModel> Present(Processor.Model.System model)
    {
      return Task.FromResult(new SystemViewModel(model.Id, model.Name, model.Description,
        model.Components.Select(elem => elem.Id)));
    }

    public async Task<Processor.Model.System> Request(SystemViewModel viewModel)
    {
      var components = new List<Component>();
      if (viewModel.ComponentIds != null)
      {
        foreach (var componentId in viewModel.ComponentIds)
        {
          var comp = await _componentService.Get(componentId);
          components.Add(comp);
        }
      }

      var model = new Processor.Model.System
      {
        Name = viewModel.Name ?? string.Empty,
        Description = viewModel.Description ?? string.Empty,
        Components = components,
        Id = viewModel.Id
      };

      return model;
    }
  }
}