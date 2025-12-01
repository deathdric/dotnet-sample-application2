using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Application2.DataLayer
{

    public interface IApplicationDbContext
    {
        DbSet<DbTodoItem> TodoItems { get; }
        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
    }

    public class AppDbContext : DbContext, IApplicationDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<DbTodoItem> TodoItems => Set<DbTodoItem>();
    }
}