using FilmForge.Models.Utility;
using FilmForge.Repository.UserRepository;

namespace FilmForge.Service.UserService;

public class UserService : IUserService
{
    private readonly IUserRepository userRepository;
    private readonly ILogger<UserService> logger;
    private readonly IMapper mapper;

    public UserService(IUserRepository userRepository,
        ILogger<UserService> logger,
        IMapper mapper)
    {
        this.userRepository = userRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<UserDto> CreateAsync(UserDto userDto)
    {
        try
        {
            (userDto.CreatedOn, userDto.ModifiedOn) = (DateTime.Now, DateTime.Now);

            logger.LogInformation($"Mapping UserDto to User (Entity) in UserService CreateAsync");
            
            var user = mapper.Map<User>(userDto);

            return await userRepository.CreateAsync(user, userDto);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map User. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<UserDto> LoginUserAsync(LoginRequest login)
        => await userRepository.GetUserByEmailAndPassord(login.Email, login.Password);

    public async Task<bool> DeleteByIdAsync(int id)
        => await userRepository.DeleteByIdAsync(id);

    public async Task<UserDto[]> GetAllAsync()
        => await userRepository.GetAllAsync();

    public async Task<UserDto> GetByIdAsync(int id)
        => await userRepository.GetByIdAsync(id);

    public async Task<UserDto> GetUserByEmailAndPassord(string email, string password)
        => await userRepository.GetUserByEmailAndPassord(email, password);

    public async Task<UserDto> UpdateAsync(int id, UserDto userDto)
    {
        try
        {
            logger.LogInformation($"Mapping UserDto to User (Entity) in UserService UpdateAsync");
            var user = mapper.Map<User>(userDto);

            return await userRepository.UpdateAsync(id, user);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map User. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}
