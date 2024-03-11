using FilmForge.Models.Dtos;
using FilmForge.Service.UserService;
using Microsoft.AspNetCore.Mvc;

namespace FilmForge.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;

    public UserController(IUserService userService)
    {
        this.userService = userService;
    }

    // GET: /User/all
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        var users = await userService.GetAllAsync();
        return Ok(users);
    }

    // GET: /User/{id}
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        var user = await userService.GetByIdAsync(id);

        if (user == null)
            return NotFound(id);

        return Ok(user);
    }

    // POST: /User/add
    [HttpPost("add")]
    public async Task<ActionResult<UserDto>> CreateUser(UserDto userDto)
    {
        var createdUser = await userService.CreateAsync(userDto);

        return CreatedAtAction(nameof(GetUserById), new { id = createdUser.Id }, createdUser);
    }

    // PUT: /User/update/{id}
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody]UserDto userDto)
    {
        if (id != userDto.Id)
            return BadRequest("Path and Body id don't match");

        var user = await userService.UpdateAsync(id, userDto);

        return Ok(user);
    }

    // DELETE: /User/delete/{id}
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        var isDeleted = await userService.DeleteByIdAsync(id);

        return Ok(isDeleted);
    }
}
