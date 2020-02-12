using System;
using System.Collections.Generic;

namespace SawMill.Processor.Model
{
  public class NormalizedLog
  {
    public NormalizedLog(
      DateTime dateTime,
      SeverityLevel severity,
      string message,
      int componentId,
      int rawLogId,
      int id,
      params CustomAttributeValue[] customAttributeValues)
    {
      DateTime = dateTime;
      Severity = severity;
      Message = message;
      ComponentId = componentId;
      RawLogId = rawLogId;
      Id = id;
      CustomAttributeValues = new HashSet<CustomAttributeValue>();
      foreach (var customAttribute in customAttributeValues)
        if (customAttribute != null)
        {
          customAttribute.NormalizedLog = this;
          customAttribute.NormalizedLogId = Id;
          CustomAttributeValues.Add(customAttribute);
        }
    }

    public int Id { get; set; }
    public DateTime DateTime { get; set; }
    public SeverityLevel Severity { get; set; }
    public string Message { get; set; }
    public int ComponentId { get; set; }

    public int RawLogId { get; set; }

    // Custom attributes must be unique by name
    public ISet<CustomAttributeValue> CustomAttributeValues { get; set; }
  }
}