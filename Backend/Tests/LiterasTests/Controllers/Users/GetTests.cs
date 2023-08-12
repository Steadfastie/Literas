using AutoMapper;
using LiterasCore.Abstractions;
using LiterasData.DTO;
using LiterasWebAPI.Controllers;
using LiterasWebAPI.Models.Responses;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace TestsLiteras.Controllers.Users;

public class GetTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IUsersService> _usersServiceMock;
    private readonly Mock<IEditorsService> _contributorsServiceMock;

    public GetTests(IMapper mapper)
    {
        _mapper = mapper;
        _usersServiceMock = new Mock<IUsersService>();
        _contributorsServiceMock = new Mock<IEditorsService>();
    }

    [Theory]
    [MemberData(nameof(GetData), 0, 1)]
    public async Task Get_Returns200WithModel_WhenIdValid(Guid userId, UserDto userDto)
    {
        _usersServiceMock.Setup(service => service.GetUserByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(userDto);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Details(userId);
        var responseModel = _mapper.Map<UserResponseModel>(userDto);

        Assert.NotNull(response);
        Assert.IsType<OkObjectResult>(response);

        var responseValue = ((OkObjectResult)response).Value;
        Assert.NotNull(responseValue);
        Assert.IsType<UserResponseModel>(responseValue);
        Assert.Equivalent(responseModel, responseValue);
    }

    [Theory]
    [MemberData(nameof(GetData), 1, 1)]
    public async Task Get_Returns400_WhenIdEmpty(Guid userId)
    {
        _usersServiceMock.Setup(service => service.GetUserByIdAsync(It.IsAny<Guid>()))!
            .ReturnsAsync(new UserDto());

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Details(userId);

        Assert.NotNull(response);
        Assert.IsType<BadRequestResult>(response);
    }

    [Theory]
    [MemberData(nameof(GetData), 2, 1)]
    public async Task Get_Returns404_WhenUserDoesNotExist(Guid userId)
    {
        _usersServiceMock.Setup(service => service.GetUserByIdAsync(It.IsAny<Guid>()))!
            .ReturnsAsync(null as UserDto);

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Details(userId);

        Assert.NotNull(response);
        Assert.IsType<NotFoundResult>(response);
    }

    [Theory]
    [MemberData(nameof(GetData), 2, 1)]
    public async Task Get_Returns500_OnException(Guid userId)
    {
        _usersServiceMock.Setup(service => service.GetUserByIdAsync(It.IsAny<Guid>()))!
            .ThrowsAsync(new Exception());

        var usersContoller = new UsersController(_usersServiceMock.Object, _mapper, _contributorsServiceMock.Object);
        var response = await usersContoller.Details(userId) as ObjectResult;

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
                new UserDto() { Id = Guid.Empty, Login = "Login", Password = "Password", Fullname = "Name" }
            },

            // 2. Details with empty id
            new object[]
            {
                Guid.Empty
            },

            // 3. Details with empty dto
            new object[]
            {
                Guid.NewGuid()
            },
        };

        return allSeeds.Skip(index).Take(count);
    }
}