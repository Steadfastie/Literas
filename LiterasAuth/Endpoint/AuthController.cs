using Duende.IdentityServer;

using Microsoft.AspNetCore.Authentication;
using Duende.IdentityServer.Services;
using LiterasAuth.Auth;
using LiterasModels.Responses;
using LiterasModels.System;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace LiterasAuth.Endpoint;

[Route("auth")]
[EnableCors("literas")]
[ApiController]
public class AuthController : ControllerBase
{
    private readonly UserManager<LiterasUser> _userManager;
    private readonly SignInManager<LiterasUser> _signInManager;
    private readonly IIdentityServerInteractionService _interaction;

    public AuthController(
        UserManager<LiterasUser> userManager,
        SignInManager<LiterasUser> signInManager,
        IIdentityServerInteractionService interaction
        )
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interaction = interaction;
    }
    
    [HttpPost("login")]
    [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Login([FromBody] UserCredentialsModel credentials)
    {
        try
        {
            if (!ModelState.IsValid) throw new ArgumentException("ModelState invalid");

            var signInResult = await _signInManager
                .PasswordSignInAsync(
                    credentials.Username,
                    credentials.Password,
                    isPersistent: true,
                    lockoutOnFailure: false);

            if (!signInResult.Succeeded)
                return Unauthorized(new OperationResponse
                {
                    Type = "Login",
                    Succeeded = false,
                    ErrorMessage = "Invalid login or password"
                });

            var user = await _userManager.FindByNameAsync(credentials.Username);
            var context = await _interaction.GetAuthorizationContextAsync(credentials.ReturnUrl);

            if (context == null)
                return BadRequest(new OperationResponse
                {
                    Type = "Username",
                    Succeeded = false,
                    ErrorMessage = "Invalid login or password"
                });

            //await HttpContext.SignInAsync(
            //    new IdentityServerUser(user.Id.ToString()), 
            //    new AuthenticationProperties()
            //    {
            //        AllowRefresh = true,
            //        IsPersistent = true
            //    });

            return Ok(new OperationResponse
            {
                Type = "Username",
                Succeeded = true,
                ReturnUrl = credentials.ReturnUrl
            });

        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                      $"{Environment.NewLine} {Environment.NewLine} " +
                      $"{ex.StackTrace} " +
                      $"{Environment.NewLine} {Environment.NewLine}");
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    [HttpPost("signup")]
    [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(OperationResponse), StatusCodes.Status409Conflict)]
    [ProducesResponseType(typeof(ObjectResult), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Signup([FromBody] UserCredentialsModel credentials)
    {
        try
        {
            if (!ModelState.IsValid) throw new ArgumentException("Invalid model");

            var signInResult = await _signInManager
                .PasswordSignInAsync(
                    credentials.Username,
                    credentials.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);

            if (signInResult.Succeeded)
                return Conflict(new OperationResponse
                {
                    Type = "Signup",
                    Succeeded = false,
                    ErrorMessage = "User already created"
                });

            var context = await _interaction.GetAuthorizationContextAsync(credentials.ReturnUrl);

            if (context == null)
                return BadRequest(new OperationResponse
                {
                    Type = "Signup",
                    Succeeded = false,
                    ErrorMessage = "Invalid return url"
                });

            var literasUser = new LiterasUser(credentials.Username);
            var creationResult = await _userManager.CreateAsync(literasUser, credentials.Password);

            if (!creationResult.Succeeded)
            {
                throw new InvalidOperationException("Could not create user");
            }

            var createdUser = await _userManager.FindByNameAsync(credentials.Username);

            await HttpContext.SignInAsync(new IdentityServerUser(createdUser.Id.ToString()));
            return Ok(new OperationResponse
            {
                Type = "Signup",
                Succeeded = true,
                ReturnUrl = credentials.ReturnUrl
            });

        }
        catch (Exception ex)
        {
            Log.Error($"!--- {ex.Message} ---! " +
                      $"{Environment.NewLine} {Environment.NewLine} " +
                      $"{ex.StackTrace} " +
                      $"{Environment.NewLine} {Environment.NewLine}");
            return Problem(detail: ex.Message, statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
