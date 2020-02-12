using System.Collections.Generic;

namespace SawMill.WebApi.ViewModel.System
{
  public class SystemViewModel
  {
    public SystemViewModel(int id, string name, string description, IEnumerable<int> componentIds)
    {
      Id = id;
      Name = name;
      Description = description;
      ComponentIds = componentIds;
    }

    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public IEnumerable<int> ComponentIds { get; }
  }
}