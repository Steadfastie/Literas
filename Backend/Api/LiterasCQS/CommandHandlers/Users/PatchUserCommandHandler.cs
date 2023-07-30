using AutoMapper;
using LiterasCQS.Commands.Users;
using LiterasData;
using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace MedprCQS.Handlers.CommandHandlers.Users;

public class PatchUserCommandHandler : IRequestHandler<PatchUserCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public PatchUserCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(PatchUserCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<User>(request.User);

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