using AutoMapper;
using LiterasCore.Abstractions;
using LiterasCore.System;
using LiterasData.DTO;
using LiterasWebAPI.Controllers;
using LiterasWebAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestsLiteras.Controllers.Docs;

public class GetTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IDocsService> _docsServiceMock;
    private readonly Mock<IEditorsService> _contributorsServiceMock;

    public GetTests(IMapper mapper)
    {
        _mapper = mapper;
        _docsServiceMock = new Mock<IDocsService>();
        _contributorsServiceMock = new Mock<IEditorsService>();
    }

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task Get_Returns200WithModel_WhenIdValid(Guid docId, CrudResult<DocDto> crudResult)
    {
        _docsServiceMock.Setup(service => service.GetDocByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(crudResult);

        var docsContoller = new DocsController(_docsServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await docsContoller.Details(docId);
        var responseModel = _mapper.Map<DocResponse>(crudResult.Result);

        Assert.NotNull(response);
        Assert.IsType<OkObjectResult>(response);

        var responseValue = ((OkObjectResult)response).Value;
        Assert.NotNull(responseValue);
        Assert.IsType<DocResponse>(responseValue);
        Assert.Equivalent(responseModel, responseValue);
    }

    [Theory]
    [MemberData(nameof(GetData), 1, 1)]
    public async Task Get_Returns400_WhenIdEmpty(Guid docId)
    {
        _docsServiceMock.Setup(service => service.GetDocByIdAsync(It.IsAny<Guid>()))!
            .ReturnsAsync(new CrudResult<DocDto>());

        var docsContoller = new DocsController(_docsServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await docsContoller.Details(docId);

        Assert.NotNull(response);
        Assert.IsType<BadRequestResult>(response);
    }

    [Theory]
    [MemberData(nameof(GetData), 2, 1)]
    public async Task Get_Returns404_WhenDocDoesNotExist(Guid docId)
    {
        _docsServiceMock.Setup(service => service.GetDocByIdAsync(It.IsAny<Guid>()))!
            .ReturnsAsync(new CrudResult<DocDto>());

        var docsContoller = new DocsController(_docsServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await docsContoller.Details(docId);

        Assert.NotNull(response);
        Assert.IsType<NotFoundResult>(response);
    }

    [Theory]
    [MemberData(nameof(GetData), 2, 1)]
    public async Task Get_Returns500_OnException(Guid docId)
    {
        _docsServiceMock.Setup(service => service.GetDocByIdAsync(It.IsAny<Guid>()))!
            .ThrowsAsync(new Exception());

        var docsContoller = new DocsController(_docsServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await docsContoller.Details(docId) as ObjectResult;

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
            // 1. Details with valid id
            new object[]
            {
                Guid.NewGuid(),
                new CrudResult<DocDto>(new DocDto()
                {
                    Id = Guid.Empty,
                    CreatorId = Guid.Empty,
                    Title = string.Empty,
                    Content = string.Empty
                }) 
            },

            // 2. Details with empty id
            new object[]
            {
                Guid.Empty
            },

            // 3-4. Details with empty dto and exception thrown
            new object[]
            {
                Guid.NewGuid()
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}