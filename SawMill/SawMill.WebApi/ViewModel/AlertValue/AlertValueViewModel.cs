using System;

namespace SawMill.WebApi.ViewModel.AlertValue
{
  public class AlertValueViewModel
  {
    public AlertValueViewModel(int id, int alertId, DateTime timespanStart, DateTime timespanEnd)
    {
      Id = id;
      AlertId = alertId;
      TimespanStart = timespanStart;
      TimespanEnd = timespanEnd;
    }

    public int Id { get; }
    public int AlertId { get; }

    public DateTime TimespanStart { get; }
    public DateTime TimespanEnd { get; }
  }
}