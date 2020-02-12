using System.Collections.Generic;
using System.Threading.Tasks;

namespace SawMill.Processor.Services.Interface
{
  public interface ICrudServiceAsync<T>
  {
    Task<T> Create(T model);
    Task<T> Edit(T model);
    Task<IEnumerable<T>> GetAll();
    Task<T> Get(int id);
    Task Delete(int id);
  }
}