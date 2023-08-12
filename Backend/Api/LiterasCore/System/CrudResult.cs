using LiterasData.DTO;

namespace LiterasCore.System;

public class CrudResult<T> where T : IBaseDto
{
    public T? Result { get; set; }
    public OperationResult ResultStatus { get; set; }

    public CrudResult()
    {
        ResultStatus = OperationResult.Failure;
    }

    public CrudResult(T result)
    {
        Result = result;
        ResultStatus = OperationResult.Success;
    }
}