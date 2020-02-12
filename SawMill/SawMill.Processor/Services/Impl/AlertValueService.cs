using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class AlertValueService : IAlertValueService
  {
    private readonly IAlertRepository _alertRepository;
    private readonly IAlertValueRepository _alertValueRepository;
    private readonly INormalizedLogRepository _normalizedLogRepository;

    public AlertValueService(IAlertValueRepository alertValueRepository, IAlertRepository alertRepository,
      INormalizedLogRepository normalizedLogRepository)
    {
      _alertValueRepository = alertValueRepository;
      _alertRepository = alertRepository;
      _normalizedLogRepository = normalizedLogRepository;
    }

    public async Task<AlertValue> Create(AlertValue model)
    {
      var createdId = await _alertValueRepository.Create(model);
      return await _alertValueRepository.Read(createdId);
    }

    public async Task<AlertValue> Edit(AlertValue model)
    {
      var isUpdated = await _alertValueRepository.Update(model);
      if (isUpdated)
      {
        return await _alertValueRepository.Read(model.Id);
      }

      throw new Exception($"Failed to update alert value {model.Id}");
    }

    public async Task<IEnumerable<AlertValue>> GetAll()
    {
      return await _alertValueRepository.ReadAll();
    }

    public async Task<AlertValue> Get(int id)
    {
      var result = await _alertValueRepository.Read(id);
      if (result == null)
      {
        throw new ArgumentException($"Cannot read alert value {id}", nameof(id));
      }

      return result;
    }

    public async Task Delete(int id)
    {
      var result = await _alertValueRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete alert value {id}", nameof(id));
      }
    }

    public async Task<IEnumerable<AlertValue>> GenerateAlerts(IList<NormalizedLog> logs)
    {
      var componentId = logs.First().ComponentId;
      var componentsAlerts = (await _alertRepository.ReadAll()).Where(elem => elem.ComponentId == componentId).ToList();
      var alertValues = new List<AlertValue>();

      if (componentsAlerts.Any())
      {
        var nonConstantValueAlerts = componentsAlerts.Where(elem => !elem.HasConstantValue).ToList();
        var constantValueAlerts = componentsAlerts.Where(elem => elem.HasConstantValue).ToList();

        var nonConstantAlertValues = await GenerateAlertsNonConstantValue(logs, nonConstantValueAlerts);
        var constantAlertValues = await GenerateAlertsWithConstantValue(logs, constantValueAlerts);


        alertValues.AddRange(nonConstantAlertValues);
        alertValues.AddRange(constantAlertValues);
      }

      return alertValues;
    }

    public async Task<IEnumerable<AlertValue>> AlertValuesForAlert(int alertId)
    {
      var temp = (await _alertValueRepository.ReadAll()).Where(elem => elem.AlertId == alertId).ToList();
      return temp;
    }


    private async Task<IEnumerable<AlertValue>> GenerateAlertsNonConstantValue(IList<NormalizedLog> logs,
      IList<Alert> alerts)
    {
      var componentId = logs.First().ComponentId;
      var alertValues = new List<AlertValue>();


      if (alerts.Any())
      {
        var longestTimespan = alerts.Select(elem => elem.Timespan).Max();
        var firstLogTime = logs.Select(elem => elem.DateTime).Min();
        var lastLogTime = logs.Select(elem => elem.DateTime).Max();
        var startDateTime = firstLogTime - longestTimespan;
        var endDateTime = lastLogTime + longestTimespan;

        var materializedLogs =
          (await _normalizedLogRepository.WithinTimeFrameForComponents(new[] {componentId}, startDateTime, endDateTime,
            null)).OrderBy(elem => elem.DateTime).ToArray();


        foreach (var alert in alerts)
        {
          var earliestEvent = firstLogTime - alert.Timespan;
          var latestEvent = lastLogTime + alert.Timespan;
          var rule = alert.GeneralRule;
          var valueMatcher = alert.Value;

          var logsWithinTimeFrame =
            materializedLogs.Where(elem => elem.DateTime >= earliestEvent && elem.DateTime <= latestEvent).ToArray();

          NormalizedLog startLog = null;

          var eventCount = 0;

          foreach (var log in logsWithinTimeFrame)
          {
            var isMatch = IsMatch(rule.Id, log, valueMatcher, out var matchedValue);

            if (startLog == null)
            {
              if (isMatch)
              {
                startLog = log;
                eventCount++;
              }
            }
            else
            {
              if (isMatch)
              {
                eventCount++;
              }

              if (eventCount >= alert.Threshold)
              {
                // create new alert value and save it to db
                var alertValue = new AlertValue
                {
                  Id = 0, AlertId = alert.Id, TimespanStart = startLog.DateTime, TimespanEnd = log.DateTime,
                  ConstantValue = null
                };
                var createdId = await _alertValueRepository.Create(alertValue);
                alertValues.Add(await _alertValueRepository.Read(createdId));

                // reset event count
                eventCount = 0;
                // start search for new window beginning
                startLog = null;
              }
              else
              {
                var diff = startLog.DateTime - log.DateTime;
                if (diff > alert.Timespan)
                {
                  // find next start, search until new positive match is found or until end of current windows is found
                  var nextLogStart = logsWithinTimeFrame.FirstOrDefault(elem =>
                    elem.DateTime > startLog.DateTime && elem.DateTime <= log.DateTime &&
                    IsMatch(rule.Id, elem, valueMatcher, out var nextConstantValue));

                  // current window is closed, we restart search
                  if (nextLogStart == null || nextLogStart.Id == log.Id)
                  {
                    eventCount = 0;
                    startLog = null;
                  }
                  // we found middle match
                  else
                  {
                    // we lost previous first match
                    eventCount--;

                    startLog = nextLogStart;
                  }
                }
              }
            }
          }
        }
      }

      return alertValues;
    }

    private static (int, NormalizedLog) IncrementCounter(Dictionary<string, (int, NormalizedLog)> dict, string key)
    {
      var tuple = (dict[key].Item1 + 1, dict[key].Item2);
      return tuple;
    }

    private async Task<IEnumerable<AlertValue>> GenerateAlertsWithConstantValue(IList<NormalizedLog> logs,
      IList<Alert> alerts)
    {
      var componentId = logs.First().ComponentId;
      var alertValues = new List<AlertValue>();

      if (alerts.Any())
      {
        var longestTimespan = alerts.Select(elem => elem.Timespan).Max();
        var firstLogTime = logs.Select(elem => elem.DateTime).Min();
        var lastLogTime = logs.Select(elem => elem.DateTime).Max();
        var startDateTime = firstLogTime - longestTimespan;
        var endDateTime = lastLogTime + longestTimespan;

        var materializedLogs =
          (await _normalizedLogRepository
            .WithinTimeFrameForComponents(new[] {componentId}, startDateTime, endDateTime, null)
          ).OrderBy(elem => elem.DateTime).ToArray();


        foreach (var alert in alerts)
        {
          // value => (count, startLog)
          var constantValues = new Dictionary<string, (int, NormalizedLog)>();
          var earliestEvent = firstLogTime - alert.Timespan;
          var latestEvent = lastLogTime + alert.Timespan;
          var rule = alert.GeneralRule;
          var valueMatcher = alert.Value;

          var logsWithinTimeFrame =
            materializedLogs.Where(elem => elem.DateTime >= earliestEvent && elem.DateTime <= latestEvent).ToArray();

          foreach (var log in logsWithinTimeFrame)
          {
            var isMatch = IsMatch(rule.Id, log, valueMatcher, out var matchedValue);

            if (!constantValues.ContainsKey(matchedValue))
            {
              if (isMatch)
              {
                constantValues.Add(matchedValue, (1, log));
              }
            }
            else // constant value was previously found
            {
              if (isMatch)
              {
                constantValues[matchedValue] = IncrementCounter(constantValues, matchedValue);
              }

              if (constantValues[matchedValue].Item1 >= alert.Threshold)
              {
                // create new alert value and save it to db
                var alertValue = new AlertValue
                {
                  Id = 0,
                  AlertId = alert.Id,
                  TimespanStart = constantValues[matchedValue].Item2.DateTime,
                  TimespanEnd = log.DateTime,
                  ConstantValue = matchedValue,
                  AlertGroupValueId = null
                };
                var createdId = await _alertValueRepository.Create(alertValue);
                alertValues.Add(await _alertValueRepository.Read(createdId));

                // we exceeded threshold so we remove value from list
                constantValues.Remove(matchedValue);
              }
              else
              {
                var diff = constantValues[matchedValue].Item2.DateTime - log.DateTime;
                if (diff > alert.Timespan)
                {
                  // find next start, search until new positive match is found or until end of current windows is found
                  var nextLogStart = logsWithinTimeFrame.FirstOrDefault(
                    elem =>
                      elem.DateTime > constantValues[matchedValue].Item2.DateTime &&
                      elem.DateTime <= log.DateTime &&
                      IsMatch(rule.Id, elem, valueMatcher, out var nextConstantValue) &&
                      nextConstantValue.Equals(matchedValue));

                  // current window is closed, we restart search
                  if (nextLogStart == null || nextLogStart.Id == log.Id)
                  {
                    constantValues.Remove(matchedValue);
                  }
                  // we found middle match
                  else
                  {
                    constantValues[matchedValue] = (constantValues[matchedValue].Item1 - 1, nextLogStart);
                  }
                }
              }
            }
          }
        }
      }

      return alertValues;
    }


    private static bool IsMatch(int ruleId, NormalizedLog log, Regex valueMatcher, out string matchedValue)
    {
      // select custom attribute value if it was generated by custom rule. otherwise match on message
      var valueToMatchOn = log.CustomAttributeValues.Select(elem => elem.CustomAttributeRuleId).Contains(ruleId)
        ? log.CustomAttributeValues.First(elem => elem.CustomAttributeRuleId == ruleId).Value
        : log.Message;


      matchedValue = string.Empty;

      var match = valueMatcher.Match(valueToMatchOn);
      if (match.Success)
      {
        matchedValue = match.Value;
      }

      return match.Success;
    }
  }
}