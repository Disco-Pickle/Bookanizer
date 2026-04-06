using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Bookanizer.REST.DAL
{
    public class DataContextDesignFactory : IDesignTimeDbContextFactory<DataContext>
    {
        public DataContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<DataContext>()
                .UseNpgsql("Host=localhost;Port=5432;Database=bookanizer;Username=postgres;Password=postgres");
            return new DataContext(optionsBuilder.Options);
        }
    }
}