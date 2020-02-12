using System.Collections.Generic;

namespace SawMill.Collector
{
  public class Component
  {
    public Component(string name, string directory, string filePattern)
    {
      Name = name;
      Directory = directory;
      FilePattern = filePattern;
    }

    public string Name { get; }
    public string Directory { get; }
    public string FilePattern { get; }

    public IEnumerable<string> MyFiles()
    {
      return System.IO.Directory.EnumerateFiles(Directory, FilePattern);
    }

    protected bool Equals(Component other)
    {
      return Name == other.Name;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj.GetType() != GetType())
      {
        return false;
      }

      return Equals((Component) obj);
    }

    public override int GetHashCode()
    {
      return Name != null ? Name.GetHashCode() : 0;
    }
  }
}