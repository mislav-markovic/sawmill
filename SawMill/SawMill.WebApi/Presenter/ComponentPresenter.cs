using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;
using SawMill.WebApi.ViewModel.Component;

namespace SawMill.WebApi.Presenter
{
  public class ComponentPresenter : IPresenter<ComponentViewModel, Component>
  {
    private readonly IParsingRulesService _parsingRulesService;

    public ComponentPresenter(IParsingRulesService parsingRulesService)
    {
      _parsingRulesService = parsingRulesService;
    }

    public Task<ComponentViewModel> Present(Component model)
    {
      return Task.FromResult(new ComponentViewModel(model.Id, model.Name, model.SystemId.GetValueOrDefault(0),
        model.Description, model.ParsingRulesId));
    }

    public async Task<Component> Request(ComponentViewModel viewModel)
    {
      ParsingRules rules = null;
      try
      {
        rules = await _parsingRulesService.GetComponentsParsingRules(viewModel.Id);
      }
      catch
      {
      }

      int? systemId = viewModel.SystemId;
      if (systemId.Value == 0)
      {
        systemId = null;
      }

      return new Component(viewModel.Id, viewModel.Name, rules?.Id, systemId, viewModel.Description);
    }
  }
}