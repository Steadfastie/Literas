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
    [MemberData(nameof(GetData), parameters: 1)]
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

    public static IEnumerable<object[]> GetData(int index)
    {
        var allSeeds = new List<object[]>()
        {
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
        };

        return allSeeds.Where((_, i) => i == index);
    }
}