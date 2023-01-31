using AutoMapper;
using LiterasCQS.Commands.Documents;
using LiterasData;
using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.Handlers.CommandHandlers.Documents;

public class PatchDocumentCommandHandler : IRequestHandler<PatchDocumentCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public PatchDocumentCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(PatchDocumentCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Document>(request.Document);

        var nameValuePropertiesPairs = request.PatchList
            .ToDictionary(
                patchModel => patchModel.PropertyName,
                patchModel => patchModel.PropertyValue);

        var dbEntityEntry = _context.Entry(entity);
        dbEntityEntry.CurrentValues.SetValues(nameValuePropertiesPairs);
        dbEntityEntry.State = EntityState.Modified;

        return await _context.SaveChangesAsync(cancellationToken);
    }
}