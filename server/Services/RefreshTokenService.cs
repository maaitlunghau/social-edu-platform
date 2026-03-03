using Microsoft.EntityFrameworkCore;
using server.Data;
using server.Repositories;
using shared;

namespace server.Services;

public class RefreshTokenService : IRefreshTokenRepository
{
    private readonly DataContext _dbContext;
    public RefreshTokenService(DataContext dbContext) => _dbContext = dbContext;


    public async Task<RefreshTokenRecords?> GetByRefreshTokenAsync(string? refreshToken)
    {
        return await _dbContext.RefreshTokenRecords.FirstOrDefaultAsync(
            rft => rft.RefreshToken == refreshToken
        );
    }

    public async Task<RefreshTokenRecords?> GetByAccessTokenJtiAsync(string? jti)
    {
        return await _dbContext.RefreshTokenRecords.FirstOrDefaultAsync(
            rft => rft.AccessTokenJTI == jti
        );
    }


    public async Task<int> GetActiveTokenCountByUserIdAsync(Guid userId)
    {
        return await _dbContext.RefreshTokenRecords.CountAsync(rft =>
            rft.UserId == userId &&
            rft.RevokedAtUTC == null &&
            rft.ExpireAtUTC >= DateTime.UtcNow
        );
    }


    public async Task<RefreshTokenRecords?> CreateRefreshTokenAsync(RefreshTokenRecords refreshToken)
    {
        await _dbContext.RefreshTokenRecords.AddAsync(refreshToken);
        await _dbContext.SaveChangesAsync();

        return refreshToken;
    }


    public async Task RevokeAsync(RefreshTokenRecords refreshToken)
    {
        refreshToken.RevokedAtUTC = DateTime.UtcNow;

        _dbContext.RefreshTokenRecords.Update(refreshToken);
        await _dbContext.SaveChangesAsync();
    }
}
