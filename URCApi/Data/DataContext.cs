using Microsoft.EntityFrameworkCore;
using URCApi.Data;
using URCApi.Entitites;

namespace URCApi.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<Setting> Settings { get; set; }
        public DbSet<Menu> MenuBar { get; set; }
        public DbSet<News> News { get; set; }
        public DbSet<Contact> Contacts { get; set; }
    }
}

//public class NewsDataContext : DbContext
//{
//    public NewsDataContext(DbContextOptions<NewsDataContext> options) : base(options)
//    {

//    }
//    public DbSet<News> Images { get; set; } 

//}