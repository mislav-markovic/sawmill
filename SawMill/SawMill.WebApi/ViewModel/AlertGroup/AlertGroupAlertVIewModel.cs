using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SawMill.WebApi.ViewModel.AlertGroup
{
  public class AlertGroupAlertVIewModel
  {
    public AlertGroupAlertVIewModel(int alertId, int position, bool not, string alertName)
    {
      AlertId = alertId;
      Position = position;
      Not = not;
      AlertName = alertName;
    }

    public int AlertId { get; }
    public string AlertName { get; }
    public int Position { get; }
    public bool Not { get; }
  }
}
