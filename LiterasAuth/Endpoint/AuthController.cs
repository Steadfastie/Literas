using Duende.IdentityServer;
using Duende.IdentityServer.Events;
using Duende.IdentityServer.Models;
using Duende.IdentityServer.Services;
using LiterasAuth.Auth;
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
    public async Task<IActionResult> Login([FromBody] UserCredentialsModel credentials)
    {
        try
        {
            if (!ModelState.IsValid) throw new ArgumentException("ModelState invalid");

            var signInResult = await _signInManager
                .PasswordSignInAsync(
                    credentials.Username,
                    credentials.Password,
                    isPersistent: false,
                    lockoutOnFailure: false);

            if (!signInResult.Succeeded)
                return BadRequest(new OperationResponse
                {
                    Type = "Username",
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

            await HttpContext.SignInAsync(new IdentityServerUser(user.Id.ToString()));
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
            ErrorModel errorModel = new()
            {
                Message = "Could not find doc",
                StatusCode = StatusCodes.Status500InternalServerError,
            };
            return Problem(detail: errorModel.Message, statusCode: errorModel.StatusCode);
        }
    }
}
