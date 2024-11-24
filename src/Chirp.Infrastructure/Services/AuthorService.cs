using Chirp.Core;

namespace Chirp.Infrastructure;

public interface IAuthorService
{
        public Task<Author?> FindAuthorByName(string? name);
        public Task CreateAuthor(AuthorDto authorDto);
        public Task FollowAuthor(string userName, string targetUserName);
        public Task<bool> IsFollowing(string userName, string? targetUserName);
        public Task UnfollowAuthor(string userName, string targetUserName);
        public Task<IList<AuthorDto>> GetFollowers(string authorName);
}

public class AuthorService : IAuthorService
{
        private AuthorRepository _authorRepo;
        public AuthorService(ChirpDbContext context)
        {
                _authorRepo = new AuthorRepository(context);
        }
        
        public async Task<Author?> FindAuthorByName(string? name) => await _authorRepo.FindAuthorByName(name);
        public Task CreateAuthor(AuthorDto authorDto) => _authorRepo.CreateAuthor(authorDto);
        public Task FollowAuthor(string userName, string targetUserName) => _authorRepo.FollowAuthor(userName, targetUserName);
        public Task<bool> IsFollowing(string userName, string? targetUserName) => _authorRepo.IsFollowing(userName, targetUserName);
        public Task UnfollowAuthor(string userName, string targetUserName) => _authorRepo.UnfollowAuthor(userName, targetUserName);
        public async Task<IList<AuthorDto>> GetFollowers(string authorName)=> await _authorRepo.GetFollowers(authorName);
}