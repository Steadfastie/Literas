using AutoMapper;
using LiterasDataTransfer.Dto;
using LiterasDataTransfer.ServiceAbstractions;
using LiterasModels.Requests;
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
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Details(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var userDto = await _usersService.GetUserByIdAsync(userId);

            if (userDto != null)
            {
                var responseModel = _mapper.Map<UserResponseModel>(userDto);

                return Ok(responseModel);
            }
            else
            {
                return NotFound();
            }
        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                $"{Environment.NewLine} {Environment.NewLine} " +
                $"{ex.StackTrace} " +
                $"{Environment.NewLine} {Environment.NewLine}");
            ErrorModel errorModel = new()
            {
                Message = "Could not find user",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }

    [HttpPost]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Register([FromBody] UserRequestModel userModel)
    {
        try
        {
            var userDto = _mapper.Map<UserDto>(userModel);
            var creationResult = await _usersService.CreateUserAsync(userDto);

            if (creationResult.Result == OperationResult.Success)
            {
                var responseModel = _mapper.Map<UserResponseModel>(creationResult.Dto);
                return Ok(responseModel);
            }
            else
            {
                return BadRequest("Could not register new user");
            }
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

    [HttpPatch("{id}")]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(UserRequestModel), StatusCodes.Status304NotModified)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Patch(Guid userId, [FromBody] UserRequestModel userModel)
    {
        try
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var userDto = _mapper.Map<UserDto>(userModel);
            var patchedResult = await _usersService.PatchUserAsync(userId, userDto);

            if (patchedResult.Result == OperationResult.Success)
            {
                var responseModel = _mapper.Map<UserResponseModel>(patchedResult.Dto);
                return Ok(responseModel);
            }
            else
            {
                return StatusCode(StatusCodes.Status304NotModified, userModel);
            }
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

    [HttpDelete("{id}")]
    [ProducesResponseType(typeof(UserResponseModel), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(Nullable), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorModel), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(Guid userId)
    {
        try
        {
            if (userId == Guid.Empty)
            {
                return BadRequest();
            }

            var deleteResult = await _usersService.DeleteUserAsync(userId);

            if (deleteResult.Result == OperationResult.Success)
            {
                var responseModel = _mapper.Map<UserResponseModel>(deleteResult.Dto);
                return Ok(responseModel);
            }
            else
            {
                return BadRequest("Operation unsuccessful");
            }
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