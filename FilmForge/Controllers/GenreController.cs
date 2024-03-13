using FilmForge.Common.Enum;
using FilmForge.Models.Utility;
using FilmForge.Service.GenreService;
using Microsoft.AspNetCore.Authorization;
using Service.Security;

namespace FilmForge.Controllers;

[ApiController]
[Route("[controller]")]
public class GenreController : ControllerBase
{
    private readonly IGenreService genreService;
    private readonly ILogger<GenreController> logger;

    public GenreController(
        IGenreService genreService,
        ILogger<GenreController> logger)
    {
        this.genreService = genreService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all genres.
    /// </summary>
    /// <example>
    ///     Example of GenreDtos in json array form
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
    /// <response code="200">Returns the list of genres</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if that there are no genres in the Database</response>
    // GET: /Genre/all
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<GenreDto>>> GetAllGenres()
    {
        logger.LogInformation("Triggered Endpoint GET: genre/all");

        try
        {
            logger.LogInformation("Triggering Genre Service: GetAllAsync");

            var genres = await genreService.GetAllAsync();

            if (genres == null)
            {
                logger.LogWarning("No Genres not found.");

                return NotFound("No Genres not found.");
            }

            return Ok(genres);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a genre by their ID.
    /// </summary>
    /// <example>
    ///     Example of GenreDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the genre to retrieve.</param>
    /// <response code="200">Returns the genre corresponding to the specified ID</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no genre is found with the specified ID</response>
    // GET: /Genre/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<GenreDto>> GetGenreById(int id)
    {
        logger.LogInformation($"Triggered Endpoint GET: genre/{id}");

        try
        {
            logger.LogInformation("Triggering Genre Service: GetByIdAsync");

            var genre = await genreService.GetByIdAsync(id);

            if (genre == null)
            {
                logger.LogInformation($"Genre with id: {id} not found");

                return NotFound(id);
            }

            return Ok(genre);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Creates a new genre.
    /// </summary>
    /// <example>
    ///     Example of GenreDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="genreDto">The genre data transfer object.</param>
    /// <response code="201">Returns the newly created genre</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when genre not authorized to use this endpoint</response>
    // POST: /Genre/add
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPost("add")]
    public async Task<ActionResult<GenreDto>> CreateGenre([FromBody] GenreDto genreDto)
    {
        logger.LogInformation("Triggered Endpoint POST: genre/add");

        try
        {
            if (ModelState.IsValid)
            {
                logger.LogInformation("Triggering Genre Service: CreateAsync");

                var createdGenre = await genreService.CreateAsync(genreDto);

                return CreatedAtAction(
                    nameof(GetGenreById),
                    new { id = createdGenre.Id });
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
    /// Updates an existing genre.
    /// </summary>
    /// <example>
    ///     Example of GenreDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the genre to update.</param>
    /// <param name="genreDto">The updated genre data transfer object.</param>
    /// <response code="200">If the genre was updated successfully</response>
    /// <response code="400">If the ID does not match the genreDto ID or some other error message with what happened</response>
    /// <response code="401">Returns when genre not authorized to use this endpoint</response>
    // PUT: /Genre/update/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPut("update/{id}")]
    public async Task<ActionResult<GenreDto>> UpdateGenre(int id, [FromBody] GenreDto genreDto)
    {
        logger.LogInformation($"Triggered Endpoint PUT: genre/update/{id}");

        try
        {
            logger.LogInformation("Triggering Genre Service: UpdateGenre");

            if (id != genreDto.Id)
            {
                logger.LogError($"The id: {id} and DTO id: {genreDto.Id} don't match");

                return BadRequest("Path and Body id don't match");
            }

            var genre = await genreService.UpdateAsync(id, genreDto);

            return Ok(genre);
        }
        catch (Exception e)
        {

            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Deletes a genre by their ID.
    /// </summary>
    /// <param name="id">The ID of the genre to delete.</param>
    /// <response code="200">Returns true if the genre was deleted successfully</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when genre not authorized to use this endpoint</response>
    // DELETE: /Genre/delete/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> DeleteGenre(int id)
    {
        logger.LogInformation($"Triggered Endpoint DELETE: genre/delete/{id}");

        try
        {
            logger.LogInformation("Triggering Genre Service: DeleteByIdAsync");

            var isDeleted = await genreService.DeleteByIdAsync(id);

            if (!isDeleted)
            {
                logger.LogError($"Failed to delete genre with id: {id}");

                return BadRequest($"Failed to delete genre with id: {id}");
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
