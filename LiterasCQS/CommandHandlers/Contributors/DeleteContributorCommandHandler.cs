using AutoMapper;
using LiterasCQS.Commands.Contributors;
using LiterasData;
using LiterasData.Entities;
using MediatR;

namespace LiterasCQS.Handlers.CommandHandlers.Contributors;

public class DeleteContributorCommandHandler : IRequestHandler<DeleteContributorCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public DeleteContributorCommandHandler(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(DeleteContributorCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Contributor>(request.Contributor);

        if (entity != null)
        {
            _context.Contributors.Remove(entity);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Request contains {request.Contributor} user");
        }
    }
}