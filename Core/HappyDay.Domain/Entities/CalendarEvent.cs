using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using HappyDay.Domain.Entities.BaseEntites;

namespace HappyDay.Domain.Models
{
    public class CalendarEvent:BaseEntity
    {
        
        public string Title { get; set; } = null!;
        
        public string? Description { get; set; }
        
        public DateTime StartUtc { get; set; }
        
        public DateTime EndUtc { get; set; }
        
        public Guid CompanyId { get; set; }

        public bool IsPublic { get; set; } = true;

        public bool IsAllDay { get; set; } = false;

    
    }
}