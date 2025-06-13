
using Npgsql;
using OnimeBestofrieeeendo.Models;

namespace OnimeBestofrieeeendo.Components.Services
{
    public class UserProfileService
    {
        private readonly string? _connString;

        public UserProfileService(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection");
        }

        public async Task<List<UserProfile>> LoadProfilesAsync()
        {
            var profiles = new List<UserProfile>();

            try
            {
                await using var conn = new NpgsqlConnection(_connString);
                await conn.OpenAsync();

                var query = @"
                    SELECT u.id, u.username, u.email, u.join_date, u.balance, u.role,
                           p.avatar_url, p.level, p.bio
                    FROM users u
                    JOIN profiles p ON u.id = p.user_id";

                await using var cmd = new NpgsqlCommand(query, conn);
                await using var reader = await cmd.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    profiles.Add(new UserProfile
                    {
                        Id = reader.GetInt32(0),
                        Username = reader.GetString(1),
                        Email = reader.GetString(2),
                        JoinDate = reader.GetDateTime(3),
                        Balance = reader.GetDecimal(4),
                        Role = reader.GetString(5),
                        AvatarUrl = reader.IsDBNull(6) ? null : reader.GetString(6),
                        Level = reader.GetInt32(7),
                        Bio = reader.IsDBNull(8) ? null : reader.GetString(8)
                    });
                }
            }
            catch
            {
                // можно добавить лог или вывод ошибки
            }

            return profiles;
        }

        public string GetAvatarUrl(UserProfile profile)
        {
            var url = profile.AvatarUrl;

            if (string.IsNullOrWhiteSpace(url))
                return "/images/default-avatar.jpg";

            if (url.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return url;

            var path = Path.Combine("wwwroot", url.TrimStart('/'));
            return File.Exists(path) ? "/" + url.TrimStart('/') : "/images/default-avatar.jpg";
        }
    }
}