namespace ErikLieben.Data
{
    using System.Data.Entity;

    public interface IDatabaseFactory
    {
        DbContext CreateContext();
    }
}
