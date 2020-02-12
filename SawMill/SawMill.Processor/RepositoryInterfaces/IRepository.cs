using System.Collections.Generic;
using System.Threading.Tasks;

namespace SawMill.Processor.RepositoryInterfaces
{
  public interface IRepository<T>
  {
    Task<int> Create(T model);
    Task<T> Read(int id);
    Task<IEnumerable<T>> ReadAll();
    Task<bool> Update(T model);
    Task<bool> Delete(int id);
  }
}