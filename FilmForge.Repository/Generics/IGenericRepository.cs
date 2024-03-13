namespace FilmForge.Repository.Generics;

public interface IGenericRepository<TDto, TEntity>
    where TDto : class 
    where TEntity : class
{
    Task<TDto[]> GetAllAsync();
    Task<TDto> GetByIdAsync(int id);
    Task<TDto> CreateAsync(TEntity entity, TDto dto);
    Task<TDto> UpdateAsync(int id, TEntity entity);
    Task<bool> DeleteByIdAsync(int id);
}
