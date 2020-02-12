using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SawMill.WebApi.ViewModel.AlertValue;

namespace SawMill.WebApi.ViewModel.AlertGroupValue
{
  public class AlertGroupValueViewModel
  {
    public AlertGroupValueViewModel(int id, int alertGroupId, DateTime timespanStart, DateTime timespanEnd, AlertValueViewModel[] alertValues)
    {
      Id = id;
      AlertGroupId = alertGroupId;
      TimespanStart = timespanStart;
      TimespanEnd = timespanEnd;
      AlertValues = alertValues;
    }

    public int Id { get;  }
    public int AlertGroupId { get;  }
    public DateTime TimespanStart { get;  }
    public DateTime TimespanEnd { get; }
    public AlertValueViewModel[] AlertValues { get; }
  }
}
