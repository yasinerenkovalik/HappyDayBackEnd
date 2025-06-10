using System.Runtime.InteropServices.JavaScript;

namespace HappyDay.Domain.Entities.BaseEntites;

public class BaseEntity
{
    public Guid Id { get; set; }
    public DateTime CreateDate { get; set; }
    public DateTime UpdateDate { get; set; }
    public DateTime DeleteDate { get; set; }
    public bool IsActivated { get; set; }
}