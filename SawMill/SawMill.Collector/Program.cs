using System;
using Topshelf;

namespace SawMill.Collector
{
  public class Program
  {
    private static void Main(string[] args)
    {
      var configPath = string.Empty;
      ;

      var rc = HostFactory.Run(x =>
      {
        x.AddCommandLineDefinition("config", path => configPath = path);
        x.Service<FileWatcher>(s =>
        {
          s.ConstructUsing(name => new FileWatcher(configPath));
          s.WhenStarted(tc => tc.Start());
          s.WhenStopped(tc => tc.Stop());
        });
        x.RunAsLocalSystem();
        x.StartAutomatically();

        x.SetDescription("Collects log files and sends them to Sawmill server for analysis");
        x.SetDisplayName("Sawmill.FileWatcher");
        x.SetServiceName("Sawmill.FileWatcher");
      });

      var exitCode = (int) Convert.ChangeType(rc, rc.GetTypeCode());
      Environment.ExitCode = exitCode;
    }
  }
}