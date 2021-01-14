using Bfm.Diet.Core.Dto;

namespace Bfm.Diet.Dto.Authorization
{
    public class GrupDto : DtoBase<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
    }
}