using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUser _userRepository;
    private readonly IMapper _mapper;
    public UserController(IUser userRepository, IMapper mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    [HttpGet]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> GetAllUsers()
    {
        var users = await _userRepository.GetAllUsers();
        var userMapped = _mapper.Map<List<UserOutputDTO>>(users);
        return Ok(userMapped);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUserById(int id)
    {
        var user = await _userRepository.GetUserById(id);
        if (user == null)
        {
            return NotFound();
        }
        var userMapped = _mapper.Map<UserOutputDTO>(user);
        return Ok(userMapped);
    }

    [HttpPost("CreateUser")]
    public async Task<IActionResult> CreateUser(UserInputDTO user)
    {
        if (user == null)
        {
            return BadRequest();
        }
        var userToCreate = _mapper.Map<User>(user);
        var createdUser = await _userRepository.CreateUser(userToCreate);
        var userMapped = _mapper.Map<UserOutputDTO>(createdUser);
        return Ok(userMapped);  
    }

    [HttpDelete("DeleteUser/{id}")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var userExist = await _userRepository.GetUserById(id);
        if (userExist == null)
        {
            return NotFound();
        }
        await _userRepository.DeleteUser(userExist);
        return Ok("User deleted successfully");
    }


}