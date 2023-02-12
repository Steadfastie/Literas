using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Editors;

public class DeleteEditorCommand : IRequest<int>
{
    public EditorDto Editor { get; set; }
}