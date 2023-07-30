using AutoMapper;
using LiterasDataTransfer.Dto;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.Responses;
using LiterasModels.System;
using LiterasWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestsLiteras.Controllers.Users;

public class DeleteTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUsersService> _usersServiceMock;
    private readonly Mock<IEditorsService> _contributorsServiceMock;

    public DeleteTests(IMapper mapper)
    {
        _mapper = mapper;
        _usersServiceMock = new Mock<IUsersService>();
        _contributorsServiceMock = new Mock<IEditorsService>();
    }

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task Delete_Returns200WithModel_WhenIdValid(
        Guid userId, CrudResult<UserDto> operationResult)
    {
        _usersServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<Guid>()))
            .ReturnsAsync(operationResult);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Delete(userId);
        var responseModel = _mapper.Map<UserResponseModel>(operationResult.Result);

        Assert.NotNull(response);
        Assert.IsType<OkObjectResult>(response);

        var responseValue = ((OkObjectResult)response).Value;
        Assert.NotNull(responseValue);
        Assert.IsType<UserResponseModel>(responseValue);
        Assert.Equivalent(responseModel, responseValue);
    }

    [Theory]
    [MemberData(nameof(GetData), 1, 1)]
    public async Task Delete_Returns400_WhenIdEmpty(Guid userId)
    {
        _usersServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<Guid>()))!
            .ReturnsAsync(null as CrudResult<UserDto>);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Delete(userId);

        Assert.NotNull(response);
        Assert.IsType<BadRequestResult>(response);
    }

    [Theory]
    [MemberData(nameof(GetData), 2, 1)]
    public async Task Delete_Returns400WithMessage_WhenDeleteIsUnsuccessful(
        Guid userId, CrudResult<UserDto> operationResult)
    {
        _usersServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<Guid>()))
            .ReturnsAsync(operationResult);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Delete(userId);

        Assert.NotNull(response);
        Assert.IsType<BadRequestObjectResult>(response);
        Assert.NotNull((response as BadRequestObjectResult)?.Value);
    }

    [Theory]
    [MemberData(nameof(GetData), 3, 1)]
    public async Task Delete_Returns500_OnException(Guid userId)
    {
        _usersServiceMock.Setup(service => service.DeleteUserAsync(It.IsAny<Guid>()))
            .ThrowsAsync(new Exception());

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Delete(userId) as ObjectResult;

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
            // 1. Delete with valid input
            new object[]
            {
                Guid.NewGuid(),
                new CrudResult<UserDto>() {
                    Result = new UserDto() { Login = "Login", Password = "Password", Fullname = "New name" },
                    ResultStatus = OperationResult.Success
                },
            },

            // 2. Delete with empty id
            new object[]
            {
                Guid.Empty,
            },

            // 3. Delete with spoiled operation
            new object[]
            {
                Guid.NewGuid(),
                new CrudResult<UserDto>() {
                    ResultStatus = OperationResult.Failure
                },
            },

            // 4. Delete with error
            new object[]
            {
                Guid.NewGuid(),
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}