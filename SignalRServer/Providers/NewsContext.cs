using Microsoft.EntityFrameworkCore;

namespace SignalRServer.Providers
{
    public class NewsContext : DbContext
    {
        public NewsContext(DbContextOptions<NewsContext> options) :base(options)
        { }

        public DbSet<NewsItemEntity> NewsItemEntities { get; set; }

        public DbSet<Equity> NewsGroups { get; set; }
    }
}