using AutoMapper;
using LiterasCore.Abstractions;
using LiterasCore.System;
using LiterasData;
using LiterasData.CQS;
using LiterasData.CQS.Commands;
using LiterasData.CQS.Queries;
using LiterasData.DTO;
using LiterasData.Entities;
using LiterasData.Exceptions;
using MediatR;
using Polly;

namespace LiterasCore.Services;

public class DocsService : IDocsService
{
    private readonly IMediator _mediator;

    public DocsService(IMediator mediator)
    {
        _mediator = mediator;
    }

    public async Task<CrudResult<DocDto>> GetDocByIdAsync(Guid docId)
    {
        if (docId == Guid.Empty)
        {
            throw new ArgumentException(
                $"Provided ID (..{docId.ToString()[^5..]}) is empty");
        }

        var docFromDb = await _mediator.Send(new GetDocByIdQuery()
        {
            Id = docId
        });

        return docFromDb != null
            ? new CrudResult<DocDto>(docFromDb)
            : new CrudResult<DocDto>();
    }

    public async Task<CrudResults<IEnumerable<DocDto>>> GetDocThumbnailsAsync()
    {
        var docThumbnails = await _mediator.Send(new GetDocThumbnailsQuery());

        return docThumbnails != null
            ? new CrudResults<IEnumerable<DocDto>>(docThumbnails)
            : new CrudResults<IEnumerable<DocDto>>();
    }

    public async Task<CrudResult<DocDto>> GetDocByCreatorIdAsync(Guid creatorId)
    {
        if (creatorId != Guid.Empty)
        {
            throw new ArgumentException(
                $"Provided ID (..{creatorId.ToString()[^5..]}) is empty");
        }

        var docFromDb = await _mediator.Send(new GetDocByCreatorIdQuery()
        {
            CreatorId = creatorId
        });

        return docFromDb != null
            ? new CrudResult<DocDto>(docFromDb)
            : new CrudResult<DocDto>();
    }

    public async Task<CrudResult<DocDto>> GetDocByTitleAsync(string title)
    {
        if (title != string.Empty) throw new ArgumentException("Title is empty");

        var docFromDb = await _mediator.Send(new GetDocByTitleQuery()
        {
            Title = title
        });

        return docFromDb != null
            ? new CrudResult<DocDto>(docFromDb)
            : new CrudResult<DocDto>();
    }

    public async Task<Guid> CreateDocAsync(DocDto docDto, Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new GeneralException("Service could not recognize user");
        }

        docDto.CreatorId = userId;
        var saveChangesResult = await _mediator.Send(new CreateDocCommand() { Doc = docDto });

        if (saveChangesResult != 1)
        {
            throw new GeneralException("Doc creation went wrong");
        }
        return docDto.Id;
    }

    public async Task PatchDocAsync(DocDto docDto, Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new GeneralException("Service could not recognize user");
        }

        // TODO: Check userId against editors list

        var saveChangesResult = await _mediator.Send(new FindAndPatchDocCommand()
        {
            Doc = docDto,
            UserId = userId
        });

        // TODO: Send event to RabbitMQ about document being modified

        if (saveChangesResult != 1)
        {
            throw new GeneralException("Doc update went wrong");
        }
    }

    public async Task DeleteDocAsync(Guid docId, Guid userId)
    {
        if (userId == Guid.Empty)
        {
            throw new GeneralException("Service could not recognize user");
        }

        var saveChangesResult = await _mediator.Send(new FindAndDeleteDocCommand()
        {
            DocId = docId,
            UserId = userId
        });

        // TODO: Send event to RabbitMQ about document being deleted

        if (saveChangesResult != 1)
        {
            throw new GeneralException("Doc update went wrong");
        }
    }
}
