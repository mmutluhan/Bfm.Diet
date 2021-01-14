namespace Bfm.Diet.Core.EntityFrameworkCore
{
    public interface IDbContextProvider<out TDbContext>
    {
        TDbContext GetDbContext();
    }
}