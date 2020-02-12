using System;
using System.Collections.Generic;
using System.Globalization;
using SawMill.Processor.Model;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class ParseService : IParseService
  {
    public DateTime ParseDateTime(string line, DateTimeRule rule)
    {
      line = ApplyAnchors(line, rule.GeneralRule.StartAnchor, rule.GeneralRule.EndAnchor);

      var parsedValue = rule.GeneralRule.Matcher.Match(line);

      if (!parsedValue.Success)
      {
        throw new ArgumentException(
          $"Could not apply matcher '{rule.GeneralRule.Matcher}' to line '{line}' to extract date time");
      }

      if (!string.IsNullOrWhiteSpace(rule.DateFormat))
      {
        var parsedDateTime =
          DateTime.ParseExact(parsedValue.Value.Trim(), rule.DateFormat, CultureInfo.InvariantCulture);

        return parsedDateTime;
      }
      else
      {
        var parsedDateTime = DateTime.Parse(parsedValue.Value);

        return parsedDateTime;
      }
    }

    public SeverityLevel ParseSeverityLevel(string line, SeverityRule rule)
    {
      line = ApplyAnchors(line, rule.GeneralRule.StartAnchor, rule.GeneralRule.EndAnchor);

      var parsedValue = rule.GeneralRule.Matcher.Match(line);

      if (!parsedValue.Success)
      {
        throw new ArgumentException(
          $"Could not apply matcher '{rule.GeneralRule.Matcher}' to line '{line}' to extract severity level");
      }

      if (rule.Fatal.IsMatch(parsedValue.Value))
      {
        return SeverityLevel.Fatal;
      }

      if (rule.Error.IsMatch(parsedValue.Value))
      {
        return SeverityLevel.Error;
      }

      if (rule.Warning.IsMatch(parsedValue.Value))
      {
        return SeverityLevel.Warning;
      }

      if (rule.Info.IsMatch(parsedValue.Value))
      {
        return SeverityLevel.Info;
      }

      if (rule.Trace.IsMatch(parsedValue.Value))
      {
        return SeverityLevel.Trace;
      }

      if (rule.Debug.IsMatch(parsedValue.Value))
      {
        return SeverityLevel.Debug;
      }

      throw new ArgumentException($"Parsed value did not match any defined level: parsed '{parsedValue.Value}'");
    }

    public string ParseMessage(string line, MessageRule rule)
    {
      line = ApplyAnchors(line, rule.GeneralRule.StartAnchor, rule.GeneralRule.EndAnchor);

      var parsedValue = rule.GeneralRule.Matcher.Match(line);

      if (!parsedValue.Success)
      {
        throw new ArgumentException(
          $"Could not apply matcher '{rule.GeneralRule.Matcher}' to line '{line}' to extract message");
      }

      return parsedValue.Value;
    }

    public string ParseCustomAttribute(string line, GeneralRule rule)
    {
      line = ApplyAnchors(line, rule.StartAnchor, rule.EndAnchor);

      var parsedValue = rule.Matcher.Match(line);

      if (!parsedValue.Success)
      {
        return string.Empty;
      }

      return parsedValue.Value.Trim();
    }

    public NormalizedLog ParseAll(string line, DateTimeRule dateTimeRule, SeverityRule severityRule,
      MessageRule messageRule, IEnumerable<GeneralRule> customAttributeRules)
    {
      var dateTime = ParseDateTime(line, dateTimeRule);
      var severity = ParseSeverityLevel(line, severityRule);
      var message = ParseMessage(line, messageRule);
      var customAttributes = new List<CustomAttributeValue>();

      foreach (var customAttributeRule in customAttributeRules)
      {
        var result = ParseCustomAttribute(line, customAttributeRule);
        customAttributes.Add(new CustomAttributeValue
        {
          GeneralRule = customAttributeRule, Id = 0, Value = result,
          CustomAttributeRuleId = customAttributeRule.Id
        });
      }

      // TODO check this
      return new NormalizedLog(dateTime, severity, message, 0, 0, 0, customAttributes.ToArray());
    }

    public NormalizedLog ParseAll(RawLog line, ParsingRules rules)
    {
      var parsed = ParseAll(line.Message, rules.TimestampParsingRule, rules.SeverityParsingRule,
        rules.MessageParsingRule,
        rules.CustomAttributeRules);

      parsed.RawLogId = line.Id;
      parsed.ComponentId = line.ParentComponentId;
      return parsed;
    }

    private static string ApplyAnchors(string line, string start, string end)
    {
      var result = new string(line);

      if (!string.IsNullOrEmpty(start))
      {
        var startIndex = result.IndexOf(start, StringComparison.Ordinal);
        if (startIndex > -1) // se found start anchor
        {
          result = result.Substring(startIndex +
                                    start.Length); // trim everything before start anchor, including start anchor
        }
      }

      if (!string.IsNullOrEmpty(end))
      {
        var endIndex = result.LastIndexOf(end, StringComparison.Ordinal);

        if (endIndex > -1)
        {
          result = result.Substring(0, endIndex);
        }
      }

      return result;
    }
  }
}