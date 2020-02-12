using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using SawMill.Data.Models;
using SawMill.Processor.Model;
using Alert = SawMill.Processor.Model.Alert;
using AlertGroup = SawMill.Processor.Model.AlertGroup;
using AlertGroupValue = SawMill.Processor.Model.AlertGroupValue;
using AlertValue = SawMill.Processor.Model.AlertValue;
using Component = SawMill.Processor.Model.Component;
using ComponentReport = SawMill.Processor.Model.ComponentReport;
using ComponentReportAlert = SawMill.Processor.Model.ComponentReportAlert;
using CustomAttributeValue = SawMill.Processor.Model.CustomAttributeValue;
using DateTimeRule = SawMill.Processor.Model.DateTimeRule;
using GeneralRule = SawMill.Processor.Model.GeneralRule;
using MessageRule = SawMill.Processor.Model.MessageRule;
using NormalizedLog = SawMill.Processor.Model.NormalizedLog;
using ParsingRules = SawMill.Processor.Model.ParsingRules;
using RawLog = SawMill.Processor.Model.RawLog;
using Settings = SawMill.Processor.Model.Settings;
using SeverityLevel = SawMill.Processor.Model.SeverityLevel;
using SystemReport = SawMill.Processor.Model.SystemReport;
using SystemReportAlertGroup = SawMill.Processor.Model.SystemReportAlertGroup;

namespace SawMill.Data.Converters
{
  public static class ToBusinessModelExtensions
  {
    public static Processor.Model.SystemReport ToBusinessModel(this Models.SystemReport model)
    {
      if (model == null)
      {
        return null;
      }

      var componentReports = model.ComponentReports.Select(componentReport => componentReport.ToBusinessModel()).ToList();
      var systemReportAlertGroups = model.SystemReportAlertGroup.Select(e => e.ToBusinessModel()).ToList();

      var report = new Processor.Model.SystemReport
      {
        Id = model.SystemReportId,
        SystemId = model.SystemId,
        Timestamp = model.Timestamp,
        ComponentReports = componentReports,
        SystemReportAlertGroup = systemReportAlertGroups
      };

      return report;
    }

    public static ComponentReport ToBusinessModel(this Models.ComponentReport model)
    {
      if (model == null)
      {
        return null;
      }

      var reportAlert = model.ComponentReportAlert.Select(e => e.ToBusinessModel()).ToList();

      var report = new ComponentReport
      {
        Id = model.ComponentReportId,
        ComponentId = model.ComponentId,
        Timestamp = model.Timestamp,
        SystemReportId = model.SystemReportId,
        ComponentReportAlert = reportAlert
      };

      return report;
    }

    public static ComponentReportAlert ToBusinessModel(this Models.ComponentReportAlert model)
    {
      if (model == null)
      {
        return null;
      }

      var report = new ComponentReportAlert
      {
        Id = model.ComponentReportAlertId,
        ComponentReportId = model.ComponentReportId,
        AlertId = model.AlertId,
        Count = model.Count
      };

      return report;
    }

    public static SystemReportAlertGroup ToBusinessModel(this Models.SystemReportAlertGroup model)
    {
      if (model == null)
      {
        return null;
      }

      var result = new SystemReportAlertGroup
      {
        Id = model.SystemReportAlertGroupId,
        SystemReportId = model.SystemReportId,
        AlertGroupId = model.AlertGroupId,
        Count = model.Count
      };

      return result;
    }

    public static Settings ToBusinessModel(this Models.Settings model)
    {
      if (model == null)
      {
        return null;
      }

      return new Settings
      {
        Id = model.SettingsId,
        Name = model.Name,
        Frequency = model.Frequency
      };
    }

    public static Processor.Model.System ToBusinessModel(this Models.System model)
    {
      if (model == null)
      {
        return null;
      }

      return new Processor.Model.System(model.SystemId, model.SystemName,
        model.Component.Select(comp => comp.ToBusinessModel()).ToList(), model.SystemDescription);
    }

    public static Component ToBusinessModel(this Models.Component model)
    {
      if (model == null)
      {
        return null;
      }

      return new Component(model.ComponentId, model.ComponentName, model.ParsingRulesId,
        null, model.SystemId, model.ComponentDescription);
    }

