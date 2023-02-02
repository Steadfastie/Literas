using AutoMapper;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.Responses;
using LiterasModels.System;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiterasWebAPI.Controllers;

[Route("users")]
public class UsersController : ControllerBase
{
    private readonly IUsersService _usersService;
    private readonly IContributorsService _contributorsService;
    private readonly IMapper _mapper;

    public UsersController(IUsersService usersService, IMapper mapper, IContributorsService contributorsService)
    {
        _usersService = usersService;
        _mapper = mapper;
        _contributorsService = contributorsService;
    }

    [HttpGet("{id}")]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    public async Task<IActionResult> GetUserById(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var userDto = await _usersService.GetUserByIdAsync(userId);
            var responseModel = _mapper.Map<UserResponseModel>(userDto);

            return Ok(responseModel);
        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                $"{Environment.NewLine} {Environment.NewLine} " +
                $"{ex.StackTrace} " +
                $"{Environment.NewLine} {Environment.NewLine}");
            ErrorModel errorModel = new()
            {
                Message = "Could not register new user",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }
}