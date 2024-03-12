using FilmForge.Common.Enum;
using Microsoft.AspNetCore.Authorization;

namespace FilmForge.Controllers;

[ApiController]
[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserService userService;
    private readonly ILogger<UserController> logger;

    public UserController(
        IUserService userService,
        ILogger<UserController> logger)
    {
        this.userService = userService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all users.
    /// </summary>
    /// <example>
    ///     Example of UserDtos in json array form
    ///     [
    ///         {
    ///             "id": int,
    ///             "name": string,
    ///             "email": int,
    ///             "password": int,
    ///             "role": int
    ///         },
    ///         {
    ///             "id": int,
    ///             "name": string,
    ///             "email": int,
    ///             "password": int,
    ///             "role": int
    ///         },
    ///         {
    ///             ...
    ///         }
    ///     ]
    /// </example>
    /// <response code="200">Returns the list of users</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if that there are no users in the Database</response>
    // GET: /User/all
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<UserDto>>> GetAllUsers()
    {
        logger.LogInformation("Triggered Endpoint GET: users/all");

        try
        {
            logger.LogInformation("Triggering User Service: GetAllAsync");

            var users = await userService.GetAllAsync();
            
            if (users == null)
            {
                logger.LogWarning("No Users not found.");

                return NotFound("No Users not found.");
            }

            return Ok(users);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a user by their ID.
    /// </summary>
    /// <example>
    ///     Example of UserDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": int,
    ///         "password": int,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the user to retrieve.</param>
    /// <response code="200">Returns the user corresponding to the specified ID</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no user is found with the specified ID</response>
    // GET: /User/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<UserDto>> GetUserById(int id)
    {
        logger.LogInformation($"Triggered Endpoint GET:/User/{id}");

        try
        {
            logger.LogInformation("Triggering User Service: GetByIdAsync");

            var user = await userService.GetByIdAsync(id);

            if (user == null)
            {
                logger.LogInformation($"User with id: {id} not found");

                return NotFound(id);
            }

            return Ok(user);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Creates a new user.
    /// </summary>
    /// <example>
    ///     Example of UserDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": int,
    ///         "password": int,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="userDto">The user data transfer object.</param>
    /// <response code="201">Returns the newly created user</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when user not authorized to use this endpoint</response>
    // POST: /User/add
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPost("add")]
    public async Task<ActionResult<UserDto>> CreateUser([FromBody] UserDto userDto)
    {
        logger.LogInformation("Triggered Endpoint POST:/User/add");

        try
        {
            if (ModelState.IsValid)
            {

                logger.LogInformation("Triggering User Service: CreateAsync");

                var createdUser = await userService.CreateAsync(userDto);


                return CreatedAtAction(
                    nameof(GetUserById),
                    new { id = createdUser.Id });
            }

            logger.LogError("Model didn't pass validation");

            return BadRequest(ModelState);
        }
        catch (Exception e)
        {
            
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Updates an existing user.
    /// </summary>
    /// <example>
    ///     Example of UserDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": int,
    ///         "password": int,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the user to update.</param>
    /// <param name="userDto">The updated user data transfer object.</param>
    /// <response code="200">If the user was updated successfully</response>
    /// <response code="400">If the ID does not match the userDto ID or some other error message with what happened</response>
    /// <response code="401">Returns when user not authorized to use this endpoint</response>
    // PUT: /User/update/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPut("update/{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UserDto userDto)
    {
        logger.LogInformation($"Triggered Endpoint PUT:update/{id}");

        try
        {
            logger.LogInformation("Triggering User Service: UpdateUser");

            if (id != userDto.Id)
            {
                logger.LogError($"The id: {id} and DTO id: {userDto.Id} don't match");

                return BadRequest("Path and Body id don't match");
            }

            var user = await userService.UpdateAsync(id, userDto);

            return Ok(user);
        }
        catch (Exception e)
        {

            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Deletes a user by their ID.
    /// </summary>
    /// <param name="id">The ID of the user to delete.</param>
    /// <response code="200">Returns true if the user was deleted successfully</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when user not authorized to use this endpoint</response>
    // DELETE: /User/delete/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpDelete("delete/{id}")]
    public async Task<IActionResult> DeleteUser(int id)
    {
        logger.LogInformation($"Triggered Endpoint DELETE:delete/{id}");

        try
        {
            logger.LogInformation("Triggering User Service: DeleteByIdAsync");

            var isDeleted = await userService.DeleteByIdAsync(id);

            if (!isDeleted)
            {
                logger.LogError($"Failed to delete user with id: {id}");

                return BadRequest($"Failed to delete user with id: {id}");
            }

            return Ok(isDeleted);
        }
        catch (Exception e)
        {

            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }
}
