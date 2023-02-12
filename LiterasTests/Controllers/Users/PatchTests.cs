using AutoMapper;
using LiterasDataTransfer.Dto;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.Requests;
using LiterasModels.Responses;
using LiterasModels.System;
using LiterasWebAPI.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestsLiteras.Controllers.Users;

public class PatchTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUsersService> _usersServiceMock;
    private readonly Mock<IEditorsService> _contributorsServiceMock;

    public PatchTests(IMapper mapper)
    {
        _mapper = mapper;
        _usersServiceMock = new Mock<IUsersService>();
        _contributorsServiceMock = new Mock<IEditorsService>();
    }

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task Patch_Returns200WithModel_WhenIdValid(
        Guid userId, UserRequestModel userModel, CrudResult<UserDto> operationResult)
    {
        _usersServiceMock.Setup(service => service.PatchUserAsync(It.IsAny<Guid>(), It.IsAny<UserDto>()))
            .ReturnsAsync(operationResult);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Patch(userId, userModel);
        var responseModel = _mapper.Map<UserResponseModel>(operationResult.Dto);

        Assert.NotNull(response);
        Assert.IsType<OkObjectResult>(response);

        var responseValue = ((OkObjectResult)response).Value;
        Assert.NotNull(responseValue);
        Assert.IsType<UserResponseModel>(responseValue);
        Assert.Equivalent(responseModel, responseValue);
    }

    [Theory]
    [MemberData(nameof(GetData), 1, 1)]
    public async Task Patch_Returns400_WhenIdEmpty(Guid userId, UserRequestModel userModel)
    {
        _usersServiceMock.Setup(service => service.PatchUserAsync(It.IsAny<Guid>(), It.IsAny<UserDto>()))!
            .ReturnsAsync(null as CrudResult<UserDto>);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Patch(userId, userModel);

        Assert.NotNull(response);
        Assert.IsType<BadRequestResult>(response);
    }

    [Theory]
    [MemberData(nameof(GetData), 2, 1)]
    public async Task Patch_Returns304_WhenPatchIsUnsuccessful(
        Guid userId, UserRequestModel userModel, CrudResult<UserDto> operationResult)
    {
        _usersServiceMock.Setup(service => service.PatchUserAsync(It.IsAny<Guid>(), It.IsAny<UserDto>()))
            .ReturnsAsync(operationResult);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Patch(userId, userModel);

        Assert.NotNull(response);
        Assert.IsType<ObjectResult>(response);

        var responseObject = response as ObjectResult;
        Assert.Equal(304, responseObject?.StatusCode);
        Assert.NotNull(responseObject?.Value);
        Assert.IsType<UserRequestModel>(responseObject?.Value);
        Assert.Equivalent(userModel, responseObject?.Value);
    }

    [Theory]
    [MemberData(nameof(GetData), 3, 1)]
    public async Task Patch_Returns500_OnException(Guid userId, UserRequestModel userModel)
    {
        _usersServiceMock.Setup(service => service.PatchUserAsync(It.IsAny<Guid>(), It.IsAny<UserDto>()))
            .ThrowsAsync(new Exception());

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Patch(userId, userModel) as ObjectResult;

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
            // 1. Patch with valid input
            new object[]
            {
                Guid.NewGuid(),
                new UserRequestModel() { Login = "Login", Password = "Password", Fullname = "New name" },
                new CrudResult<UserDto>() {
                    Dto = new UserDto() { Login = "Login", Password = "Password", Fullname = "New name" },
                    Result = OperationResult.Success
                },
            },

            // 2. Patch with empty id
            new object[]
            {
                Guid.Empty,
                new UserRequestModel() { Login = "Login", Password = "Password", Fullname = "New name" }
            },

            // 3. Patch with spoiled operation
            new object[]
            {
                Guid.NewGuid(),
                new UserRequestModel() { Login = "Login", Password = "Password", Fullname = "New name" },
                new CrudResult<UserDto>() {
                    Result = OperationResult.Failure
                },
            },

            // 4. Patch with error
            new object[]
            {
                Guid.NewGuid(),
                new UserRequestModel() { Login = "Login", Password = "Password", Fullname = "New name" }
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}