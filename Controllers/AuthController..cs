using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authentication;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUser _iuser;
    private readonly IMapper _mapper;
    private readonly TokenService _tokenService;
    public AuthController(IUser userRepo, IMapper mapper, TokenService tokenService)
    {
        _iuser = userRepo;
        _mapper = mapper;
        _tokenService = tokenService;
    }

    [HttpGet("google/login")]
    public IActionResult GoogleLogin()
    {
        var properties = new AuthenticationProperties
        {
            RedirectUri = "/api/auth/google/callback"
        };
        return Challenge(properties, "Google");
    }

    [HttpGet("google/callback")]
    public async Task<IActionResult> GoogleCallBack()
    {
        var result = await HttpContext.AuthenticateAsync("External");

        if (!result.Succeeded)
            return Unauthorized($"Google Authentication Failed: {result.Failure?.Message ?? "Unknown error"}");

        var googleId = result.Principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var email = result.Principal.FindFirst(ClaimTypes.Email)?.Value;
        var name = result.Principal.FindFirst(ClaimTypes.Name)?.Value;

        if (googleId == null || email == null)
            return Unauthorized("Could not retrieve user information from Google");

        var existingUser = await _iuser.GetUserByGoogleId(googleId);
        if (existingUser != null)
        {
            var userMapped = _mapper.Map<UserOutputDTO>(existingUser);
            var token = _tokenService.GenerateToken(existingUser);
            return Ok(new
            {
                token, userMapped
            });
        }

        var newUser = new User
        {
            GoogleId = googleId,
            Email = email,
            Username = name,
        };

        var createdUser = await _iuser.CreateUser(newUser);
        var nowToken = _tokenService.GenerateToken(createdUser);
        //var userOutput = _mapper.Map<UserOutputDTO>(createdUser);

        return Ok(new
        {
            token = nowToken,
            user = _mapper.Map<UserOutputDTO>(createdUser)
        } );
            }
}