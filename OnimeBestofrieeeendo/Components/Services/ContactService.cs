using Npgsql;
using OnimeBestofrieeeendo.Models;

namespace OnimeBestofrieeeendo.Components.Services;


public class ContactService(IConfiguration configuration)
{
    private readonly string _connectionString = configuration.GetConnectionString("DefaultConnection") ?? "";

    public async Task SaveContactAsync(ContactFormModel contact)
    {
        try
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            
            var cmd = new NpgsqlCommand("INSERT INTO contacts (name, email, subject, message) VALUES (@name, @email, @subject, @message)", conn);
            cmd.Parameters.AddWithValue("name", contact.Name);
            cmd.Parameters.AddWithValue("email", contact.Email);
            cmd.Parameters.AddWithValue("subject", contact.Subject);
            cmd.Parameters.AddWithValue("message", contact.Message);

            await cmd.ExecuteNonQueryAsync();
        }
        catch (Exception ex)
        {
            // Log error for developers to see
            ErrorHandler.LogError(ex, "ContactService.SaveContactAsync");
            throw; // Re-throw so the calling component can handle UI feedback
        }
    }
    
}