using System;

namespace HappyDay.Domain.Entities
{
    public class PasswordResetToken
    {
        public Guid Id { get; set; }
        public Guid CompanyId { get; set; }
        public string TokenHash { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }
        public string Purpose { get; set; } = "password_reset"; // sabit

        public Company Company { get; set; } = null!;
    }
}