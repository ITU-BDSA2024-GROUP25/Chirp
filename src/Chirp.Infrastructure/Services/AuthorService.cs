using Chirp.Core;

namespace Chirp.Infrastructure;

public interface IAuthorService
{
        public Task<Author?> FindAuthorByName(string name);
        public Task CreateAuthor(Author author);
}

public class AuthorService : IAuthorService
{
        private AuthorRepository _authorRepo;
        public AuthorService(ChirpDbContext context)
        {
                _authorRepo = new AuthorRepository(context);
        }
        
        public async Task<Author?> FindAuthorByName(string name) => await _authorRepo.FindAuthorByName(name);
        public Task CreateAuthor(Author author) => _authorRepo.CreateAuthor(author);
}