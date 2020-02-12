using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class AlertGroupValueService : IAlertGroupValueService
  {
    private readonly IAlertGroupRepository _alertGroupRepository;
    private readonly IAlertGroupValueRepository _alertGroupValueRepository;
    private readonly IAlertValueService _alertValueService;

    public AlertGroupValueService(IAlertGroupValueRepository alertGroupValueRepository,
      IAlertGroupRepository alertGroupRepository, IAlertValueService alertValueService)
    {
      _alertGroupValueRepository = alertGroupValueRepository;
      _alertGroupRepository = alertGroupRepository;
      _alertValueService = alertValueService;
    }

    public async Task<AlertGroupValue> Create(AlertGroupValue model)
    {
      var createdId = await _alertGroupValueRepository.Create(model);
      return await _alertGroupValueRepository.Read(createdId);
    }

    public async Task<AlertGroupValue> Edit(AlertGroupValue model)
    {
      var updateResult = await _alertGroupValueRepository.Update(model);
      if (updateResult)
      {
        return await _alertGroupValueRepository.Read(model.Id);
      }

      return model;
    }

    public async Task<IEnumerable<AlertGroupValue>> GetAll()
    {
      return await _alertGroupValueRepository.ReadAll();
    }

    public async Task<AlertGroupValue> Get(int id)
    {
      return await _alertGroupValueRepository.Read(id);
    }

    public async Task Delete(int id)
    {
      var result = await _alertGroupValueRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete alert group value {id}", nameof(id));
      }
    }

    public async Task<IEnumerable<AlertGroupValue>> CorrelateAlertsForSystem(int systemId)
    {
      var alertGroups = (await _alertGroupRepository.ReadAll()).Where(elem => elem.SystemId.Equals(systemId)).ToList();
      var result = new List<AlertGroupValue>();

      foreach (var alertGroup in alertGroups)
        if (alertGroup.Alerts != null && alertGroup.Alerts.Any())
        {
          var alertValues = await AlertValuesForAlertGroup(alertGroup);
          // A0 start time => Av0,Av1..Avn
          var groups = new List<List<AlertValue>>();
          // av.id => number of times used
          var usedAlertValues = new Dictionary<int, int>();
          var isFirstAlert = true;

          foreach (var alert in alertGroup.Alerts.OrderBy(a => a.Position))
            // init with first cause
            if (isFirstAlert)
            {
              isFirstAlert = false;
              foreach (var alertValue in alertValues.Where(av => av.AlertId.Equals(alert.Id)))
              {
                var list = new List<AlertValue> {alertValue};
                groups.Add(list);
              }
            }
            else
            {
              var groupsToRemove = new List<List<AlertValue>>();
              foreach (var group in groups)
              {
                var endTime = group.Last().TimespanEnd;
                // find next alert value in chain that started after previous alert value ended
                var next = alertValues.FirstOrDefault(av => av.AlertId.Equals(alert.Id) && av.TimespanStart >= endTime);

                if (next != null && !alert.Not)
                {
                  group.Add(next);
                }
                // if such alert value does not exist
                else
                {
                  groupsToRemove.Add(group);
                }
              }

              // remove groups that could not be fully formed
              foreach (var group in groupsToRemove) groups.Remove(group);
            }

          // foreach group candidate order by timespan in which correlation takes place(shortest to longest)
          foreach (var (groupCandidate, timeWindow) in groups
            .ToDictionary(l => l, list => list.Last().TimespanEnd - list.First().TimespanStart)
            .OrderBy(pair => pair.Value))
          {
            var alertValuesInGroup = groupCandidate.Select(grp => grp.Id).ToList();

            //check that group satisfies time constraint if such constraint exists
            if (alertGroup.CorrelationWindow != null && timeWindow <= alertGroup.CorrelationWindow)
            {
              //check that alert value was not already associated with another alert group value
              if (!usedAlertValues.Keys.Any(elem => alertValuesInGroup.Contains(elem)))
              {
                // create new instance of group
                var alertGroupValue = new AlertGroupValue
                {
                  Id = 0,
                  AlertGroupId = alertGroup.Id,
                  TimespanStart = groupCandidate.First().TimespanStart,
                  TimespanEnd = groupCandidate.Last().TimespanEnd
                };
                var createdId = await _alertGroupValueRepository.Create(alertGroupValue);

                // assign each alertValue to newly created group
                foreach (var alertValue in groupCandidate)
                {
                  alertValue.AlertGroupValueId = createdId;
                  await _alertValueService.Edit(alertValue);
                  // keep track of which alert values were used
                  usedAlertValues.Add(alertValue.Id, 1);
                }

                result.Add(await _alertGroupValueRepository.Read(createdId));
              }
            }
          }
        }

      return result;
    }

    public async Task<IEnumerable<AlertGroupValue>> AlertGroupValuesForAlertGroup(int alertGroupId)
    {
      var all = (await _alertGroupValueRepository.ReadAll()).Where(elem => elem.AlertGroupId == alertGroupId);

      return all;
    }

    private async Task<IList<AlertValue>> AlertValuesForAlertGroup(AlertGroup group)
    {
      var alertValues = new List<AlertValue>();

      foreach (var alert in group.Alerts)
      {
        var values = await _alertValueService.AlertValuesForAlert(alert.Id);
        alertValues.AddRange(values);
      }

      return alertValues.Where(av => !av.AlertGroupValueId.HasValue).OrderBy(e => e.TimespanStart).ToList();
    }
  }
}