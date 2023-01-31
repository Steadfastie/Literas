using LiterasDataTransfer.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiterasCQS.Queries.Contributors;

public class GetUsersByDocumentIdQuery : IRequest<IEnumerable<UserDTO>>
{
    public Guid DocumentId { get; set; }
}
