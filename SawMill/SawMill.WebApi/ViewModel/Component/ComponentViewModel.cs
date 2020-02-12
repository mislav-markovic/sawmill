namespace SawMill.WebApi.ViewModel.Component
{
  public class ComponentViewModel
  {
    public ComponentViewModel(int id, string name, int systemId, string description, int? parsingRulesId)
    {
      Id = id;
      Name = name;
      SystemId = systemId;
      Description = description;
      ParsingRulesId = parsingRulesId;
    }

    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public int SystemId { get; }
    public int? ParsingRulesId { get; }
  }
}