    public static ParsingRules ToBusinessModel(this Models.ParsingRules model)
    {
      if (model == null)
      {
        return null;
      }

      var customAttributes = new HashSet<GeneralRule>();
      if (model.CustomAttributeRuleParsingRules.Any())
      {
        customAttributes = model.CustomAttributeRuleParsingRules
          .Select(customAttributeRule => customAttributeRule.GeneralRule.ToBusinessModel()).ToHashSet();
      }

      var components = new List<Component>();
      if (model.Components != null && model.Components.Any())
      {
        foreach (var component in model.Components) components.Add(component.ToBusinessModel());
      }

      return new ParsingRules
      {
        Id = model.ParsingRulesId,
        DateTimeRuleId = model.DateTimeRuleId, TimestampParsingRule = model.DateTimeRule?.ToBusinessModel(),
        MessageRuleId = model.MessageRuleId, MessageParsingRule = model.MessageRule?.ToBusinessModel(),
        SeverityRuleId = model.SeverityLevelRuleId, SeverityParsingRule = model.SeverityLevelRule?.ToBusinessModel(),
        Components = components,
        CustomAttributeRules = customAttributes
      };
    }

    public static GeneralRule ToBusinessModel(this Models.GeneralRule model)
    {
      if (model == null)
      {
        return null;
      }

      return new GeneralRule
      {
        Id = model.GeneralRuleId, Description = model.GeneralRuleDescription, Name = model.GeneralRuleName,
        EndAnchor = model.GeneralRuleEndAnchor, StartAnchor = model.GeneralRuleStartAnchor,
        Matcher = new Regex(model.GeneralRuleMatcher)
      };
    }

    public static DateTimeRule ToBusinessModel(this Models.DateTimeRule model)
    {
      if (model == null)
      {
        return null;
      }


      return new DateTimeRule
      {
        Id = model.DateTimeRuleId, GeneralRuleId = model.GeneralRuleId, DateFormat = model.DateTimeFormat,
        GeneralRule = model.GeneralRule.ToBusinessModel()
      };
    }

    public static SeverityRule ToBusinessModel(this SeverityLevelRule model)
    {
      if (model == null)
      {
        return null;
      }

      return new SeverityRule
      {
        Id = model.SeverityLevelRuleId, GeneralRuleId = model.GeneralRuleId, Trace = new Regex(model.Trace),
        Debug = new Regex(model.Debug),
        Info = new Regex(model.Info), Warning = new Regex(model.Warning), Error = new Regex(model.Error),
        Fatal = new Regex(model.Fatal),
        GeneralRule = model.GeneralRule.ToBusinessModel()
      };
    }

    public static MessageRule ToBusinessModel(this Models.MessageRule model)
    {
      if (model == null)
      {
        return null;
      }

      return new MessageRule
      {
        Id = model.MessageRuleId, GeneralRuleId = model.GeneralRuleId, MaxLength = model.MaxLength,
        GeneralRule = model.GeneralRule.ToBusinessModel()
      };
    }


    public static CustomAttributeValue ToBusinessModel(this Models.CustomAttributeValue model)
    {
      if (model == null)
      {
        return null;
      }

      return new CustomAttributeValue
      {
        Id = model.CustomAttributeValueId, Value = model.Value, CustomAttributeRuleId = model.GeneralRuleId,
        NormalizedLogId = model.NormalizedLogId,
        GeneralRule = model.GeneralRule?.ToBusinessModel()
      };
    }

    public static NormalizedLog ToBusinessModel(this Models.NormalizedLog model)
    {
      if (model == null)
      {
        return null;
      }

      var customValues = new List<CustomAttributeValue>();

      if (model.CustomAttributeValues != null && model.CustomAttributeValues.Any())
      {
        foreach (var customAttributeValue in model.CustomAttributeValues)
          customValues.Add(customAttributeValue.ToBusinessModel());
      }


      var normalizedLog = new NormalizedLog(model.Timestamp, model.SeverityLevel.ToBusinessModel(),
        model.Message, model.ComponentId, model.RawLogId, model.NormalizedLogId, customValues.ToArray());

      return normalizedLog;
    }

    public static RawLog ToBusinessModel(this Models.RawLog model)
    {
      if (model == null)
      {
        return null;
      }

      var rawLog = new RawLog
      {
        Id = model.RawLogId, ParentComponentId = model.ComponentId, Message = model.Message,
        ParentComponent = model.Component.ToBusinessModel()
      };

      return rawLog;
    }

