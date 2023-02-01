using AutoMapper;
using LiterasBusiness.Services;
using LiterasCQS.Commands.Users;
using LiterasDataTransfer;
using LiterasDataTransfer.DTO;
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
    [MemberData(nameof(GetUserAndPatchModel), parameters: true)]
    public async Task PatchUser_WithValidPatchModel(UserDTO User, List<PatchModel> patchlist)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new UsersService(_mapper, _mediatrMock.Object);

        var result = await service.PatchUserAsync(User, patchlist);
        Assert.Equal(1, result);
    }

    [Theory]
    [MemberData(nameof(GetUserAndPatchModel), parameters: false)]
    public void PatchUser_WithIdInPatchModel_ThrowsArgumentException(UserDTO User, List<PatchModel> patchlist)
    {
        _mediatrMock.Setup(mediator => mediator.Send(
            It.IsAny<PatchUserCommand>(), It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var service = new UsersService(_mapper, _mediatrMock.Object);

        Assert.ThrowsAsync<ArgumentException>(async () => await service.PatchUserAsync(User, patchlist));
    }

    public static IEnumerable<object[]> GetUserAndPatchModel(bool valid)
    {
        var allDrugs = new List<object[]>()
        {
            new object[]
            {
                new UserDTO()
                {
                    Id = Guid.NewGuid(),
                    Login = "Login",
                    Password = "Password"
                },
                new List<PatchModel>()
                {
                    new PatchModel()
                    {
                        PropertyName = "Login",
                        PropertyValue = "New login",
                    },
                }
            },
            new object[]
            {
                new UserDTO()
                {
                    Id = Guid.NewGuid(),
                    Login = "Login",
                    Password = "Password"
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