using System;
using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

public class CheepRepository : ICheepRepository
{
    private readonly ChirpDbContext _context;

    public CheepRepository(ChirpDbContext context)
    {
        _context = context;
    }

    //adapted from slides session 6 page 8
    public async Task<List<CheepDto>> GetCheeps(string? author = null, int pageNumber = 1)
    {
        var query = (from cheep in _context.Cheeps
                orderby cheep.TimeStamp descending
                select cheep)
            .Include(c => c.Author)
            .Where(c => author == null || c.Author.Name == author)
            .Skip((pageNumber - 1) * 32).Take(32)
            .Select(cheep => new CheepDto(cheep.Text, cheep.TimeStamp.ToString(), cheep.Author.Name));
        var result = await query.ToListAsync();
        return result;
    }

    public async Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers,
        int pageNumber = 1)
    {
        var authorNames = followers.Select(a => a.userName).ToList();
        authorNames.Add(authorName); // also include owners cheeps

        var query = (from cheep in _context.Cheeps
                where authorNames.Contains(cheep.Author.Name)
                orderby cheep.TimeStamp descending
                select cheep)
            .Include(c => c.Author)
            .Skip((pageNumber - 1) * 32).Take(32)
            .Select(cheep => new CheepDto(cheep.Text, cheep.TimeStamp.ToString(), cheep.Author.Name));

        var result = await query.ToListAsync();
        return result;
    }

    public int GetTotalCheepsCount(string? author = null)
    {
        var query = _context.Cheeps.AsQueryable();

        if (!string.IsNullOrEmpty(author))
        {
            query = query.Where(c => c.Author.Name == author);
        }

        return query.Count();
    }

    public async Task<List<CheepDto>> GetAllCheeps(string? author)
    {
        var query = (from cheep in _context.Cheeps
                orderby cheep.TimeStamp descending
                select cheep)
            .Include(c => c.Author)
            .Where(c => author == null || c.Author.Name == author)
            .Select(cheep => new CheepDto(cheep.Text, cheep.TimeStamp.ToString(), cheep.Author.Name));
        var result = await query.ToListAsync();
        return result;
    }


    public async Task CreateCheep(Cheep cheep)
    {
        if (cheep.TimeStamp == default)
            cheep.TimeStamp = DateTime.UtcNow;

        _context.Cheeps.Add(cheep);
        await _context.SaveChangesAsync();
    }

    public async Task CreateCheep(CheepDto cheep, string authorName)
    {
        var author = await _context.Authors
            .Where(a => a.Name == authorName)
            .FirstOrDefaultAsync();

        if (author == null)
        {
            throw new Exception("Author not found"); // Replace with appropriate error handling.
        }

        Cheep newCheep = new Cheep
        {
            Text = cheep.text,
            TimeStamp = DateTime.Parse(cheep.postedTime),
            AuthorId = author.AuthorId,
            Author = author
        };

        _context.Cheeps.Add(newCheep);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteCheep(CheepDto cheepDto)
    {
        // Find the cheep by matching DTO
        var cheep = await _context.Cheeps
            .Include(c => c.LikedBy)
            .Include(c => c.DislikedBy)
            .Include(c => c.Author)
            .FirstOrDefaultAsync(c =>
                c.Author.Name == cheepDto.authorName &&
                c.Text == cheepDto.text &&
                c.TimeStamp == DateTime.Parse(cheepDto.postedTime));

        if (cheep == null) throw new Exception("Cannot delete cheep, because it doesn't exist");

        // Remove the cheep from the author's Cheeps collection
        var author = cheep.Author;
        if (author == null) throw new Exception("Cannot delete cheep, author not found");
        author.Cheeps.Remove(cheep);

        // Remove relations from LikedBy
        if (cheep.LikedBy != null)
        {
            foreach (var likedByAuthor in cheep.LikedBy.ToList())
            {
                likedByAuthor.LikedCheeps?.Remove(cheep);
            }
            cheep.LikedBy.Clear();
        }

        // Remove relations from DislikedBy
        if (cheep.DislikedBy != null)
        {
            foreach (var dislikedByAuthor in cheep.DislikedBy.ToList())
            {
                dislikedByAuthor.DislikedCheeps?.Remove(cheep);
            }
            cheep.DislikedBy.Clear();
        }

        _context.Cheeps.Remove(cheep);
        await _context.SaveChangesAsync();
    }


    public async Task<int> FindCheepID(CheepDto cheepDto)
    {
        var cheep = await _context.Cheeps
            .Where(a => a.Author.Name == cheepDto.authorName && a.Text == cheepDto.text &&
                        a.TimeStamp == DateTime.Parse(cheepDto.postedTime)).FirstOrDefaultAsync();
        if (cheep == null) throw new Exception("Cannot fetch cheep ID, because it doesn't exist");

        return cheep.CheepId;
    }

    public async Task<bool> IsCheepLikedByAuthor(string authorName, CheepDto cheep)
    {
        var author = await _context.Authors
            .Include(a => a.LikedCheeps)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new Exception("Author not found");
        if (author.LikedCheeps == null) author.LikedCheeps = new List<Cheep>();

        
        var cheepId = await FindCheepID(cheep);
        return author.LikedCheeps.Any(c => c.CheepId == cheepId);
    }

    public async Task<bool> IsCheepDislikedByAuthor(string authorName, CheepDto cheep)
    {
        var author = await _context.Authors
            .Include(a => a.DislikedCheeps)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new Exception("Author not found");
        if (author.DislikedCheeps == null) author.DislikedCheeps = new List<Cheep>();

        
        var cheepId = await FindCheepID(cheep);
        return author.DislikedCheeps.Any(c => c.CheepId == cheepId);
    }

    public async Task LikeCheep(string authorName, CheepDto cheepDto)
    {
        var author = await _context.Authors
            .Include(a => a.LikedCheeps)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new Exception("Author not found");
        if (author.LikedCheeps == null) author.LikedCheeps = new List<Cheep>();
        
        var cheepId = await FindCheepID(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null) throw new Exception("Cheep not found");

        if (!author.LikedCheeps.Contains(cheep))
        {
            author.LikedCheeps.Add(cheep);
            cheep.LikeAmount += 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task DislikeCheep(string authorName, CheepDto cheepDto)
    {
        var author = await _context.Authors
            .Include(a => a.DislikedCheeps)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new Exception("Author not found");
        if (author.DislikedCheeps == null) author.DislikedCheeps = new List<Cheep>();
        
        var cheepId = await FindCheepID(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null) throw new Exception("Cheep not found");

        if (!author.DislikedCheeps.Contains(cheep))
        {
            author.DislikedCheeps.Add(cheep);
            cheep.DislikeAmount += 1;
            await _context.SaveChangesAsync();
        }
    }
    
    public async Task RemoveLikeCheep(string authorName, CheepDto cheepDto)
    {
        var author = await _context.Authors
            .Include(a => a.LikedCheeps)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new Exception("Author not found");
        if (author.LikedCheeps == null) author.LikedCheeps = new List<Cheep>();
        
        var cheepId = await FindCheepID(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null) throw new Exception("Cheep not found");

        if (author.LikedCheeps.Contains(cheep))
        {
            author.LikedCheeps.Remove(cheep);
            cheep.LikeAmount -= 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveDislikeCheep(string authorName, CheepDto cheepDto)
    {
        var author = await _context.Authors
            .Include(a => a.DislikedCheeps)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new Exception("Author not found");
        if (author.DislikedCheeps == null) author.DislikedCheeps = new List<Cheep>();
        
        var cheepId = await FindCheepID(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null) throw new Exception("Cheep not found");

        if (author.DislikedCheeps.Contains(cheep))
        {
            author.DislikedCheeps.Remove(cheep);
            cheep.DislikeAmount -= 1;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<int> GetCheepLikesCount(CheepDto cheepDto)
    {
        var cheepId = await FindCheepID(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);
        
        if (cheep == null) throw new Exception("Cheep not found");
        
        return cheep.LikeAmount;
    }

    public async Task<int> GetCheepDislikesCount(CheepDto cheepDto)
    {
        var cheepId = await FindCheepID(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);
                
        if (cheep == null) throw new Exception("Cheep not found");
        
        return cheep.DislikeAmount;
    }

    public async Task<List<CheepDto>> GetLikedCheeps(string? authorName)
    {
        var author = await _context.Authors
            .Include(a => a.LikedCheeps!.AsQueryable()) // Ensure compatibility
            .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new Exception("Author not found");

        if (author.LikedCheeps == null || !author.LikedCheeps.Any())
            return new List<CheepDto>();
        
        var likedCheeps = author.LikedCheeps
            .Where(c => c.Author != null) // Exclude null authors
            .OrderByDescending(c => c.TimeStamp)
            .Select(c => new CheepDto(c.Text, c.TimeStamp.ToString(), c.Author!.Name)) 
            .ToList();

        return likedCheeps;
    }



    public async Task<List<CheepDto>> GetDislikedCheeps(string? authorName)
    {
        var author = await _context.Authors
            .Include(a => a.DislikedCheeps!.AsQueryable()) // Ensure compatibility
            .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new Exception("Author not found");

        if (author.DislikedCheeps == null || !author.DislikedCheeps.Any())
            return new List<CheepDto>();
        
        var dislikedCheeps = author.DislikedCheeps
            .Where(c => c.Author != null) // Exclude null authors
            .OrderByDescending(c => c.TimeStamp)
            .Select(c => new CheepDto(c.Text, c.TimeStamp.ToString(), c.Author!.Name)) 
            .ToList();

        return dislikedCheeps;
    }
}