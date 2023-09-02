using System.Runtime.Serialization;

namespace LiterasData.Exceptions;

public class NotModifiedException : GeneralException
{
    public NotModifiedException()
    {
    }

    public NotModifiedException(string message) : base(message)
    {
    }

    public NotModifiedException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected NotModifiedException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
