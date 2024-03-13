using FilmForge.Common.Enum;
using Microsoft.AspNetCore.Authorization;

namespace FilmForge.Controllers;

[ApiController]
[Route("[controller]")]
public class RatingController : ControllerBase
{
    private readonly IRatingService ratingService;
    private readonly ILogger<RatingController> logger;

    public RatingController(
        IRatingService ratingService,
        ILogger<RatingController> logger)
    {
        this.ratingService = ratingService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all ratings.
    /// </summary>
    /// <example>
    ///     Example of RatingDtos in json array form
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
    /// <response code="200">Returns the list of ratings</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if that there are no ratings in the Database</response>
    // GET: /Rating/all
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<RatingDto>>> GetAllRatings()
    {
        logger.LogInformation("Triggered Endpoint GET: rating/all");

        try
        {
            logger.LogInformation("Triggering Rating Service: GetAllAsync");

            var ratings = await ratingService.GetAllAsync();

            if (ratings == null)
            {
                logger.LogWarning("No Ratings not found.");

                return NotFound("No Ratings not found.");
            }

            return Ok(ratings);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a rating by their ID.
    /// </summary>
    /// <example>
    ///     Example of RatingDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the rating to retrieve.</param>
    /// <response code="200">Returns the rating corresponding to the specified ID</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no rating is found with the specified ID</response>
    // GET: /Rating/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<RatingDto>> GetRatingById(int id)
    {
        logger.LogInformation($"Triggered Endpoint GET: rating/{id}");

        try
        {
            logger.LogInformation("Triggering Rating Service: GetByIdAsync");

            var rating = await ratingService.GetByIdAsync(id);

            if (rating == null)
            {
                logger.LogInformation($"Rating with id: {id} not found");

                return NotFound(id);
            }

            return Ok(rating);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Creates a new rating.
    /// </summary>
    /// <example>
    ///     Example of RatingDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="ratingDto">The rating data transfer object.</param>
    /// <response code="201">Returns the newly created rating</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when rating not authorized to use this endpoint</response>
    // POST: /Rating/add
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPost("add")]
    public async Task<ActionResult<RatingDto>> CreateRating([FromBody] RatingDto ratingDto)
    {
        logger.LogInformation("Triggered Endpoint POST: rating/add");

        try
        {
            if (ModelState.IsValid)
            {
                logger.LogInformation("Triggering Rating Service: CreateAsync");

                var createdRating = await ratingService.CreateAsync(ratingDto);

                return CreatedAtAction(
                    nameof(GetRatingById),
                    new { id = createdRating.Id });
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
    /// Updates an existing rating.
    /// </summary>
    /// <example>
    ///     Example of RatingDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the rating to update.</param>
    /// <param name="ratingDto">The updated rating data transfer object.</param>
    /// <response code="200">If the rating was updated successfully</response>
    /// <response code="400">If the ID does not match the ratingDto ID or some other error message with what happened</response>
    /// <response code="401">Returns when rating not authorized to use this endpoint</response>
    // PUT: /Rating/update/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPut("update/{id}")]
    public async Task<ActionResult<RatingDto>> UpdateRating(int id, [FromBody] RatingDto ratingDto)
    {
        logger.LogInformation($"Triggered Endpoint PUT: rating/update/{id}");

        try
        {
            logger.LogInformation("Triggering Rating Service: UpdateRating");

            if (id != ratingDto.Id)
            {
                logger.LogError($"The id: {id} and DTO id: {ratingDto.Id} don't match");

                return BadRequest("Path and Body id don't match");
            }

            var rating = await ratingService.UpdateAsync(id, ratingDto);

            return Ok(rating);
        }
        catch (Exception e)
        {

            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Deletes a rating by their ID.
    /// </summary>
    /// <param name="id">The ID of the rating to delete.</param>
    /// <response code="200">Returns true if the rating was deleted successfully</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when rating not authorized to use this endpoint</response>
    // DELETE: /Rating/delete/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> DeleteRating(int id)
    {
        logger.LogInformation($"Triggered Endpoint DELETE: rating/delete/{id}");

        try
        {
            logger.LogInformation("Triggering Rating Service: DeleteByIdAsync");

            var isDeleted = await ratingService.DeleteByIdAsync(id);

            if (!isDeleted)
            {
                logger.LogError($"Failed to delete rating with id: {id}");

                return BadRequest($"Failed to delete rating with id: {id}");
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
