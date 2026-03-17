using Microsoft.EntityFrameworkCore;

public class UserRepository : IUser
{
    private readonly TrackAmDbContext _dbContext;
    public UserRepository(TrackAmDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<User>> GetAllUsers()
    {
        var users = await _dbContext.Users.AsNoTracking()
        .ToListAsync();
        return users;
    }
    public async Task<User> CreateUser(User user)
    {
        user.CreatedAt = DateTime.UtcNow;
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
        return user;
    }

    public async Task DeleteUser(User user)
    {
        _dbContext.Users.Remove(user);
        await _dbContext.SaveChangesAsync();
    }
    public async Task<User?> GetUserById(int Id)
    {
        var user = await _dbContext.Users.AsNoTracking()
        .FirstOrDefaultAsync(u => u.Id == Id);
        return user;
    }

    public async Task<User?> GetUserByGoogleId(string googleId)
    {
        var user = await _dbContext.Users.AsNoTracking()
        .FirstOrDefaultAsync(u => u.GoogleId == googleId);
        return user;
    }
    
}