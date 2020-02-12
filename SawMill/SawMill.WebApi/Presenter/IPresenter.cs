using System.Threading.Tasks;

namespace SawMill.WebApi.Presenter
{
  public interface IPresenter<TView, TModel>
  {
    Task<TView> Present(TModel model);
    Task<TModel> Request(TView viewModel);
  }
}