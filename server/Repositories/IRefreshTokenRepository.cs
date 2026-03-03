using shared;

namespace server.Repositories;

public interface IRefreshTokenRepository
{
    public Task<RefreshTokenRecords?> GetByAccessTokenJtiAsync(string? jti);

    public Task<RefreshTokenRecords?> GetByRefreshTokenAsync(string? refreshToken);

    public Task<RefreshTokenRecords?> CreateRefreshTokenAsync(RefreshTokenRecords refreshToken);

    public Task RevokeAsync(RefreshTokenRecords refreshToken);

    public Task<int> GetActiveTokenCountByUserIdAsync(Guid userId);
}
