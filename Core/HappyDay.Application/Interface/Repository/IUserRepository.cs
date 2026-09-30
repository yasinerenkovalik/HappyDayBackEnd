using HappyDay.Domain.Entities;

namespace HappyDay.Application.Interface.Repository;

public interface IUserRepository:IGenericRepository<User>
{
    Task<User?> GetByEmailAsync(string email);

    /// <summary>Admin paneli: tüm kullanıcılar, bağlı oldukları firma adıyla birlikte.</summary>
    Task<List<UserAdminListItem>> GetAllForAdmin(string? search, int skip, int take);

    /// <summary>
    /// Pasifleştirilmiş kayıtları da bulur. Admin panelinde bir kullanıcıyı
    /// yeniden aktifleştirmek için gereklidir (GetByIdAsync yalnızca aktif kayıtları döner).
    /// </summary>
    Task<User?> GetByIdIncludingInactiveAsync(Guid id);
}

/// <summary>Admin paneli kullanıcı liste satırı.</summary>
public class UserAdminListItem
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public string SurName { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PhoneNumber { get; set; } = default!;
    public bool IsActivated { get; set; }
    public DateTime CreateDate { get; set; }
    public Guid? CompanyId { get; set; }
    public string? CompanyName { get; set; }
}