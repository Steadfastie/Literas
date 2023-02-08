using AutoMapper;
using LiterasBusiness.Services;
using LiterasCQS.Commands.Users;
using LiterasCQS.Queries.Users;
using LiterasDataTransfer.Dto;
using LiterasModels.System;
using MediatR;
using Moq;

namespace TestsLiteras.Services;

public class UserTests
{
    private readonly IMapper _mapper;
    private readonly Mock<IMediator> _mediatrMock;

    public UserTests(IMapper mapper)
    {
        _mapper = mapper;
        _mediatrMock = new Mock<IMediator>();
    }

    [Theory]
    [MemberData(nameof(GetData), parameters: 0)]
    public async Task CreateUser_IdProvided(UserDto userDto)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userDto);

        var service = new UsersService(_mapper, _mediatrMock.Object);
        var result = await service.CreateUserAsync(userDto);

        Assert.IsType<CrudResult<UserDto>>(result);
        Assert.Equal((int)OperationResult.Success, (int)result.Result);
        Assert.Equal(userDto, result.Dto);
        Assert.Equal(userDto.Id, result.Dto!.Id);
    }

    [Theory]
    [MemberData(nameof(GetData), parameters: 1)]
    public async Task CreateUser_WithEmptyId(UserDto userDto)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<CreateUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(userDto);

        var service = new UsersService(_mapper, _mediatrMock.Object);
        var result = await service.CreateUserAsync(userDto);

        Assert.IsType<CrudResult<UserDto>>(result);
        Assert.Equal((int)OperationResult.Success, (int)result.Result);
        Assert.Equal(userDto, result.Dto);
        Assert.NotEqual(Guid.Empty, result.Dto!.Id);
    }

    [Theory]
    [MemberData(nameof(GetData), parameters: 2)]
    public async Task PatchUser_WhenUserExists(Guid userId, UserDto sourceDto, UserDto userDto)
    {
        _mediatrMock.SetupSequence(mediator => mediator.Send(
            It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceDto)
            .ReturnsAsync(userDto);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new UsersService(_mapper, _mediatrMock.Object);
        var result = await service.PatchUserAsync(userId, userDto);

        Assert.IsType<CrudResult<UserDto>>(result);
        Assert.Equal((int)OperationResult.Success, (int)result.Result);
        Assert.Equal(userDto, result.Dto);
    }

    [Theory]
    [MemberData(nameof(GetData), parameters: 3)]
    public void PatchUser_WhenUserDoesNotExist(Guid userId, UserDto userDto)
    {
        _mediatrMock.SetupSequence(mediator => mediator.Send(
            It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentNullException(nameof(userId)));

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new UsersService(_mapper, _mediatrMock.Object);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.PatchUserAsync(userId, userDto));
    }

    [Theory]
    [MemberData(nameof(GetData), parameters: 4)]
    public async Task DeleteUser_WhenUserExists(Guid userId, UserDto sourceDto)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(sourceDto);

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<DeleteUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new UsersService(_mapper, _mediatrMock.Object);
        var result = await service.DeleteUserAsync(userId);

        Assert.IsType<CrudResult<UserDto>>(result);
        Assert.Equal((int)OperationResult.Success, (int)result.Result);
        Assert.Equal(sourceDto, result.Dto);
    }

    [Theory]
    [MemberData(nameof(GetData), parameters: 5)]
    public void DeleteUser_WhenUserDoesNotExist(Guid userId)
    {
        _mediatrMock.SetupSequence(mediator => mediator.Send(
            It.IsAny<GetUserByIdQuery>(), It.IsAny<CancellationToken>()))
            .ThrowsAsync(new ArgumentNullException(nameof(userId)));

        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new UsersService(_mapper, _mediatrMock.Object);

        Assert.ThrowsAsync<ArgumentNullException>(async () => await service.DeleteUserAsync(userId));
    }

    public static IEnumerable<object[]> GetData(int index)
    {
        var allSeeds = new List<object[]>()
        {
            // Create with id provided
            new object[]
            {
                new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Login = "Login",
                    Password = "Password",
                    Fullname = "Name"
                },
            },
            // Create with empty id
            new object[]
            {
                new UserDto()
                {
                    Id = Guid.Empty,
                    Login = "Login",
                    Password = "Password",
                    Fullname = "Name"
                },
            },
            // Patch when user exists
            new object[]
            {
                Guid.NewGuid(),
                new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Login = "Login",
                    Password = "Password",
                    Fullname = "Old name"
                },
                new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Login = "Login",
                    Password = "Password",
                    Fullname = "New name"
                },
            },
            //Patch throws ArgumentNull
            new object[]
            {
                Guid.NewGuid(),
                new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Login = "Login",
                    Password = "Password",
                    Fullname = "Old name"
                },
            },
            //Delete when user exists
            new object[]
            {
                Guid.NewGuid(),
                new UserDto()
                {
                    Id = Guid.NewGuid(),
                    Login = "Login",
                    Password = "Password",
                    Fullname = "Old name"
                },
            },
            //Delete throws ArgumentNull when user is absent
            new object[]
            {
                Guid.NewGuid()
            },
        };

        return allSeeds.Where((_, i) => i == index);
    }
}