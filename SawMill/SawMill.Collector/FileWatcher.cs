using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace SawMill.Collector
{
  public class FileWatcher
  {
    private readonly Config _config;
    private AutoResetEvent _timer;

    public FileWatcher(string configFilePath)
    {
      Console.WriteLine("Started Service");
      _config = new Config(configFilePath);
    }

    private static IEnumerable<string> NewFiles(IEnumerable<string> existingFiles, Component component)
    {
      var allFiles = component.MyFiles();
      return allFiles.Except(existingFiles);
    }

    private static IEnumerable<string> RemovedFiles(IEnumerable<string> existingFiles, Component component)
    {
      var allFiles = component.MyFiles();
      return existingFiles.Except(allFiles);
    }

    private static Dictionary<string, long> UpdateFilesAndSizes(Dictionary<string, long> currentFilesAndSizes,
      Component component)
    {
      var removedFiles = RemovedFiles(currentFilesAndSizes.Keys.ToList(), component).ToList();
      if (removedFiles.Any())
      {
        foreach (var removedFile in removedFiles) currentFilesAndSizes.Remove(removedFile);
      }

      var newFiles = NewFiles(currentFilesAndSizes.Keys.ToList(), component).ToList();
      if (newFiles.Any())
      {
        foreach (var newFile in newFiles) currentFilesAndSizes.Add(newFile, 0);
      }

      return currentFilesAndSizes;
    }

    public void Start()
    {
      _timer = new AutoResetEvent(false);
      var componentsAndFiles = _config.Components.ToDictionary(component => component,
        component => component.MyFiles().ToDictionary(path => path, s => 0L));
      var apiClient = new RestClient(_config.ApiUrl);

      while (!_timer.WaitOne(_config.FrequencyInMilliseconds))
      {
        var components = componentsAndFiles.Keys.ToList();
        foreach (var component in components)
        {
          componentsAndFiles[component] = UpdateFilesAndSizes(componentsAndFiles[component], component);
          foreach (var (path, size) in componentsAndFiles[component].ToDictionary(pair => pair.Key, pair => pair.Value))
          {
            var file = new FileInfo(path);
            if (size < file.Length)
            {
              var newSize = size;
              var lines = new List<string>();
              using (var fs = new FileStream(file.FullName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
              {
                fs.Seek(newSize, SeekOrigin.Begin);

                using (var stringReader = new StreamReader(fs))
                {
                  while (stringReader.Peek() > 0)
                  {
                    var line = stringReader.ReadLine();

                    if (!string.IsNullOrEmpty(line))
                    {
                      lines.Add(line);
                    }
                  }

                  newSize = file.Length;
                  componentsAndFiles[component][path] = newSize;
                  apiClient.SendLog(lines, component.Name);
                }
              }
            }
          }
        }
      }
    }

    public void Stop()
    {
      _timer.Set();
      _timer.Close();
    }
  }
}