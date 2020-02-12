namespace SawMill.Processor.Model
{
  public class RawLog
  {
    public int Id { get; set; }
    public string Message { get; set; }
    public int ParentComponentId { get; set; }
    public Component ParentComponent { get; set; }
  }
}