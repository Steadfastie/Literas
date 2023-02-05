using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LiterasModels.Abstractions;

namespace LiterasModels.System;

public class CrudResult<T> where T : IBaseDto
{
    public T? Dto { get; set; }
    public OperationResult Result { get; set; }

    public CrudResult()
    {
        Result = OperationResult.Failure;
    }
    public CrudResult(T dto)
    {
        Dto = dto;
        Result = OperationResult.Success;
    }
}
