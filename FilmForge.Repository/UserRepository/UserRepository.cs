using Service.Security;

namespace FilmForge.Repository.UserRepository;

public class UserRepository : IUserRepository
{
    private readonly FilmForgeDbContext dbContext;
    private readonly ISecurityService securityService;
    private readonly ILogger<UserRepository> logger;
    private IMapper mapper;

    public UserRepository(
        FilmForgeDbContext dbContext,
        ISecurityService securityService,
        ILogger<UserRepository> logger,
        IMapper mapper)
    {
        this.dbContext = dbContext;
        this.securityService = securityService;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<UserDto> CreateAsync(User user, UserDto userDto)
    {
        logger.LogInformation("Adding a new User.");

        try
        {
            (user.Password, user.Salt) = securityService.HashPassword(userDto.Password);

            await dbContext.AddAsync(user);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"User created successfully with Name: {user.Name} and Email: {user.Email}.");

            return mapper.Map<UserDto>(user);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to add User. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
    {
        logger.LogInformation($"Atempting to deleting User by id: {id}.");

        try
        {
            var user = await dbContext
                .Users
                .FindAsync(id);

            if (user == null)
            {
                logger.LogWarning($"Solar power plant with ID: {id} not found.");

                return false;
            }

            dbContext.Users.Remove(user);
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"User with id: {id} successfully deleted.");

            return true;

        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to delete Userwith id: {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<UserDto[]> GetAllAsync()
    {
        logger.LogInformation("Getting all Users.");

        try
        {
            var users = await dbContext
                .Users
                .ToArrayAsync();

            if (users == null)
            {
                logger.LogWarning("No Users not found.");

                return null;
            }

            var userDtos = mapper.Map<UserDto[]>(users);

            logger.LogInformation($"Retrieved all {userDtos.Count()} Users.");

            return userDtos;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to add User. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<UserDto> GetByIdAsync(int id)
    {
        logger.LogInformation($"Getting User with id: {id}.");

        try
        {
            var user = await dbContext
                .Users
                .FindAsync(id);

            if (user == null)
            {
                logger.LogWarning($"User with id: {id} not found.");

                return null;
            }

            logger.LogInformation($"User found successfully with Id: {user.Id} and Username: {user.Name}.");

            return mapper.Map<UserDto>(user);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find User with id {id}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<UserDto> GetUserByEmailAndPassord(string email, string password)
    {
        logger.LogInformation($"Getting User with email: {email}.");

        try
        {
            var user = await dbContext
                .Users
                .FirstOrDefaultAsync(u => u.Email == email);

            if (user == null)
            {
                logger.LogWarning($"Attempted to retrieve non-existent user with Email: {email}");
                return null;
            }

            if (!securityService.VerifyPassword(password, user.Password, user.Salt))
            {
                logger.LogWarning($"Invalid password attempt for user with Email: {email}");
                return null;
            }

            var userDto = mapper.Map<UserDto>(user);

            logger.LogInformation($"User retrieved successfully with Id: {userDto.Id}, Username: {userDto.Name} and Email: {userDto.Email}.");

            return userDto;
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to find User with Email {email}. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<UserDto> UpdateAsync(int id, User user)
    {
        logger.LogInformation($"Updating User: {user.Name}.");

        try
        {
            var userEntity = await dbContext
                .Users
                .FindAsync(user.Id);

            if (user == null)
            {
                logger.LogWarning($"User with id: {id} not found.");

                return null;
            }

            user.CreatedOn = userEntity.CreatedOn;
            user.ModifiedOn = DateTime.Now;
            user.Password = userEntity.Password;
            user.Salt = userEntity.Salt;

            userEntity = mapper.Map<User>(user);

            dbContext.Entry(userEntity).State = EntityState.Modified;
            await dbContext.SaveChangesAsync();

            logger.LogInformation($"User updated successfully with Username: {user.Name}.");

            return mapper.Map<UserDto>(userEntity);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to update User. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}
