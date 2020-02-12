using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class AlertService : IAlertService
  {
    private readonly IAlertRepository _alertRepository;

    public AlertService(IAlertRepository alertRepository)
    {
      _alertRepository = alertRepository;
    }

    public async Task<Alert> Create(Alert model)
    {
      var createdId = await _alertRepository.Create(model);
      return await _alertRepository.Read(createdId);
    }

    public async Task<Alert> Edit(Alert model)
    {
      var updateResult = await _alertRepository.Update(model);
      if (updateResult)
      {
        return await _alertRepository.Read(model.Id);
      }

      return model;
    }

    public async Task<IEnumerable<Alert>> GetAll()
    {
      return await _alertRepository.ReadAll();
    }

    public async Task<Alert> Get(int id)
    {
      return await _alertRepository.Read(id);
    }

    public async Task Delete(int id)
    {
      var result = await _alertRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete alert {id}", nameof(id));
      }
    }
  }
}