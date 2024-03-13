namespace FilmForge.Repository.MovieRepository
{
    public class MovieRepository : IMovieRepository
    {
        public Task<MovieDto> CreateAsync(Movie entity, MovieDto dto)
        {
            throw new NotImplementedException();
        }

        public Task<bool> DeleteByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDto[]> GetAllAsync()
        {
            throw new NotImplementedException();
        }

        public Task<MovieDto> GetByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<MovieDto> UpdateAsync(int id, Movie entity)
        {
            throw new NotImplementedException();
        }
    }
}
