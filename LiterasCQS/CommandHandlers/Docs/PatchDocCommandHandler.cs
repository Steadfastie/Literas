using AutoMapper;
using LiterasCQS.Commands.Docs;
using LiterasData;
using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.Handlers.CommandHandlers.Docs;

public class PatchDocCommandHandler : IRequestHandler<PatchDocCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public PatchDocCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(PatchDocCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Doc>(request.Doc);

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