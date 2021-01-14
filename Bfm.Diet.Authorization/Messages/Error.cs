namespace Bfm.Diet.Authorization.Messages
{
    public class Error
    {
        public const string UserNotFound = "Kullanıcı bulunamadı";
        public const string UserAlreadyExists = "Kullanıcı daha önce kaydedilmiş";
        public const string UserAccountNotActive = "Kullanıcı hesabi aktif değil";
        public const string PasswordError = "Kullanıcı yada parola hatalı";
        public const string TokenCreationError = "Token oluşturulamadı";
    }
}