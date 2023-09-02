using LiterasData.DTO;

namespace LiterasCore.System;

public class CrudResult<T> where T : IBaseDto
{
    public CrudResult()
    {
        ResultStatus = OperationResult.Failure;
    }

    public CrudResult(T result)
    {
        Result = result;
        ResultStatus = OperationResult.Success;
    }

    public T? Result { get; set; }
    public OperationResult ResultStatus { get; set; }
}
