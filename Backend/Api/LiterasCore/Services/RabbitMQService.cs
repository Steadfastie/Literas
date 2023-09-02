using LiterasCore.Abstractions;
using LiterasData.DTO;

namespace LiterasCore.Services;

public class RabbitMQService: IEventBus
{
    public Task Notify(IBaseDto objectToSend)
    {
        return Task.CompletedTask;
    }

    public Task NotifyDeleted(Guid docId)
    {
        return Task.CompletedTask;
    }
}
