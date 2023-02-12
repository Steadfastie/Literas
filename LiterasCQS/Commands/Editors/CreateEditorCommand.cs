using LiterasDataTransfer.Dto;
using MediatR;

namespace LiterasCQS.Commands.Editors;

public class CreateEditorCommand : IRequest<int>
{
    public EditorDto Editor { get; set; }
}