namespace SawMill.Data.Models
{
  public class SystemReportAlertGroup
  {
    public int SystemReportAlertGroupId { get; set; }
    public int SystemReportId { get; set; }
    public virtual SystemReport SystemReport { get; set; }
    public int AlertGroupId { get; set; }
    public virtual AlertGroup AlertGroup { get; set; }
    public int Count { get; set; }
  }
}