namespace Bfm.Diet.Core.Dto
{
    public interface IDtoBase<TPrimaryKey>
    {
        TPrimaryKey Id { get; set; }
    }
}