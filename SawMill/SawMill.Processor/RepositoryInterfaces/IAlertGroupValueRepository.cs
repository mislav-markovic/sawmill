using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;

namespace SawMill.Processor.RepositoryInterfaces
{
  public interface IAlertGroupValueRepository : IRepository<AlertGroupValue>
  {
    Task<IEnumerable<AlertGroupValue>> ValuesForAlertGroup(int alertGroupId);
  }
}