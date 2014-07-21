namespace ErikLieben.Data.Repository
{
    using System.Data.Entity;

    public interface IDatabaseFactory
    {
        DbContext CreateContext();
    }
}
