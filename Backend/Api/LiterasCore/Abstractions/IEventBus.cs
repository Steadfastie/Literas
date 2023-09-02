using LiterasData.DTO;

namespace LiterasCore.Abstractions;

public interface IEventBus
{
    Task Notify(IBaseDto objectToSend);
    Task NotifyDeleted(Guid docId);
}
