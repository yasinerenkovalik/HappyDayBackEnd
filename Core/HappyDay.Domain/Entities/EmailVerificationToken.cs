// Core/HappyDay.Domain/Entities/EmailVerificationToken.cs
using System;
using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Entities
{
    public class EmailVerificationToken:BaseEntity
    {
   
        public Guid CompanyId { get; set; }
        public string TokenHash { get; set; } = string.Empty;
        public string Purpose { get; set; } = "email_confirm";
        public DateTime ExpiresAt { get; set; }
        public DateTime? UsedAt { get; set; }

        // opsiyonel iz kayıtları
        public string? CreatedIp { get; set; }
        public string? UserAgent { get; set; }
    }
}