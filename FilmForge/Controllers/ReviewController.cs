using FilmForge.Common.Enum;
using FilmForge.Models.Utility;
using FilmForge.Service.ReviewService;
using Microsoft.AspNetCore.Authorization;
using Service.Security;

namespace FilmForge.Controllers;

[ApiController]
[Route("[controller]")]
public class ReviewController : ControllerBase
{
    private readonly IReviewService reviewService;
    private readonly ILogger<ReviewController> logger;

    public ReviewController(
        IReviewService reviewService,
        ILogger<ReviewController> logger)
    {
        this.reviewService = reviewService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all reviews.
    /// </summary>
    /// <example>
    ///     Example of ReviewDtos in json array form
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
    /// <response code="200">Returns the list of reviews</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if that there are no reviews in the Database</response>
    // GET: /Review/all
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ReviewDto>>> GetAllReviews()
    {
        logger.LogInformation("Triggered Endpoint GET: review/all");

        try
        {
            logger.LogInformation("Triggering Review Service: GetAllAsync");

            var reviews = await reviewService.GetAllAsync();

            if (reviews == null)
            {
                logger.LogWarning("No Reviews not found.");

                return NotFound("No Reviews not found.");
            }

            return Ok(reviews);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a review by their ID.
    /// </summary>
    /// <example>
    ///     Example of ReviewDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the review to retrieve.</param>
    /// <response code="200">Returns the review corresponding to the specified ID</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no review is found with the specified ID</response>
    // GET: /Review/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ReviewDto>> GetReviewById(int id)
    {
        logger.LogInformation($"Triggered Endpoint GET: review/{id}");

        try
        {
            logger.LogInformation("Triggering Review Service: GetByIdAsync");

            var review = await reviewService.GetByIdAsync(id);

            if (review == null)
            {
                logger.LogInformation($"Review with id: {id} not found");

                return NotFound(id);
            }

            return Ok(review);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Creates a new review.
    /// </summary>
    /// <example>
    ///     Example of ReviewDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="reviewDto">The review data transfer object.</param>
    /// <response code="201">Returns the newly created review</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when review not authorized to use this endpoint</response>
    // POST: /Review/add
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPost("add")]
    public async Task<ActionResult<ReviewDto>> CreateReview([FromBody] ReviewDto reviewDto)
    {
        logger.LogInformation("Triggered Endpoint POST: review/add");

        try
        {
            if (ModelState.IsValid)
            {
                logger.LogInformation("Triggering Review Service: CreateAsync");

                var createdReview = await reviewService.CreateAsync(reviewDto);

                return CreatedAtAction(
                    nameof(GetReviewById),
                    new { id = createdReview.Id });
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
    /// Updates an existing review.
    /// </summary>
    /// <example>
    ///     Example of ReviewDto in json form
    ///     {
    ///         "id": int,
    ///         "name": string,
    ///         "email": string,
    ///         "password": string,
    ///         "role": int
    ///     }
    /// </example>
    /// <param name="id">The ID of the review to update.</param>
    /// <param name="reviewDto">The updated review data transfer object.</param>
    /// <response code="200">If the review was updated successfully</response>
    /// <response code="400">If the ID does not match the reviewDto ID or some other error message with what happened</response>
    /// <response code="401">Returns when review not authorized to use this endpoint</response>
    // PUT: /Review/update/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPut("update/{id}")]
    public async Task<ActionResult<ReviewDto>> UpdateReview(int id, [FromBody] ReviewDto reviewDto)
    {
        logger.LogInformation($"Triggered Endpoint PUT: review/update/{id}");

        try
        {
            logger.LogInformation("Triggering Review Service: UpdateReview");

            if (id != reviewDto.Id)
            {
                logger.LogError($"The id: {id} and DTO id: {reviewDto.Id} don't match");

                return BadRequest("Path and Body id don't match");
            }

            var review = await reviewService.UpdateAsync(id, reviewDto);

            return Ok(review);
        }
        catch (Exception e)
        {

            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Deletes a review by their ID.
    /// </summary>
    /// <param name="id">The ID of the review to delete.</param>
    /// <response code="200">Returns true if the review was deleted successfully</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when review not authorized to use this endpoint</response>
    // DELETE: /Review/delete/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> DeleteReview(int id)
    {
        logger.LogInformation($"Triggered Endpoint DELETE: review/delete/{id}");

        try
        {
            logger.LogInformation("Triggering Review Service: DeleteByIdAsync");

            var isDeleted = await reviewService.DeleteByIdAsync(id);

            if (!isDeleted)
            {
                logger.LogError($"Failed to delete review with id: {id}");

                return BadRequest($"Failed to delete review with id: {id}");
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
