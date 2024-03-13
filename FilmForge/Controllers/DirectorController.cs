using FilmForge.Common.Enum;
using FilmForge.Models.Utility;
using FilmForge.Service.DirectorService;
using Microsoft.AspNetCore.Authorization;
using Service.Security;

namespace FilmForge.Controllers;

[ApiController]
[Route("[controller]")]
public class DirectorController : ControllerBase
{
    private readonly IDirectorService directorService;
    private readonly ILogger<DirectorController> logger;

    public DirectorController(
        IDirectorService directorService,
        ILogger<DirectorController> logger)
    {
        this.directorService = directorService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all directors.
    /// </summary>
    /// <example>
    ///     Example of DirectorDtos in json array form
    ///     [
    ///         {
    ///             "id": int,
    ///             "name": string,
    ///             "email": string,
    ///             "password": string,
    ///             "role": int
    ///         },
    ///         {
    ///             "id": int,
    ///             "name": string,
    ///             "email": string,
    ///             "password": string,
    ///             "role": int
    ///         },
    ///         {
    ///             ...
    ///         }
    ///     ]
    /// </example>
    /// <response code="200">Returns the list of directors</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if that there are no directors in the Database</response>
    // GET: /Director/all
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<DirectorDto>>> GetAllDirectors()
    {
        logger.LogInformation("Triggered Endpoint GET: director/all");

        try
        {
            logger.LogInformation("Triggering Director Service: GetAllAsync");

            var directors = await directorService.GetAllAsync();

            if (directors == null)
            {
                logger.LogWarning("No Directors not found.");

                return NotFound("No Directors not found.");
            }

            return Ok(directors);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a director by their ID.
    /// </summary>
    /// <example>
    ///     Example of DirectorDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the director to retrieve.</param>
    /// <response code="200">Returns the director corresponding to the specified ID</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no director is found with the specified ID</response>
    // GET: /Director/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<DirectorDto>> GetDirectorById(int id)
    {
        logger.LogInformation($"Triggered Endpoint GET: director/{id}");

        try
        {
            logger.LogInformation("Triggering Director Service: GetByIdAsync");

            var director = await directorService.GetByIdAsync(id);

            if (director == null)
            {
                logger.LogInformation($"Director with id: {id} not found");

                return NotFound(id);
            }

            return Ok(director);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Creates a new director.
    /// </summary>
    /// <example>
    ///     Example of DirectorDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="directorDto">The director data transfer object.</param>
    /// <response code="201">Returns the newly created director</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when director not authorized to use this endpoint</response>
    // POST: /Director/add
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPost("add")]
    public async Task<ActionResult<DirectorDto>> CreateDirector([FromBody] DirectorDto directorDto)
    {
        logger.LogInformation("Triggered Endpoint POST: director/add");

        try
        {
            if (ModelState.IsValid)
            {
                logger.LogInformation("Triggering Director Service: CreateAsync");

                var createdDirector = await directorService.CreateAsync(directorDto);

                return CreatedAtAction(
                    nameof(GetDirectorById),
                    new { id = createdDirector.Id });
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
    /// Updates an existing director.
    /// </summary>
    /// <example>
    ///     Example of DirectorDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the director to update.</param>
    /// <param name="directorDto">The updated director data transfer object.</param>
    /// <response code="200">If the director was updated successfully</response>
    /// <response code="400">If the ID does not match the directorDto ID or some other error message with what happened</response>
    /// <response code="401">Returns when director not authorized to use this endpoint</response>
    // PUT: /Director/update/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPut("update/{id}")]
    public async Task<ActionResult<DirectorDto>> UpdateDirector(int id, [FromBody] DirectorDto directorDto)
    {
        logger.LogInformation($"Triggered Endpoint PUT: director/update/{id}");

        try
        {
            logger.LogInformation("Triggering Director Service: UpdateDirector");

            if (id != directorDto.Id)
            {
                logger.LogError($"The id: {id} and DTO id: {directorDto.Id} don't match");

                return BadRequest("Path and Body id don't match");
            }

            var director = await directorService.UpdateAsync(id, directorDto);

            return Ok(director);
        }
        catch (Exception e)
        {

            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Deletes a director by their ID.
    /// </summary>
    /// <param name="id">The ID of the director to delete.</param>
    /// <response code="200">Returns true if the director was deleted successfully</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when director not authorized to use this endpoint</response>
    // DELETE: /Director/delete/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> DeleteDirector(int id)
    {
        logger.LogInformation($"Triggered Endpoint DELETE: director/delete/{id}");

        try
        {
            logger.LogInformation("Triggering Director Service: DeleteByIdAsync");

            var isDeleted = await directorService.DeleteByIdAsync(id);

            if (!isDeleted)
            {
                logger.LogError($"Failed to delete director with id: {id}");

                return BadRequest($"Failed to delete director with id: {id}");
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
