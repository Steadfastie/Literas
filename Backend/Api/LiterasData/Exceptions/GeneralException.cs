using System.Runtime.Serialization;

namespace LiterasData.Exceptions;
public class GeneralException : Exception
{
    public GeneralException()
    {
    }
    public GeneralException(string message) : base(message)
    {
    }
    public GeneralException(string message, Exception innerException) : base(message, innerException)
    {
    }
    protected GeneralException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }
}