using Bfm.Diet.Core.Base;

namespace Bfm.Diet.Authorization.Model
{
    public class Kullanici : ModelBase<int>
    {
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string Email { get; set; }
        public byte[] PasswordSalt { get; set; }
        public byte[] PasswordHash { get; set; }
        public bool Durum { get; set; }
    }
}