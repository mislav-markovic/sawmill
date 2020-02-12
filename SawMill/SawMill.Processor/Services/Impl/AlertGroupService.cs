using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class AlertGroupService : IAlertGroupService
  {
    private readonly IAlertGroupRepository _alertGroupRepository;

    public AlertGroupService(IAlertGroupRepository alertGroupRepository)
    {
      _alertGroupRepository = alertGroupRepository;
    }

    public async Task<AlertGroup> Create(AlertGroup model)
    {
      var createdId = await _alertGroupRepository.Create(model);
      return await _alertGroupRepository.Read(createdId);
    }

    public async Task<AlertGroup> Edit(AlertGroup model)
    {
      var updateResult = await _alertGroupRepository.Update(model);
      if (updateResult)
      {
        return await _alertGroupRepository.Read(model.Id);
      }

      return model;
    }

    public async Task<IEnumerable<AlertGroup>> GetAll()
    {
      return await _alertGroupRepository.ReadAll();
    }

    public async Task<AlertGroup> Get(int id)
    {
      return await _alertGroupRepository.Read(id);
    }

    public async Task Delete(int id)
    {
      var result = await _alertGroupRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete alert {id}", nameof(id));
      }
    }
  }
}