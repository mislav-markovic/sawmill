using System;
using System.Collections.Generic;
using System.Linq;
using SawMill.Data.Models;
using SawMill.Processor.Model;
using Alert = SawMill.Data.Models.Alert;
using AlertGroup = SawMill.Data.Models.AlertGroup;
using AlertGroupValue = SawMill.Data.Models.AlertGroupValue;
using AlertValue = SawMill.Data.Models.AlertValue;
using Component = SawMill.Data.Models.Component;
using CustomAttributeValue = SawMill.Data.Models.CustomAttributeValue;
using DateTimeRule = SawMill.Data.Models.DateTimeRule;
using GeneralRule = SawMill.Data.Models.GeneralRule;
using MessageRule = SawMill.Data.Models.MessageRule;
using NormalizedLog = SawMill.Data.Models.NormalizedLog;
using ParsingRules = SawMill.Data.Models.ParsingRules;
using RawLog = SawMill.Data.Models.RawLog;
using Settings = SawMill.Data.Models.Settings;
using SeverityLevel = SawMill.Data.Models.SeverityLevel;

namespace SawMill.Data.Converters
{
  public static class ToDomainModelExtensions
  {
    public static Settings ToDomainModel(this Processor.Model.Settings model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new Settings
      {
        SettingsId = model.Id,
        Frequency = model.Frequency,
        Name = model.Name
      };

      return dto;
    }

    public static Models.System ToDomainModel(this Processor.Model.System model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new Models.System
      {
        SystemId = model.Id, SystemDescription = model.Description, SystemName = model.Name,
        Component = model.Components?.Select(c => c.ToDomainModel()).ToArray()
      };
      return dto;
    }

    public static Component ToDomainModel(this Processor.Model.Component model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new Component
      {
        ComponentId = model.Id, SystemId = model.SystemId, ComponentName = model.Name,
        ParsingRulesId = model.ParsingRulesId, ComponentDescription = model.Description
      };
      return dto;
    }

    public static ParsingRules ToDomainModel(this Processor.Model.ParsingRules model)
    {
      if (model == null)
      {
        return null;
      }

      var customAttributeRules = new List<CustomAttributeRuleParsingRules>();
      if (model.CustomAttributeRules != null && model.CustomAttributeRules.Any())
      {
        customAttributeRules =
          model.CustomAttributeRules.Select(customAttributeRule => new CustomAttributeRuleParsingRules
            {GeneralRuleId = customAttributeRule.Id, ParsingRulesId = model.Id}).ToList();
      }

      var components = new List<Component>();
      if (model.Components != null && model.Components.Any())
      {
        foreach (var component in model.Components) components.Add(component.ToDomainModel());
      }

      var dto = new ParsingRules
      {
        ParsingRulesId = model.Id, DateTimeRuleId = model.DateTimeRuleId, MessageRuleId = model.MessageRuleId,
        SeverityLevelRuleId = model.SeverityRuleId, Components = components,
        CustomAttributeRuleParsingRules = customAttributeRules
      };

      return dto;
    }

    public static CustomAttributeValue ToDomainModel(this Processor.Model.CustomAttributeValue model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new CustomAttributeValue
      {
        GeneralRuleId = model.CustomAttributeRuleId, Value = model.Value,
        NormalizedLogId = model.NormalizedLogId, CustomAttributeValueId = model.Id
      };

      return dto;
    }

    public static MessageRule ToDomainModel(this Processor.Model.MessageRule model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new MessageRule
        {GeneralRuleId = model.GeneralRuleId, MaxLength = model.MaxLength, MessageRuleId = model.Id};

      return dto;
    }

    public static DateTimeRule ToDomainModel(this Processor.Model.DateTimeRule model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new DateTimeRule
        {DateTimeRuleId = model.Id, GeneralRuleId = model.GeneralRuleId, DateTimeFormat = model.DateFormat};

      return dto;
    }

    public static SeverityLevelRule ToDomainModel(this SeverityRule model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new SeverityLevelRule
      {
        SeverityLevelRuleId = model.Id, GeneralRuleId = model.GeneralRuleId, Debug = model.Debug.ToString(),
        Error = model.Error.ToString(),
        Fatal = model.Fatal.ToString(), Info = model.Info.ToString(), Trace = model.Trace.ToString(),
        Warning = model.Warning.ToString()
      };

      return dto;
    }

    public static GeneralRule ToDomainModel(this Processor.Model.GeneralRule model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new GeneralRule
      {
        GeneralRuleId = model.Id, GeneralRuleDescription = model.Description,
        GeneralRuleMatcher = model.Matcher.ToString(), GeneralRuleName = model.Name,
        GeneralRuleStartAnchor = model.StartAnchor, GeneralRuleEndAnchor = model.EndAnchor
      };

      return dto;
    }

