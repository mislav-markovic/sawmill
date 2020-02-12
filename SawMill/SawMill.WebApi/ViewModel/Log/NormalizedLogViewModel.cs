using System;
using System.Collections.Generic;
using System.Linq;
using SawMill.WebApi.ViewModel.CustomAttributeRule;
using SawMill.WebApi.ViewModel.SeverityRule;

namespace SawMill.WebApi.ViewModel.Log
{
  public class NormalizedLogViewModel
  {
    public NormalizedLogViewModel(int id, int componentId, DateTime dateTime, string message,
      SeverityLevelViewModel severityLevel, IEnumerable<CustomAttributeValueViewModel> customValues, int rawLogId)
    {
      Id = id;
      ComponentId = componentId;
      DateTime = dateTime;
      Message = message;
      SeverityLevel = severityLevel;
      CustomValues = customValues;
      RawLogId = rawLogId;

      if (customValues == null || !CustomValues.Any())
      {
        customValues = new CustomAttributeValueViewModel[0];
      }
    }

    public int Id { get; }
    public int ComponentId { get; }
    public int RawLogId { get; }
    public DateTime DateTime { get; }
    public string Message { get; }
    public SeverityLevelViewModel SeverityLevel { get; }
    public IEnumerable<CustomAttributeValueViewModel> CustomValues { get; }
  }
}