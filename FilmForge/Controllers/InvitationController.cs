using FilmForge.Common.Enum;
using FilmForge.Service.InvitationService;
using Microsoft.AspNetCore.Authorization;

namespace FilmForge.Controllers;

[ApiController]
[Route("[controller]")]
public class InvitationController : Controller
{
    private readonly IInvitationService invitationService;
    private readonly ILogger<InvitationController> logger;

    public InvitationController(
        IInvitationService invitationService,
        ILogger<InvitationController> logger)
    {
        this.invitationService = invitationService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves a invitations by their Actor Id.
    /// </summary>
    /// <param name="actorId">The Actor Id of the invitations to retrieve.</param>
    /// <response code="200">Returns the invitations corresponding to the specified Actor Id</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no invitations is found with the specified Actor Id</response>
    // GET: /Invitation/actor/{id}
    [Authorize]
    [HttpGet("actor/{actorId}")]
    public async Task<ActionResult<InvitationDto>> GetByActorId(int actorId)
    {
        logger.LogInformation($"Triggered Endpoint GET: invitation/actor/{actorId}");

        try
        {
            logger.LogInformation("Triggering Invitation Service: GetByActorIdAsync");

            var invitation = await invitationService.GetByActorIdAsync(actorId);

            if (invitation == null)
            {
                logger.LogInformation($"Invitation with Actor id: {actorId} not found");

                return NotFound(actorId);
            }

            return Ok(invitation);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a invitations by their Movie Id.
    /// </summary>
    /// <param name="movieId">The Movie Id of the invitations to retrieve.</param>
    /// <response code="200">Returns the invitations corresponding to the specified Movie Id</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no invitations is found with the specified Movie Id</response>
    // GET: /Invitation/movie/{id}
    [Authorize]
    [HttpGet("movie/{movieId}")]
    public async Task<ActionResult<InvitationDto>> GetByMovieId(int movieId)
    {
        logger.LogInformation($"Triggered Endpoint GET: invitation/movie/{movieId}");

        try
        {
            logger.LogInformation("Triggering Invitation Service: GetByMovieIdAsync");

            var invitation = await invitationService.GetByMovieIdAsync(movieId);

            if (invitation == null)
            {
                logger.LogInformation($"Invitation with Movie id: {movieId} not found");

                return NotFound(movieId);
            }

            return Ok(invitation);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a invitations by their Actor Id and Movie Id..
    /// </summary>
    /// <param name="actorId">The Actor Id of the invitations to retrieve.</param>
    /// <param name="movieId">The Movie Id of the invitations to retrieve.</param>
    /// <response code="200">Returns the invitations corresponding to the specified Actor Id and Movie Id.</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no invitations is found with the specified Actor Id and Movie Id.</response>
    // GET: /Invitationactor-movie/{actorId}/{movieId}
    [Authorize]
    [HttpGet("actor-movie/{actorId}/{movieId}")]
    public async Task<ActionResult<InvitationDto>> GetByActorIdAndMovieId(int actorId, int movieId)
    {
        logger.LogInformation($"Triggered Endpoint GET: invitation/actor-movie/{actorId}/{movieId}");

        try
        {
            logger.LogInformation("Triggering Invitation Service: GetByActorIdAndMovieIdAsync");

            var invitation = await invitationService.GetByActorIdAndMovieIdAsync(actorId, movieId);

            if (invitation == null)
            {
                logger.LogInformation($"Invitation with Actor id: {actorId} and Movie id: {movieId} not found");

                return NotFound(actorId);
            }

            return Ok(invitation);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Creates a new invitation.
    /// </summary>
    /// <example>
    ///     Example of InvitationDto in json form
    ///     {
    ///  		"id": int,
    ///  		"hasAccepted ": boolean,
    ///  		"InvitationType": int,
    ///  		"actorId": int,
    ///  		"actor":
    ///  		{
    ///  			...
    ///  		}
    ///  		"movieId": int,
    ///  		"movie": 
    /// 		{
    /// 			...
    /// 		}
    ///  	}
    /// </example>
    /// <param name="invitationDto">The invitation data transfer object.</param>
    /// <response code="201">Returns the newly created invitation</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when invitation not authorized to use this endpoint</response>
    // POST: /Invitation/add
    [Authorize(Roles = 
        nameof(UserRole.SuperAdministrator) + "," + 
        nameof(UserRole.Director))]
    [HttpPost("add")]
    public async Task<ActionResult<InvitationDto>> CreateInvitation([FromBody] InvitationDto invitationDto)
    {
        logger.LogInformation("Triggered Endpoint POST: invitation/add");

        try
        {
            if (ModelState.IsValid)
            {
                logger.LogInformation("Triggering Invitation Service: CreateAsync");

                var createdInvitation = await invitationService.CreateAsync(invitationDto);

                return CreatedAtAction(
                    nameof(GetByMovieId),
                    new { movieId = createdInvitation.MovieId });
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
    /// Updates an existing invitation.
    /// </summary>
    /// <example>
    ///     Example of InvitationDto in json form
    ///     {
    ///  		"id": int,
    ///  		"hasAccepted ": boolean,
    ///  		"InvitationType": int,
    ///  		"actorId": int,
    ///  		"actor":
    ///  		{
    ///  			...
    ///  		}
    ///  		"movieId": int,
    ///  		"movie": 
    /// 		{
    /// 			...
    /// 		}
    ///  	}
    /// </example>
    /// <param name="id">The ID of the invitation to update.</param>
    /// <param name="invitationDto">The updated invitation data transfer object.</param>
    /// <response code="200">If the invitation was updated successfully</response>
    /// <response code="400">If the ID does not match the invitationDto ID or some other error message with what happened</response>
    /// <response code="401">Returns when invitation not authorized to use this endpoint</response>
    // PUT: /Invitation/update/{id}
    [Authorize()]
    [HttpPut("update/{id}")]
    public async Task<ActionResult<InvitationDto>> UpdateInvitation(int id, [FromBody] InvitationDto invitationDto)
    {
        logger.LogInformation($"Triggered Endpoint PUT: invitation/update/{id}");

        try
        {
            logger.LogInformation("Triggering Invitation Service: UpdateInvitation");

            if (id != invitationDto.Id)
            {
                logger.LogError($"The id: {id} and DTO id: {invitationDto.Id} don't match");

                return BadRequest("Path and Body id don't match");
            }

            var invitation = await invitationService.UpdateAsync(id, invitationDto);

            return Ok(invitation);
        }
        catch (Exception e)
        {

            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Deletes a invitation by their ID.
    /// </summary>
    /// <param name="id">The ID of the invitation to delete.</param>
    /// <response code="200">Returns true if the invitation was deleted successfully</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when invitation not authorized to use this endpoint</response>
    // DELETE: /Invitation/delete/{id}
    [Authorize()]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> DeleteInvitation(int id)
    {
        logger.LogInformation($"Triggered Endpoint DELETE: invitation/delete/{id}");

        try
        {
            logger.LogInformation("Triggering Invitation Service: DeleteByIdAsync");

            var isDeleted = await invitationService.DeleteByIdAsync(id);

            if (!isDeleted)
            {
                logger.LogError($"Failed to delete invitation with id: {id}");

                return BadRequest($"Failed to delete invitation with id: {id}");
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
