using System.Runtime.Serialization;

namespace LiterasData.Exceptions;

public class RaceException : GeneralException
{
    public RaceException()
    {
    }
    public RaceException(string message) : base(message)
    {
    }
    public RaceException(string message, Exception innerException) : base(message, innerException)
    {
    }
    protected RaceException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}
