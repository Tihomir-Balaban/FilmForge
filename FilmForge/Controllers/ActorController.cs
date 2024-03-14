using FilmForge.Common.Enum;
using Microsoft.AspNetCore.Authorization;

namespace FilmForge.Controllers;

[ApiController]
[Route("[controller]")]
public class ActorController : ControllerBase
{
    private readonly IActorService actorService;
    private readonly ILogger<ActorController> logger;

    public ActorController(
        IActorService actorService,
        ILogger<ActorController> logger)
	{
        this.actorService = actorService;
        this.logger = logger;
    }

    /// <summary>
    /// Retrieves all actors.
    /// </summary>
    /// <example>
    ///     Example of ActorDtos in json array form
    ///     [
    ///      	{
    ///      		"id": int,
    ///      		"name": string,
    ///      		"bio ": string,
    ///      		"userId": int,
    ///      		"movies": 
    ///      		[
    ///      			{
    ///      				...
    ///      			}
    ///      		],
    ///      		[
    ///      			{
    ///      				...
    ///      			}
    ///      		]
    ///      	},
    ///      	{
    ///      		"id": int,
    ///      		"name": string,
    ///      		"bio ": string,
    ///      		"userId": int,
    ///      		"movies": 
    ///      		[
    ///      			{
    ///      				...
    ///      			}
    ///      		],
    ///      		[
    ///      			{
    ///      				...
    ///      			}
    ///      		]
    ///      	}
    ///      ]
    /// </example>
    /// <response code="200">Returns the list of actors</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">Returns message if that there are no actors in the Database</response>
    // GET: /Actor/all
    [Authorize]
    [HttpGet("all")]
    public async Task<ActionResult<IEnumerable<ActorDto>>> GetAllActors()
    {
        logger.LogInformation("Triggered Endpoint GET: actor/all");

        try
        {
            logger.LogInformation("Triggering Actor Service: GetAllAsync");

            var actors = await actorService.GetAllAsync();
            
            if (actors == null)
            {
                logger.LogWarning("No Actors not found.");

                return NotFound("No Actors not found.");
            }

            return Ok(actors);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Retrieves a actor by their ID.
    /// </summary>
    /// <example>
    ///     Example of ActorDto in json form
    ///     {
    ///   		"id": int,
    ///   		"name": string,
    ///   		"bio ": string,
    ///   		"userId": int,
    ///   		"movies": 
    ///   		[
    ///   			{
    ///   				...
    ///   			}
    ///   		],
    ///   		[
    ///   			{
    ///   				...
    ///   			}
    ///   		]
    ///   	}
    /// </example>
    /// <param name="id">The ID of the actor to retrieve.</param>
    /// <response code="200">Returns the actor corresponding to the specified ID</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="404">If no actor is found with the specified ID</response>
    // GET: /Actor/{id}
    [Authorize]
    [HttpGet("{id}")]
    public async Task<ActionResult<ActorDto>> GetActorById(int id)
    {
        logger.LogInformation($"Triggered Endpoint GET: actor/{id}");

        try
        {
            logger.LogInformation("Triggering Actor Service: GetByIdAsync");

            var actor = await actorService.GetByIdAsync(id);

            if (actor == null)
            {
                logger.LogInformation($"Actor with id: {id} not found");

                return NotFound(id);
            }

            return Ok(actor);
        }
        catch (Exception e)
        {
            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Creates a new actor.
    /// </summary>
    /// <example>
    ///     Example of ActorDto in json form
    ///     {
    ///  		"id": int,
    ///  		"name": string,
    ///  		"bio ": string,
    ///  		"userId": int,
    ///  		"movies": 
    ///  		[
    ///  			{
    ///  				...
    ///  			}
    ///  		],
    ///  		[
    ///  			{
    ///  				...
    ///  			}
    ///  		]
    ///  	}
    /// </example>
    /// <param name="actorDto">The actor data transfer object.</param>
    /// <response code="201">Returns the newly created actor</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when actor not authorized to use this endpoint</response>
    // POST: /Actor/add
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpPost("add")]
    public async Task<ActionResult<ActorDto>> CreateActor([FromBody] ActorDto actorDto)
    {
        logger.LogInformation("Triggered Endpoint POST: actor/add");

        try
        {
            if (ModelState.IsValid)
            {
                logger.LogInformation("Triggering Actor Service: CreateAsync");

                var createdActor = await actorService.CreateAsync(actorDto);

                return CreatedAtAction(
                    nameof(GetActorById),
                    new { id = createdActor.Id });
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
    /// Updates an existing actor.
    /// </summary>
    /// <example>
    ///     Example of ActorDto in json form
    ///     {
    ///  		"id": int,
    ///  		"name": string,
    ///  		"bio ": string,
    ///  		"userId": int,
    ///  		"movies": 
    ///  		[
    ///  			{
    ///  				...
    ///  			}
    ///  		],
    ///  		[
    ///  			{
    ///  				...
    ///  			}
    ///  		]
    ///  	}
    /// </example>
    /// <param name="id">The ID of the actor to update.</param>
    /// <param name="actorDto">The updated actor data transfer object.</param>
    /// <response code="200">If the actor was updated successfully</response>
    /// <response code="400">If the ID does not match the actorDto ID or some other error message with what happened</response>
    /// <response code="401">Returns when actor not authorized to use this endpoint</response>
    // PUT: /Actor/update/{id}
    [Authorize(Roles = 
        nameof(UserRole.SuperAdministrator) + "," + 
        nameof(UserRole.Actor))]
    [HttpPut("update/{id}")]
    public async Task<ActionResult<ActorDto>> UpdateActor(int id, [FromBody] ActorDto actorDto)
    {
        logger.LogInformation($"Triggered Endpoint PUT: actor/update/{id}");

        try
        {
            logger.LogInformation("Triggering Actor Service: UpdateActor");

            if (id != actorDto.Id)
            {
                logger.LogError($"The id: {id} and DTO id: {actorDto.Id} don't match");

                return BadRequest("Path and Body id don't match");
            }

            var actor = await actorService.UpdateAsync(id, actorDto);

            return Ok(actor);
        }
        catch (Exception e)
        {

            logger.LogError($"Error caught: {nameof(e.InnerException)}");

            BadRequest(e.Message);

            throw new ApplicationException(e.Message);
        }
    }

    /// <summary>
    /// Deletes a actor by their ID.
    /// </summary>
    /// <param name="id">The ID of the actor to delete.</param>
    /// <response code="200">Returns true if the actor was deleted successfully</response>
    /// <response code="400">Returns error message with what happened</response>
    /// <response code="401">Returns when actor not authorized to use this endpoint</response>
    // DELETE: /Actor/delete/{id}
    [Authorize(Roles = nameof(UserRole.SuperAdministrator))]
    [HttpDelete("delete/{id}")]
    public async Task<ActionResult<bool>> DeleteActor(int id)
    {
        logger.LogInformation($"Triggered Endpoint DELETE: actor/delete/{id}");

        try
        {
            logger.LogInformation("Triggering Actor Service: DeleteByIdAsync");

            var isDeleted = await actorService.DeleteByIdAsync(id);

            if (!isDeleted)
            {
                logger.LogError($"Failed to delete actor with id: {id}");

                return BadRequest($"Failed to delete actor with id: {id}");
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
