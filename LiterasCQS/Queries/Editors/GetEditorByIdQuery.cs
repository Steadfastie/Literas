using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Queries.Editors;

public class GetEditorByIdQuery : IRequest<EditorDto>
{
    public Guid Id { get; set; }
}