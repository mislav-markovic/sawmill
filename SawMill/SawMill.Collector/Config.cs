using System.Collections.Generic;
using System.IO;
using Microsoft.Extensions.Configuration.Ini;
using Microsoft.Extensions.FileProviders;

namespace SawMill.Collector
{
  public class Config
  {
    public ICollection<Component> Components;

    public Config(string configFilePath)
    {
      var fileProvider = new PhysicalFileProvider(Path.GetDirectoryName(configFilePath));
      var configSource = new IniConfigurationSource
        {Path = Path.GetFileName(configFilePath), FileProvider = fileProvider};

      var iniReader = new IniConfigurationProvider(configSource);
      iniReader.Load();

      iniReader.TryGet(FormatIniPath(Section.General, GeneralKeys.Frequency), out var frequency);
      iniReader.TryGet(FormatIniPath(Section.General, GeneralKeys.ApiUrl), out var url);

      FrequencyInSeconds = int.Parse(frequency);
      ApiUrl = url;

      Components = new List<Component>();

      foreach (var componentKey in iniReader.GetChildKeys(new string[0], Section.Components))
      {
        iniReader.TryGet(FormatIniPath(Section.Components, componentKey), out var dir);
        var patternExists = iniReader.TryGet(FormatIniPath(Section.FilePatterns, componentKey), out var pattern);
        pattern = patternExists ? pattern : "*";

        Components.Add(new Component(componentKey, dir, pattern));
      }
    }

    public int FrequencyInSeconds { get; }
    public int FrequencyInMilliseconds => FrequencyInSeconds * 1000;
    public string ApiUrl { get; }

    private static string FormatIniPath(string section, string key)
    {
      return $"{section}:{key}";
    }

    internal static class Section
    {
      public const string FilePatterns = "patterns";
      public const string General = "general";
      public const string Components = "components";
    }

    internal static class GeneralKeys
    {
      public const string ApiUrl = "url";
      public const string Frequency = "frequency";
    }
  }
}