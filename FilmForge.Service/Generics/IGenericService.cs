namespace FilmForge.Repository.Generics;

public interface IGenericService<T> where T : class
{
    Task<T[]> GetAllAsync();
    Task<T> GetByIdAsync(int id);
    Task<T> CreateAsync(T dto);
    Task<T> UpdateAsync(int id, T dto);
    Task<bool> DeleteByIdAsync(int id);
}
