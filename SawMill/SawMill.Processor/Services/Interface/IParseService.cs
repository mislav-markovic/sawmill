using System;
using System.Collections.Generic;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface IParseService
  {
    DateTime ParseDateTime(string line, DateTimeRule rule);
    SeverityLevel ParseSeverityLevel(string line, SeverityRule rule);
    string ParseMessage(string line, MessageRule rule);
    string ParseCustomAttribute(string line, GeneralRule customAttributeRule);

    NormalizedLog ParseAll(string line, DateTimeRule dateTimeRule, SeverityRule severityRule, MessageRule messageRule,
      IEnumerable<GeneralRule> customAttributeRules);

    NormalizedLog ParseAll(RawLog line, ParsingRules rules);
  }
}