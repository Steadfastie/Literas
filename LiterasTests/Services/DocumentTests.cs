using AutoMapper;
using LiterasBusiness.Services;
using LiterasCQS.Commands.Documents;
using LiterasDataTransfer.DTO;
using LiterasModels.System;
using MediatR;
using Moq;

namespace TestsLiteras.Services;

public class DocumentTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IMediator> _mediatrMock;

    public DocumentTests(IMapper mapper)
    {
        _mapper = mapper;
        _mediatrMock = new Mock<IMediator>();
    }

    [Theory]
    [MemberData(nameof(GetDocumentAndPatchModel), parameters: true)]
    public async Task PatchDocument_WithValidPatchModel(DocumentDTO document, List<PatchModel> patchlist)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchDocumentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new DocumentsService(_mapper, _mediatrMock.Object);

        var result = await service.PatchDocumentAsync(document, patchlist);
        Assert.Equal(1, result);
    }

    [Theory]
    [MemberData(nameof(GetDocumentAndPatchModel), parameters: false)]
    public void PatchDocument_WithIdInPatchModel_ThrowsArgumentException(DocumentDTO document, List<PatchModel> patchlist)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchDocumentCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new DocumentsService(_mapper, _mediatrMock.Object);

        Assert.ThrowsAsync<ArgumentException>(async () => await service.PatchDocumentAsync(document, patchlist));
    }

    public static IEnumerable<object[]> GetDocumentAndPatchModel(bool valid)
    {
        var allDrugs = new List<object[]>()
        {
            new object[]
            {
                new DocumentDTO()
                {
                    Id = Guid.NewGuid(),
                    Title = "Title",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typese"
                },
                new List<PatchModel>()
                {
                    new PatchModel()
                    {
                        PropertyName = "Title",
                        PropertyValue = "New title",
                    },
                }
            },
            new object[]
            {
                new DocumentDTO()
                {
                    Id = Guid.NewGuid(),
                    Title = "Title",
                    Content = "Lorem Ipsum is simply dummy text of the printing and typese"
                },
                new List<PatchModel>()
                {
                    new PatchModel()
                    {
                        PropertyName = "Id",
                        PropertyValue = Guid.NewGuid(),
                    },
                }
            }
        };

        return valid ? allDrugs.Take(1) : allDrugs.TakeLast(1);
    }
}