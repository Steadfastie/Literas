using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using MediatR;

namespace LiterasData.CQS.Commands;

public class CreateDocCommand : IRequest<int>
{
    public DocDto Doc { get; set; }
}
public class CreateDoc : IRequestHandler<CreateDocCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public CreateDoc(NotesDBContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<int> Handle(CreateDocCommand request, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Doc>(request.Doc);

        if (entity != null)
        {
            await _context.Docs.AddAsync(entity, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }
        else
        {
            throw new ArgumentException($"Request contains {request.Doc} user");
        }
    }
}