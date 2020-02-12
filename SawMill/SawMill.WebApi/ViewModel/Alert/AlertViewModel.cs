namespace SawMill.WebApi.ViewModel.Alert
{
  public class AlertViewModel
  {
    public AlertViewModel(int id, int threshold, string value, int componentId, string name, string description,
      long timespan, int generalRuleId, bool hasConstantValue, int position, int alertGroupId)
    {
      Id = id;
      Threshold = threshold;
      Value = value;
      ComponentId = componentId;
      Name = name;
      Description = description;
      Timespan = timespan;
      GeneralRuleId = generalRuleId;
      HasConstantValue = hasConstantValue;
      Position = position;
      AlertGroupId = alertGroupId;
    }

    public int Id { get; }
    public int Threshold { get; }
    public long Timespan { get; }
    public bool HasConstantValue { get; }
    public int Position { get; }
    public string Value { get; }
    public string Name { get; }
    public string Description { get; }
    public int ComponentId { get; }
    public int GeneralRuleId { get; }
    public int AlertGroupId { get; }
  }
}