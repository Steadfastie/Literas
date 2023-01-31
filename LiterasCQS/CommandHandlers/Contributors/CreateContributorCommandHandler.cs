using AutoMapper;
using LiterasCQS.Commands.Contributors;
using LiterasData;
using LiterasData.Entities;
using MediatR;

namespace LiterasCQS.Handlers.CommandHandlers.Contributors;

public class CreateContributorCommandHandler : IRequestHandler<CreateContributorCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public CreateContributorCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateContributorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Contributor>(request.Contributor);

        if (entity != null)
        {
            await _context.Contributors.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Request contains {request.Contributor} user");
        }
    }
}