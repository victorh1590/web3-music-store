using Microsoft.EntityFrameworkCore;
using Web3MusicStore.API.Models;

namespace Web3MusicStore.API.Data.Repositories;

public class AlbumRepository : IAlbumRepository
{
  private readonly StoreDbContext _context;
  private readonly ILogger _logger;
  private const int PageSize = 20;
  private int PageSkip(int pageNumber) => (pageNumber - 1) * PageSize;

  public AlbumRepository(StoreDbContext context, ILogger<RepositoryBase<Album>> logger)
  {
    _context = context;
    _logger = logger;
  }
  
  public async Task<IEnumerable<Album>?> GetPagesAsync(int pageNumber = 1, int? userId = null)
  {
    var query = _context.Albums.AsQueryable().AsNoTracking();
    if (userId != null) query = query.Where(item => userId == item.UserId);
    return await query.Skip(PageSkip(pageNumber)).Take(PageSize).ToListAsync();
  }

  public async Task<IEnumerable<Album>?>  GetRandomPagesAsync(int pageNumber = 1, Guid? guid = null)
  {
    guid ??= new Guid();

    return await _context.Albums
      .AsNoTracking()
      .OrderBy(item => guid)
      .Skip(PageSkip(pageNumber))
      .Take(PageSize)
      .ToListAsync();
  }

  public async Task<Album?> FindById(int albumId = 0)
  {
    return await _context.Albums.FindAsync(albumId);
  }

  public async Task InsertAsync(Album album)
  {
    await _context.Albums.AddAsync(album);
  }

  public void Update(Album album)
  {
    _context.Albums.Update(album);
  }

  public void Remove(Album album)
  {
    _context.Albums.Remove(album);
  }

  public async void RemoveByIdAsync(int albumId)
  {
    var album = await _context.Albums.FindAsync(albumId);
    if (album != null) _context.Albums.Remove(album);
  }

  public  void RemoveRange(IEnumerable<Album> albums)
  {
    _context.Albums.RemoveRange(albums);
  }
}