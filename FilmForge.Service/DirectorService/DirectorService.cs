using FilmForge.Repository.DirectorRepository;

namespace FilmForge.Service.DirectorService;

public class DirectorService : IDirectorService
{
    private readonly IDirectorRepository directorRepository;
    private readonly ILogger<DirectorService> logger;
    private readonly IMapper mapper;

    public DirectorService(
        IDirectorRepository directorRepository,
        ILogger<DirectorService> logger,
        IMapper mapper)
    {
        this.directorRepository = directorRepository;
        this.logger = logger;
        this.mapper = mapper;
    }

    public async Task<DirectorDto> CreateAsync(DirectorDto directorDto)
    {
        try
        {
            (directorDto.CreatedOn, directorDto.ModifiedOn) = (DateTime.Now, DateTime.Now);

            logger.LogInformation($"Mapping DirectorDto to Director (Entity) in DirectorService CreateAsync");

            var director = mapper.Map<Director>(directorDto);

            return await directorRepository.CreateAsync(director, directorDto);
        }
        catch (Exception e)
        {

            logger.LogError(e, $"Failed to map Director. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }

    public async Task<bool> DeleteByIdAsync(int id)
        => await directorRepository.DeleteByIdAsync(id);

    public async Task<DirectorDto[]> GetAllAsync()
        => await directorRepository.GetAllAsync();

    public async Task<DirectorDto> GetByIdAsync(int id)
        => await directorRepository.GetByIdAsync(id);

    public async Task<DirectorDto> UpdateAsync(int id, DirectorDto directorDto)
    {
        try
        {
            logger.LogInformation($"Mapping DirectorDto to Director (Entity) in DirectorService UpdateAsync");
            var director = mapper.Map<Director>(directorDto);

            return await directorRepository.UpdateAsync(id, director);
        }
        catch (Exception e)
        {
            logger.LogError(e, $"Failed to map Director. Error: {e.Message}.");

            throw new ApplicationException(e.Message);
        }
    }
}