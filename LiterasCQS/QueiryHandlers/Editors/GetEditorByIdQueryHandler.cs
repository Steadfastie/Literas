using AutoMapper;
using LiterasCQS.Queries.Editors;
using LiterasData;
using LiterasDataTransfer.Dto;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace LiterasCQS.QueiryHandlers.Users;

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
            .FirstOrDefaultAsync(con => con.Id == request.Id, cancellationToken: cancellationToken);
        return _mapper.Map<EditorDto>(entity);
    }
}