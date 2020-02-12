using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using SawMill.Processor.Model;
using SawMill.Processor.RepositoryInterfaces;
using SawMill.Processor.Services.Interface;

namespace SawMill.Processor.Services.Impl
{
  public class MessageRuleService : IMessageRuleService
  {
    private readonly IMessageRuleRepository _messageRuleRepository;

    public MessageRuleService(IMessageRuleRepository messageRuleRepository)
    {
      _messageRuleRepository = messageRuleRepository;
    }

    public async Task<MessageRule> Create(MessageRule model)
    {
      var createdId = await _messageRuleRepository.Create(model);
      return await _messageRuleRepository.Read(createdId);
    }

    public async Task<MessageRule> Edit(MessageRule model)
    {
      var isUpdated = await _messageRuleRepository.Update(model);
      if (isUpdated)
      {
        return await _messageRuleRepository.Read(model.Id);
      }

      throw new Exception($"Failed to update message rule {model.Id}");
    }

    public async Task<IEnumerable<MessageRule>> GetAll()
    {
      return await _messageRuleRepository.ReadAll();
    }

    public async Task<MessageRule> Get(int id)
    {
      var result = await _messageRuleRepository.Read(id);
      if (result == null)
      {
        throw new ArgumentException($"Cannot read message rule {id}", nameof(id));
      }

      return result;
    }

    public async Task Delete(int id)
    {
      var result = await _messageRuleRepository.Delete(id);
      if (!result)
      {
        throw new ArgumentException($"Cant delete message rule {id}", nameof(id));
      }
    }

    public async Task<int> GetGeneralRuleId(int id)
    {
      var model = await _messageRuleRepository.Read(id);
      return model.GeneralRuleId;
    }
  }
}