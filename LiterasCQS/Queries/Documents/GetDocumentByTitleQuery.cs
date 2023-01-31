using LiterasDataTransfer.DTO;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiterasCQS.Queries.Documents;

public class GetDocumentByTitleQuery : IRequest<DocumentDTO>
{
    public string Title { get; set; }
}
