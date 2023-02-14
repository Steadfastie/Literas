using LiterasModels.Abstractions;

namespace LiterasModels.System;

public class CrudResults<T> where T : IEnumerable<IBaseDto>
{
    public T? Results { get; set; }
    public OperationResult ResultStatus { get; set; }

    public CrudResults()
    {
        ResultStatus = OperationResult.Failure;
    }

    public CrudResults(T results)
    {
        Results = results;
        ResultStatus = OperationResult.Success;
    }
}