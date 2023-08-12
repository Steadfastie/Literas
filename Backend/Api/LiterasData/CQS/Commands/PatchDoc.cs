using AutoMapper;
using LiterasData.DTO;
using LiterasData.Entities;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Commands;

public class PatchDocCommand : IRequest<int>
{
    public DocDto Doc { get; set; }
    public List<PatchModel> PatchList { get; set; }
}
public class PatchDoc : IRequestHandler<PatchDocCommand, int>
{
    private readonly NotesDBContext _context;
    private readonly IMapper _mapper;

    public PatchDoc(NotesDBContext context, IMapper mapper)
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