using Microsoft.EntityFrameworkCore;

namespace Application2.DataLayer
{
    public interface IAppDbContextFactory : IDbContextFactory<AppDbContext>
    {
        
    }

    public class ApplicationDbContextFactory : IAppDbContextFactory
    {
        private readonly IConnectionStringFactory _connFactory;

        public ApplicationDbContextFactory(IConnectionStringFactory connFactory)
        {
            _connFactory = connFactory;
        }

        public AppDbContext CreateDbContext()
        {
            // MUST block here: CreateDbContext is sync API
            var connectionString = _connFactory.BuildConnectionStringAsync("DefaultConnection").GetAwaiter().GetResult();

            var builder = new DbContextOptionsBuilder<AppDbContext>();
            builder.UseNpgsql(connectionString);

            return new AppDbContext(builder.Options);
        }
    }

}