using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace SawMill.WebApi.ViewModel.AlertGroup
{
  public class AlertGroupViewModel
  {
    public AlertGroupViewModel(int id, string name, string description, long timespan, int systemId, AlertGroupAlertVIewModel[] alerts)
    {
      Id = id;
      Name = name;
      Description = description;
      Timespan = timespan;
      SystemId = systemId;
      Alerts = alerts;
    }

    public int Id { get; }
    public string Name { get; }
    public string Description { get; }
    public long Timespan { get; }
    public int SystemId { get; }
    public AlertGroupAlertVIewModel[] Alerts { get; }
  }
}
