using FilmForge.Common.Enum;
using Microsoft.AspNetCore.Authorization;

namespace FilmForge.Controllers;

[ApiController]
[Route("[controller]")]
public class MovieController : ControllerBase
{
    private readonly IMovieService movieService;
    private readonly ILogger<MovieController> logger;

    public MovieController(
        IMovieService movieService,
        ILogger<MovieController> logger)
    {
        this.movieService = movieService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all movies.
    /// </summary>
    /// <example>
    ///     Example of MovieDtos in json array form
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
    /// <response code="200">Returns the list of movies</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if that there are no movies in the Database</response>
    // GET: /Movie/all
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<MovieDto>>> GetAllMovies()
    {
        logger.LogInformation("Triggered Endpoint GET: movie/all");

        try
        {
            logger.LogInformation("Triggering Movie Service: GetAllAsync");

            var movies = await movieService.GetAllAsync();

            if (movies == null)
            {
                logger.LogWarning("No Movies not found.");

                return NotFound("No Movies not found.");
            }

            return Ok(movies);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a movie by their ID.
    /// </summary>
    /// <example>
    ///     Example of MovieDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the movie to retrieve.</param>
    /// <response code="200">Returns the movie corresponding to the specified ID</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no movie is found with the specified ID</response>
    // GET: /Movie/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<MovieDto>> GetMovieById(int id)
    {
        logger.LogInformation($"Triggered Endpoint GET: movie/{id}");

        try
        {
            logger.LogInformation("Triggering Movie Service: GetByIdAsync");

            var movie = await movieService.GetByIdAsync(id);

            if (movie == null)
            {
                logger.LogInformation($"Movie with id: {id} not found");

                return NotFound(id);
            }

            return Ok(movie);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Creates a new movie.
    /// </summary>
    /// <example>
    ///     Example of MovieDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="movieDto">The movie data transfer object.</param>
    /// <response code="201">Returns the newly created movie</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when movie not authorized to use this endpoint</response>
    // POST: /Movie/add
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPost("add")]
    public async Task<ActionResult<MovieDto>> CreateMovie([FromBody] MovieDto movieDto)
    {
        logger.LogInformation("Triggered Endpoint POST: movie/add");

        try
        {
            if (ModelState.IsValid)
            {
                logger.LogInformation("Triggering Movie Service: CreateAsync");

                var createdMovie = await movieService.CreateAsync(movieDto);

                return CreatedAtAction(
                    nameof(GetMovieById),
                    new { id = createdMovie.Id });
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
    /// Updates an existing movie.
    /// </summary>
    /// <example>
    ///     Example of MovieDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the movie to update.</param>
    /// <param name="movieDto">The updated movie data transfer object.</param>
    /// <response code="200">If the movie was updated successfully</response>
    /// <response code="400">If the ID does not match the movieDto ID or some other error message with what happened</response>
    /// <response code="401">Returns when movie not authorized to use this endpoint</response>
    // PUT: /Movie/update/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPut("update/{id}")]
    public async Task<ActionResult<MovieDto>> UpdateMovie(int id, [FromBody] MovieDto movieDto)
    {
        logger.LogInformation($"Triggered Endpoint PUT: movie/update/{id}");

        try
        {
            logger.LogInformation("Triggering Movie Service: UpdateMovie");

            if (id != movieDto.Id)
            {
                logger.LogError($"The id: {id} and DTO id: {movieDto.Id} don't match");

                return BadRequest("Path and Body id don't match");
            }

            var movie = await movieService.UpdateAsync(id, movieDto);

            return Ok(movie);
        }
        catch (Exception e)
        {

            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Deletes a movie by their ID.
    /// </summary>
    /// <param name="id">The ID of the movie to delete.</param>
    /// <response code="200">Returns true if the movie was deleted successfully</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when movie not authorized to use this endpoint</response>
    // DELETE: /Movie/delete/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> DeleteMovie(int id)
    {
        logger.LogInformation($"Triggered Endpoint DELETE: movie/delete/{id}");

        try
        {
            logger.LogInformation("Triggering Movie Service: DeleteByIdAsync");

            var isDeleted = await movieService.DeleteByIdAsync(id);

            if (!isDeleted)
            {
                logger.LogError($"Failed to delete movie with id: {id}");

                return BadRequest($"Failed to delete movie with id: {id}");
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