    public static Alert ToBusinessModel(this Models.Alert model)
    {
      if (model == null)
      {
        return null;
      }

      var alertValues = new List<AlertValue>();

      if (model.AlertValues != null && model.AlertValues.Any())
      {
        alertValues.AddRange(model.AlertValues.Select(ToBusinessModel));
      }

      var alert = new Alert
      {
        Id = model.AlertId,
        Value = new Regex(model.Value),
        ComponentId = model.ComponentId,
        Name = model.Name,
        Description = model.Description,
        GeneralRuleId = model.GeneralRuleId, Threshold = model.Threshold,
        Timespan = TimeSpan.FromSeconds(model.Timespan),
        AlertValues = alertValues,
        GeneralRule = model.GeneralRule.ToBusinessModel(),
        HasConstantValue = model.HasConstantValue
      };

      return alert;
    }

    public static AlertValue ToBusinessModel(this Models.AlertValue model)
    {
      if (model == null)
      {
        return null;
      }

      var alertValue = new AlertValue
      {
        Id = model.AlertValueId,
        AlertId = model.AlertId,
        TimespanStart = model.TimespanStart,
        TimespanEnd = model.TimespanEnd,
        AlertGroupValueId = model.AlertGroupValueId,
        ConstantValue = model.ConstantValue
      };

      return alertValue;
    }

    public static AlertGroup ToBusinessModel(this Models.AlertGroup model)
    {
      if (model == null)
      {
        return null;
      }

      var alerts = new List<Alert>();

      if (model.AlertGroupAlert != null && model.AlertGroupAlert.Any())
      {
        alerts.AddRange(model.AlertGroupAlert.Select(alert =>
        {
          var temp = alert.Alert.ToBusinessModel();
          temp.Position = alert.Position;
          temp.Not = alert.Not;
          return temp;
        }));
      }

      var alertGroupValues = new List<AlertGroupValue>();

      if (model.AlertGroupValues != null && model.AlertGroupValues.Any())
      {
        alertGroupValues.AddRange(model.AlertGroupValues.Select(value => value.ToBusinessModel()));
      }

      var alertGroup = new AlertGroup
      {
        Id = model.AlertGroupId,
        SystemId = model.SystemId,
        Description = model.Description,
        Name = model.Name,
        Alerts = alerts,
        AlertGroupValues = alertGroupValues,
        CorrelationWindow = TimeSpan.FromSeconds(model.CorrelationWindow)
      };

      if (model.CorrelationWindow == 0)
      {
        alertGroup.CorrelationWindow = null;
      }

      return alertGroup;
    }

    public static AlertGroupValue ToBusinessModel(this Models.AlertGroupValue model)
    {
      if (model == null)
      {
        return null;
      }

      var alertValues = model.AlertValues.Select(elem => elem.ToBusinessModel()).ToList();

      var alertGroupValue = new AlertGroupValue
      {
        Id = model.AlertGroupValueId,
        AlertGroupId = model.AlertGroupId,
        TimespanEnd = model.TimespanEnd,
        TimespanStart = model.TimespanStart,
        AlertValues = alertValues
      };

      return alertGroupValue;
    }

    public static SeverityLevel ToBusinessModel(this Models.SeverityLevel severityLevel)
    {
      if (severityLevel.SeverityLevelId.Equals((int) SeverityLevel.Debug))
      {
        return SeverityLevel.Debug;
      }

      if (severityLevel.SeverityLevelId.Equals((int) SeverityLevel.Trace))
      {
        return SeverityLevel.Trace;
      }

      if (severityLevel.SeverityLevelId.Equals((int) SeverityLevel.Info))
      {
        return SeverityLevel.Info;
      }

      if (severityLevel.SeverityLevelId.Equals((int) SeverityLevel.Warning))
      {
        return SeverityLevel.Warning;
      }

      if (severityLevel.SeverityLevelId.Equals((int) SeverityLevel.Error))
      {
        return SeverityLevel.Error;
      }

      if (severityLevel.SeverityLevelId.Equals((int) SeverityLevel.Fatal))
      {
        return SeverityLevel.Fatal;
      }

      throw new ArgumentOutOfRangeException();
    }
  }
}