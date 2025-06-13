using Npgsql;
using OnimeBestofrieeeendo.Models;

namespace OnimeBestofrieeeendo.Components.Services
{
    public class UserProfileService
    {
        private readonly string _connString;

        public UserProfileService(IConfiguration configuration)
        {
            _connString = configuration.GetConnectionString("DefaultConnection") ?? "";
        }

        public async Task<List<UserProfile>> LoadProfilesAsync()
        {
            var profiles = new List<UserProfile>();

            try
            {
                await using var conn = new NpgsqlConnection(_connString);
                await conn.OpenAsync();

                var cmd = new NpgsqlCommand(@"
                    SELECT u.id, u.username, u.email, u.join_date, u.balance, u.role,
                           p.avatar_url, p.level, p.bio
                    FROM users u
                    JOIN profiles p ON u.id = p.user_id", conn);

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
            catch (Exception)
            {
                // Лог ошибок можно добавить позже
            }

            return profiles;
        }

        public string GetAvatarUrl(UserProfile profile)
        {
            var avatar = profile.AvatarUrl;
            if (string.IsNullOrWhiteSpace(avatar))
                return "/images/default-avatar.jpg";

            if (avatar.StartsWith("http://") || avatar.StartsWith("https://"))
                return avatar;

            var path = avatar.StartsWith("/") ? avatar : "/" + avatar;
            var fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", path.TrimStart('/'));

            return File.Exists(fullPath) ? path : "/images/default-avatar.jpg";
        }
    }
}