using Microsoft.AspNetCore.Identity;

namespace Blazor.Models
{
    public class Users
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public DateOnly JoinDate { get; set; } = DateOnly.FromDateTime(DateTime.Today);
        public int balance { get; set; } = 500;
        public string role { get; set; } = "user";

    }
}