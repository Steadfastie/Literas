using AutoMapper;
using LiterasDataTransfer.Dto;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.Responses;
using LiterasModels.System;
using LiterasWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestsLiteras.Controllers.Docs;

public class GetThumbnailsTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IDocsService> _docsServiceMock;
    private readonly Mock<IEditorsService> _contributorsServiceMock;

    public GetThumbnailsTests(IMapper mapper)
    {
        _mapper = mapper;
        _docsServiceMock = new Mock<IDocsService>();
        _contributorsServiceMock = new Mock<IEditorsService>();
    }

    // TODO: Write one test to check attributes for Swagger/URL

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task GetThumbnails_Returns200WithModels_WhenOk(CrudResults<IEnumerable<DocDto>> crudResults)
    {
        _docsServiceMock.Setup(service => service.GetDocThumbnailsAsync())
            .ReturnsAsync(crudResults);

        var docsContoller = new DocsController(_docsServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await docsContoller.GetDocTumbnails();
        var responseModel = _mapper.Map<IEnumerable<DocThumbnailResponseModel>>(crudResults.Results);

        Assert.NotNull(response);
        Assert.IsType<OkObjectResult>(response);

        var responseValue = ((OkObjectResult)response).Value;
        Assert.NotNull(responseValue);
        Assert.IsAssignableFrom<IEnumerable<DocThumbnailResponseModel>>(responseValue);
        Assert.Equivalent(responseModel, responseValue);
    }

    [Fact]
    public async Task Get_Returns404_WhenDataAbsent()
    {
        _docsServiceMock.Setup(service => service.GetDocThumbnailsAsync())!
            .ReturnsAsync(new CrudResults<IEnumerable<DocDto>>());

        var docsContoller = new DocsController(_docsServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await docsContoller.GetDocTumbnails();

        Assert.NotNull(response);
        Assert.IsType<NotFoundResult>(response);
    }

    [Fact]
    public async Task Get_Returns500_OnException()
    {
        _docsServiceMock.Setup(service => service.GetDocThumbnailsAsync())!
            .ThrowsAsync(new Exception());

        var docsContoller = new DocsController(_docsServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await docsContoller.GetDocTumbnails() as ObjectResult;

        Assert.NotNull(response);
        Assert.IsType<ProblemDetails>(response.Value);
        Assert.Equal(500, ((ProblemDetails)response.Value).Status);
        Assert.NotNull(((ProblemDetails)response.Value).Detail);
    }

    /// <summary>
    /// This method is used to provide test method with data. Look through the data below to pick.
    /// </summary>
    /// <param name="index">Index of array of object</param>
    /// <param name="count">How many of them to take</param>
    public static IEnumerable<object[]> GetData(int index, int count)
    {
        var allSeeds = new List<object[]>()
        {
            // 1. Thumbnails with output
            new object[]
            {
                new CrudResults<IEnumerable<DocDto>>(new List<DocDto>()
                {
                    new DocDto()
                    {
                        Id = Guid.Empty,
                        CreatorId = Guid.Empty,
                        Title = string.Empty,
                        Content = string.Empty
                    },
                    new DocDto()
                    {
                        Id = Guid.Empty,
                        CreatorId = Guid.Empty,
                        Title = string.Empty,
                        Content = string.Empty
                    }
                }) 
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}