namespace HappyDay.Application.Messages;

public static class MessageConstants
{
    //Company mesaşları
    public const string CompanyCreated = "Firma başarıyla oluşturuldu.";
    public const string CompanyUpdated = "Firma başarıyla güncellendi.";
    public const string CompanyDeleted = "Firma başarıyla silindi.";

    // Hata mesajları
    public const string CompanyAdressNotEmty = "adres boş geçilemez";
    public const string CompanyNotFound = "Firma bulunamadı.";
    public const string InvalidCompanyData = "Geçersiz firma bilgileri.";
    public const string UnexpectedError = "Beklenmeyen bir hata oluştu.";
    public const string RegisterNotFound = "Kayıt Bulunamadı";
    
    
    // Doğrulama (Validation) mesajları
    public const string RequiredField = "Bu alan zorunludur.";
    public const string InvalidEmail = "Geçersiz e-posta adresi.";
    
    //Organizasyon mesaşları
    public const string OrganizationCreated = "Organizasyon başarıyla oluşturuldu.";
    public const string OrganizationUpdated = "Organizasyon başarıyla güncellendi.";
    public const string OrganizationDeleted = "Organizasyon başarıyla silindi.";

    // Hata mesajları
    public const string OrganizationNotFound = "Organizasyon bulunamadı.";
    public const string InvalidOrganizationData = "Geçersiz Organizasyon bilgileri.";
    
    //User mesaşları
    public const string UserCreated = "Kullanıcı başarıyla oluşturuldu.";
    public const string UserUpdated = "Kullanıcı başarıyla güncellendi.";
    public const string UserDeleted = "Kullanıcı başarıyla silindi.";
    public const string UserLogin = "Kullanıcı başarıyla Giriş Yaptı.";
    public const string UserGetAll = "Kullanıcıların hepsi başarıyla getirildi.";
    public const string UserById = "Kullanıcı başarıyla getirildi.";

    // Hata mesajları
    public const string UserNotFound = "Kullanıcı bulunamadı.";
    public const string InvalidUserData = "Geçersiz Kullanıcı bilgileri.";
    
}