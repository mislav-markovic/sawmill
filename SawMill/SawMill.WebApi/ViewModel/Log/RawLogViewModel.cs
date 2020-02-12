namespace SawMill.WebApi.ViewModel.Log
{
  public class RawLogViewModel
  {
    public RawLogViewModel(int id, string message, int componentId)
    {
      Id = id;
      Message = message;
      ComponentId = componentId;
    }

    public int Id { get; }
    public string Message { get; }
    public int ComponentId { get; }
  }
}