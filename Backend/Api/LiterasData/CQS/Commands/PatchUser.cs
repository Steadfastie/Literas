using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Commands;

public class PatchUserCommand : IRequest<int>
{
    public UserDto User { get; set; }
    public List<PatchModel> PatchList { get; set; }
}
public class PatchUser : IRequestHandler<PatchUserCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public PatchUser(NotesDBContext context, IMapper mapper)
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