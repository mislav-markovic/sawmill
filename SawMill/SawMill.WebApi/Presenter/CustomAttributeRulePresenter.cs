using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.WebApi.ViewModel.CustomAttributeRule;

namespace SawMill.WebApi.Presenter
{
  public class CustomAttributeRulePresenter : IPresenter<CustomAttributeRuleViewModel, GeneralRule>
  {
    public Task<CustomAttributeRuleViewModel> Present(GeneralRule model)
    {
      return Task.FromResult(new CustomAttributeRuleViewModel(model.Id, model.Name, model.Description,
        model.Matcher.ToString(), model.StartAnchor, model.EndAnchor));
    }

    public Task<GeneralRule> Request(CustomAttributeRuleViewModel viewModel)
    {
      var generalRule = new GeneralRule
      {
        Id = viewModel.Id,
        Description = viewModel.Description,
        Name = viewModel.Name,
        EndAnchor = viewModel.EndAnchor,
        StartAnchor = viewModel.StartAnchor,
        Matcher = new Regex(viewModel.Matcher)
      };

      return Task.FromResult(generalRule);
    }
  }
}