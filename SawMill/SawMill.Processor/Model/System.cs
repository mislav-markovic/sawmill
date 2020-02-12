using System.Collections.Generic;

namespace SawMill.Processor.Model
{
  public class System
  {
    public System(int id, string name, IList<Component> components, string description)
    {
      Id = id;
      Name = name;
      Description = description;
      Components = components ?? new List<Component>();
    }

    public System(int id, string name, string description) : this(id, name, new List<Component>(), description)
    {
    }

    public System()
    {
      Components = new List<Component>();
    }

    public int Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public IList<Component> Components { get; set; }
  }
}