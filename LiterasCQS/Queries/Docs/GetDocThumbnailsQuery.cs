using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Docs;

public class GetDocThumbnailsQuery : IRequest<IEnumerable<DocDto>>
{
}