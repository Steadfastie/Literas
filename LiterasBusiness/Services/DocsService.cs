using AutoMapper;
using LiterasCQS.Commands.Docs;
using LiterasCQS.Queries.Docs;
using LiterasDataTransfer.Dto;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.System;
using MediatR;
using System.Reflection;

namespace LiterasBusiness.Services;

public class DocsService : IDocsService
{
    private readonly IMapper _mapper;
    private readonly IMediator _mediator;

    public DocsService(IMapper mapper, IMediator mediator)
    {
        _mapper = mapper;
        _mediator = mediator;
    }

    public async Task<CrudResult<DocDto>> GetDocByIdAsync(Guid docId)
    {
        if (docId != Guid.Empty)
        {
            var docFromDb = await _mediator.Send(new GetDocByIdQuery()
            {
                Id = docId
            });

            if (docFromDb != null)
            {
                return new CrudResult<DocDto>(docFromDb);
            }
            else
            {
                return new CrudResult<DocDto>();
            }
        }
        else
        {
            throw new ArgumentException("Provided id is empty");
        }
    }

    public async Task<CrudResult<DocDto>> GetDocByCreatorIdAsync(Guid creatorId)
    {
        if (creatorId != Guid.Empty)
        {
            var docFromDb = await _mediator.Send(new GetDocByCreatorIdQuery()
            {
                CreatorId = creatorId
            });

            if (docFromDb != null)
            {
                return new CrudResult<DocDto>(docFromDb);
            }
            else
            {
                return new CrudResult<DocDto>();
            }
        }
        else
        {
            throw new ArgumentException("Provided id is empty");
        }
    }

    public async Task<CrudResult<DocDto>> GetDocByTitleAsync(string title)
    {
        if (title != string.Empty)
        {
            var docFromDb = await _mediator.Send(new GetDocByTitleQuery()
            {
                Title = title
            });

            if (docFromDb != null)
            {
                return new CrudResult<DocDto>(docFromDb);
            }
            else
            {
                return new CrudResult<DocDto>();
            }
        }
        else
        {
            throw new ArgumentException("Title is empty");
        }
    }

    public async Task<CrudResult<DocDto>> CreateDocAsync(DocDto docDto)
    {
        if (docDto.Id == Guid.Empty)
        {
            docDto.Id = Guid.NewGuid();
        }

        if (docDto.CreatorId == Guid.Empty)
        {
            throw new ArgumentException("Creator ID can't be empty");
        }

        int saveChangesResult = await _mediator.Send(new CreateDocCommand()
        {
            Doc = docDto
        });

        if (saveChangesResult == 1)
        {
            return new CrudResult<DocDto>(docDto);
        }
        else
        {
            return new CrudResult<DocDto>();
        }
    }

    public async Task<CrudResult<DocDto>> PatchDocAsync(Guid docId, DocDto docDto)
    {
        var sourceDto = await _mediator.Send(new GetDocByIdQuery() { Id = docId });
        if (sourceDto == null)
        {
            throw new ArgumentException("Found no doc with provided ID", nameof(docId));
        }

        PropertyInfo[] ignoreFields = new PropertyInfo[]
        {
            docDto.GetType().GetProperty("Id")!,
            docDto.GetType().GetProperty("CreatorId")!,
        };

        var patchList = PatchModelCreator<DocDto>.Generate(sourceDto, docDto, ignoreFields);

        if (patchList.Any())
        {
            await _mediator.Send(new PatchDocCommand()
            {
                Doc = docDto,
                PatchList = patchList
            });

            var updatedDto = await _mediator.Send(new GetDocByIdQuery() { Id = docId });

            return new CrudResult<DocDto>(updatedDto);
        }
        else
        {
            return new CrudResult<DocDto>();
        }
    }

    public async Task<CrudResult<DocDto>> DeleteDocAsync(Guid docId)
    {
        var sourceDto = await _mediator.Send(new GetDocByIdQuery() { Id = docId });
        if (sourceDto == null)
        {
            throw new ArgumentException("Found no doc with provided ID", nameof(docId));
        }

        int saveChangesResult = await _mediator.Send(new DeleteDocCommand()
        {
            Doc = sourceDto
        });

        if (saveChangesResult == 1)
        {
            return new CrudResult<DocDto>(sourceDto);
        }
        else
        {
            return new CrudResult<DocDto>();
        }
    }
}