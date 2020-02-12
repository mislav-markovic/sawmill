namespace SawMill.Processor.Model
{
  public class Component
  {
    public Component(int id, string name, int? parsingRulesId, int? systemId, string description) : this(id, name,
      parsingRulesId, null, systemId, description)
    {
    }

    public Component(
      int id,
      string name,
      int? parsingRulesId,
      ParsingRules parsingRule,
      int? systemId,
      string description)
    {
      Id = id;
      Name = name;
      ParsingRulesId = parsingRulesId;
      ParsingRule = parsingRule;
      SystemId = systemId;
      Description = description;
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public int? ParsingRulesId { get; set; }
    public ParsingRules ParsingRule { get; set; }
    public int? SystemId { get; set; }
  }
}