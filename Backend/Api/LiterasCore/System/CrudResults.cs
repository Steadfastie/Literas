using LiterasData.DTO;

namespace LiterasCore.System;

public class CrudResults<T> where T : IEnumerable<IBaseDto>
{
    public CrudResults()
    {
        ResultStatus = OperationResult.Failure;
    }

    public CrudResults(T results)
    {
        Results = results;
        ResultStatus = OperationResult.Success;
    }

    public T? Results { get; set; }
    public OperationResult ResultStatus { get; set; }
}
