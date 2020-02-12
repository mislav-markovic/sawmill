﻿using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.Services.Interface
{
  public interface ISeverityRuleService : ICrudServiceAsync<SeverityRule>
  {
    Task<int> GetGeneralRuleId(int id);
  }
}