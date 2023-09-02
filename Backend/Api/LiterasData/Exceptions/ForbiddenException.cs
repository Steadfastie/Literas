using System.Runtime.Serialization;

namespace LiterasData.Exceptions;

public class ForbiddenException : GeneralException
{
    public ForbiddenException()
    {
    }

    public ForbiddenException(string message) : base(message)
    {
    }

    public ForbiddenException(string message, Exception innerException) : base(message, innerException)
    {
    }

    protected ForbiddenException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
