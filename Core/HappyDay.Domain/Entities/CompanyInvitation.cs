using HappyDay.Domain.Entities.BaseEntites;

public class CompanyInvitation : BaseEntity
{
    public string TokenHash { get; set; } = default!;      // Davetiye kodunun hash'i

    public string? Email { get; set; }                     // Opsiyonel: sadece belirli maile özel davetiye
    public string? CompanyNameHint { get; set; }           // Opsiyonel: görsel ipucu (firma adı vs.)

    public DateTime? ExpiresAt { get; set; }               // Süreli davetiye için
    public DateTime? UsedAt { get; set; }                  // Kullanıldığı zaman
    public Guid? UsedByCompanyId { get; set; }             // Kullanan firma Id

    public bool IsUsed => UsedAt.HasValue;                 // Tek kullanımlık kontrol
}