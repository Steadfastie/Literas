using AutoMapper;
using LiterasBusiness.Services;
using LiterasCQS.Commands.Docs;
using LiterasCQS.Queries.Docs;
using LiterasDataTransfer.Dto;
using LiterasModels.System;
using MediatR;
using Moq;

namespace TestsLiteras.Services;

public class DocTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IMediator> _mediatrMock;

    public DocTests(IMapper mapper)
    {
        _mapper = mapper;
        _mediatrMock = new Mock<IMediator>();
    }

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task CreateDoc_IdProvided(DocDto docDto)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<CreateDocCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<GetDocByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(docDto);

        var service = new DocsService(_mapper, _mediatrMock.Object);
        var result = await service.CreateDocAsync(docDto);

        Assert.IsType<CrudResult<DocDto>>(result);
        Assert.Equal((int)OperationResult.Success, (int)result.ResultStatus);
        Assert.Equal(docDto, result.Result);
        Assert.Equal(docDto.Id, result.Result!.Id);
    }

    [Theory]
    [MemberData(nameof(GetData), 1, 1)]
    public async Task CreateDoc_WithEmptyId(DocDto docDto)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<CreateDocCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<GetDocByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(docDto);

        var service = new DocsService(_mapper, _mediatrMock.Object);
        var result = await service.CreateDocAsync(docDto);

        Assert.IsType<CrudResult<DocDto>>(result);
        Assert.Equal((int)OperationResult.Success, (int)result.ResultStatus);
        Assert.Equal(docDto, result.Result);
        Assert.NotEqual(Guid.Empty, result.Result!.Id);
    }

    [Theory]
    [MemberData(nameof(GetData), 2, 1)]
    public async Task CreateDoc_WithEmptyCreatorId(DocDto docDto)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<CreateDocCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<GetDocByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(docDto);

        var service = new DocsService(_mapper, _mediatrMock.Object);

        await Assert.ThrowsAsync<ArgumentException>(async () => await service.CreateDocAsync(docDto));
    }

    [Theory]
    [MemberData(nameof(GetData), 3, 1)]
    public async Task PatchDoc_WhenDocExists(Guid docId, DocDto sourceDto, DocDto docDto)
    {
        _mediatrMock.SetupSequence(mediator => mediator.Send(
            It.IsAny<GetDocByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceDto)
            .ReturnsAsync(docDto);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchDocCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new DocsService(_mapper, _mediatrMock.Object);
        var result = await service.PatchDocAsync(docId, docDto);

        Assert.IsType<CrudResult<DocDto>>(result);
        Assert.Equal((int)OperationResult.Success, (int)result.ResultStatus);
        Assert.Equal(docDto.Title, result.Result?.Title);
        Assert.Equal(docDto.Content, result.Result?.Content);
    }

    [Theory]
    [MemberData(nameof(GetData), 4, 1)]
    public void PatchDoc_WhenDocDoesNotExist(Guid docId, DocDto docDto)
    {
        _mediatrMock.SetupSequence(mediator => mediator.Send(
            It.IsAny<GetDocByIdQuery>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(null as DocDto);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchDocCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new DocsService(_mapper, _mediatrMock.Object);

        Assert.ThrowsAsync<ArgumentException>(async () => await service.PatchDocAsync(docId, docDto));
    }

    [Theory]
    [MemberData(nameof(GetData), 5, 1)]
    public async Task DeleteDoc_WhenDocExists(Guid docId, DocDto sourceDto)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<GetDocByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceDto);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<DeleteDocCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new DocsService(_mapper, _mediatrMock.Object);
        var result = await service.DeleteDocAsync(docId);

        Assert.IsType<CrudResult<DocDto>>(result);
        Assert.Equal((int)OperationResult.Success, (int)result.ResultStatus);
        Assert.Equal(sourceDto, result.Result);
    }

    [Theory]
    [MemberData(nameof(GetData), 6, 1)]
    public void DeleteDoc_WhenDocDoesNotExist(Guid docId)
    {
        _mediatrMock.SetupSequence(mediator => mediator.Send(
            It.IsAny<GetDocByIdQuery>(), It.IsAny<CancellationToken>()))!
            .ReturnsAsync(null as DocDto);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchDocCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new DocsService(_mapper, _mediatrMock.Object);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.DeleteDocAsync(docId));
    }

    public static IEnumerable<object[]> GetData(int index, int count = 1)
    {
        var allSeeds = new List<object[]>()
        {
            // 1. Create correct doc
            new object[]
            {
                new DocDto()
                {
                    Id = Guid.NewGuid(),
                    CreatorId = Guid.NewGuid(),
                    Title = "Title",
                    Content = "Content"
                },
            },

            // 2. Create doc with empty id
            new object[]
            {
                new DocDto()
                {
                    Id = Guid.Empty,
                    CreatorId = Guid.NewGuid(),
                    Title = "Title",
                    Content = "Content"
                },
            },

            // 3. Create doc with empty creator id
            new object[]
            {
                new DocDto()
                {
                    Id = Guid.NewGuid(),
                    CreatorId = Guid.Empty,
                    Title = "Title",
                    Content = "Content"
                },
            },

            // 4. Patch doc with source
            new object[]
            {
                Guid.NewGuid(),
                new DocDto()
                {
                    Id = Guid.NewGuid(),
                    CreatorId = Guid.NewGuid(),
                    Title = "Title",
                    Content = "Content"
                },
                new DocDto()
                {
                    Id = Guid.NewGuid(),
                    CreatorId = Guid.NewGuid(),
                    Title = "New title",
                    Content = "New content"
                },
            },

            // 5. Patch doc without source
            new object[]
            {
                Guid.NewGuid(),
                new DocDto()
                {
                    Id = Guid.NewGuid(),
                    CreatorId = Guid.NewGuid(),
                    Title = "New title",
                    Content = "New content"
                },
            },

            // 6. Delete doc with source
            new object[]
            {
                Guid.NewGuid(),
                new DocDto()
                {
                    Id = Guid.NewGuid(),
                    CreatorId = Guid.NewGuid(),
                    Title = "Title",
                    Content = "Content"
                },
            },

            // 7. Delete doc without source
            new object[]
            {
                Guid.NewGuid()
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}