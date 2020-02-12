namespace SawMill.Data.Models
{
  public class ComponentReportAlert
  {
    public int ComponentReportAlertId { get; set; }
    public int AlertId { get; set; }
    public virtual Alert Alert { get; set; }
    public int Count { get; set; }
    public int ComponentReportId { get; set; }
    public virtual ComponentReport ComponentReport { get; set; }
  }
}