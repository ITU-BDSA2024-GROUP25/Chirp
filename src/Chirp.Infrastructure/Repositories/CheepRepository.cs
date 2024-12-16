using System.Globalization;
using Microsoft.EntityFrameworkCore;
using Chirp.Core;

namespace Chirp.Infrastructure;

/// <summary>
/// Provides CRUD operations for the Cheep entity and manages database relationships.
/// </summary>
public class CheepRepository : ICheepRepository
{
    private readonly ChirpDbContext _context;

    /// <summary>
    /// Initializes a new instance of the CheepRepository class with a specified database context.
    /// </summary>
    /// <param name="context">The database context used for accessing the database.</param>
    public CheepRepository(ChirpDbContext context)
    {
        _context = context;
    }

    #region Queries

    /// <inheritdoc/>
    public async Task<List<CheepDto>> GetCheeps(string? authorName = null, int pageNumber = 1)
    {
        var query = _context.Cheeps
            .OrderByDescending(c => c.TimeStamp)
            .Include(c => c.Author)
            .Where(c => authorName == null || c.Author.Name == authorName)
            .Skip((pageNumber - 1) * 32).Take(32)
            .Select(c => new CheepDto(c.Text, c.TimeStamp.ToString(CultureInfo.CurrentCulture), c.Author.Name));

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public int GetTotalCheepsCount(string? authorName = null)
    {
        return string.IsNullOrEmpty(authorName)
            ? _context.Cheeps.Count()
            : _context.Cheeps.Count(c => c.Author.Name == authorName);
    }

    /// <inheritdoc/>
    public async Task<List<CheepDto>> GetCheepsFromFollowers(string authorName, IList<AuthorDto> followers, int pageNumber = 1)
    {
        var authorNames = followers.Select(a => a.userName).Append(authorName).ToList();

        var query = _context.Cheeps
            .Where(c => authorNames.Contains(c.Author.Name))
            .OrderByDescending(c => c.TimeStamp)
            .Include(c => c.Author)
            .Skip((pageNumber - 1) * 32).Take(32)
            .Select(c => new CheepDto(c.Text, c.TimeStamp.ToString(CultureInfo.CurrentCulture), c.Author.Name));

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<List<CheepDto>> GetAllCheeps(string authorName)
    {
        var query = _context.Cheeps
            .Where(c => c.Author.Name == authorName)
            .OrderByDescending(c => c.TimeStamp)
            .Include(c => c.Author)
            .Select(c => new CheepDto(c.Text, c.TimeStamp.ToString(CultureInfo.CurrentCulture), c.Author.Name));

        return await query.ToListAsync();
    }

    /// <inheritdoc/>
    public async Task<int> FindCheepId(CheepDto cheepDto)
    {
        var cheep = await _context.Cheeps
            .FirstOrDefaultAsync(c =>
                c.Author.Name == cheepDto.authorName &&
                c.Text == cheepDto.text &&
                c.TimeStamp == DateTime.Parse(cheepDto.postedTime, CultureInfo.CurrentCulture));

        if (cheep == null) throw new NullReferenceException("Cheep not found");

        return cheep.CheepId;
    }

    /// <inheritdoc/>
    public async Task<bool> IsCheepLikedByAuthor(string authorName, CheepDto cheep)
    {
        var author = await _context.Authors
            .Include(a => a.LikedCheeps)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new NullReferenceException("Author not found");

        var cheepId = await FindCheepId(cheep);
        return author.LikedCheeps.Any(c => c.CheepId == cheepId);
    }

    /// <inheritdoc/>
    public async Task<bool> IsCheepDislikedByAuthor(string authorName, CheepDto cheep)
    {
        var author = await _context.Authors
            .Include(a => a.DislikedCheeps)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new NullReferenceException("Author not found");

        var cheepId = await FindCheepId(cheep);
        return author.DislikedCheeps.Any(c => c.CheepId == cheepId);
    }

    /// <inheritdoc/>
    public async Task<int> GetCheepLikesCount(CheepDto cheepDto)
    {
        var cheepId = await FindCheepId(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null) throw new NullReferenceException("Cheep not found");

        return cheep.LikeAmount;
    }

    /// <inheritdoc/>
    public async Task<int> GetCheepDislikesCount(CheepDto cheepDto)
    {
        var cheepId = await FindCheepId(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null) throw new NullReferenceException("Cheep not found");

        return cheep.DislikeAmount;
    }

    /// <inheritdoc/>
    public async Task<List<CheepDto>> GetLikedCheeps(string authorName)
    {
        var author = await _context.Authors
            .Include(a => a.LikedCheeps)
            .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new NullReferenceException("Author not found");

        return author.LikedCheeps
            .OrderByDescending(c => c.TimeStamp)
            .Select(c => new CheepDto(c.Text, c.TimeStamp.ToString(CultureInfo.CurrentCulture), c.Author.Name))
            .ToList();
    }

    /// <inheritdoc/>
    public async Task<List<CheepDto>> GetDislikedCheeps(string authorName)
    {
        var author = await _context.Authors
            .Include(a => a.DislikedCheeps)
            .ThenInclude(c => c.Author)
            .FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new NullReferenceException("Author not found");

        return author.DislikedCheeps
            .OrderByDescending(c => c.TimeStamp)
            .Select(c => new CheepDto(c.Text, c.TimeStamp.ToString(CultureInfo.CurrentCulture), c.Author.Name))
            .ToList();
    }

    #endregion

    #region Commands

    /// <inheritdoc/>
    public async Task CreateCheep(CheepDto cheep, string authorName)
    {
        var author = await _context.Authors.FirstOrDefaultAsync(a => a.Name == authorName);

        if (author == null) throw new NullReferenceException("Author not found");

        var newCheep = new Cheep
        {
            Text = cheep.text,
            TimeStamp = DateTime.Parse(cheep.postedTime, CultureInfo.CurrentCulture),
            AuthorId = author.AuthorId,
            Author = author
        };

        _context.Cheeps.Add(newCheep);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task DeleteCheep(CheepDto cheepDto)
    {
        var cheepId = await FindCheepId(cheepDto);
        var cheep = await _context.Cheeps
            .Include(c => c.Author)
            .Include(c => c.LikedBy)
            .Include(c => c.DislikedBy)
            .FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null) throw new NullReferenceException("Cheep not found");

        _context.Cheeps.Remove(cheep);
        await _context.SaveChangesAsync();
    }

    /// <inheritdoc/>
    public async Task LikeCheep(string authorName, CheepDto cheepDto)
    {
        var author = await _context.Authors.Include(a => a.LikedCheeps).FirstOrDefaultAsync(a => a.Name == authorName);
        var cheepId = await FindCheepId(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null || author == null) throw new NullReferenceException("Cheep or Author not found");

        if (!author.LikedCheeps.Contains(cheep))
        {
            author.LikedCheeps.Add(cheep);
            cheep.LikeAmount++;
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task DislikeCheep(string authorName, CheepDto cheepDto)
    {
        var author = await _context.Authors.Include(a => a.DislikedCheeps).FirstOrDefaultAsync(a => a.Name == authorName);
        var cheepId = await FindCheepId(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null || author == null) throw new NullReferenceException("Cheep or Author not found");

        if (!author.DislikedCheeps.Contains(cheep))
        {
            author.DislikedCheeps.Add(cheep);
            cheep.DislikeAmount++;
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task RemoveLikeCheep(string authorName, CheepDto cheepDto)
    {
        var author = await _context.Authors.Include(a => a.LikedCheeps).FirstOrDefaultAsync(a => a.Name == authorName);
        var cheepId = await FindCheepId(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null || author == null) throw new NullReferenceException("Cheep or Author not found");

        if (author.LikedCheeps.Contains(cheep))
        {
            author.LikedCheeps.Remove(cheep);
            cheep.LikeAmount--;
            await _context.SaveChangesAsync();
        }
    }

    /// <inheritdoc/>
    public async Task RemoveDislikeCheep(string authorName, CheepDto cheepDto)
    {
        var author = await _context.Authors.Include(a => a.DislikedCheeps).FirstOrDefaultAsync(a => a.Name == authorName);
        var cheepId = await FindCheepId(cheepDto);
        var cheep = await _context.Cheeps.FirstOrDefaultAsync(c => c.CheepId == cheepId);

        if (cheep == null || author == null) throw new NullReferenceException("Cheep or Author not found");

        if (author.DislikedCheeps.Contains(cheep))
        {
            author.DislikedCheeps.Remove(cheep);
            cheep.DislikeAmount--;
            await _context.SaveChangesAsync();
        }
    }

    #endregion
}