    public static NormalizedLog ToDomainModel(this Processor.Model.NormalizedLog model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new NormalizedLog
      {
        NormalizedLogId = model.Id,
        RawLogId = model.RawLogId,
        ComponentId = model.ComponentId,
        Message = model.Message,
        Timestamp = model.DateTime,
        SeverityLevelId = model.Severity.ToDomainModel().SeverityLevelId
      };
      return dto;
    }

    public static RawLog ToDomainModel(this Processor.Model.RawLog model)
    {
      if (model == null)
      {
        return null;
      }

      var dto = new RawLog {RawLogId = model.Id, ComponentId = model.ParentComponentId, Message = model.Message};

      return dto;
    }

    public static Alert ToDomainModel(this Processor.Model.Alert model)
    {
      if (model == null)
      {
        return null;
      }

      var alertValues = new List<AlertValue>();

      if (model.AlertValues != null && model.AlertValues.Any())
      {
        alertValues.AddRange(model.AlertValues.Select(ToDomainModel));
      }

      var alert = new Alert
      {
        AlertId = model.Id,
        Value = model.Value.ToString(),
        ComponentId = model.ComponentId,
        Name = model.Name,
        Description = model.Description,
        Timespan = (long) model.Timespan.TotalSeconds,
        Threshold = model.Threshold,
        GeneralRuleId = model.GeneralRuleId,
        AlertValues = alertValues,
        HasConstantValue = model.HasConstantValue
      };

      return alert;
    }

    public static AlertValue ToDomainModel(this Processor.Model.AlertValue model)
    {
      if (model == null)
      {
        return null;
      }

      var alertValue = new AlertValue
      {
        AlertId = model.AlertId,
        AlertValueId = model.Id,
        TimespanStart = model.TimespanStart,
        TimespanEnd = model.TimespanEnd,
        AlertGroupValueId = model.AlertGroupValueId,
        ConstantValue = model.ConstantValue
      };

      return alertValue;
    }

    public static AlertGroup ToDomainModel(this Processor.Model.AlertGroup model)
    {
      if (model == null)
      {
        return null;
      }

      var alertGroupAlert = new List<AlertGroupAlert>();

      foreach (var modelAlert in model.Alerts)
      {
        var temp = new AlertGroupAlert{AlertId = modelAlert.Id, Position = modelAlert.Position.Value, AlertGroupId = modelAlert.AlertGroupId.Value, Not = modelAlert.Not};
        alertGroupAlert.Add(temp);
      }

      var alertGroup = new AlertGroup
      {
        AlertGroupId = model.Id,
        SystemId = model.SystemId,
        Description = model.Description,
        Name = model.Name,
        CorrelationWindow = model.CorrelationWindow.HasValue ? (long) model.CorrelationWindow.Value.TotalSeconds : 0,
        AlertGroupAlert = alertGroupAlert
      };

      return alertGroup;
    }

    public static AlertGroupValue ToDomainModel(this Processor.Model.AlertGroupValue model)
    {
      if (model == null)
      {
        return null;
      }

      var alertGroupValue = new AlertGroupValue
      {
        AlertGroupValueId = model.Id,
        AlertGroupId = model.AlertGroupId,
        TimespanStart = model.TimespanStart,
        TimespanEnd = model.TimespanEnd
      };

      return alertGroupValue;
    }

    public static SeverityLevel ToDomainModel(this Processor.Model.SeverityLevel severityLevel)
    {
      switch (severityLevel)
      {
        case Processor.Model.SeverityLevel.Debug:
          return new SeverityLevel
            {SeverityLevelId = (int) Processor.Model.SeverityLevel.Debug, SeverityLevelValue = "Debug"};
        case Processor.Model.SeverityLevel.Trace:
          return new SeverityLevel
            {SeverityLevelId = (int) Processor.Model.SeverityLevel.Trace, SeverityLevelValue = "Trace"};
        case Processor.Model.SeverityLevel.Info:
          return new SeverityLevel
            {SeverityLevelId = (int) Processor.Model.SeverityLevel.Info, SeverityLevelValue = "Info"};
        case Processor.Model.SeverityLevel.Warning:
          return new SeverityLevel
            {SeverityLevelId = (int) Processor.Model.SeverityLevel.Warning, SeverityLevelValue = "Warning"};
        case Processor.Model.SeverityLevel.Error:
          return new SeverityLevel
            {SeverityLevelId = (int) Processor.Model.SeverityLevel.Error, SeverityLevelValue = "Error"};
        case Processor.Model.SeverityLevel.Fatal:
          return new SeverityLevel
            {SeverityLevelId = (int) Processor.Model.SeverityLevel.Fatal, SeverityLevelValue = "Fatal"};
        default:
          throw new ArgumentOutOfRangeException(nameof(severityLevel), severityLevel, null);
      }
    }
  }
}