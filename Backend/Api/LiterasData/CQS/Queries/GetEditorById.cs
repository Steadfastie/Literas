using AutoMapper;
using LiterasData.DTO;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasData.CQS.Queries;

public class GetEditorByIdQuery : IRequest<EditorDto>
{
    public Guid Id { get; set; }
}
public class GetEditorByTitleQueryHandler : IRequestHandler<GetEditorByIdQuery, EditorDto>
{
    private readonly IMapper _mapper;
    private readonly NotesDBContext _dbContext;

    public GetEditorByTitleQueryHandler(NotesDBContext context, IMapper mapper)
    {
        _dbContext = context;
        _mapper = mapper;
    }

    public async Task<EditorDto> Handle(GetEditorByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _dbContext.Editors
            .AsNoTracking()
            .Include(ent => ent.Doc)
            .Include(ent => ent.User)
            .FirstOrDefaultAsync(con => con.Id == request.Id, cancellationToken: cancellationToken);
        return _mapper.Map<EditorDto>(entity);
    }
}