using Microsoft.EntityFrameworkCore;
using TestProject.Models;

namespace TestProject.DataAccess
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {

        }

        public DbSet<ResponseServiceInformation> ResponseServiceInformations { get; set; }
    }
}
