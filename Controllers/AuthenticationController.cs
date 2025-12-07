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
            return NotFound(new
            {
                message = "User not found!",
                id = (string?)null
            });
            
        }

        if (user.Password == UtilService.ReturnMD5(login.Password)){
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
        else{
            AccessLog.Error($"User '{login.Login}' Password '{login.Password}'");
            return Unauthorized(new
            {
                message = "Wrong password!",
                id = user.Id
            });    
        }
        
    }

    [HttpPost]
    [Route("loginsql")]
    public async Task<ActionResult> LoginSQL([FromBody]LoginRequest login)
    {
        var user = await UserRepository.LoginSQL(login);


        if (user == null)
        {
            return Unauthorized(new
            {
                message = "User not found!"
            });
        }else{
            user.Password = user.Password;  
            var token = TokenService.GenerateToken(user);
            AccessLog.Info($"User '{login.Login}' Password '{login.Password}' logged");
            return Ok(new
            {
                User = user,
                token = token
            });              
        }
        
    }

}