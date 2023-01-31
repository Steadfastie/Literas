using LiterasDataTransfer.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiterasCQS.Queries.Contributors;

public class GetContributorByIdQuery : IRequest<ContributorDTO>
{
    public Guid Id { get; set; }
}
