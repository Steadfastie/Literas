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

public class PostTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUsersService> _usersServiceMock;
    private readonly Mock<IEditorsService> _contributorsServiceMock;

    public PostTests(IMapper mapper)
    {
        _mapper = mapper;
        _usersServiceMock = new Mock<IUsersService>();
        _contributorsServiceMock = new Mock<IEditorsService>();
    }

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task Post_Returns200WithModel_WhenModelIsValid(UserRequestModel requestModel, CrudResult<UserDto> created)
    {
        _usersServiceMock.Setup(service => service.CreateUserAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(created);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Register(requestModel);
        var responseModel = _mapper.Map<UserResponseModel>(created.Dto);

        Assert.NotNull(response);
        Assert.IsType<OkObjectResult>(response);

        var responseValue = ((OkObjectResult)response).Value;
        Assert.NotNull(responseValue);
        Assert.IsType<UserResponseModel>(responseValue);
        Assert.Equal(responseModel.ToString(), responseValue.ToString());
    }

    [Theory]
    [MemberData(nameof(GetData), 1, 1)]
    public async Task Get_Returns400_WhenModelIsInvalid(UserRequestModel requestModel, CrudResult<UserDto> created)
    {
        _usersServiceMock.Setup(service => service.CreateUserAsync(It.IsAny<UserDto>()))
            .ReturnsAsync(created);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Register(requestModel);

        Assert.NotNull(response);
        Assert.IsType<BadRequestObjectResult>(response);
    }

    [Theory]
    [MemberData(nameof(GetData), 2, 1)]
    public async Task Post_Returns500_OnExceptionAsync(UserRequestModel userModel)
    {
        _usersServiceMock.Setup(service => service.CreateUserAsync(It.IsAny<UserDto>()))!
            .ThrowsAsync(new Exception());

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Register(userModel) as ObjectResult;

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
            // 1. Post with valid model
            new object[]
            {
                new UserRequestModel() { Login = "Login", Password = "Password" },
                new CrudResult<UserDto>() {
                    Result = OperationResult.Success,
                    Dto = new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" }
                }
            },

            // 2. Post with invalid model
            new object[]
            {
                new UserRequestModel() { Password = "Password" },
                new CrudResult<UserDto>() {
                    Result = OperationResult.Failure,
                }
            },

            // 2. Post with exception
            new object[]
            {
                new UserRequestModel() { Login = "Login", Password = "Password" },
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}