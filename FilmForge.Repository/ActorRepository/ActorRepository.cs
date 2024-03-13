namespace FilmForge.Repository.ActorRepository;

public class ActorRepository : IActorRepository
{
    public Task<ActorDto> CreateAsync(Actor entity, ActorDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ActorDto[]> GetAllAsync()
    {
        throw new NotImplementedException();
    }

    public Task<ActorDto> GetByIdAsync(int id)
    {
        throw new NotImplementedException();
    }

    public Task<ActorDto> UpdateAsync(int id, Actor entity)
    {
        throw new NotImplementedException();
    }
}