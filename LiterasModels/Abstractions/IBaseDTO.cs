using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiterasModels.Abstractions;

public interface IBaseDto
{
    Guid Id { get; set; }
}
