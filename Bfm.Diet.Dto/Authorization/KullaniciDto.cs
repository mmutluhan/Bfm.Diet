using Bfm.Diet.Core.Dto;

namespace Bfm.Diet.Dto.Authorization
{
    public class KullaniciDto : DtoBase<long>
    {
        public string Adi { get; set; }
        public string Soyadi { get; set; }
        public string Email { get; set; }
        public bool Durum { get; set; }
        public string Parola { get; set; }
    }
}