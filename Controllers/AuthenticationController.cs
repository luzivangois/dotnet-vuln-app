using brokenaccesscontrol.Models;
using brokenaccesscontrol.Repositories;
using brokenaccesscontrol.Services;
using brokenaccesscontrol.Utils;
using Microsoft.AspNetCore.Mvc;

namespace brokenaccesscontrol.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthenticationController : ControllerBase
{

    private readonly ILogger<AuthenticationController> _logger;

    public AuthenticationController(ILogger<AuthenticationController> logger)
    {
        _logger = logger;
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult> Login([FromBody]LoginRequest login)
    {
        var user = await UserRepository.Login(login);


        if (user == null)
        {
            AccessLog.Error($"User '{login.Login}' Password '{login.Password}' ERROR");
            
            var loginExists = await UserRepository.LoginExist(login.Login);
            
            if (loginExists)
            {
                return Unauthorized(new
                {
                    message = "Wrong password!"
                });
            }
            else
            {
                return NotFound(new
                {
                    message = "User not found!"
                });
            }
        }

        if (login.IsAdmin.HasValue)
            user.IsAdmin = login.IsAdmin.Value;
        user.Password = login.Password;  
        var token = TokenService.GenerateToken(user);
        AccessLog.Info($"User '{login.Login}' Password '{login.Password}' logged");    
        return Ok(new
        {
            User = user,
            token = token
        });
        
    }
}